using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace OneDo.View
{
    public class ModalView : ExtendedUserControl
    {
        public virtual ModalContainer SubContainer => null;
    }
}