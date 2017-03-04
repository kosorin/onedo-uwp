using System.Threading.Tasks;
using OneDo.ViewModel;
using OneDo.Core.Args;
using Windows.UI.Xaml.Controls;
using System.Linq;
using OneDo.View.Core;

namespace OneDo.View
{
    public sealed partial class LogView : ModalView, IView<LogViewModel>
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

        private void Log_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VM.SelectedItems = Log.SelectedItems.Cast<string>().ToList();
        }
    }
}
