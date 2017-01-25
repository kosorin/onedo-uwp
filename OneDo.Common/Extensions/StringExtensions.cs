using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Common.Extensions
{
    public static class StringExtensions
    {
        public static string TrimNull(this string text)
        {
            return text?.Trim() ?? "";
        }
    }
}
