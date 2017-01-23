using OneDo.ViewModel;

namespace OneDo.View
{
    public sealed partial class DebugView : ModalView, IXBind<DebugViewModel>
    {
        public DebugViewModel VM => (DebugViewModel)ViewModel;

        public DebugView()
        {
            InitializeComponent();
        }
    }
}
