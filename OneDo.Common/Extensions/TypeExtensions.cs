using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Common.Extensions
{
    public static class TypeExtensions
    {
        public static bool ImplementsGenericInterface(this Type type, Type genericInterfaceType)
        {
            return type
                .GetInterfaces()
                .Where(i => i.GetTypeInfo().IsGenericType)
                .Any(i => i.GetGenericTypeDefinition() == genericInterfaceType);
        }
    }
}
