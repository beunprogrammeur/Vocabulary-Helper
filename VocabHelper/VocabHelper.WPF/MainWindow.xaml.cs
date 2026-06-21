using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using VocabHelper.Interfaces;
using VocabHelper.WPF.Business.ViewModels;
using VocabHelper.WPF.Windows;

namespace VocabHelper.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [RegisterService<IWindow<MainViewModel>>]
    public partial class MainWindow : Window, IWindow<MainViewModel>
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
        }

        public MainViewModel ViewModel { get => DataContext as MainViewModel; set => DataContext = value; }
    }
}