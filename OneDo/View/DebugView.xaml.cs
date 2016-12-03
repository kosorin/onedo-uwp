using OneDo.ViewModel;

namespace OneDo.View
{
    public sealed partial class DebugView : ModalBase, IXBind<DebugViewModel>
    {
        public DebugViewModel VM => (DebugViewModel)ViewModel;

        public DebugView()
        {
            InitializeComponent();
        }
    }
}
