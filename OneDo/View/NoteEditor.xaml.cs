using OneDo.Application.Queries.Notes;
using OneDo.Common.Extensions;
using OneDo.Core.Args;
using OneDo.View.Controls;
using OneDo.ViewModel;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace OneDo.View
{
    public sealed partial class NoteEditor : ModalView, IXBind<NoteEditorViewModel>
    {
        public NoteEditorViewModel VM => ViewModel as NoteEditorViewModel;

        public override ModalContainer SubContainer => ModalContainer;

        public Guid? NoteId { get; }

        public NoteEditor(Guid? noteId)
        {
            InitializeComponent();
            InitializeModalAnimations();

            NoteId = noteId;
        }

        private void InitializeModalAnimations()
        {
            var datePickerFadeInAnimation = compositor.CreateScalarKeyFrameAnimation();
            datePickerFadeInAnimation.Duration = TimeSpan.FromMilliseconds(ModalContainer.DefaultDuration);
            datePickerFadeInAnimation.InsertExpressionKeyFrame(0, "Height");
            datePickerFadeInAnimation.InsertKeyFrame(1, 0, ModalContainer.DefaultEasing);
            ModalContainer.AddFadeInAnimation<DatePicker>("Offset.Y", datePickerFadeInAnimation);

            var datePickerFadeOutAnimation = compositor.CreateScalarKeyFrameAnimation();
            datePickerFadeOutAnimation.Duration = TimeSpan.FromMilliseconds(ModalContainer.DefaultDuration);
            datePickerFadeOutAnimation.InsertKeyFrame(0, 0);
            datePickerFadeOutAnimation.InsertExpressionKeyFrame(1, "Height", ModalContainer.DefaultEasing);
            ModalContainer.AddFadeOutAnimation<DatePicker>("Offset.Y", datePickerFadeOutAnimation);

            var reminderPickerFadeInAnimation = compositor.CreateScalarKeyFrameAnimation();
            reminderPickerFadeInAnimation.Duration = TimeSpan.FromMilliseconds(ModalContainer.DefaultDuration);
            reminderPickerFadeInAnimation.InsertExpressionKeyFrame(0, "Height");
            reminderPickerFadeInAnimation.InsertKeyFrame(1, 0, ModalContainer.DefaultEasing);
            ModalContainer.AddFadeInAnimation<TimePicker>("Offset.Y", reminderPickerFadeInAnimation);

            var reminderPickerFadeOutAnimation = compositor.CreateScalarKeyFrameAnimation();
            reminderPickerFadeOutAnimation.Duration = TimeSpan.FromMilliseconds(ModalContainer.DefaultDuration);
            reminderPickerFadeOutAnimation.InsertKeyFrame(0, 0);
            reminderPickerFadeOutAnimation.InsertExpressionKeyFrame(1, "Height", ModalContainer.DefaultEasing);
            ModalContainer.AddFadeOutAnimation<TimePicker>("Offset.Y", reminderPickerFadeOutAnimation);
        }

        protected override async Task OnFirstLoad()
        {
            await VM.Load(NoteId);
        }


        private void HideDatePicker()
        {
            SubContainer.TryClose();
        }

        private void HideTimePicker()
        {
            SubContainer.TryClose();
        }

        private void ShowDatePicker()
        {
            var picker = new DatePicker(VM.Date ?? DateTime.Today);
            picker.DateChanged += DatePicker_DateChanged;
            SubContainer.Show(picker);
        }

        private void ShowTimePicker()
        {
            var picker = new TimePicker(VM.Reminder ?? DateTime.Now.ToTime());
            picker.TimeChanged += ReminderPicker_TimeChanged;
            SubContainer.Show(picker);
        }


        private void DatePicker_DateChanged(DatePicker sender, DateChangedEventArgs args)
        {
            VM.Date = args.Date;
            HideDatePicker();
        }

        private void ReminderPicker_TimeChanged(TimePicker sender, TimeChangedEventArgs args)
        {
            VM.Reminder = args.Time;
            HideTimePicker();
        }


        private void DateButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ShowDatePicker();
        }

        private void ReminderButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ShowTimePicker();
        }
    }
}
