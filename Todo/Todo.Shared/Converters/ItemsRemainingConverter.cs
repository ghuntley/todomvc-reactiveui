using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace Todo.Shared.Converters
{
    public class ItemsRemainingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            var remaining = (int)value;

            if (remaining > 1)
            {
                return string.Format("{0} items remain", remaining);
            }
            else
            {
                return string.Format("{0} item remains", remaining);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException();
        }
    }
}
