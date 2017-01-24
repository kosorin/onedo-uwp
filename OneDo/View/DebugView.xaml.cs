using System.Threading.Tasks;
using OneDo.ViewModel;
using OneDo.ViewModel.Parameters;

namespace OneDo.View
{
    public sealed partial class DebugView : ModalView, IXBind<DebugViewModel>
    {
        public DebugViewModel VM => (DebugViewModel)ViewModel;

        public DebugView()
        {
            InitializeComponent();
        }

        protected override async Task OnFirstLoad()
        {
            await VM.LoadLog();
        }
    }
}
