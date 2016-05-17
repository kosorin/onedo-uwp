using System.Text.RegularExpressions;

namespace OneDo.Common.Logging
{
    public class DebugLogger : LoggerBase
    {
        protected override void WriteLine(string message)
        {
            lock (syncObject)
            {
                foreach (var line in Regex.Split(message, @"\r?\n"))
                {
                    System.Diagnostics.Debug.WriteLine(">>> " + line);
                }
            }
        }
    }
}
