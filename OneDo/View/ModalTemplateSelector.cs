using GalaSoft.MvvmLight;
using OneDo.ViewModel;
using OneDo.ViewModel.Modals;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace OneDo.View
{
    public class ModalTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FolderEditorTemplate { get; set; }

        public DataTemplate NoteEditorTemplate { get; set; }

        public DataTemplate FolderPickerTemplate { get; set; }

        public DataTemplate SettingsTemplate { get; set; }

        public DataTemplate DebugTemplate { get; set; }

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
            else if (modal is FolderPickerViewModel)
            {
                return FolderPickerTemplate;
            }
            else if (modal is SettingsViewModel)
            {
                return SettingsTemplate;
            }
            else if (modal is DebugViewModel)
            {
                return DebugTemplate;
            }
            return null;
        }
    }
}