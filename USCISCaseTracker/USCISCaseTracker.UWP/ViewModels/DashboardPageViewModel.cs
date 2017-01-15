using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USCISCaseTracker.Repositories;
using USCISCaseTracker.UWP.Shared.Services;

namespace USCISCaseTracker.UWP.ViewModels
{
    public class DashboardPageViewModel : ViewModelBase
    {
        public DashboardPageViewModel()
        {
        }

        public int CasesCount { get; set; }

        public int UnreadCasesCount { get; set; }

        public DateTime LastSynchronizedTime { get; set; }

        public DateTime LastUpdatedTime { get; set; }

        public void Update()
        {
            var repo = new CaseRepository(LocalDbConnectionService.Connect());
            CasesCount = repo.Count();
            UnreadCasesCount = repo.UnreadCount();
            LastSynchronizedTime = repo.LastSynchronizedTime();
            LastUpdatedTime = repo.LastUpdatedTime();
        }
    }
}
