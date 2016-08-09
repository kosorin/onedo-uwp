using System;

namespace OneDo.Common.Logging
{
    public abstract class LoggerBase : ILogger
    {
        protected readonly object syncObject = new object();

        public virtual string DateTimeFormat { get; set; } = "yyyy-MM-dd HH:mm:ss.fff";

        public virtual void Line()
        {
            WriteLine("");
        }

        public virtual void Line(string message)
        {
            WriteLine(message);
        }

        public virtual void Trace(string message)
        {
            WriteLogLine("TRACE", message);
        }

        public virtual void Trace(string message, Exception exception)
        {
            WriteLogLine("TRACE", message);
            WriteLine(exception.Message);
            WriteLine(exception.StackTrace);
        }

        public virtual void Debug(string message)
        {
            WriteLogLine("DEBUG", message);
        }

        public virtual void Debug(string message, Exception exception)
        {
            WriteLogLine("DEBUG", message);
            WriteLine(exception.Message);
            WriteLine(exception.StackTrace);
        }

        public virtual void Info(string message)
        {
            WriteLogLine("INFO", message);
        }

        public virtual void Info(string message, Exception exception)
        {
            WriteLogLine("INFO", message);
            WriteLine(exception.Message);
            WriteLine(exception.StackTrace);
        }

        public virtual void Warn(string message)
        {
            WriteLogLine("WARN", message);
        }

        public virtual void Warn(Exception exception)
        {
            WriteLogLine("WARN", exception.Message);
            WriteLine(exception.StackTrace);
        }

        public virtual void Warn(string message, Exception exception)
        {
            WriteLogLine("WARN", message);
            WriteLine(exception.Message);
            WriteLine(exception.StackTrace);
        }

        public virtual void Error(string message)
        {
            WriteLogLine("ERROR", message);
        }

        public virtual void Error(Exception exception)
        {
            WriteLogLine("ERROR", exception.Message);
            WriteLine(exception.StackTrace);
        }

        public virtual void Error(string message, Exception exception)
        {
            WriteLogLine("ERROR", message);
            WriteLine(exception.Message);
            WriteLine(exception.StackTrace);
        }

        public virtual void Fatal(string message)
        {
            WriteLogLine("FATAL", message);
        }

        public virtual void Fatal(Exception exception)
        {
            WriteLogLine("FATAL", exception.Message);
            WriteLine(exception.StackTrace);
        }

        public virtual void Fatal(string message, Exception exception)
        {
            WriteLogLine("FATAL", message);
            WriteLine(exception.Message);
            WriteLine(exception.StackTrace);
        }


        protected virtual void WriteLogLine(string level, string message)
        {
            WriteLine($"{DateTime.Now.ToString(DateTimeFormat)} | {level,-5} | {message}");
        }

        protected abstract void WriteLine(string message);
    }
}