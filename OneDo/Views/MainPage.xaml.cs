using OneDo.ViewModels;

namespace OneDo.Views
{
    public sealed partial class MainPage : IXBindPage<MainViewModel>
    {
        public MainViewModel VM => ViewModel as MainViewModel;

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
