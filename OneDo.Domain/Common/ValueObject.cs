using OneDo.Common;
using OneDo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Domain.Common
{
    public abstract class ValueObject<T> : Equatable<T> where T : ValueObject<T>
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
    }
}
