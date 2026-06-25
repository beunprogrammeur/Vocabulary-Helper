using CommunityToolkit.Mvvm.ComponentModel;
using VocabHelper.Interfaces;
using VocabHelper.WPF.Business.Models;

namespace VocabHelper.WPF.Business.ViewModels
{
    [RegisterService(RegistrationType.Singleton)]
    public partial class StatusViewModel : BaseViewModel
    {
        [ObservableProperty] private int completedTranslations;
        [ObservableProperty] private int totalTranslations;

        public VocabularyRepositoryModel WordRepository { get; set; } = new VocabularyRepositoryModel();

        public event EventHandler WordRepositoryUpdated;

        public StatusViewModel()
        {

        }

        public void UpdateWordRepository()
        {
            WordRepositoryUpdated?.Invoke(this, System.EventArgs.Empty);
        }
    }
}
