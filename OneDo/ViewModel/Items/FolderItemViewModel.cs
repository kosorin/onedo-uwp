﻿using GalaSoft.MvvmLight;
using OneDo.Common.Media;
using OneDo.Model.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace OneDo.ViewModel.Items
{
    public class FolderItemViewModel : ExtendedViewModel
    {
        public string Name => Folder.Name;

        public Color Color => Common.Media.ColorHelper.FromHex(Folder.Color);

        public bool HasParent => Folder.ParentId != null;

        public int? ParentId => Folder.ParentId;


        public Folder Folder { get; }

        public FolderItemViewModel(Folder folder)
        {
            Folder = folder;
        }

        public void Refresh()
        {
            RaisePropertyChanged(nameof(Name));
            RaisePropertyChanged(nameof(Color));
        }
    }
}