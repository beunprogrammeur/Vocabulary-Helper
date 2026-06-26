using VocabHelper.Interfaces.Enums;

namespace VocabHelper.WPF.Business.EventArgs
{
    public class LogMessageEventArgs : System.EventArgs
    {
        public LogSeverity Severity { get; set; }
        public string Message { get; set; }
        public LogMessageEventArgs(string message, LogSeverity severity)
        {
            Message = message;
            Severity = severity;
        }
    }
}
