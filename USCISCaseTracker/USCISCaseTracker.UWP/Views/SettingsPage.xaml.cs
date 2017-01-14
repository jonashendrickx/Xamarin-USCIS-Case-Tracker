using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using USCISCaseTracker.UWP.Shared;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace USCISCaseTracker.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private BackgroundAccessStatus _backgroundAccessStatus = BackgroundAccessStatus.Unspecified;

        public SettingsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (BackgroundTaskExists(BackgroundTasksConfiguration.CaseUpdaterBackgroundTaskName))
            {
                AllowBackgroundUpdatesCheckBox.IsChecked = true;
            }
            else
            {
                AllowBackgroundUpdatesCheckBox.IsChecked = false;
            }
        }

        private async void AllowBackgroundUpdatesCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (BackgroundTaskExists(BackgroundTasksConfiguration.CaseUpdaterBackgroundTaskName))
            {
                return;
            }

            try
            {
                _backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
            }
            catch (COMException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + '\n' + ex.StackTrace);
            }


            //abort if access isn't granted
            if (_backgroundAccessStatus == BackgroundAccessStatus.DeniedBySystemPolicy || _backgroundAccessStatus == BackgroundAccessStatus.DeniedByUser)
            {
                return;
            }

            var builder = new BackgroundTaskBuilder();

            builder.Name = BackgroundTasksConfiguration.CaseUpdaterBackgroundTaskName;
            builder.TaskEntryPoint = BackgroundTasksConfiguration.CaseUpdaterBackgroundTaskEntryPoint;
            SystemCondition internetCondition = new SystemCondition(SystemConditionType.InternetAvailable);
            SystemCondition lowCpu = new SystemCondition(SystemConditionType.BackgroundWorkCostNotHigh);
            builder.AddCondition(internetCondition);
            builder.AddCondition(lowCpu);
            var timeTrigger = new TimeTrigger(180, false);
            builder.SetTrigger(timeTrigger);

            builder.Register();
        }

        private async void AllowBackgroundUpdatesCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                _backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
            }
            catch (COMException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + '\n' + ex.StackTrace);
            }


            //abort if access isn't granted
            if (_backgroundAccessStatus == BackgroundAccessStatus.DeniedBySystemPolicy || _backgroundAccessStatus == BackgroundAccessStatus.DeniedByUser)
            {
                return;
            }

            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == BackgroundTasksConfiguration.CaseUpdaterBackgroundTaskName)
                {
                    System.Diagnostics.Debug.WriteLine($"{task.Value.Name} is being unregistered");
                    task.Value.Unregister(true);
                    break;
                }
            }
        }

        private bool BackgroundTaskExists(string name)
        {
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
