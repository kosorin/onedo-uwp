using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace OneDo.Model.Business.Validation
{
    public abstract class Validator<T> where T : class
    {
        protected abstract List<Rule<T>> Rules { get; }

        public bool IsValid(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return Rules.All(r => r.Test(obj));
        }
    }
}
