using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using VocabHelper.WPF.Business.ViewModels;

namespace VocabHelper.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = App.Services.GetRequiredService<MainViewModel>();
        }
    }
}