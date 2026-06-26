using VocabHelper.Interfaces;
using VocabHelper.Interfaces.Enums;
using VocabHelper.WPF.Business.EventArgs;

namespace VocabHelper.WPF.Business.Services
{
    [RegisterService<ILoggerService>(RegistrationType.Singleton)]
    public class LoggerService : ILoggerService
    {
        public event EventHandler<LogMessageEventArgs> MessageLogged;
        public void LogInformation(string message) => 
            MessageLogged?.Invoke(this, new LogMessageEventArgs(message, LogSeverity.Info));
        public void LogError(string message, LogSeverity severity = LogSeverity.Error) => 
            MessageLogged?.Invoke(this, new LogMessageEventArgs(message, severity));
        public void LogException(Exception exception, LogSeverity severity = LogSeverity.Error) => 
            MessageLogged?.Invoke(this, new LogMessageEventArgs(exception.Message + "\n" + exception.StackTrace, severity));
    }
}
