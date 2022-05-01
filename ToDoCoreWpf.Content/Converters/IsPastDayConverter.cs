using System;
using System.Globalization;
using System.Windows.Data;

namespace MinatoProject.Apps.ToDoCoreWpf.Content.Converters
{
    /// <summary>
    /// 過去日かどうかを判定するコンバーター
    /// </summary>
    internal class IsPastDayConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime sourceDate)
            {
                return sourceDate.Date < DateTime.Now.Date;
            }
            throw new InvalidCastException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
