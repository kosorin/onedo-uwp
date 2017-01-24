using OneDo.ViewModel;

namespace OneDo.View
{
    public sealed partial class ConfirmationView : ModalView, IXBind<ConfirmationViewModel>
    {
        public ConfirmationViewModel VM => ViewModel as ConfirmationViewModel;

        public ConfirmationView()
        {
            InitializeComponent();
        }
    }
}
