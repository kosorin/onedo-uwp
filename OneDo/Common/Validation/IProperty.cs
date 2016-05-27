using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Common.Validation
{
    public interface IProperty : INotifyPropertyChanged
    {
        bool IsValid { get; }

        bool IsDirty { get; }

        bool IsOriginalSet { get; }

        void Revert();

        void MarkAsClean();
    }

    public interface IProperty<T> : IProperty
    {
        T OriginalValue { get; set; }

        T Value { get; set; }

        void Validate(Func<T, bool> validator);
    }
}