using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USCISCaseTracker.UWP.Shared
{
    public class BackgroundTasksConfiguration
    {
        public const string CaseUpdaterBackgroundTaskName = "UpdaterBackgroundTask";
        public const string CaseUpdaterBackgroundTaskEntryPoint = "USCISCaseTracker.UWP.Background.UpdaterBackgroundTask";
    }
}
