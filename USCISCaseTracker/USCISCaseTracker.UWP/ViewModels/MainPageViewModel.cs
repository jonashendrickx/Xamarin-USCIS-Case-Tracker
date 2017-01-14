using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
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
