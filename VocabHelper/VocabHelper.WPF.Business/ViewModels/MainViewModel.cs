using VocabHelper.Interfaces;

namespace VocabHelper.WPF.Business.ViewModels
{
    [RegisterService]
    public class MainViewModel : BaseViewModel
    {
        public EBookViewModel EBookViewModel { get; set; }
        public MainViewModel(EBookViewModel eBookViewModel)
        {
            EBookViewModel = eBookViewModel;
        }
    }
}
