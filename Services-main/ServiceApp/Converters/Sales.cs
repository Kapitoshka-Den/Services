using ServiceApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ServiceApp.Converters
{
    internal class Sales : IValueConverter
    {
        ServicesContext context = new ServicesContext();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int idService = (int)value;
            var test = context.Services.Where(t => t.Id == idService && t.Discount != null).Select(t => t).FirstOrDefault();
            
            if(test == null)
            {

            }
            else
            {

            }
            return null; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
