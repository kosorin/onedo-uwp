using OneDo.Common.Mvvm;
using OneDo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace OneDo.View
{
    [ContentProperty(Name = nameof(Modal))]
    [TemplatePart(Name = RootGridPartName, Type = typeof(Grid))]
    [TemplatePart(Name = BackgroundControlPartName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = ContentControlPartName, Type = typeof(ContentControl))]
    public class ModalContainer : ExtendedContentControl
    {
        private const string RootGridPartName = "PART_RootGrid";

        private const string BackgroundControlPartName = "PART_BackgroundControl";

        private const string ContentControlPartName = "PART_ContentControl";


        private class NullModal : ModalView
        {
        }

        public static ModalView Null { get; } = new NullModal();


        public CubicBezierEasingFunction DefaultEasing { get; }

        public int DefaultDuration { get; }


        public ModalView Modal
        {
            get { return (ModalView)GetValue(ModalProperty); }
            private set { SetValue(ModalProperty, value); }
        }

        public static readonly DependencyProperty ModalProperty =
            DependencyProperty.Register(nameof(Modal), typeof(ModalView), typeof(ModalContainer), new PropertyMetadata(Null, OnModalChanged));

        private static void OnModalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var container = (ModalContainer)d;
            container.OnModalChanged(e.NewValue as ModalView, e.OldValue as ModalView);
        }


        public ModalView ActualModal
        {
            get { return (ModalView)GetValue(ActualModalProperty); }
            private set { SetValue(ActualModalProperty, value); }
        }

        public static readonly DependencyProperty ActualModalProperty =
            DependencyProperty.Register(nameof(ActualModal), typeof(ModalView), typeof(ModalContainer), new PropertyMetadata(Null));



        private Grid rootGrid;

        private FrameworkElement backgroundControl;

        private ContentControl contentControl;


        private Visual backgroundVisual;

        private Visual contentVisual;

        private ScalarKeyFrameAnimation opacityFadeInAnimation;

        private ScalarKeyFrameAnimation opacityFadeOutAnimation;

        private AnimationInfo defaultFadeInAnimationInfo;

        private AnimationInfo defaultFadeOutAnimationInfo;

        private Dictionary<Type, AnimationInfo> fadeInAnimationInfos;

        private Dictionary<Type, AnimationInfo> fadeOutAnimationInfos;


        private SystemNavigationManager navigationManager;

        public ModalContainer()
        {
            navigationManager = SystemNavigationManager.GetForCurrentView();
            navigationManager.BackRequested += OnBackRequested;

            DefaultStyleKey = typeof(ModalContainer);

            DefaultEasing = compositor.CreateCubicBezierEasingFunction(new Vector2(0.25f, 0.1f), new Vector2(0.25f, 1.0f));
            DefaultDuration = 300;

            InitializeBackgroundAnimations();
            InitializeContentAnimations();
        }

        protected override void OnApplyTemplate()
        {
            rootGrid = (Grid)GetTemplateChild(RootGridPartName);
            rootGrid.Visibility = Modal != Null
                ? Visibility.Visible
                : Visibility.Collapsed;

            backgroundControl = (FrameworkElement)GetTemplateChild(BackgroundControlPartName);
            backgroundControl.Tapped += OnBackgroundControlTapped;
            backgroundVisual = ElementCompositionPreview.GetElementVisual(backgroundControl);

            contentControl = (ContentControl)GetTemplateChild(ContentControlPartName);
            contentVisual = ElementCompositionPreview.GetElementVisual(contentControl);
        }

        private void OnBackgroundControlTapped(object sender, TappedRoutedEventArgs e)
        {
            Modal = Null;
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

        private void InitializeContentAnimations()
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


        public bool TryClose()
        {
            if (ActualModal != Null)
            {
                Close();
                return true;
            }
            return false;
        }

        public void Close()
        {
            Modal = Null;
        }

        public void Show(ModalView modal)
        {
            Modal = modal;
        }

        public void AddFadeInAnimation<TModal>(string propertyName, CompositionAnimation animation)
            where TModal : ModalView
        {
            fadeInAnimationInfos[typeof(TModal)] = new AnimationInfo(propertyName, animation);
        }

        public void AddFadeOutAnimation<TModal>(string propertyName, CompositionAnimation animation)
            where TModal : ModalView
        {
            fadeOutAnimationInfos[typeof(TModal)] = new AnimationInfo(propertyName, animation);
        }


        private void OnModalChanged(ModalView newModal, ModalView oldModal)
        {
            UpdateBackButtonVisibility();

            if (newModal != Null)
            {
                OnModalShowed(newModal);
            }
            else if (oldModal != Null)
            {
                OnModalClosed(oldModal);
            }
        }

        private void OnModalShowed(ModalView modal)
        {
            rootGrid.Visibility = Visibility.Visible;
            ActualModal = modal;

            backgroundVisual.StartAnimation("Opacity", opacityFadeInAnimation);
            contentVisual.StartAnimation("Opacity", opacityFadeInAnimation);

            contentVisual.Offset = new Vector3();
            RunContentAnimation(fadeInAnimationInfos, modal.GetType(), defaultFadeInAnimationInfo);
        }

        private void OnModalClosed(ModalView modal)
        {
            var batch = compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
            batch.Completed += (s, e) =>
            {
                rootGrid.Visibility = Visibility.Collapsed;
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
            animationInfo.Animation.SetScalarParameter("Width", (float)ActualWidth);
            animationInfo.Animation.SetScalarParameter("Height", (float)ActualHeight);
            animationInfo.Start(contentVisual);
        }


        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (TryClose())
            {
                e.Handled = true;
            }
        }

        private void UpdateBackButtonVisibility()
        {
            navigationManager.AppViewBackButtonVisibility = Modal != Null
                ? AppViewBackButtonVisibility.Visible
                : AppViewBackButtonVisibility.Collapsed;
        }
    }
}
