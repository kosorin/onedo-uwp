using OneDo.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace OneDo.View.Controls
{
    public sealed partial class ModalContainer : UserControl
    {
        public ModalViewModel Modal
        {
            get { return (ModalViewModel)GetValue(ModalModal); }
            set { SetValue(ModalModal, value); }
        }

        public static readonly DependencyProperty ModalModal =
            DependencyProperty.Register(nameof(Modal), typeof(ModalViewModel), typeof(ModalContainer), new PropertyMetadata(null));

        public ModalContainer()
        {
            InitializeComponent();
        }
    }
}
