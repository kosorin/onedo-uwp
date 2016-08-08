using OneDo.ViewModels;
using OneDo.ViewModels.Pages;

namespace OneDo.Views.Pages
{
    public sealed partial class MainPage : IXBind<MainViewModel>
    {
        public MainViewModel VM => ViewModel as MainViewModel;

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
