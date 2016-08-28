using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using OneDo.Common.UI;
using OneDo.Services.ModalService;
using OneDo.ViewModel.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace OneDo.View.Controls
{
    public sealed partial class ModalContainer : ExtendedUserControl, IXBind<ModalService>
    {
        public ModalService VM => (ModalService)ViewModel;

        private ScalarKeyFrameAnimation backgroundFadeInAnimation;

        private ScalarKeyFrameAnimation backgroundFadeOutAnimation;

        public ModalContainer()
        {
            InitializeComponent();
            InitializeBackgroundControl();
        }

        private void InitializeBackgroundControl()
        {
            BackgroundControl.Visibility = Visibility.Collapsed;

            InitializeBackgroundBlur();
            InitializeBackgroundAnimations();
        }

        private void InitializeBackgroundBlur()
        {
            var visual = ElementCompositionPreview.GetElementVisual(BackgroundControl);

            var blurEffect = new GaussianBlurEffect()
            {
                Name = "Blur",
                BlurAmount = 10,
                BorderMode = EffectBorderMode.Hard,
                Optimization = EffectOptimization.Balanced,
                Source = new CompositionEffectSourceParameter("Backdrop"),
            };

            var factory = compositor.CreateEffectFactory(blurEffect);
            var brush = factory.CreateBrush();
            brush.SetSourceParameter("Backdrop", compositor.CreateBackdropBrush());

            var spriteVisual = compositor.CreateSpriteVisual();
            spriteVisual.Brush = brush;
            BackgroundControl.SizeChanged += (s, e) =>
            {
                spriteVisual.Size = e.NewSize.ToVector2();
            };

            ElementCompositionPreview.SetElementChildVisual(BackgroundControl, spriteVisual);
        }

        private void InitializeBackgroundAnimations()
        {
            backgroundFadeInAnimation = compositor.CreateScalarKeyFrameAnimation();
            backgroundFadeInAnimation.Duration = TimeSpan.FromMilliseconds(650);
            backgroundFadeInAnimation.InsertKeyFrame(0, 0);
            backgroundFadeInAnimation.InsertKeyFrame(1, 1);

            backgroundFadeOutAnimation = compositor.CreateScalarKeyFrameAnimation();
            backgroundFadeOutAnimation.DelayTime = TimeSpan.FromMilliseconds(600);
            backgroundFadeOutAnimation.Duration = TimeSpan.FromMilliseconds(350);
            backgroundFadeOutAnimation.InsertKeyFrame(0, 1);
            backgroundFadeOutAnimation.InsertKeyFrame(1, 0);

            VM.CanCloseCurrentChanged += (s, e) =>
            {
                var visual = ElementCompositionPreview.GetElementVisual(BackgroundControl);
                visual.StopAnimation(nameof(visual.Opacity));
                if (VM.CanCloseCurrent)
                {
                    BackgroundControl.Visibility = Visibility.Visible;
                    visual.StartAnimation(nameof(visual.Opacity), backgroundFadeInAnimation);
                }
                else
                {
                    var batch = compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
                    batch.Completed += (cs, ce) =>
                    {
                        BackgroundControl.Visibility = Visibility.Collapsed;
                    };
                    visual.StartAnimation(nameof(visual.Opacity), backgroundFadeOutAnimation);
                    batch.End();
                }
            };
        }
    }
}
