using OneDo.ViewModels;

namespace OneDo.Views
{
    public sealed partial class AboutPage : IXBindPage<AboutViewModel>
    {
        public AboutViewModel VM => ViewModel as AboutViewModel;

        public AboutPage()
        {
            InitializeComponent();
        }
    }
}
