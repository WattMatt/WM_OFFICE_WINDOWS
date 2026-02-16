using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace WMOffice.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool boolValue = value is bool b && b;

            if (parameter is string paramStr && paramStr.Equals("Inverse", StringComparison.OrdinalIgnoreCase))
            {
                boolValue = !boolValue;
            }

            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            // Not implemented for now as we don't need two-way binding on visibility
            return DependencyProperty.UnsetValue;
        }
    }
}
