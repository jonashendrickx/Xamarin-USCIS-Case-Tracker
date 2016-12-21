using System.Collections.ObjectModel;
using System.Threading.Tasks;
using USCISCaseTracker.Models;
using USCISCaseTracker.Repositories;
using USCISCaseTracker.UWP.Shared;
using USCISCaseTracker.UWP.Shared.Services;
using Windows.ApplicationModel.Background;

namespace USCISCaseTracker.UWP.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private ObservableCollection<Case> _cases;

        public MainPageViewModel()
        {
            LoadLocalCases();
            Init();
        }

        public async Task Init()
        {
            var taskRegistered = false;

            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == BackgroundTasksConfiguration.CaseUpdaterBackgroundTaskName)
                {
                    taskRegistered = true;
                    break;
                }
            }

            if (!taskRegistered)
            {
                 //required call
                var access = BackgroundExecutionManager.RequestAccessAsync().GetResults();
                
                 //abort if access isn't granted
                if (access == BackgroundAccessStatus.DeniedBySystemPolicy || access == BackgroundAccessStatus.DeniedByUser)
                    return;

                var builder = new BackgroundTaskBuilder();

                builder.Name = BackgroundTasksConfiguration.CaseUpdaterBackgroundTaskName;
                builder.TaskEntryPoint = BackgroundTasksConfiguration.CaseUpdaterBackgroundTaskEntryPoint;
                SystemCondition internetCondition = new SystemCondition(SystemConditionType.InternetAvailable);
                SystemCondition lowCpu = new SystemCondition(SystemConditionType.BackgroundWorkCostNotHigh);
                builder.AddCondition(internetCondition);
                builder.AddCondition(lowCpu);
                var timeTrigger = new TimeTrigger(15, false);
                builder.SetTrigger(timeTrigger);

                builder.Register();
            }
        }

        public void LoadLocalCases()
        {
            var repo = new CaseRepository(LocalDbConnectionService.Connect());
            Cases = new ObservableCollection<Case>(repo.Read());
        }

        public ObservableCollection<Case> Cases
        {
            get
            {
                return _cases;
            }
            set
            {
                SetProperty(ref _cases, value, "Cases");
            }
        }
    }
}
