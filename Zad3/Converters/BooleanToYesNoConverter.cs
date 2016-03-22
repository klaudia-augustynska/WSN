using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Zad3.Converters
{
    /// <summary>
    /// Żeby w GUI się wyświetlało "tak" "nie" do informowania o życiu.
    /// </summary>
    public class BooleanToYesNoConverter : IValueConverter
    {
        public string TrueString { get; set; }
        public string FalseString { get; set; }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                return (bool)value
                    ? TrueString
                    : FalseString;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
