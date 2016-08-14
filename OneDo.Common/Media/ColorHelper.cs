using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace OneDo.Common.Media
{
    public static class ColorHelper
    {
        public static Brush FromHex(string hex)
        {
            if (string.IsNullOrEmpty(hex)) return new SolidColorBrush(Colors.Transparent);

            byte a = 255, r = 0, g = 0, b = 0;
            if (hex.Length == 7)
            {
                r = HexToByte(hex, 1);
                g = HexToByte(hex, 3);
                b = HexToByte(hex, 5);
            }
            else if (hex.Length == 9)
            {
                a = HexToByte(hex, 1);
                r = HexToByte(hex, 3);
                g = HexToByte(hex, 5);
                b = HexToByte(hex, 7);
            }

            var color = Color.FromArgb(a, r, g, b);
            return new SolidColorBrush(color);
        }

        private static byte HexToByte(string text, int offset)
        {
            return byte.Parse(text.Substring(offset, 2), NumberStyles.HexNumber);
        }
    }
}
