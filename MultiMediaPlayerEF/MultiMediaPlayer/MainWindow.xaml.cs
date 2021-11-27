using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BL.DbOperations;
using BL.Directory;
using MultiMediaPlayer.ViewModels;
using Path = System.IO.Path;

namespace MultiMediaPlayer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
        /// <summary>
		/// Default constructor
		/// </summary>
		public MainWindow()
		{
            InitializeComponent();
            DataContext = new DirectoryStructureViewModel(this);
			TreeView.DataContext = DataContext;
        }
        /// <summary>
        /// Allows to enable the button click in the grid list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayButton_OnClick(object sender, RoutedEventArgs e)
        {
            var ds = DataContext as DirectoryStructureViewModel;
            ds?.PlayButtonCommand.Execute(sender);

        }
        /// <summary>
        /// Allows to enable the button click in the grid list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var ds = DataContext as DirectoryStructureViewModel;
            ds?.EditButtonCommand.Execute(sender);
        }
    }
}
