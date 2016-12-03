using System;

namespace OneDo.Services.InfoService
{
    public static class InfoMessageDurations
    {
        public static TimeSpan Default => TimeSpan.FromSeconds(8);

        public static TimeSpan Delete => TimeSpan.FromSeconds(20);
    }
}
