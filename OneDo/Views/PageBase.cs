﻿using GalaSoft.MvvmLight;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;

namespace OneDo.Views
{
    public abstract class PageBase : Page
    {
        protected ViewModelBase ViewModel { get; set; }

        protected PageBase()
        {
            DataContextChanged += (s, e) =>
            {
                ViewModel = e.NewValue as ViewModelBase;
            };

            Transitions = new TransitionCollection
            {
                new PaneThemeTransition() { Edge = EdgeTransitionLocation.Left }
            };
        }
    }
}
