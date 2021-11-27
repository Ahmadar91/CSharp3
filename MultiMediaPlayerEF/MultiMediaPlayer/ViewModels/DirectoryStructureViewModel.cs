using BL.DbOperations;
using BL.Directory;
using BL.Models;
using MultiMediaPlayer.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MultiMediaPlayer.Mapper;
using Utilities.Common;

namespace MultiMediaPlayer.ViewModels
{
    public class DirectoryStructureViewModel : BaseViewModel
    {
        private readonly MainWindow _mainWindow;
        public ObservableCollection<DirectoryItemViewModel> Items { get; set; }
        public ObservableCollection<AlbumViewModel> Albums { set; get; }
        public MediaFileTypes MediaFileTypes { get; set; }
        public ICommand AddButtonCommand { get; set; }
        public ICommand OpenOptionsButton { get; set; }
        public ICommand DeleteButtonCommand { get; set; }
        public ICommand MoveUpCommand { get; set; }
        public ICommand MoveDownCommand { get; set; }
        public ICommand PlayButtonCommand { get; set; }
        public ICommand EditButtonCommand { get; set; }
        public ICommand AddPlayListButtonCommand { get; set; }
        public ICommand DeletePlayListButtonCommand { get; set; }
        public ICommand MoveUpPlayListCommand { get; set; }
        public ICommand MoveDownPlayListCommand { get; set; }
        public ICommand EditPlayListButtonCommand { get; set; }
        public IMapper Mapper { get; set; }
        public DbOperation DbOperation { get; set; }
        /// <summary>
        ///  constructor
        /// </summary>
        /// <param name="mainWindow"></param>
        public DirectoryStructureViewModel(MainWindow mainWindow)
        {
            DbOperation = new DbOperation();
            Mapper = new Mapper.Mapper();
            MediaFileTypes = new MediaFileTypes
            {
                JPG =
                {
                    IsChecked = false
                },
                PNG =
                {
                    IsChecked = false
                },
                MP4 =
                {
                    IsChecked = false
                },
                WAV =
                {
                    IsChecked = false
                }
            };
            var du = new DirectoryUtils();
            AddButtonCommand = new ViewUtils.RelayCommand(o => AddButtonClick());
            OpenOptionsButton = new ViewUtils.RelayCommand(o => OpenOptions());
            PlayButtonCommand = new ViewUtils.RelayCommand(o => PlayButtonClick());
            DeleteButtonCommand = new ViewUtils.RelayCommand(o => DeleteButtonClick());
            MoveUpCommand = new ViewUtils.RelayCommand(o => MoveUpButtonClick());
            MoveDownCommand = new ViewUtils.RelayCommand(o => MoveDownButtonClick());
            EditButtonCommand = new ViewUtils.RelayCommand(o => EditButtonClick());
            EditPlayListButtonCommand = new ViewUtils.RelayCommand(o => EditPlayListButtonClick());


            AddPlayListButtonCommand = new ViewUtils.RelayCommand(o => AddPlayListItem());
            DeletePlayListButtonCommand = new ViewUtils.RelayCommand(o => DeletePlayListItemButtonClick());
            MoveUpPlayListCommand = new ViewUtils.RelayCommand(o => MovePlayListItemUpButtonClick());
            MoveDownPlayListCommand = new ViewUtils.RelayCommand(o => MovePlayListItemDownButtonClick());
            _mainWindow = mainWindow;
            Items = new ObservableCollection<DirectoryItemViewModel>(du.GetLogicalDrives().Select(drive => new DirectoryItemViewModel(drive.FullPath, DirectoryItemType.Drive, du)));
            _mainWindow.TreeView.MouseDoubleClick += TreeView_MouseDoubleClick;
            _mainWindow.AlbumPlayList.MouseDoubleClick += AlbumPlayList_MouseDoubleClick;
            InitializeComponent();

        }

        private void InitializeComponent()
        {
            Albums = new ObservableCollection<AlbumViewModel>();
            _mainWindow.AlbumPlayList.DataContext = Albums;
            var albumCollection = DbOperation.AlbumDbOperation.GetAll();

            foreach (var album in albumCollection)
            {
                Albums.Add(Mapper.Map(album));
            }
        }



        /// <summary>
        /// Edit a playList item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlbumPlayList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditPlayListButtonClick();
        }
        /// <summary>
        /// Add a play list item
        /// </summary>
        private void AddPlayListItem()
        {
            try
            {
                var selectedAlbum = _mainWindow.Grid.SelectedValue as AlbumViewModel;
                if (selectedAlbum != null)
                {
                    var selectedItem = _mainWindow.TreeView.FolderView.SelectedValue as DirectoryItemViewModel;
                    if (selectedItem != null && selectedItem.Type.Equals(DirectoryItemType.File))
                    {
                        if (MediaFileTypes.PNG.IsChecked != null && MediaFileTypes.JPG.IsChecked != null && (MediaFileTypes.JPG.IsChecked.Value && selectedItem.FullPath.ToLowerInvariant().EndsWith(".jpg")
                                || MediaFileTypes.PNG.IsChecked.Value && selectedItem.FullPath.ToLowerInvariant().EndsWith(".png")) || MediaFileTypes.WAV.IsChecked != null && MediaFileTypes.MP4.IsChecked != null && (MediaFileTypes.MP4.IsChecked.Value && selectedItem.FullPath.ToLowerInvariant().EndsWith(".mp4")
                                || MediaFileTypes.WAV.IsChecked.Value && selectedItem.FullPath.ToLowerInvariant().EndsWith(".wav")))
                        {
                            var descriptionView = new DescriptionView();
                            var descDialog = descriptionView.ShowDialog();
                            if (descriptionView.DialogResult.HasValue && descriptionView.DialogResult.Value)
                            {
                                DbOperation.PlayListDbOperation.Add(Mapper.Map(new PlayListViewModel
                                {
                                    Description = descriptionView.DescriptionTextBox.Text,
                                    AlbumId = selectedAlbum.Id,
                                    FileName = selectedItem.FileName,
                                    FullPath = selectedItem.FullPath
                                }));
                                var albumToedit = DbOperation.AlbumDbOperation.GetById(selectedAlbum.Id);
                                albumToedit.Count++;
                                DbOperation.AlbumDbOperation.Edit(albumToedit);
                                InitializeComponent();
                            }
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
        /// <summary>
        /// Allow double click for the tree view to add an item to the Album
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AddPlayListItem();
        }
        /// <summary>
        ///  Edit a Album Description
        /// </summary>
        private void EditButtonClick()
        {
            var selectedAlbum = _mainWindow.Grid.SelectedValue as AlbumViewModel;
            if (selectedAlbum != null)
            {
                var descriptionView = new DescriptionView();
                descriptionView.DescriptionTextBox.Text = selectedAlbum.Description;
                var desDialog = descriptionView.ShowDialog();
                if (descriptionView.DialogResult.HasValue && descriptionView.DialogResult.Value)
                {
                    var albumToEdit = DbOperation.AlbumDbOperation.GetById(selectedAlbum.Id);
                    albumToEdit.Description = descriptionView.DescriptionTextBox.Text;
                    DbOperation.AlbumDbOperation.Edit(albumToEdit);
                    InitializeComponent();
                }
            }

        }
        /// <summary>
        /// Edit a playlist item Description
        /// </summary>
        private void EditPlayListButtonClick()
        {
            var selectedAlbum = _mainWindow.Grid.SelectedValue as AlbumViewModel;
            if (selectedAlbum != null)
            {
                var item = _mainWindow.AlbumPlayList.SelectedValue as PlayListViewModel;
                if (item != null)
                {
                    var descriptionView = new DescriptionView
                    {
                        DescriptionTextBox =
                        {
                            Text = item.Description
                        }
                    };
                    var desDialog = descriptionView.ShowDialog();
                    if (descriptionView.DialogResult.HasValue && descriptionView.DialogResult.Value)
                    {
                        var itemToEdit = DbOperation.PlayListDbOperation.GetById(item.Id);
                        itemToEdit.Description = descriptionView.DescriptionTextBox.Text;
                        DbOperation.PlayListDbOperation.Edit(itemToEdit);
                        InitializeComponent();
                    }
                }

            }

        }
        /// <summary>
        /// Add a new album
        /// </summary>
        private void AddButtonClick()
        {
            try
            {
                var descriptionView = new DescriptionView();
                var decDialog = descriptionView.ShowDialog();
                if (descriptionView.DialogResult.HasValue && descriptionView.DialogResult.Value)
                {
                    DbOperation.AlbumDbOperation.Add(Mapper.Map(new AlbumViewModel()
                    {
                        Description = descriptionView.DescriptionTextBox.Text,
                        PlayList = new ObservableCollection<PlayListViewModel>(),
                    }));

                    var AlbumCollection = DbOperation.AlbumDbOperation.GetAll();
                    Albums = new ObservableCollection<AlbumViewModel>();
                    _mainWindow.AlbumPlayList.DataContext = Albums;

                    foreach (AlbumDto album in AlbumCollection)
                    {
                        Albums.Add(Mapper.Map(album));
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error {e.Message}");
            }

        }
        /// <summary>
        /// Delete an album
        /// </summary>
        private void DeleteButtonClick()
        {
            try
            {
                if (_mainWindow.Grid.SelectedValue is AlbumViewModel selectedItem)
                {
                    DbOperation.AlbumDbOperation.Delete(selectedItem.Id);
                }
                InitializeComponent();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error {e.Message}");
            }

        }
        /// <summary>
        ///Move an Album up
        /// </summary>
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
        /// <summary>
        ///Move an Album down
        /// </summary>
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

        }/// <summary>
         /// Play the Album
         /// </summary>
        private void PlayButtonClick()
        {
            var index = _mainWindow.Grid.SelectedIndex;
            if (index >= 0)
            {
                if (Albums[_mainWindow.Grid.SelectedIndex].PlayList.Count <= 0)
                {
                    MessageBox.Show(" playlist is empty please insert Items in to the playlist");
                    return;
                }
                var player = new Player(Albums[_mainWindow.Grid.SelectedIndex].PlayList);
                player.ShowDialog();
            }
            else
            {
                MessageBox.Show("Select an Album to play");
            }

        }
        /// <summary>
        ///Delete a play list item
        /// </summary>
        private void DeletePlayListItemButtonClick()
        {
            try
            {
                var selectedAlbum = _mainWindow.Grid.SelectedValue as AlbumViewModel;
                if (selectedAlbum != null)
                {
                    var item = _mainWindow.AlbumPlayList.SelectedValue as PlayListViewModel;
                    if (item != null)
                    {
                        DbOperation.PlayListDbOperation.Delete(item.Id);
                        var alb = DbOperation.AlbumDbOperation.GetById(selectedAlbum.Id);
                        alb.Count--;
                        DbOperation.AlbumDbOperation.Edit(alb);
                        InitializeComponent();
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
        /// <summary>
        ///Move a play list item up
        /// </summary>
        private void MovePlayListItemDownButtonClick()
        {
            try
            {
                var selectedAlbum = _mainWindow.Grid.SelectedIndex;
                if (selectedAlbum >= 0)
                {
                    var selectedIndex = _mainWindow.AlbumPlayList.SelectedIndex;
                    if (selectedIndex + 1 < Albums[selectedAlbum].PlayList.Count)
                    {
                        var itemToMoveDown = Albums[selectedAlbum].PlayList[selectedIndex];
                        Albums[selectedAlbum].PlayList.RemoveAt(selectedIndex);
                        Albums[selectedAlbum].PlayList.Insert(selectedIndex + 1, itemToMoveDown);
                        _mainWindow.AlbumPlayList.SelectedIndex = selectedIndex + 1;
                    }
                }
                else
                    MessageBox.Show("No album selected");

            }
            catch (Exception e)
            {
                MessageBox.Show($"Error {e.Message}");
            }
        }
        /// <summary>
        ///Move a play list item down
        /// </summary>
        private void MovePlayListItemUpButtonClick()
        {
            try
            {
                var selectedAlbum = _mainWindow.Grid.SelectedIndex;
                if (selectedAlbum >= 0)
                {
                    var selectedIndex = _mainWindow.AlbumPlayList.SelectedIndex;
                    if (selectedIndex > 0)
                    {
                        var itemToMoveUp = Albums[selectedAlbum].PlayList[selectedIndex];
                        Albums[selectedAlbum].PlayList.RemoveAt(selectedIndex);
                        Albums[selectedAlbum].PlayList.Insert(selectedIndex - 1, itemToMoveUp);
                        _mainWindow.AlbumPlayList.SelectedIndex = selectedIndex - 1;
                    }
                }
                else
                    MessageBox.Show("No album selected");

            }
            catch (Exception e)
            {
                MessageBox.Show($"Error {e.Message}");
            }
        }

        /// <summary>
        ///Open Media Type settings
        /// </summary>
        private void OpenOptions()
        {
            MediaFileTypes.Show();
        }
    }

    /// <summary>
    /// View model for Albums
    /// </summary>
    public class AlbumViewModel : BaseViewModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public ObservableCollection<PlayListViewModel> PlayList { set; get; }
        public int Count { get; set; }
    }

    public class PlayListViewModel
    {
        public Guid Id { get; set; }
        public string FullPath { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public Guid AlbumId { get; set; }
        public BitmapImage LoadedImage { get; set; }
    }
}