using OneDo.Common.Mvvm;
using OneDo.Common.UI;
using OneDo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.System;
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
            public NullModal()
            {
            }
        }

        public static ModalView Null { get; } = new NullModal();


        public CubicBezierEasingFunction DefaultEasing { get; }

        public int DefaultDuration { get; }


        public bool IsRoot
        {
            get { return (bool)GetValue(IsRootProperty); }
            set { SetValue(IsRootProperty, value); }
        }
        public static readonly DependencyProperty IsRootProperty =
            DependencyProperty.Register("IsRoot", typeof(bool), typeof(ModalContainer), new PropertyMetadata(false));

        public ModalView Modal
        {
            get { return (ModalView)GetValue(ModalProperty); }
            private set { SetValue(ModalProperty, value ?? Null); }
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
            private set { SetValue(ActualModalProperty, value ?? Null); }
        }
        public static readonly DependencyProperty ActualModalProperty =
            DependencyProperty.Register(nameof(ActualModal), typeof(ModalView), typeof(ModalContainer), new PropertyMetadata(Null, OnActualModalChanged));
        private static void OnActualModalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var container = (ModalContainer)d;
            container.OnActualModalChanged(e.NewValue as ModalView, e.OldValue as ModalView);
        }


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

            var window = Window.Current;
            window.CoreWindow.PointerPressed += OnPointerPressed;
            window.CoreWindow.KeyDown += OnKeyDown;

            DefaultStyleKey = typeof(ModalContainer);

            DefaultEasing = compositor.CreateCubicBezierEasingFunction(new Vector2(0.25f, 0.1f), new Vector2(0.25f, 1.0f));
            DefaultDuration = 300;

            InitializeBackgroundAnimations();
            InitializeContentAnimations();
        }

        protected override void OnApplyTemplate()
        {
            rootGrid = (Grid)GetTemplateChild(RootGridPartName);

            backgroundControl = (FrameworkElement)GetTemplateChild(BackgroundControlPartName);
            backgroundControl.Tapped += OnBackgroundControlTapped;
            backgroundVisual = ElementCompositionPreview.GetElementVisual(backgroundControl);

            contentControl = (ContentControl)GetTemplateChild(ContentControlPartName);
            contentVisual = ElementCompositionPreview.GetElementVisual(contentControl);

            UpdateVisibility();
        }

        private void OnBackgroundControlTapped(object sender, TappedRoutedEventArgs e)
        {
            TryClose();
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
            if (Modal != Null)
            {
                Close();
                return true;
            }
            return false;
        }

        public void Close()
        {
            if (Modal.SubContainer == null || !Modal.SubContainer.TryClose())
            {
                Modal = Null;
            }
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

        private void OnActualModalChanged(ModalView newModal, ModalView oldModal)
        {
            UpdateVisibility();
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


        private void OnPointerPressed(CoreWindow sender, PointerEventArgs args)
        {
            var properties = args.CurrentPoint.Properties;
            if (properties.IsXButton1Pressed || properties.IsXButton2Pressed)
            {
                args.Handled = true;
            }

            if (properties.IsXButton1Pressed)
            {
                TryClose();
            }
        }

        private void OnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (!args.Handled && args.VirtualKey == VirtualKey.Escape)
            {
                if (TryClose())
                {
                    args.Handled = true;
                }
            }
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs args)
        {
            if (!args.Handled)
            {
                if (TryClose())
                {
                    args.Handled = true;
                }
            }
        }


        private void UpdateVisibility()
        {
            if (rootGrid != null)
            {
                var show = ActualModal != Null;
                rootGrid.Visibility = show
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

        private void UpdateBackButtonVisibility()
        {
            if (IsRoot)
            {
                var showBackButton = Modal != Null;
                navigationManager.AppViewBackButtonVisibility = showBackButton
                    ? AppViewBackButtonVisibility.Visible
                    : AppViewBackButtonVisibility.Collapsed;
            }
        }
    }
}
