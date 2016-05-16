using System;

namespace OneDo.Common.Logging
{
    public abstract class LoggerBase : ILogger
    {
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
            WriteLogLine(message, "TRACE");
        }

        public virtual void Debug(string message)
        {
            WriteLogLine(message, "DEBUG");
        }

        public virtual void Info(string message)
        {
            WriteLogLine(message, "INFO");
        }

        public virtual void Warn(string message)
        {
            WriteLogLine(message, "WARN");
        }

        public virtual void Warn(Exception exception)
        {
            WriteLogLine(exception.Message, "WARN");
            WriteLine(exception.StackTrace);
        }

        public virtual void Warn(string message, Exception exception)
        {
            WriteLogLine(message, "WARN");
            WriteLine(exception.Message);
            WriteLine(exception.StackTrace);
        }

        public virtual void Error(string message)
        {
            WriteLogLine(message, "ERROR");
        }

        public virtual void Error(Exception exception)
        {
            WriteLogLine(exception.Message, "ERROR");
            WriteLine(exception.StackTrace);
        }

        public virtual void Error(string message, Exception exception)
        {
            WriteLogLine(message, "ERROR");
            WriteLine(exception.Message);
            WriteLine(exception.StackTrace);
        }

        public virtual void Fatal(string message)
        {
            WriteLogLine(message, "FATAL");
        }

        public virtual void Fatal(Exception exception)
        {
            WriteLogLine(exception.Message, "FATAL");
            WriteLine(exception.StackTrace);
        }

        public virtual void Fatal(string message, Exception exception)
        {
            WriteLogLine(message, "FATAL");
            WriteLine(exception.Message);
            WriteLine(exception.StackTrace);
        }


        protected virtual string DateTimeFormat => "yyyy-MM-dd HH:mm:ss.fff";

        protected virtual void WriteLogLine(string message, string level)
        {
            WriteLine($"{DateTime.Now.ToString(DateTimeFormat)} | {level,-5} | {message}");
        }

        protected abstract void WriteLine(string message);
    }
}