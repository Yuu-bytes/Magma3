using Magma3.Infraestructure.Interfaces;
using Magma3.Infraestructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Magma3.Infraestructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfraDependency(this IServiceCollection services, string databasePath = "Data/Magma3.db")
        {
            var directory = Path.GetDirectoryName(databasePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddSingleton<IProductRepository>(sp => new LiteDbProductRepository(databasePath));

            services.AddSingleton<IClienteRepository>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                return new MongoDbClienteRepository(configuration["MongoDB:ConnectionString"]!, configuration["MongoDB:DatabaseName"]!);
            });

            return services;
        }
    }
}
