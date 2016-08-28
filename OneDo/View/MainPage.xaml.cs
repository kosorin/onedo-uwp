using OneDo.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OneDo.View
{
    public sealed partial class MainPage : ExtendedPage, IXBind<MainViewModel>
    {
        public MainViewModel VM => (MainViewModel)ViewModel;

        public MainPage()
        {
            InitializeComponent();

#if DEBUG
            InitializeDebug();
#endif
        }

#if DEBUG
        private void InitializeDebug()
        {
            var switchRequestedThemeButton = new AppBarButton
            {
                Label = "Switch RequestedTheme",
            };
            switchRequestedThemeButton.Tapped += SwitchRequestedTheme_Tapped;

            var resetDataButton = new AppBarButton
            {
                Label = "Reset data",
            };
            resetDataButton.Tapped += async (s, e) => await VM.ResetData();

            MainCommandBar.SecondaryCommands.Insert(0, new AppBarSeparator());
            MainCommandBar.SecondaryCommands.Insert(0, resetDataButton);
            MainCommandBar.SecondaryCommands.Insert(0, switchRequestedThemeButton);
        }

        private void SwitchRequestedTheme_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var targetTheme = RequestedTheme;
            switch (targetTheme)
            {
                case ElementTheme.Light: targetTheme = ElementTheme.Dark; break;
                case ElementTheme.Dark: targetTheme = ElementTheme.Light; break;
                default: targetTheme = ElementTheme.Dark; break;
            }
            RequestedTheme = targetTheme;
        }
#endif
    }
}
