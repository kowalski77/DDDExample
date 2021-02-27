using Microsoft.Extensions.DependencyInjection;
using SnackMachine.Domain.DomainServices;

namespace SnackMachine.Domain
{
    public static class DomainExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IExchangeBox, ExchangeBox>();
            services.AddScoped<AccountService>();

            return services;
        }
    }
}