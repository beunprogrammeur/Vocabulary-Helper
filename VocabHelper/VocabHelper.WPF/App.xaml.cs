using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using VocabHelper.WPF.Business.Services;
using VocabHelper.WPF.Business.ViewModels;
using VocabHelper.WPF.Services;

namespace VocabHelper.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            IServiceCollection services = new ServiceCollection();

            // Register Business-layer services
            BusinessServicesFactory.Configure(services);
            services.AddSingleton<IFileSelectionService, FileSelectionService>();

            // Register ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<EBookViewModel>();
            services.AddTransient<CardCandidateViewModel>();

            // Build the DI container
            Services = services.BuildServiceProvider();

            base.OnStartup(e);
        }
    }

}
