using OneDo.ViewModel;
using OneDo.ViewModel.Pages;

namespace OneDo.View.Pages
{
    public sealed partial class MainPage : ExtendedPage, IXBind<MainViewModel>
    {
        public MainViewModel VM => (MainViewModel)ViewModel;

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
