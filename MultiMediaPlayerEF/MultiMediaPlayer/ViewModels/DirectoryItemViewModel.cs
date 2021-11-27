using System;
using BL.Directory;
using BL.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using BL.Interfaces;
using Utilities.Common;


namespace MultiMediaPlayer.ViewModels
{
    /// <summary>
    /// A view model for each directory item
    /// </summary>
    public class DirectoryItemViewModel : BaseViewModel
    {
        private readonly IDirectoryUtils _du;
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="fullPath">The full path of this item</param>
        /// <param name="type">The type of item</param>
        /// <param name="du">DirectoryUtils</param>
        public DirectoryItemViewModel(string fullPath, DirectoryItemType type, IDirectoryUtils du)
        {
            _du = du;
            ExpandCommand = new RelayCommand(Expand);
            FullPath = fullPath;
            Type = type;
            ClearChildren();
        }
        /// <summary>
        /// The type of this item
        /// </summary>
        public DirectoryItemType Type { get; set; }

        public string ImageName => Type == DirectoryItemType.Drive ? "drive" : (Type == DirectoryItemType.File ?
            "file" : (IsExpanded ? "folder-open" : "folder-closed"));

        /// <summary>
        /// The full path to the item
        /// </summary>
        public string FullPath { get; set; }
        /// <summary>
        /// The name of this directory item
        /// </summary>
        public string FileName => Type == DirectoryItemType.Drive ? FullPath : DirectoryExtensions.GetFileFolderName(FullPath);


        /// <summary>
        /// A list of all children contained inside this item
        /// </summary>
        public ObservableCollection<DirectoryItemViewModel> Children { get; set; }

        /// <summary>
        /// Indicates if this item can be expanded
        /// </summary>
        public bool CanExpand => Type != DirectoryItemType.File;

        /// <summary>
        /// Indicates if the current item is expanded or not
        /// </summary>
        public bool IsExpanded
        {
            get => Children?.Count(f => f != null) > 0;
            set
            {
                if (value)
                    Expand();
                else
                    ClearChildren();
            }
        }

        /// <summary>
        /// The command to expand this item
        /// </summary>
        public ICommand ExpandCommand { get; set; }


        /// <summary>
        /// Removes all children from the list, adding a dummy item to show the expand icon if required
        /// </summary>
        private void ClearChildren()
        {
            Children = new ObservableCollection<DirectoryItemViewModel>();
            if (Type != DirectoryItemType.File)
                Children.Add(null);
        }
        /// <summary>
        ///  Expands this directory and finds all children
        /// </summary>
        private void Expand()
        {
            if (Type == DirectoryItemType.File)
                return;
            var children = _du.GetDirectoryContent(FullPath);
            Children = new ObservableCollection<DirectoryItemViewModel>(
                                children.Select(content => new DirectoryItemViewModel(content.FullPath, content.Type, _du)));
        }
    }
}

