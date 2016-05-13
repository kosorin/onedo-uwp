﻿using OneDo.ViewModel;

namespace OneDo.View
{
    public sealed partial class AboutPage : IXBindPage<AboutViewModel>
    {
        public AboutViewModel VM => ViewModel as AboutViewModel;

        public AboutPage()
        {
            InitializeComponent();
        }
    }
}
