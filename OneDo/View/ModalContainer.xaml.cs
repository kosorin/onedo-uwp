using GalaSoft.MvvmLight;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using OneDo.Common.Metadata;
using OneDo.Common.UI;
using OneDo.Services.ModalService;
using OneDo.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
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

namespace OneDo.View
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


        public DataTemplateSelector TemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(TemplateSelectorProperty); }
            set { SetValue(TemplateSelectorProperty, value); }
        }

        public static readonly DependencyProperty TemplateSelectorProperty =
            DependencyProperty.Register(nameof(TemplateSelector), typeof(DataTemplateSelector), typeof(ModalContainer), new PropertyMetadata(null));


        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }

        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register(nameof(CloseCommand), typeof(ICommand), typeof(ModalContainer), new PropertyMetadata(null));


        public CubicBezierEasingFunction DefaultEasing { get; }

        public int DefaultDuration { get; }


        private Visual backgroundVisual;

        private Visual contentVisual;

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

                DefaultEasing = compositor.CreateCubicBezierEasingFunction(new Vector2(0.25f, 0.1f), new Vector2(0.25f, 1.0f));
                DefaultDuration = 300;

                RootGrid.Visibility = Modal != null
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                InitializeBackgroundControl();
                InitializeAnimations();
            }
        }

        private void InitializeBackgroundControl()
        {
            InitializeBackgroundAnimations();
        }

        private void InitializeBackgroundAnimations()
        {
            opacityFadeInAnimation = compositor.CreateScalarKeyFrameAnimation();
            opacityFadeInAnimation.Duration = TimeSpan.FromMilliseconds(DefaultDuration);
            opacityFadeInAnimation.InsertKeyFrame(0, 0);
            opacityFadeInAnimation.InsertKeyFrame(1, 1);

            opacityFadeOutAnimation = compositor.CreateScalarKeyFrameAnimation();
            opacityFadeOutAnimation.DelayTime = TimeSpan.FromMilliseconds(DefaultDuration);
            opacityFadeOutAnimation.Duration = TimeSpan.FromMilliseconds(DefaultDuration);
            opacityFadeOutAnimation.InsertKeyFrame(0, 1);
            opacityFadeOutAnimation.InsertKeyFrame(1, 0);
        }

        private void InitializeAnimations()
        {
            var defaultFadeInAnimation = compositor.CreateScalarKeyFrameAnimation();
            defaultFadeInAnimation.Duration = TimeSpan.FromMilliseconds(DefaultDuration);
            defaultFadeInAnimation.InsertKeyFrame(0, -50);
            defaultFadeInAnimation.InsertKeyFrame(1, 0, DefaultEasing);
            defaultFadeInAnimationInfo = new AnimationInfo("Offset.Y", defaultFadeInAnimation);
            fadeInAnimationInfos = new Dictionary<Type, AnimationInfo>();

            var defaultFadeOutAnimation = compositor.CreateScalarKeyFrameAnimation();
            defaultFadeOutAnimation.Duration = TimeSpan.FromMilliseconds(DefaultDuration);
            defaultFadeOutAnimation.InsertKeyFrame(0, 0);
            defaultFadeOutAnimation.InsertKeyFrame(1, -50, DefaultEasing);
            defaultFadeOutAnimationInfo = new AnimationInfo("Offset.Y", defaultFadeOutAnimation);
            fadeOutAnimationInfos = new Dictionary<Type, AnimationInfo>();
        }

        public void AddFadeInAnimation<TViewModel>(string propertyName, CompositionAnimation animation)
        {
            fadeInAnimationInfos[typeof(TViewModel)] = new AnimationInfo(propertyName, animation);
        }

        public void AddFadeOutAnimation<TViewModel>(string propertyName, CompositionAnimation animation)
        {
            fadeOutAnimationInfos[typeof(TViewModel)] = new AnimationInfo(propertyName, animation);
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

            contentVisual.Offset = new Vector3();
            RunContentAnimation(fadeInAnimationInfos, modal.GetType(), defaultFadeInAnimationInfo);
        }

        private void OnModalClosed(ModalViewModel modal)
        {
            var batch = compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
            batch.Completed += (s, e) =>
            {
                RootGrid.Visibility = Visibility.Collapsed;
                ActualModal = ModalViewModel.Null;
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
            animationInfo.Animation.SetScalarParameter("Width", (float)ActualWidth);
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
