using CommunityToolkit.Mvvm.ComponentModel;

namespace VocabHelper.WPF.Business.ViewModels.Persistence
{
    public partial class ApiSettings : BaseViewModel
    {
        [ObservableProperty] private string systemPrompt;
        [ObservableProperty] private string apiEndpoint;
    }
}
