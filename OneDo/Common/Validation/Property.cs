using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Common.Validation
{
    public class Property<T> : ObservableObject, IProperty<T>
    {
        public bool IsValid { get; private set; }

        public bool IsDirty
        {
            get
            {
#pragma warning disable RECS0017 // Possible compare of value type with 'null'
                if (Value == null)
                {
                    return OriginalValue != null;
                }
                else
                {
                    return !Value.Equals(OriginalValue);
                }
#pragma warning restore RECS0017 // Possible compare of value type with 'null'
            }
        }


        private T value;
        public T Value
        {
            get { return value; }
            set
            {
                if (!IsOriginalSet)
                {
                    OriginalValue = value;
                }
                if (Set(ref this.value, value))
                {
                    RaisePropertyChanged(nameof(IsDirty));
                }
            }
        }

        private bool isOriginalSet;
        public bool IsOriginalSet
        {
            get { return isOriginalSet; }
            private set
            {
                if (Set(ref isOriginalSet, value))
                {
                    RaisePropertyChanged(nameof(IsDirty));
                }
            }
        }

        private T originalValue;
        public T OriginalValue
        {
            get { return originalValue; }
            set
            {
                IsOriginalSet = true;
                if (Set(ref originalValue, value))
                {
                    RaisePropertyChanged(nameof(IsDirty));
                }
            }
        }


        public Property()
        {
            // TODO: train the serializer
        }


        public void Revert()
        {
            Value = OriginalValue;
        }

        public void MarkAsClean()
        {
            OriginalValue = Value;
        }

        public void Validate(Func<T, bool> validator)
        {
            IsValid = validator?.Invoke(Value) ?? true;
        }
    }
}
