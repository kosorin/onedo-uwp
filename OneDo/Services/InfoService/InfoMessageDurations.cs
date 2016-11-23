using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Services.InfoService
{
    public static class InfoMessageDurations
    {
        public static TimeSpan Default => TimeSpan.FromSeconds(8);

        public static TimeSpan Delete => TimeSpan.FromSeconds(20);
    }
}
