using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ServiceApp.Converters
{
    internal class FullPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            // return "../../Resources/Images/" + value.ToString();
            Uri uri = new Uri("./../../../Resources/Images/" + value.ToString(), UriKind.Relative);
            BitmapImage bitmapImage = new BitmapImage(uri);
            return bitmapImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
