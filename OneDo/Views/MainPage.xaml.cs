using OneDo.ViewModels;
using OneDo.ViewModels.Pages;

namespace OneDo.Views
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
