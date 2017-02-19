using OneDo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Domain.Common
{
    public abstract class ValueObject<T> : IEquatable<T> where T : ValueObject<T>
    {
        public static T Load(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }
            return Serialization.Deserialize<T>(json);
        }

        public string Save()
        {
            return Serialization.Serialize(this);
        }


        public bool Equals(T other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            else
            {
                return EqualsCore(other);
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as T);
        }

        public override int GetHashCode()
        {
            return GetHashCodeCore();
        }

        public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            {
                return true;
            }
            else if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            {
                return false;
            }
            else
            {
                return a.Equals(b);
            }
        }

        public static bool operator !=(ValueObject<T> a, ValueObject<T> b)
        {
            return !(a == b);
        }

        protected abstract bool EqualsCore(T other);

        protected abstract int GetHashCodeCore();
    }
}
