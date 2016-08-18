using GalaSoft.MvvmLight;
using OneDo.Common.Media;
using OneDo.Model.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace OneDo.ViewModel.Items
{
    public class ColorItem : ExtendedViewModel
    {
        public string Name { get; }

        public Color Color { get; }

        public ColorItem(Color color)
        {
            Color = color;
        }

        public ColorItem(Color color, string name)
        {
            Name = name;
            Color = color;
        }
    }
}