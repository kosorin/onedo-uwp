using System;

namespace OneDo.Common.Logging
{
    public class NullLogger : ILogger
    {
        public string DateTimeFormat { get; set; }

        public void Line() { }

        public void Line(string message) { }

        public void Trace(string message) { }

        public void Debug(string message) { }

        public void Info(string message) { }

        public void Warn(string message) { }

        public void Warn(Exception exception) { }

        public void Warn(string message, Exception exception) { }

        public void Error(string message) { }

        public void Error(Exception exception) { }

        public void Error(string message, Exception exception) { }

        public void Fatal(string message) { }

        public void Fatal(Exception exception) { }

        public void Fatal(string message, Exception exception) { }
    }
}