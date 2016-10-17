using GalaSoft.MvvmLight;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using OneDo.Common.UI;
using OneDo.Services.ModalService;
using OneDo.ViewModel;
using OneDo.ViewModel.Items;
using OneDo.ViewModel.Modals;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using static OneDo.ViewModel.ModalViewModel;

namespace OneDo.View.Controls
{
    public sealed partial class ModalContainer : ExtendedUserControl
    {
        public ModalViewModel Modal
        {
            get { return (ModalViewModel)GetValue(ModalProperty); }
            set { SetValue(ModalProperty, value); }
        }

        public static readonly DependencyProperty ModalProperty =
            DependencyProperty.Register(nameof(Modal), typeof(ModalViewModel), typeof(ModalContainer), new PropertyMetadata(null, Modal_Changed));

        private static void Modal_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ModalContainer)?.OnModalChanged(e.NewValue as ModalViewModel, e.OldValue as ModalViewModel);
        }


        public ModalViewModel ActualModal
        {
            get { return (ModalViewModel)GetValue(ActualModalProperty); }
            private set { SetValue(ActualModalProperty, value); }
        }

        public static readonly DependencyProperty ActualModalProperty =
            DependencyProperty.Register(nameof(ActualModal), typeof(ModalViewModel), typeof(ModalContainer), new PropertyMetadata(null));


        private Visual backgroundVisual;

        private Visual contentVisual;

        private CubicBezierEasingFunction defaultEasing;

        private int defaultDuration;

        private ScalarKeyFrameAnimation opacityFadeInAnimation;

        private ScalarKeyFrameAnimation opacityFadeOutAnimation;

        private AnimationInfo defaultFadeInAnimationInfo;

        private AnimationInfo defaultFadeOutAnimationInfo;

        private Dictionary<Type, AnimationInfo> fadeInAnimationInfos;

        private Dictionary<Type, AnimationInfo> fadeOutAnimationInfos;

        public ModalContainer()
        {
            InitializeComponent();
            if (!DesignMode.DesignModeEnabled)
            {
                backgroundVisual = ElementCompositionPreview.GetElementVisual(BackgroundControl);
                contentVisual = ElementCompositionPreview.GetElementVisual(ContentControl);

                defaultEasing = compositor.CreateCubicBezierEasingFunction(new Vector2(0.25f, 0.1f), new Vector2(0.25f, 1.0f));
                defaultDuration = 300;

                RootGrid.Visibility = Modal != null
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                InitializeBackgroundControl();
                InitializeAnimations();
            }
        }

        private void InitializeBackgroundControl()
        {
            InitializeBackgroundBlur();
            InitializeBackgroundAnimations();
        }

        private void InitializeBackgroundBlur()
        {
            var blurEffect = new GaussianBlurEffect()
            {
                Name = "Blur",
                BlurAmount = 12,
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

            opacityFadeInAnimation = compositor.CreateScalarKeyFrameAnimation();
            opacityFadeInAnimation.Duration = TimeSpan.FromMilliseconds(defaultDuration);
            opacityFadeInAnimation.InsertKeyFrame(0, 0);
            opacityFadeInAnimation.InsertKeyFrame(1, 1);

            opacityFadeOutAnimation = compositor.CreateScalarKeyFrameAnimation();
            opacityFadeOutAnimation.DelayTime = TimeSpan.FromMilliseconds(defaultDuration);
            opacityFadeOutAnimation.Duration = TimeSpan.FromMilliseconds(defaultDuration);
            opacityFadeOutAnimation.InsertKeyFrame(0, 1);
            opacityFadeOutAnimation.InsertKeyFrame(1, 0);
        }

        private void InitializeAnimations()
        {
            var defaultFadeInAnimation = compositor.CreateScalarKeyFrameAnimation();
            defaultFadeInAnimation.Duration = TimeSpan.FromMilliseconds(defaultDuration);
            defaultFadeInAnimation.InsertKeyFrame(0, -50);
            defaultFadeInAnimation.InsertKeyFrame(1, 0, defaultEasing);
            defaultFadeInAnimationInfo = new AnimationInfo("Offset.Y", defaultFadeInAnimation);

            var defaultFadeOutAnimation = compositor.CreateScalarKeyFrameAnimation();
            defaultFadeOutAnimation.Duration = TimeSpan.FromMilliseconds(defaultDuration);
            defaultFadeOutAnimation.InsertKeyFrame(0, 0);
            defaultFadeOutAnimation.InsertKeyFrame(1, -50, defaultEasing);
            defaultFadeOutAnimationInfo = new AnimationInfo("Offset.Y", defaultFadeOutAnimation);

            var noteEditorFadeInAnimation = compositor.CreateScalarKeyFrameAnimation();
            noteEditorFadeInAnimation.Duration = TimeSpan.FromMilliseconds(defaultDuration * 2);
            noteEditorFadeInAnimation.InsertExpressionKeyFrame(0, "Height");
            noteEditorFadeInAnimation.InsertKeyFrame(1, 0, defaultEasing);

            var noteEditorFadeOutAnimation = compositor.CreateScalarKeyFrameAnimation();
            noteEditorFadeOutAnimation.Duration = TimeSpan.FromMilliseconds(defaultDuration * 2);
            noteEditorFadeOutAnimation.InsertKeyFrame(0, 0);
            noteEditorFadeOutAnimation.InsertExpressionKeyFrame(1, "Height", defaultEasing);

            fadeInAnimationInfos = new Dictionary<Type, AnimationInfo>
            {
                [typeof(NoteEditorViewModel)] = new AnimationInfo("Offset.Y", noteEditorFadeInAnimation)
            };
            fadeOutAnimationInfos = new Dictionary<Type, AnimationInfo>
            {
                [typeof(NoteEditorViewModel)] = new AnimationInfo("Offset.Y", noteEditorFadeOutAnimation)
            };
        }

        private void OnModalChanged(ModalViewModel newModal, ModalViewModel oldModal)
        {
            if (newModal != null)
            {
                OnModalShowed(newModal);
            }
            else if (oldModal != null)
            {
                OnModalClosed(oldModal);
            }
        }

        private void OnModalShowed(ModalViewModel modal)
        {
            RootGrid.Visibility = modal != null ? Visibility.Visible : Visibility.Collapsed;
            ActualModal = modal;

            backgroundVisual.StartAnimation("Opacity", opacityFadeInAnimation);
            contentVisual.StartAnimation("Opacity", opacityFadeInAnimation);

            RunContentAnimation(fadeInAnimationInfos, modal.GetType(), defaultFadeInAnimationInfo);
        }

        private void OnModalClosed(ModalViewModel modal)
        {
            var batch = compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
            batch.Completed += (s, e) =>
            {
                RootGrid.Visibility = Visibility.Collapsed;
                ActualModal = Null;
            };

            backgroundVisual.StartAnimation("Opacity", opacityFadeOutAnimation);
            contentVisual.StartAnimation("Opacity", opacityFadeOutAnimation);

            RunContentAnimation(fadeOutAnimationInfos, modal.GetType(), defaultFadeOutAnimationInfo);

            batch.End();
        }

        private void RunContentAnimation(Dictionary<Type, AnimationInfo> animations, Type modalType, AnimationInfo defaultAnimation)
        {
            var animationInfo = animations.ContainsKey(modalType)
                ? animations[modalType]
                : defaultAnimation;
            animationInfo.Animation.SetScalarParameter("Height", (float)ActualHeight);
            animationInfo.Start(contentVisual);
        }

        private class AnimationInfo
        {
            public AnimationInfo(string propertyName, CompositionAnimation animation)
            {
                PropertyName = propertyName;
                Animation = animation;
            }

            public string PropertyName { get; set; }

            public CompositionAnimation Animation { get; set; }

            public void Start(Visual visual)
            {
                visual.StartAnimation(PropertyName, Animation);
            }

            public void Stop(Visual visual)
            {
                visual.StopAnimation(PropertyName);
            }
        }
    }
}
