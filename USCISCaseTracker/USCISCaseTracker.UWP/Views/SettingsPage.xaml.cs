using System;
using System.Runtime.InteropServices;
using USCISCaseTracker.UWP.Shared;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using USCISCaseTracker.UWP.Background;
using USCISCaseTracker.UWP.ViewModels;

namespace USCISCaseTracker.UWP.Views
{
    public sealed partial class SettingsPage : Page
    {
        private BackgroundAccessStatus _backgroundAccessStatus = BackgroundAccessStatus.Unspecified;
        private readonly ApplicationDataContainer _localSettings;

        public SettingsPage()
        {
            InitializeComponent();

            _localSettings = ApplicationData.Current.LocalSettings;
        }

        public SettingsPageViewModel ViewModel => (SettingsPageViewModel) DataContext;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ComboBoxIntervals.ItemsSource = ViewModel.BackgroundUpdateIntervals;

            if (BackgroundTaskExists(BackgroundTasksConfiguration.CaseUpdaterBackgroundTaskName))
            {
                AllowBackgroundUpdatesCheckBox.IsChecked = true;
                ComboBoxIntervals.IsEnabled = false;
            }
            else
            {
                AllowBackgroundUpdatesCheckBox.IsChecked = false;
                ComboBoxIntervals.IsEnabled = true;
            }

            if (_localSettings.Values["background_update_interval"] != null)
            {
                var index = ViewModel.BackgroundUpdateIntervals.FindIndex(o => o.Value == (uint)_localSettings.Values["background_update_interval"]);
                ComboBoxIntervals.SelectedIndex = index;
            }
            else
            {
                ComboBoxIntervals.SelectedIndex = 0;
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
            var interval = (BackgroundUpdateInterval) ComboBoxIntervals.SelectedItem;
            if (interval != null)
            {
                var timeTrigger = new TimeTrigger(interval.Value, false);
                builder.SetTrigger(timeTrigger);
            }

            builder.Register();

            ComboBoxIntervals.IsEnabled = false;
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

            ComboBoxIntervals.IsEnabled = true;
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

        private void ComboBoxIntervals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxIntervals.SelectedItem != null)
            {
                var selectedBackgroundUpdateInterval = (BackgroundUpdateInterval) ComboBoxIntervals.SelectedItem;
                _localSettings.Values["background_update_interval"] = selectedBackgroundUpdateInterval.Value;
            }
        }
    }
}
