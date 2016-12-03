using OneDo.ViewModel;

namespace OneDo.View
{
    public sealed partial class ConfirmationView : ModalBase, IXBind<ConfirmationViewModel>
    {
        public ConfirmationViewModel VM => ViewModel as ConfirmationViewModel;

        public ConfirmationView()
        {
            InitializeComponent();
        }
    }
}
