using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace USCISCaseTracker.UWP.Converters
{
    public class DateTimeToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is DateTime))
            {
                throw new InvalidCastException("value is not DateTime");
            }
            else
            {
                DateTime nullDate = new DateTime(1, 1, 1, 0, 0, 0);
                if (DateTime.Compare(nullDate, (DateTime)value) == 0)
                {
                    return "Never";
                }
                return ((DateTime)value).ToString(@"MM\/dd\/yyyy HH:mm");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
