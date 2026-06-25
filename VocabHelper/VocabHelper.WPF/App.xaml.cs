using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;
using VocabHelper.WPF.Business.ViewModels.Persistence;
using VocabHelper.WPF.Extensions;
using VocabHelper.WPF.Factories;

namespace VocabHelper.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly string _persistenceFilePath = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "persistence.json");
        private readonly ushort _apiDefaultPort = 8083;

        private IHost _host;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var builder = Host.CreateApplicationBuilder();

            builder.Services.AddDiscoveredServices();

            builder.Services.AddSingleton<AppSettings>((services) => LoadPersistence());

            _host = builder.Build();

            // load after the DI has finished registering objects (inlcuding our persistent settings object);
            LoadPersistence();

            _host.Services.GetRequiredService<IWindowFactory>()
                .GetMainWindow()
                .ShowDialog();
        }

        private AppSettings LoadPersistence()
        {
            AppSettings appSettings = new ();

            if (File.Exists(_persistenceFilePath))
            {
                var jsonContent = File.ReadAllText(_persistenceFilePath);
                try
                {
                    appSettings = JsonSerializer.Deserialize<AppSettings>(jsonContent);
                }
                catch { /* log? */ }
            }
            else
            {
                try
                {
                    string directory = Path.GetDirectoryName(_persistenceFilePath);

                    // traverse the paths up to find our system prompt
                    for (int i = 0; i < 10; i++)
                    {
                        string systemPromptPath = Path.Combine(directory, "Docker", "system_prompt.md");
                        string envFilePath = Path.Combine(directory, "Docker", ".env");
                        if (File.Exists(systemPromptPath))
                        {
                            string prompt = File.ReadAllText(systemPromptPath);
                            appSettings.ApiSettings.SystemPrompt = prompt;

                            string portLine = File.ReadAllLines(envFilePath).FirstOrDefault(x => x.StartsWith("LLAMA_PORT"));
                            int equalIndex = portLine.IndexOf('=');
                            if(equalIndex != -1 && ushort.TryParse(portLine.Substring(equalIndex + 1), out ushort port))
                            {
                                appSettings.ApiSettings.Port = port;
                            }
                            else
                            {
                                appSettings.ApiSettings.Port = _apiDefaultPort;
                            }
                            break;
                        }

                        // move one folder up.
                        directory = Path.GetDirectoryName(directory);
                    }
                }
                catch { /* log */ }
            }

            return appSettings;
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                AppSettings appSettings = _host.Services.GetRequiredService<AppSettings>();
                var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
                string jsonContent = JsonSerializer.Serialize(appSettings, jsonOptions);

                File.WriteAllText(_persistenceFilePath, jsonContent);
            }
            catch { /* log? */ }
        }
    }
}
