using System.Threading.Tasks;
using OneDo.ViewModel;

namespace OneDo.View
{
    public sealed partial class LogView : ModalView, IXBind<LogViewModel>
    {
        public LogViewModel VM => (LogViewModel)ViewModel;

        public LogView()
        {
            InitializeComponent();
        }

        protected override async Task OnFirstLoad()
        {
            await VM.Load();
        }
    }
}
