using System.Globalization;

namespace LongoToDoApp.Converters
{
	public class BoolToTextColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var isChecked = (bool)value;

			return isChecked ? Color.FromArgb("#696969") : (object)Color.FromArgb("#000000");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return -1;
		}
	}
}
