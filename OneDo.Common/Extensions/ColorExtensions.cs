using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace OneDo.Common.Extensions
{
    public static class ColorExtensions
    {
        public static string ToHex(this Color color)
        {
            if (color.A != 255)
            {
                return string.Format("#{3:X2}{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B, color.A);
            }
            return string.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
        }

        public static Color ToColor(this string hex)
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
