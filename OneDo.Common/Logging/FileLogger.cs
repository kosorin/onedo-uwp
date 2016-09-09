using System.IO;
using System.Text.RegularExpressions;

namespace OneDo.Common.Logging
{
    public class FileLogger : LoggerBase
    {
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
                try
                {
                    File.AppendAllLines(Path, Regex.Split(message, @"\r?\n"));
                }
                catch { }
            }
        }
    }
}
