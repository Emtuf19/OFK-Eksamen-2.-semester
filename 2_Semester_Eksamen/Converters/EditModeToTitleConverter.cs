using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace _2_Semester_Eksamen.Converters
{
    public class EditModeToTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            string type = parameter as string ?? "Item";

            if (value is bool isEditMode)
                return isEditMode ? $"Rediger {type}" : $"Opret {type}";

            return type;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
