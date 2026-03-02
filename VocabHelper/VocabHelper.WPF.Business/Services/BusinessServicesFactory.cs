using Microsoft.Extensions.DependencyInjection;

namespace VocabHelper.WPF.Business.Services
{
    public static class BusinessServicesFactory
    {
        public static IServiceCollection Configure(IServiceCollection services)
        {
            services.AddSingleton<IAnkiService, AnkiService>();
            services.AddSingleton<ITranslationService, TranslationService>();
            services.AddSingleton<IEBookService, EBookService>();
            services.AddSingleton<IStemmerServiceFactory, StemmerServiceFactory>();
            return services;
        }
    }
}
