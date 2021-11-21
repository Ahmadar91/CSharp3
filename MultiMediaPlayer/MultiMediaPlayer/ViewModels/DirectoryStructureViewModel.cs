using BL.Directory;
using BL.Models;
using MultiMediaPlayer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private List<BitmapImage> Images = new List<BitmapImage>();
        private DispatcherTimer _timer = new DispatcherTimer();
        private DispatcherTimer VideoTimer = new DispatcherTimer();

        private int _imageNumber = 0;
        private int _VideoNumber = 0;

        public DirectoryStructureViewModel(MainWindow mainWindow)
        {
            MediaFileTypes = new MediaFileTypes
            {
                JPG =
                {
                    IsChecked = true
                },
                PNG =
                {
                    IsChecked = true
                },
                MP4 =
                {
                    IsChecked = true
                },
                WAV =
                {
                    IsChecked = true
                }
            };
            var du = new DirectoryUtils();


            AddButtonCommand = new ViewUtils.RelayCommand(o => AddButtonClick());
            OpenOptionsButton = new ViewUtils.RelayCommand(o => OpenOptions());
            PlayButtonCommand = new ViewUtils.RelayCommand(o => PlayButtonClick());
            DeleteButtonCommand = new ViewUtils.RelayCommand(o => DeleteButtonClick());
            MoveUpCommand = new ViewUtils.RelayCommand(o => MoveUpButtonClick());
            MoveDownCommand = new ViewUtils.RelayCommand(o => MoveDownButtonClick());
            PauseButtonCommand = new ViewUtils.RelayCommand(o => PauseButtonClick());

            PlayList = new ObservableCollection<DirectoryItemViewModel>();
            _mainWindow = mainWindow;
            Items = new ObservableCollection<DirectoryItemViewModel>(du.GetLogicalDirves()
                .Select(drive => new DirectoryItemViewModel(drive.FullPath, DirectoryItemType.Drive, du)));
            _mainWindow.TreeView.MouseDoubleClick += TreeView_MouseDoubleClick;
        }

        private void TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AddButtonClick();
        }

        public ICommand AddButtonCommand { get; set; }
        public ICommand OpenOptionsButton { get; set; }
        public ICommand DeleteButtonCommand { get; set; }
        public ICommand MoveUpCommand { get; set; }
        public ICommand MoveDownCommand { get; set; }
        public ICommand PlayButtonCommand { get; set; }
        public ICommand PauseButtonCommand { get; set; }
        private void AddButtonClick()
        {
            try
            {
                var selectedItem = _mainWindow.TreeView.FolderView.SelectedValue as DirectoryItemViewModel;
                if (selectedItem != null && selectedItem.Type.Equals(DirectoryItemType.File))
                {
                    if (MediaFileTypes.PNG.IsChecked != null && MediaFileTypes.JPG.IsChecked != null && (MediaFileTypes.JPG.IsChecked.Value && selectedItem.FullPath.ToLowerInvariant().EndsWith(".jpg")
                            || MediaFileTypes.PNG.IsChecked.Value && selectedItem.FullPath.ToLowerInvariant().EndsWith(".png")))
                    {
                        PlayList.Add(selectedItem);
                    }
                    else if (MediaFileTypes.WAV.IsChecked != null && MediaFileTypes.MP4.IsChecked != null && (MediaFileTypes.MP4.IsChecked.Value && selectedItem.FullPath.ToLowerInvariant().EndsWith(".mp4")
                                 || MediaFileTypes.WAV.IsChecked.Value && selectedItem.FullPath.ToLowerInvariant().EndsWith(".wav")))
                    {
                        PlayList.Add(selectedItem);
                    }
                    else
                        MessageBox.Show($"please choose the media types from the settings");

                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error {e.Message}");
            }

        }
        private void DeleteButtonClick()
        {
            try
            {
                var selectedItem = _mainWindow.Grid.SelectedIndex;
                if (selectedItem > 0)
                {
                    PlayList.RemoveAt(selectedItem);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error {e.Message}");
            }

        }
        private void PauseButtonClick()
        {
            try
            {

            }
            catch (Exception e)
            {
                MessageBox.Show($"Error {e.Message}");
            }

        }
        private void MoveUpButtonClick()
        {
            try
            {
                var selectedIndex = _mainWindow.Grid.SelectedIndex;
                if (selectedIndex > 0)
                {
                    var itemToMoveUp = PlayList[selectedIndex];
                    PlayList.RemoveAt(selectedIndex);
                    PlayList.Insert(selectedIndex - 1, itemToMoveUp);
                    _mainWindow.Grid.SelectedIndex = selectedIndex - 1;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error {e.Message}");
            }

        }
        private void MoveDownButtonClick()
        {
            try
            {
                var selectedIndex = _mainWindow.Grid.SelectedIndex;
                if (selectedIndex + 1 < PlayList.Count)
                {
                    var itemToMoveDown = PlayList[selectedIndex];
                    PlayList.RemoveAt(selectedIndex);
                    PlayList.Insert(selectedIndex + 1, itemToMoveDown);
                    _mainWindow.Grid.SelectedIndex = selectedIndex + 1;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error {e.Message}");
            }

        }
        private void PlayButtonClick()
        {
            if (PlayList.Count <= 0)
            {
                MessageBox.Show(" playlist is empty please insert Items in to the playlist");
            }

            var firstItem = PlayList[0].FullPath;
            if (firstItem.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase) || firstItem.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase))
            {
                _mainWindow.Image.Source = new BitmapImage(new Uri(PlayList[0].FullPath));

            }
            if (firstItem.EndsWith(".mp4", StringComparison.CurrentCultureIgnoreCase) || firstItem.EndsWith(".wav", StringComparison.CurrentCultureIgnoreCase))
            {
                _mainWindow.Video.Source = new Uri(PlayList[0].FullPath, UriKind.RelativeOrAbsolute);
            }

            // Install a timer to show each image.

            _timer.Interval = TimeSpan.FromSeconds(3);
            _timer.Tick += Tick;
            _timer.Start();



        }

        private void Tick(object sender, System.EventArgs e)
        {
            _imageNumber = (_imageNumber + 1);

            if (!(_imageNumber).Equals(PlayList.Count))
            {
                _timer.IsEnabled = true;
                ShowNextImage(_mainWindow.Image, _mainWindow.Video);
            }
            else
            {
                _timer.IsEnabled = false;
            }


        }
        private void ShowNextImage(Image img, MediaElement video)
        {
            var item = PlayList[_imageNumber];
            Storyboard sb = new Storyboard();
            if (item.FullPath.EndsWith("jpg", StringComparison.CurrentCultureIgnoreCase) || item.FullPath.EndsWith("png", StringComparison.InvariantCultureIgnoreCase))
            {
                video.Source = null;
                const double transition_time = 0.9;
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
                    new DiscreteObjectKeyFrame(new BitmapImage(new Uri(item.FullPath)), TimeSpan.Zero);
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

            if (item.FullPath.EndsWith("mp4", StringComparison.CurrentCultureIgnoreCase) || item.FullPath.EndsWith("wav", StringComparison.InvariantCultureIgnoreCase))
            {
                img.Source = null;
                if (video.Source != null)
                {
                    while (true)
                    {
                        if (video.NaturalDuration.HasTimeSpan)
                            _mainWindow.label.Content = $"{_mainWindow.Video.Position:mm\\:ss} / {_mainWindow.Video.NaturalDuration.TimeSpan:mm\\:ss}";
                        if (video.Position.Equals(video.NaturalDuration.TimeSpan))
                        {
                            video.Source = new Uri(item.FullPath, UriKind.RelativeOrAbsolute);
                            break;
                        }
                    }
                }
                else
                {
                    video.Source = new Uri(item.FullPath, UriKind.RelativeOrAbsolute);
                }
            }
        }

        private void OpenOptions()
        {
            MediaFileTypes.Show();
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

    }
}