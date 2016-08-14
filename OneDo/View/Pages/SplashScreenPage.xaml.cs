using GalaSoft.MvvmLight.Messaging;
using OneDo.ViewModel;
using System;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace OneDo.View.Pages
{
    public sealed partial class SplashScreenPage : Page
    {
        private SplashScreen splashScreen;


        public SplashScreenPage()
        {
            InitializeComponent();

            Messenger.Default.Register<SplashScreenMessage>(this, OnMessage);
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

        private void OnMessage(SplashScreenMessage message)
        {
            if (message.Text != null)
            {
                SplashText.Text = message.Text;
            }
            else
            {
                Messenger.Default.Unregister<SplashScreenMessage>(this);
            }
        }

        private async void DismissedEventHandler(SplashScreen sender, object e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, SplashDismissedStoryboard.Begin);
        }

        private void ExtendedSplash_OnResize(object sender, WindowSizeChangedEventArgs e)
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
            SplashProgressRing.SetValue(Canvas.TopProperty, rect.Y + rect.Height + (rect.Height * 0.1));
        }
    }
}
