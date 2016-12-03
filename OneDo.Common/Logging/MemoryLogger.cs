using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OneDo.Common.Logging
{
    public class MemoryLogger : LoggerBase
    {
        public List<string> Items { get; } = new List<string>();

        public MemoryLogger()
        {

        }

        protected override void WriteLine(string message)
        {
            lock (syncObject)
            {
                Items.AddRange(Regex.Split(message, @"\r?\n"));
            }
        }
    }
}