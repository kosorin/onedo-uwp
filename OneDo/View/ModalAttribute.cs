using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.View
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ModalAttribute : Attribute
    {
        public ModalAttribute(Type viewModelType)
        {
            ViewModelType = viewModelType;
        }

        public Type ViewModelType { get; }
    }
}
