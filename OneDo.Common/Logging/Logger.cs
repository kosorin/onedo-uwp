namespace OneDo.Common.Logging
{
    public static class Logger
    {
        public static ILogger Current { get; private set; }

        public static void Set(ILogger logger)
        {
            Current = logger;
        }
    }
}