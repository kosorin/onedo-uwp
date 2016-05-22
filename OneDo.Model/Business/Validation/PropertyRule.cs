using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace OneDo.Model.Business.Validation
{
    public class PropertyRule<T> : IPropertyRule<T>
    {
        private readonly Func<T, bool> test;

        public PropertyRule(Func<T, bool> test)
        {
            this.test = test;
        }

        public bool Test(T value)
        {
            return test.Invoke(value);
        }

        bool IPropertyRule.Test(object value)
        {
            return test.Invoke((T)value);
        }
    }
}