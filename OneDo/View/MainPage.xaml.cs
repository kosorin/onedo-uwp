using OneDo.ViewModel;

namespace OneDo.View
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
