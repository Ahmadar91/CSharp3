using System;
using System.Windows.Input;

namespace Utilities.Common
{
    public class RelayCommand : ICommand
	{
		private readonly Action _action;

		public RelayCommand(Action action)
		{
			_action = action;
		}
		public event EventHandler? CanExecuteChanged = (Sender, e) => { };
		/// <summary>
		/// Checks of a Command can be executed set to true
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public bool CanExecute(object? parameter)
		{
			return true;
		}
		/// <summary>
		/// Executes a Command
		/// </summary>
		/// <param name="parameter"></param>
        public void Execute(object? parameter)
		{
			_action();
		}
	}
}