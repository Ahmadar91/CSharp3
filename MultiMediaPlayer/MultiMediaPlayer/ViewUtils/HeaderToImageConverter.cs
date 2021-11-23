using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using BL.Directory;
using BL.Models;

namespace MultiMediaPlayer.ViewUtils
{
	[ValueConversion(typeof(DirectoryItemType), typeof(BitmapImage))]
	public class HeaderToImageConverter : IValueConverter
	{
		public static HeaderToImageConverter Instance = new HeaderToImageConverter();
		/// <summary>
		/// Method to allow the icons to show in the tree view
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return new BitmapImage(new Uri($"pack://application:,,,/Images/{value}.png"));
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}