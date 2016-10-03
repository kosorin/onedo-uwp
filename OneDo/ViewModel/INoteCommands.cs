﻿using GalaSoft.MvvmLight.Command;
using OneDo.Common.UI;
using OneDo.ViewModel.Commands;
using OneDo.ViewModel.Items;
using System.ComponentModel;
using System.Windows.Input;

namespace OneDo.ViewModel
{
    public interface INoteCommands : INotifyPropertyChanged
    {
        IExtendedCommand AddCommand { get; }

        IExtendedCommand EditCommand { get; }

        IExtendedCommand DeleteCommand { get; }

        IExtendedCommand MoveCommand { get; }
    }
}