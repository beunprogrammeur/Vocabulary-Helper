using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using VocabHelper.WPF.Business.ViewModels;
using VocabHelper.WPF.Extensions;
using VocabHelper.WPF.Factories;
using VocabHelper.WPF.Windows;

namespace VocabHelper.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IHost _host;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var builder = Host.CreateApplicationBuilder();

            builder.Services.AddDiscoveredServices();
            _host = builder.Build();
            _host.Services.GetRequiredService<IWindowFactory>()
                .GetMainWindow()
                .ShowDialog();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
        }
    }
}
