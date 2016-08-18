﻿using GalaSoft.MvvmLight;
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
        public DataTemplate TodoEditorTemplate { get; set; }

        public DataTemplate FolderEditorTemplate { get; set; }

        public DataTemplate SettingsTemplate { get; set; }

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
            if (modal is TodoEditorViewModel)
            {
                return TodoEditorTemplate;
            }
            else if (modal is FolderEditorViewModel)
            {
                return FolderEditorTemplate;
            }
            else if (modal is SettingsViewModel)
            {
                return SettingsTemplate;
            }
            return null;
        }
    }
}