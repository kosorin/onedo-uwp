using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace OneDo.Model.Business.Validation
{
    public abstract class Validator<T> where T : class
    {
        private static readonly Dictionary<string, PropertyInfo> cachedProperties = new Dictionary<string, PropertyInfo>();


        protected abstract List<Rule<T>> Rules { get; }

        protected abstract Dictionary<string, IPropertyRule> PropertyRules { get; }


        public bool IsValid(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return Rules.All(r => r.Test(obj)) && PropertyRules.All(r => IsValid(obj, r.Key));
        }

        public bool IsPropertyValid<TProperty>(TProperty value, string propertyName)
        {
            IPropertyRule rule;
            if (PropertyRules.TryGetValue(propertyName, out rule))
            {
                return rule.Test(value);
            }
            else
            {
                return true;
            }
        }


        private bool IsValid(T obj, string propertyName)
        {
            IPropertyRule rule;
            if (PropertyRules.TryGetValue(propertyName, out rule))
            {
                PropertyInfo property;
                if (!cachedProperties.TryGetValue(propertyName, out property))
                {
                    property = obj
                        .GetType()
                        .GetProperties()
                        .Single(p => p.Name == propertyName);
                    cachedProperties[propertyName] = property;
                }
                return rule.Test(property.GetValue(obj));
            }
            else
            {
                return true;
            }
        }
    }
}
