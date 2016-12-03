using OneDo.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OneDo.View
{
    public class ModalTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FolderEditorTemplate { get; set; }

        public DataTemplate NoteEditorTemplate { get; set; }

        public DataTemplate SettingsTemplate { get; set; }

        public DataTemplate DebugTemplate { get; set; }

        public DataTemplate DatePickerTemplate { get; set; }

        public DataTemplate TimePickerTemplate { get; set; }

        public DataTemplate ConfirmationTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            return SelectTemplate(item as ModalViewModel);
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return SelectTemplate(item as ModalViewModel);
        }

        private DataTemplate SelectTemplate(ModalViewModel modal)
        {
            if (modal is FolderEditorViewModel)
            {
                return FolderEditorTemplate;
            }
            else if (modal is NoteEditorViewModel)
            {
                return NoteEditorTemplate;
            }
            else if (modal is SettingsViewModel)
            {
                return SettingsTemplate;
            }
            else if (modal is DebugViewModel)
            {
                return DebugTemplate;
            }
            else if (modal is DatePickerViewModel)
            {
                return DatePickerTemplate;
            }
            else if (modal is TimePickerViewModel)
            {
                return TimePickerTemplate;
            }
            else if (modal is ConfirmationViewModel)
            {
                return ConfirmationTemplate;
            }
            return null;
        }
    }
}