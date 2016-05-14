namespace OneDo.Common.Logging
{
    public static class Logger
    {
        public static ILogger Current { get; }

        static Logger()
        {
#if DEBUG
            Current = new DebugLogger();
#else
            Current = new NullLogger();
#endif
        }
    }
}