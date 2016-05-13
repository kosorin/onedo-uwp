using System;
using System.Text.RegularExpressions;

namespace OneDo.Common.Logging
{
    public class DebugLogger : ILogger
    {
        public void Trace(string message)
        {
            WriteLogLine(message, "TRACE");
        }

        public void Debug(string message)
        {
            WriteLogLine(message, "DEBUG");
        }

        public void Info(string message)
        {
            WriteLogLine(message, "INFO");
        }

        public void Warn(string message)
        {
            WriteLogLine(message, "WARN");
        }

        public void Warn(Exception exception)
        {
            WriteLogLine(exception.Message, "WARN");
            WriteLine(exception.StackTrace);
        }

        public void Warn(string message, Exception exception)
        {
            WriteLogLine(message, "WARN");
            WriteLine(exception.Message);
            WriteLine(exception.StackTrace);
        }

        public void Error(string message)
        {
            WriteLogLine(message, "ERROR");
        }

        public void Error(Exception exception)
        {
            WriteLogLine(exception.Message, "ERROR");
            WriteLine(exception.StackTrace);
        }

        public void Error(string message, Exception exception)
        {
            WriteLogLine(message, "ERROR");
            WriteLine(exception.Message);
            WriteLine(exception.StackTrace);
        }

        public void Fatal(string message)
        {
            WriteLogLine(message, "FATAL");
        }

        public void Fatal(Exception exception)
        {
            WriteLogLine(exception.Message, "FATAL");
            WriteLine(exception.StackTrace);
        }

        public void Fatal(string message, Exception exception)
        {
            WriteLogLine(message, "FATAL");
            WriteLine(exception.Message);
            WriteLine(exception.StackTrace);
        }


        private void WriteLogLine(string message, string level)
        {
            WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} | {level,5} | {message}");
        }

        private void WriteLine(string message)
        {
            foreach (var line in Regex.Split(message, @"\r?\n"))
            {
                System.Diagnostics.Debug.WriteLine(">>> " + line);
            }
        }
    }
}
