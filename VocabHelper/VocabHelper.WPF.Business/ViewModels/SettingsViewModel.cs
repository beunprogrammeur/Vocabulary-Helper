using VocabHelper.Interfaces;
using VocabHelper.WPF.Business.ViewModels.Persistence;

namespace VocabHelper.WPF.Business.ViewModels
{
    [RegisterService]
    public partial class SettingsViewModel : BaseViewModel
    {
        public AppSettings AppSettings { get; }
        public SettingsViewModel(AppSettings appSettings)
        {
            AppSettings = appSettings;
        }
    }
}
