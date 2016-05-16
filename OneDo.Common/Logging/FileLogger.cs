using System.IO;
using System.Text.RegularExpressions;

namespace OneDo.Common.Logging
{
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
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
            try
            {
                File.AppendAllLines(Path, Regex.Split(message, @"\r?\n"));
            }
            catch { }
        }
    }
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
}