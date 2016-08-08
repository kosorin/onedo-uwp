using GalaSoft.MvvmLight;
using OneDo.ViewModels.Flyouts;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace OneDo.Views
{
    public class FlyoutTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TodoEditorTemplate { get; set; }

        public DataTemplate SettingsTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is TodoEditorViewModel)
            {
                return TodoEditorTemplate;
            }
            else if (item is SettingsViewModel)
            {
                return SettingsTemplate;
            }
            return null;
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return SelectTemplateCore(item);
        }
    }
}