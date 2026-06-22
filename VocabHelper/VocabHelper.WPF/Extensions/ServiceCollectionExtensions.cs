using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using VocabHelper.Interfaces;

namespace VocabHelper.WPF.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDiscoveredServices(this IServiceCollection services)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName != null && x.FullName.StartsWith(nameof(VocabHelper), StringComparison.OrdinalIgnoreCase));

            var servicesToBeRegistered = assemblies
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsClass && !x.IsAbstract)
                .Select(x => new
                {
                    Type = x,
                    Attribute = x.GetCustomAttribute<RegisterServiceAttribute>()
                })
                .Where(x => x.Attribute != null);

            foreach (var service in servicesToBeRegistered)
            {
                var attribute = service.Attribute;
                var attributeType = attribute!.GetType();
                var interfaceType = attribute.InterfaceType ?? service.Type;

                switch (attribute.Lifetime)
                {
                    case RegistrationType.Transient:
                        services.AddTransient(interfaceType, service.Type);
                        break;
                    case RegistrationType.Scoped:
                        services.AddScoped(interfaceType, service.Type);
                        break;
                    case RegistrationType.Singleton:
                        services.AddSingleton(interfaceType, service.Type);
                        break;
                }
            }
        }
    }
}
