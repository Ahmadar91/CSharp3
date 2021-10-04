using System.ComponentModel;
using System.Runtime.CompilerServices;
using Utilities.Annotations;

namespace Utilities.Common
{
	public class ObservableObject : INotifyPropertyChanged
	{

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		//public event PropertyChangedEventHandler? PropertyChanged;

		//[NotifyPropertyChangedInvocator]
		//protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		//{
		//	PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		//}
	}
}