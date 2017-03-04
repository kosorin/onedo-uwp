using Windows.UI.Xaml.Input;

namespace OneDo.View.Core
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