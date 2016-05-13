
using OneDo.ViewModel;

namespace OneDo.View
{
    public sealed partial class StartPage : IXBindPage<StartViewModel>
    {
        public StartViewModel VM => ViewModel as StartViewModel;

        public StartPage()
        {
            InitializeComponent();
        }
    }
}
