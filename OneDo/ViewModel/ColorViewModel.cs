using GalaSoft.MvvmLight;
using OneDo.Common.Extensions;
using Windows.UI;

namespace OneDo.ViewModel
{
    public class ColorViewModel : ObservableObject
    {
        private Color color;
        public Color Color
        {
            get { return color; }
            set { Set(ref color, value); }
        }

        public ColorViewModel(Color color)
        {
            Color = color;
        }

        public ColorViewModel(string hex)
        {
            Color = hex.ToColor();
        }
    }
}