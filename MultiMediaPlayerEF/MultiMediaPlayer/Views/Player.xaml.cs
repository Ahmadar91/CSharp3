using MultiMediaPlayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MultiMediaPlayer.Views
{
    /// <summary>
    /// Interaction logic for Player.xaml
    /// </summary>
    public partial class Player : Window
    {
        public Player(ObservableCollection<PlayListViewModel> playlist)
        {
            InitializeComponent();
            DataContext = new PlayerViewModel(this, playlist);
        }
    }
}
