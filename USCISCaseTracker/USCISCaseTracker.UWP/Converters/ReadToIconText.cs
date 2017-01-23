using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace USCISCaseTracker.UWP.Converters
{
    public class ReadToIconText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is bool))
            {
                throw new InvalidCastException("value is not boolean");
            }
            else
            {
                var read = (bool) value;
                return read ? "Read" : "Mail";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
