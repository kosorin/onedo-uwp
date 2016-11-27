﻿using GalaSoft.MvvmLight.Command;
using OneDo.Common.UI;
using OneDo.ViewModel.Commands;
using System.ComponentModel;
using System.Windows.Input;

namespace OneDo.ViewModel
{
    public interface INoteCommands : INotifyPropertyChanged
    {
        IExtendedCommand ShowEditorCommand { get; }

        IExtendedCommand DeleteCommand { get; }

        IExtendedCommand ToggleFlagCommand { get; }
    }
}