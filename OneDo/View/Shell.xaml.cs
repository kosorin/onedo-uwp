using OneDo.ViewModel;

namespace OneDo.View
{
    public sealed partial class Shell : IXBindPage<ShellViewModel>
    {
        public ShellViewModel VM => ViewModel as ShellViewModel;

        public Shell()
        {
            this.InitializeComponent();
        }
    }
}
