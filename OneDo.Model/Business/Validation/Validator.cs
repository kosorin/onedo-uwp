using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace OneDo.Model.Business.Validation
{
    public abstract class Validator<T> where T : class
    {
        public class Rule
        {
            public Func<T, bool> Test { get; }

            [Obsolete("Vymyslet, jak zpracovávat validační zprávy. Myslet dopředu i na lokalizaci.")]
            public string Message { get; }

            public Rule(Func<T, bool> test, string message = null)
            {
                Test = test;
                Message = message;
            }
        }

        protected abstract IDictionary<string, Rule> Rules { get; }

        protected virtual bool IsValidObject(T obj)
        {
            return true;
        }

        public bool IsValid(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return Rules.Values.All(r => r.Test(obj)) && IsValidObject(obj);
        }

        public bool IsValidProperty(T obj, string propertyName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            Rule rule;
            if (Rules.TryGetValue(propertyName, out rule))
            {
                return rule.Test(obj);
            }
            else
            {
                return true;
            }
        }
    }
}
