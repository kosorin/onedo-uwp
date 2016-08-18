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
        public static string ToHex(Color color)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
        }

        public static Color FromHex(string hex)
        {
            if (string.IsNullOrEmpty(hex)) return Colors.Transparent;

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

            return Color.FromArgb(a, r, g, b);
        }

        private static byte HexToByte(string text, int offset)
        {
            return byte.Parse(text.Substring(offset, 2), NumberStyles.HexNumber);
        }
    }
}
