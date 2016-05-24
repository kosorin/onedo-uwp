using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModels
{
    public interface IProperty : INotifyPropertyChanged
    {
        ObservableCollection<string> Errors { get; }

        bool IsValid { get; }

        bool IsDirty { get; }

        event EventHandler ValueChanged;

        void Revert();
    }
}