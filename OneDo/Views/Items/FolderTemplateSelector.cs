using GalaSoft.MvvmLight;
using OneDo.ViewModels;
using OneDo.ViewModels.Flyouts;
using OneDo.ViewModels.Items;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace OneDo.Views.Items
{
    public class FolderTemplateSelector : DataTemplateSelector
    {
        public FolderItemViewModel SelectedFolder { get; set; }

        public DataTemplate DefaultTemplate { get; set; }

        public DataTemplate SelectedTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            return SelectTemplate(item as FolderItemViewModel);
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return SelectTemplate(item as FolderItemViewModel);
        }

        private DataTemplate SelectTemplate(FolderItemViewModel folder)
        {
            return SelectedFolder == folder ? SelectedTemplate : DefaultTemplate;
        }
    }
}