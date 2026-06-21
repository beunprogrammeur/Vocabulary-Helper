using VocabHelper.WPF.Business.ViewModels;

namespace VocabHelper.WPF.Windows
{
    public interface IWindow<T> where T : class
    {
        T ViewModel { get; set; }
        bool? ShowDialog();
    }
}
