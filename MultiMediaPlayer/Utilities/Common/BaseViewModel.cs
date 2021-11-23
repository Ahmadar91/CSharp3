using System.ComponentModel;
using System.Runtime.CompilerServices;
using PropertyChanged;
using Utilities.Annotations;

namespace Utilities.Common
{
	[AddINotifyPropertyChangedInterface]
	public class BaseViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;
		/// <summary>
		/// Adds the OnProperty Change for Properties in a class
		/// </summary>
		/// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}