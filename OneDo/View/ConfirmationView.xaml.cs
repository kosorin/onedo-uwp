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

namespace OneDo.View
{
    public sealed partial class ConfirmationView : ModalBase, IXBind<ConfirmationViewModel>
    {
        public ConfirmationViewModel VM => ViewModel as ConfirmationViewModel;

        public ConfirmationView()
        {
            InitializeComponent();
        }
    }
}
