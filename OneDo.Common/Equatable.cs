﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Common
{
    public abstract class Equatable<T> : IEquatable<T> where T : Equatable<T>
    {
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

        public static bool operator ==(Equatable<T> a, Equatable<T> b)
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

        public static bool operator !=(Equatable<T> a, Equatable<T> b)
        {
            return !(a == b);
        }

        protected abstract bool EqualsCore(T other);

        protected abstract int GetHashCodeCore();
    }
}
