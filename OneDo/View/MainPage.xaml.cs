using OneDo.ViewModel;
using OneDo.ViewModel.Modals;
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
            var debugButton = new AppBarButton
            {
                Label = "Debug",
            };
            debugButton.Tapped += (s, e) => VM.ModalService.Show(new DebugViewModel(VM.ModalService, VM.DataService, VM.ProgressService));

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

            var bandButton = new AppBarButton
            {
                Label = "Band",
            };
            bandButton.Tapped += async (s, e) => await VM.Band();

            MainCommandBar.SecondaryCommands.Insert(0, new AppBarSeparator());
            MainCommandBar.SecondaryCommands.Insert(0, bandButton);
            MainCommandBar.SecondaryCommands.Insert(0, resetDataButton);
            MainCommandBar.SecondaryCommands.Insert(0, switchRequestedThemeButton);
            MainCommandBar.SecondaryCommands.Insert(0, debugButton);
        }

        private void SwitchRequestedTheme_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var targetTheme = RequestedTheme;
            if (targetTheme == ElementTheme.Default)
            {
                switch (Application.Current.RequestedTheme)
                {
                    case ApplicationTheme.Light: targetTheme = ElementTheme.Light; break;
                    case ApplicationTheme.Dark: targetTheme = ElementTheme.Dark; break;
                }
            }
            switch (targetTheme)
            {
                case ElementTheme.Light: targetTheme = ElementTheme.Dark; break;
                case ElementTheme.Dark: targetTheme = ElementTheme.Light; break;
            }
            RequestedTheme = targetTheme;
        }
#endif
    }
}
