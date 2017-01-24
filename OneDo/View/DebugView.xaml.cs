using OneDo.ViewModel;
using OneDo.ViewModel.Parameters;

namespace OneDo.View
{
    public sealed partial class DebugView : ModalView, IXBind<DebugViewModel>
    {
        public DebugViewModel VM => (DebugViewModel)ViewModel;

        public DebugView(DebugParameters parameters) : base(parameters)
        {
            InitializeComponent();
        }
    }
}
