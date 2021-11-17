using BL.Directory;
using BL.Models;
using MultiMediaPlayer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Utilities.Common;

namespace MultiMediaPlayer.ViewModels
{
	public class DirectoryStructureViewModel : BaseViewModel
	{
		private readonly MainWindow _mainWindow;
		public ObservableCollection<DirectoryItemViewModel> Items { get; set; }
		public ObservableCollection<DirectoryItemViewModel> PlayList { get; set; }
        public MediaFileTypes MediaFileTypes { get; set; }
		public int Index { get; set; }
        private List<BitmapImage> Images = new List<BitmapImage>();
        private DispatcherTimer PictureTimer = new DispatcherTimer();
        DispatcherTimer VideoTimer = new DispatcherTimer();
 
        private int ImageNumber = 0;

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
		public string VideoFullPath { get; set; }
		private void AddButtonClick()
		{
            try
            {
                var test = _mainWindow.TreeView.FolderView.SelectedValue as DirectoryItemViewModel;
                if (test.Type.Equals(DirectoryItemType.File))
                {
                    if (MediaFileTypes.JPG.IsChecked.Value && test.FullPath.ToLowerInvariant().EndsWith(".jpg")
                        || MediaFileTypes.PNG.IsChecked.Value && test.FullPath.ToLowerInvariant().EndsWith(".png")
                       )
                    {
                        PlayList.Add(test);
                        ImageNumber++;
                    }

                    if ( MediaFileTypes.MP4.IsChecked.Value && test.FullPath.ToLowerInvariant().EndsWith(".mp4")
                        || MediaFileTypes.WAV.IsChecked.Value && test.FullPath.ToLowerInvariant().EndsWith(".wav"))
                    {
                        PlayList.Add(test);
                    }
                    MessageBox.Show($"please choose the media types from the settings");

				}
			}
            catch (Exception e)
            {
                MessageBox.Show($"Error {e.Message}");
            }
			
		}
		private void PlayButtonClick()
		{
			foreach (var item in PlayList)
            {
                FullPath = "";

				if (item.FullPath.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase) || item.FullPath.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase))
                {
                    //FullPath = item.FullPath;
                    Images.Add(new BitmapImage(new Uri(item.FullPath)));
					//Task.Delay(3000).Wait();
				}

                if (item.FullPath.EndsWith(".mp4", StringComparison.CurrentCultureIgnoreCase) || item.FullPath.EndsWith(".wav", StringComparison.CurrentCultureIgnoreCase))
                {
                    _mainWindow.Video.Source = new Uri(item.FullPath, UriKind.RelativeOrAbsolute);
                }
				// else
				//FullPath = "";



				//foreach (FileInfo file_info in dir_info.GetFiles())
    //            {
    //                if ((file_info.Extension.ToLower() == ".jpg") ||
    //                    (file_info.Extension.ToLower() == ".png"))
    //                {
                       
    //                }
    //            }

                // Display the first image.
                //imgPicture.Source = Images[0];

                if (Images.Count > 0)
                {
                    _mainWindow.Image.Source = Images[0];
                    // Install a timer to show each image.
                    PictureTimer.Interval = TimeSpan.FromSeconds(3);
                    PictureTimer.Tick += Tick;
                    PictureTimer.Start();

                }
                VideoTimer.Interval = TimeSpan.FromSeconds(1);
                VideoTimer.Tick += timer_Tick;
                VideoTimer.Start();




                //_mainWindow.Image.Source =
                //	new BitmapImage(new Uri(FullPath, UriKind.RelativeOrAbsolute));
                //MessageBox.Show(item.FullPath.ToString());
            }

		}
        void timer_Tick(object sender, EventArgs e)
        {
            if (_mainWindow.Video.Source != null)
            {
           //     if (_mainWindow.Video.NaturalDuration.HasTimeSpan)
                    //lblStatus.Content = String.Format("{0} / {1}", _mainWindow.Video.Position.ToString(@"mm\:ss"), _mainWindow.Video.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            }
            //else
                //lblStatus.Content = "No file selected...";
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Video.Play();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Video.Pause();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Video.Stop();
        }



        private void Tick(object sender, System.EventArgs e)
        {
            ImageNumber = (ImageNumber + 1) % Images.Count;
            ShowNextImage(_mainWindow.Image);
        }
        private void ShowNextImage(Image img)
        {
            const double transition_time = 0.9;
            Storyboard sb = new Storyboard();

            // ***************************
            // Animate Opacity 1.0 --> 0.0
            // ***************************
            DoubleAnimation fade_out = new DoubleAnimation(1.0, 0.0,
                TimeSpan.FromSeconds(transition_time));
            fade_out.BeginTime = TimeSpan.FromSeconds(0);

            // Use the Storyboard to set the target property.
            Storyboard.SetTarget(fade_out, img);
            Storyboard.SetTargetProperty(fade_out,
                new PropertyPath(Image.OpacityProperty));

            // Add the animation to the StoryBoard.
            sb.Children.Add(fade_out);


            // *********************************
            // Animate displaying the new image.
            // *********************************
            ObjectAnimationUsingKeyFrames new_image_animation =
                new ObjectAnimationUsingKeyFrames();
            // Start after the first animation has finisheed.
            new_image_animation.BeginTime = TimeSpan.FromSeconds(transition_time);

            // Add a key frame to the animation.
            // It should be at time 0 after the animation begins.
            DiscreteObjectKeyFrame new_image_frame =
                new DiscreteObjectKeyFrame(Images[ImageNumber], TimeSpan.Zero);
            new_image_animation.KeyFrames.Add(new_image_frame);

            // Use the Storyboard to set the target property.
            Storyboard.SetTarget(new_image_animation, img);
            Storyboard.SetTargetProperty(new_image_animation,
                new PropertyPath(Image.SourceProperty));

            // Add the animation to the StoryBoard.
            sb.Children.Add(new_image_animation);


            // ***************************
            // Animate Opacity 0.0 --> 1.0
            // ***************************
            // Start when the first animation ends.
            DoubleAnimation fade_in = new DoubleAnimation(0.0, 1.0,
                TimeSpan.FromSeconds(transition_time));
            fade_in.BeginTime = TimeSpan.FromSeconds(transition_time);

            // Use the Storyboard to set the target property.
            Storyboard.SetTarget(fade_in, img);
            Storyboard.SetTargetProperty(fade_in,
                new PropertyPath(Image.OpacityProperty));

            // Add the animation to the StoryBoard.
            sb.Children.Add(fade_in);

            // Start the storyboard on the img control.
            sb.Begin(img);
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