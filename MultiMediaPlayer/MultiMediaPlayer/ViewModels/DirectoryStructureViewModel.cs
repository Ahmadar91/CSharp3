using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using BL.Directory;
using BL.Models;
using Utilities.Common;
using MultiMediaPlayer.ViewUtils;

namespace MultiMediaPlayer.ViewModels
{
	public class DirectoryStructureViewModel : BaseViewModel
	{
		private readonly MainWindow _mainWindow;

		public ObservableCollection<DirectoryItemViewModel> Items { get; set; }

		public DirectoryStructureViewModel(MainWindow mainWindow)
		{
			AddButtonCommand = new ViewUtils.RelayCommand(o => MainButtonClick());
			GetImageCommand = new ViewUtils.RelayCommand(o => GetImage());
			_mainWindow = mainWindow;
			Items = new ObservableCollection<DirectoryItemViewModel>(DirectoryUtils.GetLogicalDirves()
				.Select(drive => new DirectoryItemViewModel(drive.FullPath, DirectoryItemType.Drive)));
		}

		public ICommand AddButtonCommand { get; set; }
		public ICommand DeleteButtonCommand { get; set; }
		public ICommand MoveUpCommand { get; set; }
		public ICommand MoveDownCommand { get; set; }
		public ICommand PlayButtonCommand { get; set; }
		public ICommand PauseButtonCommand { get; set; }
		public ICommand GetImageCommand { get; set; }

		public string FullPath { get; set; }
		private void MainButtonClick()
		{

			var test = _mainWindow.TreeView.FolderView.SelectedValue as DirectoryItemViewModel;
			var test1 = test.FullPath;

			if (test.Type.Equals(DirectoryItemType.File))
			{

				//_mainWindow.Image.Source =
				//	new BitmapImage(new Uri(test1, UriKind.RelativeOrAbsolute));
				//var test55 = GetImage(test1);

				FullPath = test1;
				_mainWindow.Image.Source =
					new BitmapImage(new Uri(FullPath, UriKind.RelativeOrAbsolute));
				MessageBox.Show(test1.ToString());
			}
		}

		public BitmapImage DisplayedImage
		{
			get;
			set;
		}

		private void GetImage()
		{
			//MessageBox.Show("here-+");
			//DisplayedImage = new BitmapImage();
			//if (!string.IsNullOrEmpty(path))
			//{
			//	DisplayedImage.BeginInit();
			//	DisplayedImage.CacheOption = BitmapCacheOption.OnLoad;
			//	DisplayedImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
			//	DisplayedImage.UriSource = new Uri(path);
			//	DisplayedImage.DecodePixelWidth = 200;
			//	DisplayedImage.EndInit();
			//}
			//return DisplayedImage;


		}
	}
}