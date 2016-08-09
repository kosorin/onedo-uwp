using System;

namespace OneDo.Common.Logging
{
    public interface ILogger
    {
        string DateTimeFormat { get; set; }

        void Line();

        void Line(string message);

        void Trace(string message);

        void Trace(string message, Exception exception);

        void Debug(string message);

        void Debug(string message, Exception exception);

        void Info(string message);

        void Info(string message, Exception exception);

        void Warn(string message);

        void Warn(Exception exception);

        void Warn(string message, Exception exception);

        void Error(string message);

        void Error(Exception exception);

        void Error(string message, Exception exception);

        void Fatal(string message);

        void Fatal(Exception exception);

        void Fatal(string message, Exception exception);
    }
}
