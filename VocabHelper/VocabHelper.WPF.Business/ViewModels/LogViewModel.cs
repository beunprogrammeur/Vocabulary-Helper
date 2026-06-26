using System.Collections.ObjectModel;
using VocabHelper.Interfaces;
using VocabHelper.WPF.Business.Services;

namespace VocabHelper.WPF.Business.ViewModels
{
    [RegisterService]
    public partial class LogViewModel : BaseViewModel
    {
        private const int MaxLog = 300;

        private readonly ILoggerService _loggerService;
        private readonly IDispatcher _dispatcher;

        public ObservableCollection<string> LogEntries { get; } = [];


        public LogViewModel(ILoggerService loggerService, IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            _loggerService = loggerService;
            _loggerService.MessageLogged += OnMessageLogged;
        }

        private void OnMessageLogged(object? sender, EventArgs.LogMessageEventArgs e) =>
            _dispatcher.Invoke(() => {
                LogEntries.Add(e.Message);

                if(LogEntries.Count > MaxLog)
                {
                    LogEntries.RemoveAt(0);
                }
            });

    }
}
