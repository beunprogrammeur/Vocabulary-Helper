namespace VocabHelper.Interfaces
{
    public interface IDispatcher
    {
        void Invoke(Action action);
    }
}