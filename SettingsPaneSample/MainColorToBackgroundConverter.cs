using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace SettingsPaneSample
{
    public class MainColorToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var mainColor = (string)value;
            if (mainColor != null && mainColor.Length >= 8)

                return new SolidColorBrush(Color.FromArgb(
                                                            System.Convert.ToByte(mainColor.Substring(0, 2), 16),
                                                            System.Convert.ToByte(mainColor.Substring(2, 2), 16),
                                                            System.Convert.ToByte(mainColor.Substring(4, 2), 16),
                                                            System.Convert.ToByte(mainColor.Substring(6, 2), 16)
                                                          ));
            else
                return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
