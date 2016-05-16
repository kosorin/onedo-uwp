namespace OneDo.Common.Logging
{
    public static class Logger
    {
        public static ILogger Current { get; private set; } = new NullLogger();

        public static void Set(ILogger logger)
        {
            Current = logger;
        }
    }
}