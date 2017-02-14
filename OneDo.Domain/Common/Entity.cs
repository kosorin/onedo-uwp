using OneDo.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Domain.Common
{
    public abstract class Entity : IEquatable<Entity>
    {
        public Guid Id { get; protected set; }

        public Entity(Guid id)
        {
            Id = id;
        }

        public bool IsTransient()
        {
            return IsTransient(Id);
        }

        public static bool IsTransient(Guid id)
        {
            return id == null || id == Guid.Empty;
        }

        public static Guid GetTransientId()
        {
            return Guid.Empty;
        }

        public bool Equals(Entity other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            else if (ReferenceEquals(this, other))
            {
                return true;
            }
            else if (!IsTransient() && !other.IsTransient() && Id == other.Id)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Entity);
        }

        public override int GetHashCode()
        {
            return this.GetHashCodeFromFields(Id);
        }

        public static bool operator ==(Entity a, Entity b)
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

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }
    }
}
