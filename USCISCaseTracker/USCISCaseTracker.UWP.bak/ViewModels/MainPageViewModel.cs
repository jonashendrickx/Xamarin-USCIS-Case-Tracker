using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USCISCaseTracker.Models;
using USCISCaseTracker.Repositories;
using USCISCaseTracker.UWP.Services;

namespace USCISCaseTracker.UWP.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private ObservableCollection<Case> _cases;

        public MainPageViewModel()
        {
            LoadLocalCases();
            
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
