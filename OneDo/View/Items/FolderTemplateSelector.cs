using GalaSoft.MvvmLight;
using OneDo.ViewModel;
using OneDo.ViewModel.Modals;
using OneDo.ViewModel.Items;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace OneDo.View.Items
{
    public class FolderTemplateSelector : DataTemplateSelector
    {
        public FolderItemObject SelectedFolder { get; set; }

        public DataTemplate DefaultTemplate { get; set; }

        public DataTemplate SelectedTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            return SelectTemplate(item as FolderItemObject);
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return SelectTemplate(item as FolderItemObject);
        }

        private DataTemplate SelectTemplate(FolderItemObject folder)
        {
            return SelectedFolder == folder ? SelectedTemplate : DefaultTemplate;
        }
    }
}