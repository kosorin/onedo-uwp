using System.IO;
using System.Text.RegularExpressions;

namespace OneDo.Common.Logging
{
    public class FileLogger : LoggerBase
    {
        private bool isEnabled = true;
        private const int maxFailCount = 100;
        private int failCount = 0;

        public string Path { get; }

        /// <summary>
        /// Očekává se, že soubor už existuje.
        /// </summary>
        public FileLogger(string path)
        {
            Path = path;
        }

        protected override void WriteLine(string message)
        {
            lock (syncObject)
            {
                if (isEnabled)
                {
                    try
                    {
                        File.AppendAllLines(Path, Regex.Split(message, @"\r?\n"));
                    }
                    catch
                    {
                        if (failCount < maxFailCount)
                        {
                            isEnabled = false;
                        }
                        failCount++;
                    }
                }
            }
        }
    }
}