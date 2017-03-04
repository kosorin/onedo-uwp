using OneDo.Common.Mvvm;
using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace OneDo.View
{
    public class ModalView : ExtendedUserControl
    {
        public virtual ModalContainer SubContainer => null;

        public ModalView()
        {
            TabNavigation = KeyboardNavigationMode.Cycle;
        }
    }
}