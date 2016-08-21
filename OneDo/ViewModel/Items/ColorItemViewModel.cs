using OneDo.Common.Media;
using Windows.UI;

namespace OneDo.ViewModel.Items
{
    public class ColorItemViewModel : ExtendedViewModel
    {
        public Color Color { get; }

        public ColorItemViewModel(Color color)
        {
            Color = color;
        }

        public ColorItemViewModel(string hex)
        {
            Color = Common.Media.ColorHelper.FromHex(hex);
        }
    }
}