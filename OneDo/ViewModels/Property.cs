﻿using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModels
{
    public class Property<T> : ObservableObject, IProperty
    {
        public ObservableCollection<string> Errors { get; } = new ObservableCollection<string>();

        public bool IsValid => !Errors.Any();

        public bool IsDirty
        {
            get
            {
                if (Value == null && Original == null)
                {
                    return false;
                }
                else if (Value == null && Original != null)
                {
                    return true;
                }
                else
                {
                    return !Value.Equals(Original);
                }
            }
        }

        private T original;
        public T Original
        {
            get { return original; }
            set
            {
                valueHasBeenSet = true;
                Set(ref original, value);
            }
        }

        private T value;
        public T Value
        {
            get { return value; }
            set
            {
                if (!valueHasBeenSet)
                {
                    Original = value;
                }
                Set(ref this.value, value);
                RaisePropertyChanged(nameof(IsDirty));

                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }


        public event EventHandler ValueChanged;


        private bool valueHasBeenSet = false;

        public Property()
        {
            // TODO: train the serializer
            Errors.CollectionChanged += (s, e) => RaisePropertyChanged(nameof(IsValid));
        }


        public void Revert()
        {
            Value = Original;
        }
    }
}
