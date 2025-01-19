using SimpleEcommerce.Api.Factories;
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
    }
}
