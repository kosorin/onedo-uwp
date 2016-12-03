﻿using GalaSoft.MvvmLight;
using Windows.UI;

namespace OneDo.ViewModel
{
    public class ColorItemObject : ObservableObject
    {
        private Color color;
        public Color Color
        {
            get { return color; }
            set { Set(ref color, value); }
        }

        public ColorItemObject(Color color)
        {
            Color = color;
        }

        public ColorItemObject(string hex)
        {
            Color = Common.Media.ColorHelper.FromHex(hex);
        }
    }
}