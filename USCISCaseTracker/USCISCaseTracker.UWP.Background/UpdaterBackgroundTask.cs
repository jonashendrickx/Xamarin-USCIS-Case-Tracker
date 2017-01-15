using System;
using System.Collections.Generic;

using USCISCaseTracker.Models;
using USCISCaseTracker.Repositories;
using USCISCaseTracker.Services;
using USCISCaseTracker.UWP.Shared.Services;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI.Notifications;

namespace USCISCaseTracker.UWP.Background
{
    public sealed class UpdaterBackgroundTask : IBackgroundTask
    {
        BackgroundTaskDeferral _deferral;

        volatile bool cancelRequested = false;
        BackgroundTaskCancellationReason cancelReason = BackgroundTaskCancellationReason.Abort;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();
            taskInstance.Canceled += OnCanceled;

            try
            {
                var repo = new CaseRepository(LocalDbConnectionService.Connect());
                List<Case> cases = (List<Case>)repo.Read();

                if (cases != null && cases.Count > 0)
                {
                    foreach (var oldCase in cases)
                    {
                        var uscisSvc = new USCISService();
                        var newCase = await uscisSvc.GetCaseStatusAsync(oldCase.ReceiptNumber);

                        if (newCase == null)
                        {
                            continue;
                        }

                        bool success = repo.Save(oldCase, newCase);

                        if (success)
                        {
                            System.Diagnostics.Debug.WriteLine($"{oldCase.ReceiptNumber} ::: {oldCase.Status} ::: {oldCase.Status} ::: {oldCase.Description}");
                            SendToast(newCase);
                        }
                    }
                }
                ApplicationData.Current.LocalSettings.Values["CaseUpdate"] = $"Task Update Case finished running at {DateTime.Now.ToString()}";
                System.Diagnostics.Debug.WriteLine($"Task Update Case finished running at {DateTime.Now.ToString()}");
            }

            catch (Exception e)
            {
                ApplicationData.Current.LocalSettings.Values["FailedBackgroundTask"] = $"Task Update Case throw exception at {DateTime.Now.ToString()}";
                ApplicationData.Current.LocalSettings.Values["FailedBackgroundTaskException"] = e;
            }

            finally
            {
                _deferral.Complete();
            }
        }

        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            cancelRequested = true;
            cancelReason = reason;
        }

        private void SendToast(Case updatedCase)
        {
            var ToastContent = ToastService.CreateGenericToast("Case was updated!", $"{updatedCase.Name} with Receipt #:{updatedCase.ReceiptNumber} was updated on {updatedCase.LastSyncedDate.ToString(@"MM\/dd\/yyyy")}");
            ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(ToastContent.GetXml()));
        }
    }
}
