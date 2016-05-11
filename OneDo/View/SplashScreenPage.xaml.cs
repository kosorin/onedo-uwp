using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace OneDo.View
{
    public sealed partial class SplashScreenPage : Page
    {
        private SplashScreen splashScreen; // Variable to hold the splash screen object.

        public SplashScreenPage()
        {
            InitializeComponent();

        }

        public SplashScreenPage(SplashScreen splashScreen) : this()
        {
            Window.Current.SizeChanged += ExtendedSplash_OnResize;

            this.splashScreen = splashScreen;
            if (splashScreen != null)
            {
                splashScreen.Dismissed += DismissedEventHandler;
                Position();
            }
        }

        private async void DismissedEventHandler(SplashScreen sender, object e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, SplashDismissedStoryboard.Begin);
        }

        void ExtendedSplash_OnResize(Object sender, WindowSizeChangedEventArgs e)
        {
            if (splashScreen != null)
            {
                Position();
            }
        }

        private void Position()
        {
            PositionImage();
            PositionRing();
        }

        private void PositionImage()
        {
            var rect = splashScreen.ImageLocation;
            SplashImage.SetValue(Canvas.LeftProperty, rect.X);
            SplashImage.SetValue(Canvas.TopProperty, rect.Y);
            SplashImage.Height = rect.Height;
            SplashImage.Width = rect.Width;
        }

        private void PositionRing()
        {
            var rect = splashScreen.ImageLocation;
            SplashProgressRing.SetValue(Canvas.LeftProperty, rect.X + (rect.Width * 0.5) - (SplashProgressRing.Width * 0.5));
            SplashProgressRing.SetValue(Canvas.TopProperty, (rect.Y + rect.Height + rect.Height * 0.1));
        }
    }
}
