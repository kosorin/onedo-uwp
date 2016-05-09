using OneDo.ViewModel.Pages;

namespace OneDo.View.Pages
{
    public sealed partial class AboutPage : IXBindPage<AboutPageViewModel>
    {
        public AboutPageViewModel VM => ViewModel as AboutPageViewModel;

        public AboutPage()
        {
            InitializeComponent();
        }
    }
}
