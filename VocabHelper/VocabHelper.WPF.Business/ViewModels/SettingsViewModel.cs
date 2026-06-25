using VocabHelper.Interfaces;
using VocabHelper.WPF.Business.ViewModels.Persistence;

namespace VocabHelper.WPF.Business.ViewModels
{
    [RegisterService]
    public partial class SettingsViewModel : BaseViewModel
    {
        public PersistentSettings AppSettings { get; }
        public SettingsViewModel(PersistentSettings appSettings)
        {
            AppSettings = appSettings;
        }
    }
}
