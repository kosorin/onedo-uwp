using OneDo.ViewModel.Pages;

namespace OneDo.View.Pages
{
    public sealed partial class StartPage : IXBindPage<StartPageViewModel>
    {
        public StartPageViewModel VM => ViewModel as StartPageViewModel;

        public StartPage()
        {
            InitializeComponent();
        }
    }
}
