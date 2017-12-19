using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Sramek.FX.WPF.Converter
{
    public class StringMultibindConvertor : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Select(a=>(string) a).Aggregate((a,b)=>$"{a} {b}");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
