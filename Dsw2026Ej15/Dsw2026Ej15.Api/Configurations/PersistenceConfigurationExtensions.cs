using Dsw2026Ej15.Data;
using Dsw2026Ej15.Domain.Interfaces;

namespace Dsw2026Ej15.Api.Configurations;

public static class PersistenceConfigurationExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IPersistence, PersistenceEF>();
        return services;
    }
}
