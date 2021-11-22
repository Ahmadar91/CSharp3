using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using MultiMediaPlayer.Views;

namespace MultiMediaPlayer.ViewModels
{
    public class PlayerViewModel
    {
        private readonly Player _player;
        private readonly ObservableCollection<DirectoryItemViewModel> _playlist;
        private DispatcherTimer _timer = new DispatcherTimer();

        private int _imageNumber = 0;
        public PlayerViewModel(Player player, ObservableCollection<DirectoryItemViewModel> playlist)
        {
            _player = player;
            _playlist = playlist;
            BeginSlideShow();
        }
        private void BeginSlideShow()
        {
            if (_playlist.Count <= 0)
            {
                MessageBox.Show(" playlist is empty please insert Items in to the playlist");
            }

            var firstItem = _playlist[0].FullPath;
            if (firstItem.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase) || firstItem.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase))
            {
                _player.image.Source = new BitmapImage(new Uri(_playlist[0].FullPath));

            }
            if (firstItem.EndsWith(".mp4", StringComparison.CurrentCultureIgnoreCase) || firstItem.EndsWith(".wav", StringComparison.CurrentCultureIgnoreCase))
            {
                _player.video.Source = new Uri(_playlist[0].FullPath, UriKind.RelativeOrAbsolute);
            }

            // Install a timer to show each image.

            _timer.Interval = TimeSpan.FromSeconds(3);
            _timer.Tick += Tick;
            _timer.Start();

        }

        private void ShowNextImage(Image img, MediaElement video)
        {
            var item = _playlist[_imageNumber];
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
                            _player.label.Content = $"{video.Position:mm\\:ss} / {video.NaturalDuration.TimeSpan:mm\\:ss}";
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

        private void Tick(object sender, System.EventArgs e)
        {
            _imageNumber = (_imageNumber + 1);

            if (!(_imageNumber).Equals(_playlist.Count))
            {
                _timer.IsEnabled = true;
                ShowNextImage(_player.image, _player.video);
            }
            else
            {
                _timer.IsEnabled = false;
            }


        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            _player.video.Play();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            _player.video.Pause();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
             _player.video.Stop();
        }

    }
}