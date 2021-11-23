using BL.Directory;
using BL.Models;
using MultiMediaPlayer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
        public ObservableCollection<Album> Albums { set; get; }
        public MediaFileTypes MediaFileTypes { get; set; }
        public Description DescriptionView { get; set; }
        public ICommand AddButtonCommand { get; set; }
        public ICommand OpenOptionsButton { get; set; }
        public ICommand DeleteButtonCommand { get; set; }
        public ICommand MoveUpCommand { get; set; }
        public ICommand MoveDownCommand { get; set; }
        public ICommand PlayButtonCommand { get; set; }
        public ICommand PauseButtonCommand { get; set; }
        public ICommand EditButtonCommand { get; set; }

        public ICommand AddPlayListButtonCommand { get; set; }
        public ICommand DeletePlayListButtonCommand { get; set; }
        public ICommand MoveUpPlayListCommand { get; set; }
        public ICommand MoveDownPlayListCommand { get; set; }
        public int SelectedInt => _mainWindow.Grid.SelectedIndex;
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
            EditButtonCommand = new ViewUtils.RelayCommand(o => EditButtonClick());


            AddPlayListButtonCommand = new ViewUtils.RelayCommand(o => AddPlayListItem());
            DeletePlayListButtonCommand = new ViewUtils.RelayCommand(o => DeletePlayListItemButtonClick());
            MoveUpPlayListCommand = new ViewUtils.RelayCommand(o => MovePlayListItemUpButtonClick());
            MoveDownPlayListCommand = new ViewUtils.RelayCommand(o => MovePlayListItemDownButtonClick());

            //PlayList = new ObservableCollection<DirectoryItemViewModel>();
            _mainWindow = mainWindow;
            Items = new ObservableCollection<DirectoryItemViewModel>(du.GetLogicalDirves()
                .Select(drive => new DirectoryItemViewModel(drive.FullPath, DirectoryItemType.Drive, du)));
            _mainWindow.TreeView.MouseDoubleClick += TreeView_MouseDoubleClick;
        }



        private void AddPlayListItem()
        {
            try
            {
                var selectedInded = _mainWindow.Grid.SelectedIndex;
                if (selectedInded >= 0)
                {
                    var selectedItem = _mainWindow.TreeView.FolderView.SelectedValue as DirectoryItemViewModel;
                    if (selectedItem != null && selectedItem.Type.Equals(DirectoryItemType.File))
                    {
                        if (MediaFileTypes.PNG.IsChecked != null && MediaFileTypes.JPG.IsChecked != null && (MediaFileTypes.JPG.IsChecked.Value && selectedItem.FullPath.ToLowerInvariant().EndsWith(".jpg")
                                || MediaFileTypes.PNG.IsChecked.Value && selectedItem.FullPath.ToLowerInvariant().EndsWith(".png")))
                        {
                            selectedItem.LoadedImage = new BitmapImage(new Uri(selectedItem.FullPath));
                            Albums[selectedInded].PlayList.Add(selectedItem);
                        }
                        else if (MediaFileTypes.WAV.IsChecked != null && MediaFileTypes.MP4.IsChecked != null && (MediaFileTypes.MP4.IsChecked.Value && selectedItem.FullPath.ToLowerInvariant().EndsWith(".mp4")
                                     || MediaFileTypes.WAV.IsChecked.Value && selectedItem.FullPath.ToLowerInvariant().EndsWith(".wav")))
                        {
                            selectedItem.LoadedImage = GetThumbnail(selectedItem.FullPath, 500, 500);
                             Albums[selectedInded].PlayList.Add(selectedItem);
                        }
                        else
                            MessageBox.Show($"please choose the media types from the settings");
                    }
                }
                else
                {
                    MessageBox.Show($"please select an album to add to");
                }
               
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error {e.Message}");
            }
        }

        private void TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AddPlayListItem();
        }

        private void EditButtonClick()
        {
            var selectedIndex = _mainWindow.Grid.SelectedIndex;
            if (selectedIndex>=0)
            {
                var descriptionView = new Description();
                descriptionView.DescriptionTextBox.Text = Albums[selectedIndex].Description ;
                var test = descriptionView.ShowDialog(); 
                Albums[selectedIndex].Description = descriptionView.DescriptionTextBox.Text;
            }

        }
        private void AddButtonClick()
        {
            try
            {
                if (Albums== null)
                {
                    Albums = new ObservableCollection<Album>();
                    _mainWindow.TvBox.DataContext = Albums;
                }
                var album = new Album();
                album.PlayList = new ObservableCollection<DirectoryItemViewModel>();
                var descriptionView = new Description();
                var test = descriptionView.ShowDialog();
                album.Description = descriptionView.DescriptionTextBox.Text;
                Albums.Add(album);
                
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
                if (selectedItem >= 0)
                {
                    Albums.RemoveAt(selectedItem);
                }
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
                    var itemToMoveUp = Albums[selectedIndex];
                    Albums.RemoveAt(selectedIndex);
                    Albums.Insert(selectedIndex - 1, itemToMoveUp);
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
                if (selectedIndex + 1 < Albums.Count)
                {
                    var itemToMoveDown = Albums[selectedIndex];
                    Albums.RemoveAt(selectedIndex);
                    Albums.Insert(selectedIndex + 1, itemToMoveDown);
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
            var index = _mainWindow.Grid.SelectedIndex;
            if (index>= 0)
            {
                var player = new Player(Albums[_mainWindow.Grid.SelectedIndex].PlayList);
                player.ShowDialog();
            }
            else
            {
                MessageBox.Show("Select an Album to play");
            }
          
        }

        private void DeletePlayListItemButtonClick()
        {
            try
            {
                var selectedAlbum = _mainWindow.Grid.SelectedIndex;
                if (selectedAlbum >= 0)
                {
                    var index = _mainWindow.TvBox.SelectedIndex;
                    if (index >= 0)
                    {
                        Albums[selectedAlbum].PlayList.RemoveAt(index);
                    }
                }
                else
                {
                    MessageBox.Show("No ablum selected");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error {e.Message}");
            }
        }

        private void MovePlayListItemDownButtonClick()
        {
            try
            {
                var selectedAlbum = _mainWindow.Grid.SelectedIndex;
                if (selectedAlbum >= 0)
                {
                    var selectedIndex = _mainWindow.TvBox.SelectedIndex;
                    if (selectedIndex + 1 < Albums[selectedAlbum].PlayList.Count)
                    {
                        var itemToMoveDown = Albums[selectedAlbum].PlayList[selectedIndex];
                        Albums[selectedAlbum].PlayList.RemoveAt(selectedIndex);
                        Albums[selectedAlbum].PlayList.Insert(selectedIndex + 1, itemToMoveDown);
                        _mainWindow.TvBox.SelectedIndex = selectedIndex + 1;
                    }
                }
                else
                {
                    MessageBox.Show("No album selected");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error {e.Message}");
            }
        }

        private void MovePlayListItemUpButtonClick()
        {
            try
            {
                var selectedAlbum = _mainWindow.Grid.SelectedIndex;
                if (selectedAlbum >= 0)
                {
                    var selectedIndex = _mainWindow.TvBox.SelectedIndex;
                    if (selectedIndex > 0)
                    {
                        var itemToMoveUp = Albums[selectedAlbum].PlayList[selectedIndex];
                        Albums[selectedAlbum].PlayList.RemoveAt(selectedIndex);
                        Albums[selectedAlbum].PlayList.Insert(selectedIndex - 1, itemToMoveUp);
                        _mainWindow.TvBox.SelectedIndex = selectedIndex - 1;
                    }
                }
                else
                {
                    MessageBox.Show("No album selected");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error {e.Message}");
            }
        }


        private void OpenOptions()
        {
            MediaFileTypes.Show();
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

       private BitmapImage GetThumbnail(string mediaFile, int waitTime, int position)
        {
            MediaPlayer player = new MediaPlayer { Volume = 0, ScrubbingEnabled = true };
            player.Open(new Uri(mediaFile));
            player.Pause();
            player.Position = TimeSpan.FromMilliseconds(position);
            //We need to give MediaPlayer some time to load. 
            //The efficiency of the MediaPlayer depends                 
            //upon the capabilities of the machine it is running on and 
            //would be different from time to time
            System.Threading.Thread.Sleep(waitTime);

            //120 = thumbnail width, 90 = thumbnail height and 96x96 = horizontal x vertical DPI
            //In an real application, you would not probably use hard coded values!
            RenderTargetBitmap rtb = new RenderTargetBitmap(120, 90, 96, 96, PixelFormats.Pbgra32);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                dc.DrawVideo(player, new Rect(0, 0, 120, 90));
            }
            rtb.Render(dv);
            Duration duration = player.NaturalDuration;
            int videoLength = 0;
            if (duration.HasTimeSpan)
            {
                videoLength = (int)duration.TimeSpan.TotalSeconds;
            }
            BitmapFrame frame = BitmapFrame.Create(rtb).GetCurrentValueAsFrozen() as BitmapFrame;
            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(frame as BitmapFrame);
            MemoryStream memoryStream = new MemoryStream();
            encoder.Save(memoryStream);
            memoryStream.GetBuffer();
            //Here we have the thumbnail in the MemoryStream!
            player.Close();
            var Image = new BitmapImage();
            using (var stream = new MemoryStream(memoryStream.GetBuffer()))
            {
                Image.BeginInit();
                Image.CacheOption = BitmapCacheOption.OnLoad;
                Image.StreamSource = stream;
                Image.EndInit();
            }

            Image.Freeze();
            return Image;
        }
    }


    public class Album : BaseViewModel
    {
        public string Description { get; set; }
        public ObservableCollection<DirectoryItemViewModel> PlayList { set; get; }
    }
}