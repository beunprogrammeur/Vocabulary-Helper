namespace VocabHelper.WPF.Business.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public EBookViewModel EBookViewModel { get; set; }
        public MainViewModel(EBookViewModel eBookViewModel)
        {
            EBookViewModel = eBookViewModel;
        }
    }
}
