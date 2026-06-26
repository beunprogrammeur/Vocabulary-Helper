using VocabHelper.Interfaces.Enums;
using VocabHelper.WPF.Business.EventArgs;

namespace VocabHelper.WPF.Business.Services
{
    public interface ILoggerService
    {
        event EventHandler<LogMessageEventArgs> MessageLogged;

        void LogError(string message, LogSeverity severity = LogSeverity.Error);
        void LogException(Exception exception, LogSeverity severity = LogSeverity.Error);
        void LogInformation(string message);
    }
}