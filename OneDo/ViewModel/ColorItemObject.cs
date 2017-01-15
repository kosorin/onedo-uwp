using GalaSoft.MvvmLight;
using OneDo.Common.Extensions;
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
            Color = hex.ToColor();
        }
    }
}