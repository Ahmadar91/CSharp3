using MultiMediaPlayer.Views;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MultiMediaPlayer.ViewModels
{
    public class PlayerViewModel
    {
        private readonly Player _player;
        private readonly ObservableCollection<PlayListViewModel> _playlist;
        private readonly DispatcherTimer _timer = new();

        private int _itemNumber = 0;
        public PlayerViewModel(Player player, ObservableCollection<PlayListViewModel> playlist)
        {
            _player = player;
            _playlist = playlist;
            _player.video.Volume = 50;
            BeginSlideShow();
        }
        /// <summary>
        /// Begins the slide show
        /// </summary>
        private void BeginSlideShow()
        {
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
        /// <summary>
        /// Timer tick to update each item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tick(object sender, System.EventArgs e)
        {
            _itemNumber++;

            if (!(_itemNumber).Equals(_playlist.Count))
            {
                _timer.IsEnabled = true;
                ShowNextItem(_player.image, _player.video);
            }
            else
            {
                _timer.IsEnabled = false;
            }
        }
        /// <summary>
        /// Sets the next item to play in the play list
        /// </summary>
        /// <param name="img"></param>
        /// <param name="video"></param>
        private void ShowNextItem(Image img, MediaElement video)
        {
            var item = _playlist[_itemNumber];
            Storyboard sb = new Storyboard();
            if (item.FullPath.EndsWith("jpg", StringComparison.CurrentCultureIgnoreCase) || item.FullPath.EndsWith("png", StringComparison.InvariantCultureIgnoreCase))
            {
                if (video.NaturalDuration.HasTimeSpan)
                {
                    if (!video.Position.Equals(video.NaturalDuration.TimeSpan))
                    {
                        _itemNumber--;
                        return;
                    }
                }
                video.Source = null;
                img.Visibility = Visibility.Visible;
                const double transitionTime = 0.9;
                var fade_out = new DoubleAnimation(1.0, 0.0,
                    TimeSpan.FromSeconds(transitionTime));
                fade_out.BeginTime = TimeSpan.FromSeconds(0);

                Storyboard.SetTarget(fade_out, img);
                Storyboard.SetTargetProperty(fade_out,
                    new PropertyPath(Image.OpacityProperty));
                sb.Children.Add(fade_out);
                var new_image_animation =
                    new ObjectAnimationUsingKeyFrames();
                new_image_animation.BeginTime = TimeSpan.FromSeconds(transitionTime);
                var new_image_frame =
                    new DiscreteObjectKeyFrame(new BitmapImage(new Uri(item.FullPath)), TimeSpan.Zero);
                new_image_animation.KeyFrames.Add(new_image_frame);
                Storyboard.SetTarget(new_image_animation, img);
                Storyboard.SetTargetProperty(new_image_animation,
                    new PropertyPath(Image.SourceProperty));
                sb.Children.Add(new_image_animation);
                var fade_in = new DoubleAnimation(0.0, 1.0,
                    TimeSpan.FromSeconds(transitionTime));
                fade_in.BeginTime = TimeSpan.FromSeconds(transitionTime);

                Storyboard.SetTarget(fade_in, img);
                Storyboard.SetTargetProperty(fade_in,
                    new PropertyPath(Image.OpacityProperty));

                sb.Children.Add(fade_in);
                sb.Begin(img);
            }

            if (item.FullPath.EndsWith("mp4", StringComparison.CurrentCultureIgnoreCase) || item.FullPath.EndsWith("wav", StringComparison.InvariantCultureIgnoreCase))
            {
                img.Source = null;
                img.Visibility = Visibility.Hidden;

                if (video.Source != null)
                {
                    while (true)
                    {
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

    }
}