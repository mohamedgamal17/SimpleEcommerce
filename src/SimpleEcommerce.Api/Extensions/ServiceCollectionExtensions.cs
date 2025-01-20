using SimpleEcommerce.Api.Factories;
using SimpleEcommerce.Api.Services;
using System.Reflection;

namespace SimpleEcommerce.Api.Extensions
{
    public static class ServiceCollectionExtensions 
    {
        public static IServiceCollection AddResponseFactory(this IServiceCollection services , Assembly assembly)
        {
           var factoriesTypes = assembly.GetTypes()
                .Where(x => !x.IsAbstract && x.GetInterfaces().Any(c => c.IsGenericType && c.GetGenericTypeDefinition() == typeof(IResponseFactory<,>)))
                .ToList();

            foreach (var type in factoriesTypes)
            {
                services.AddTransient(type);
            }

            return services;
        }

        public static IServiceCollection AddApplicaitonService(this IServiceCollection services , Assembly assembly)
        {
            var servicesTypes = assembly.GetTypes()
                .Where(x => !x.IsAbstract && x.GetInterfaces().Any(c => c == typeof(IApplicationService)))
                .ToList();

            foreach (var serviceType in servicesTypes)
            {
                var serviceInterfaceType = serviceType.GetInterfaces().Where(x => x != typeof(IApplicationService)).FirstOrDefault();

                if(serviceInterfaceType != null)
                {
                    services.AddTransient(serviceInterfaceType, serviceType);
                }

                services.AddTransient(serviceType);
            }

            return services;
        }
    }
}
