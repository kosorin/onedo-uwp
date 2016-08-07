namespace OneDo.Common.Logging
{
    public static class Logger
    {
        private static ILogger current;
        public static ILogger Current
        {
            get { return current ?? (current = new NullLogger()); }
            set { current = value; }
        }
    }
}