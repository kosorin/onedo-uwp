using OneDo.ViewModel.Pages;

namespace OneDo.View.Pages
{
    public sealed partial class MainPage : IXBindPage<MainPageViewModel>
    {
        public MainPageViewModel VM => ViewModel as MainPageViewModel;

        public MainPage()
        {
            this.InitializeComponent();
        }
    }
}
