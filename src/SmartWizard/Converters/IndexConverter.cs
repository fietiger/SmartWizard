using System;
using System.Globalization;
using System.Windows.Data;

namespace SmartWizard.Converters
{
    /// <summary>
    /// 索引转换器，用于步骤指示器显示步骤编号
    /// </summary>
    public class IndexConverter : IValueConverter
    {
        public static readonly IndexConverter Instance = new IndexConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int index)
            {
                return (index + 1).ToString();
            }
            return "1";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}