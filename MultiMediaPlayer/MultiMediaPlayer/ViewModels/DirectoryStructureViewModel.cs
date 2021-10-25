using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using BL.Directory;
using BL.Models;
using MultiMediaPlayer.Views;
using Utilities.Common;
using MultiMediaPlayer.ViewUtils;

namespace MultiMediaPlayer.ViewModels
{
	public class DirectoryStructureViewModel : BaseViewModel
	{
		private readonly MainWindow _mainWindow;
		public List<CheckListClass> AvailablePresentationObjects { get; set; }
		public ObservableCollection<DirectoryItemViewModel> Items { get; set; }
		public bool? PNG { get; set; }
		public bool? JPG { get; set; }
		public MediaFileTypes MediaFileTypes { get; set; }

		public DirectoryStructureViewModel(MainWindow mainWindow)
		{
			MediaFileTypes = new MediaFileTypes();
			var du = new DirectoryUtils();


			AddButtonCommand = new ViewUtils.RelayCommand(o => MainButtonClick());
			GetImageCommand = new ViewUtils.RelayCommand(o => GetImage());
			OpenOptionsButton = new ViewUtils.RelayCommand(o => OpenOptions());


			_mainWindow = mainWindow;
			Items = new ObservableCollection<DirectoryItemViewModel>(du.GetLogicalDirves()
				.Select(drive => new DirectoryItemViewModel(drive.FullPath, DirectoryItemType.Drive, du)));
		}

		public ICommand AddButtonCommand { get; set; }
		public ICommand OpenOptionsButton { get; set; }
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
				if ((bool)MediaFileTypes.JPG.IsChecked)
				{
					_mainWindow.Image.Source =
						new BitmapImage(new Uri(test1, UriKind.RelativeOrAbsolute));
					//var test55 = GetImage(test1);

					FullPath = test1;
					_mainWindow.Image.Source =
						new BitmapImage(new Uri(FullPath, UriKind.RelativeOrAbsolute));
					MessageBox.Show(test1.ToString());
				}

			}
		}

		public BitmapImage DisplayedImage
		{
			get;
			set;
		}

		private void OpenOptions()
		{
			MediaFileTypes.Show();
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

		public class CheckListClass
		{
			public string Name { get; set; }
			public bool IsChecked { get; set; }

		}
	}
}