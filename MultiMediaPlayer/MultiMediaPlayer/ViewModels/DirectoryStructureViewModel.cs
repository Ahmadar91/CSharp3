using BL.Directory;
using BL.Models;
using MultiMediaPlayer.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Utilities.Common;

namespace MultiMediaPlayer.ViewModels
{
	public class DirectoryStructureViewModel : BaseViewModel
	{
		private readonly MainWindow _mainWindow;
		public ObservableCollection<DirectoryItemViewModel> Items { get; set; }
		public ObservableCollection<DirectoryItemViewModel> PlayList { get; set; }
		public bool? PNG { get; set; }
		public bool? JPG { get; set; }
		public MediaFileTypes MediaFileTypes { get; set; }
		public int Index { get; set; }

		public DirectoryStructureViewModel(MainWindow mainWindow)
		{
			MediaFileTypes = new MediaFileTypes();
			MediaFileTypes.JPG.IsChecked = false;
			MediaFileTypes.PNG.IsChecked = false;
			MediaFileTypes.MP4.IsChecked = false;
			MediaFileTypes.WAV.IsChecked = false;
			var du = new DirectoryUtils();


			AddButtonCommand = new ViewUtils.RelayCommand(o => AddButtonClick());
			GetImageCommand = new ViewUtils.RelayCommand(o => GetImage());
			OpenOptionsButton = new ViewUtils.RelayCommand(o => OpenOptions());
			PlayButtonCommand = new ViewUtils.RelayCommand(o => PlayButtonClick());
			PlayList = new ObservableCollection<DirectoryItemViewModel>();

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
		private void AddButtonClick()
		{
			var test = _mainWindow.TreeView.FolderView.SelectedValue as DirectoryItemViewModel;
			if (test.Type.Equals(DirectoryItemType.File))
			{
				if (MediaFileTypes.JPG.IsChecked.Value && test.FullPath.ToLowerInvariant().EndsWith(".jpg"))
				{
					PlayList.Add(test);

				}
			}
		}
		private void PlayButtonClick()
		{
			foreach (var item in PlayList)
			{
				if (item.FullPath.EndsWith(".jpg"))
				{
					FullPath = item.FullPath;
				}
		
				//_mainWindow.Image.Source =
				//	new BitmapImage(new Uri(FullPath, UriKind.RelativeOrAbsolute));
				//MessageBox.Show(item.FullPath.ToString());
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

	}
}