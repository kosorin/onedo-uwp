using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace OneDo.Model.Business.Validation
{
    public class Rule<T> where T : class
    {
        private readonly Func<T, bool> test;

        public Rule(Func<T, bool> test)
        {
            this.test = test;
        }

        public bool Test(T value)
        {
            return test.Invoke(value);
        }
    }
}