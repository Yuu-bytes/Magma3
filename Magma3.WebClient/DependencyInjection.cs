using Flurl.Http.Configuration;
using Magma3.WebClient.Config;
using Magma3.WebClient.Interface;
using Magma3.WebClient.WebClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Magma3.WebClient
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMagma3Dependecy(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IFlurlClientFactory, DefaultFlurlClientFactory>();
            services.AddSingleton<IMagma3WebClient, Magma3WebClient>();

            var magma3ConfigSection = configuration.GetSection("Magma3Config");
            services.Configure<Magma3Config>(magma3ConfigSection);

            services.AddSingleton<IMagma3Config>(sp => sp.GetRequiredService<IOptions<Magma3Config>>().Value);

            return services;
        }
    }
}
