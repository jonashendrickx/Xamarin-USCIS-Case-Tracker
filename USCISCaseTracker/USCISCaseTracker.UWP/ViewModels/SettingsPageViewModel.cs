using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USCISCaseTracker.UWP.Background;

namespace USCISCaseTracker.UWP.ViewModels
{
    public sealed class SettingsPageViewModel : ViewModelBase
    {
        public SettingsPageViewModel()
        {
            BackgroundUpdateIntervals = new List<BackgroundUpdateInterval>();
            BackgroundUpdateIntervals.Add(new BackgroundUpdateInterval(30));
            BackgroundUpdateIntervals.Add(new BackgroundUpdateInterval(60));
            BackgroundUpdateIntervals.Add(new BackgroundUpdateInterval(90));
            BackgroundUpdateIntervals.Add(new BackgroundUpdateInterval(120));
            BackgroundUpdateIntervals.Add(new BackgroundUpdateInterval(180));
            BackgroundUpdateIntervals.Add(new BackgroundUpdateInterval(240));
            BackgroundUpdateIntervals.Add(new BackgroundUpdateInterval(360));
        }

        public List<BackgroundUpdateInterval> BackgroundUpdateIntervals { get; set; }
    }
}
