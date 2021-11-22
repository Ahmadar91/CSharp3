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
                        selectedItem.LoadedImage = new BitmapImage(new Uri(selectedItem.FullPath));
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
            var player = new Player(PlayList);
            player.ShowDialog();
        }

     
        private void OpenOptions()
        {
            MediaFileTypes.Show();
        }

    }
}