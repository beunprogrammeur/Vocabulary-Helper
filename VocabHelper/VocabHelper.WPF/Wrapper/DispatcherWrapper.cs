using System.Windows.Threading;
using VocabHelper.Interfaces;

namespace VocabHelper.WPF.Wrapper
{
    [RegisterService<IDispatcher>(RegistrationType.Singleton)]
    public class DispatcherWrapper : IDispatcher
    {
        private readonly Dispatcher _dispatcher;
        public DispatcherWrapper()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        public void Invoke(Action action)
        {
            _dispatcher.Invoke(action);
        }
    }
}
