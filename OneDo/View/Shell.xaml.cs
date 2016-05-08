using OneDo.ViewModel;
using Windows.UI.Xaml.Controls;

namespace OneDo.View
{
    public sealed partial class Shell : BasePage, IXBindPage<ShellViewModel>
    {
        public ShellViewModel VM => ViewModel as ShellViewModel;

        public Shell()
        {
            InitializeComponent();
        }
    }
}
