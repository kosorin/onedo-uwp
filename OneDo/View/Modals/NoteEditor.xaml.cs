using System;
using OneDo.ViewModel;
using OneDo.ViewModel.Args;
using OneDo.ViewModel.Modals;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using System.Numerics;
using Windows.UI.Composition;

namespace OneDo.View.Modals
{
    public sealed partial class NoteEditor : ModalBase, IXBind<NoteEditorViewModel>
    {
        public NoteEditorViewModel VM => ViewModel as NoteEditorViewModel;

        private ScalarKeyFrameAnimation toVisibleOpacityAnimation;

        private ScalarKeyFrameAnimation toCollapsedOpacityAnimation;

        private Visual datePickerVisual;

        public NoteEditor()
        {
            InitializeComponent();
            InitializeAnimations();
            InitializeDatePicker();
        }

        protected override void OnViewModelChanging()
        {
            if (VM != null)
            {
                VM.DatePicker.DateChanged -= OnDateChanged;
                VM.ReminderPicker.TimeChanged -= OnReminderChanged;
            }
        }

        protected override void OnViewModelChanged()
        {
            if (VM != null)
            {
                VM.DatePicker.DateChanged += OnDateChanged;
                VM.ReminderPicker.TimeChanged += OnReminderChanged;
            }
        }


        private void InitializeAnimations()
        {
            toVisibleOpacityAnimation = compositor.CreateScalarKeyFrameAnimation();
            toVisibleOpacityAnimation.Duration = TimeSpan.FromMilliseconds(450);
            toVisibleOpacityAnimation.InsertKeyFrame(0, 0);
            toVisibleOpacityAnimation.InsertKeyFrame(1, 1);

            toCollapsedOpacityAnimation = compositor.CreateScalarKeyFrameAnimation();
            toCollapsedOpacityAnimation.Duration = TimeSpan.FromMilliseconds(450);
            toCollapsedOpacityAnimation.InsertKeyFrame(0, 1);
            toCollapsedOpacityAnimation.InsertKeyFrame(1, 0);
        }


        private void InitializeDatePicker()
        {
            datePickerVisual = ElementCompositionPreview.GetElementVisual(DatePickerBorder);
            datePickerVisual.Opacity = 0;

        }

        private void OnDateChanged(DatePickerViewModel sender, DatePickerEventArgs args)
        {
            CloseDatePicker();
        }

        private void DateButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ShowDatePicker();
        }

        private void CloseDatePicker()
        {
            datePickerVisual.StopAnimation("Opacity");

            var batch = compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
            datePickerVisual.StartAnimation("Opacity", toCollapsedOpacityAnimation);
            batch.Completed += (s, e) =>
            {
                DatePickerBorder.Visibility = Visibility.Collapsed;
                VM.ModalService.CloseSub();
            };
            batch.End();
        }

        private void ShowDatePicker()
        {
            datePickerVisual.StopAnimation("Opacity");

            DatePickerBorder.Visibility = Visibility.Visible;
            datePickerVisual.StartAnimation("Opacity", toVisibleOpacityAnimation);

            if (VM.DatePicker.Date == null)
            {
                VM.DatePicker.DateChanged -= OnDateChanged;
                VM.DatePicker.Date = DateTime.Today;
                VM.DatePicker.DateChanged += OnDateChanged;
            }
            VM.ModalService.ShowSub(() =>
            {
                CloseDatePicker();
            });
        }


        private void OnReminderChanged(TimePickerViewModel sender, TimePickerEventArgs args)
        {
            //ReminderPickerFlyout.Hide();
        }

        private void ReminderButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            //ReminderPickerFlyout.ShowAt(ReminderPickerGrid);
        }
    }
}
