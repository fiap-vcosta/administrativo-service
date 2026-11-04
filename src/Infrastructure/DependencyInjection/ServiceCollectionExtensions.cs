using Application.Abstractions.Gateways;
using Infrastructure.Database;
using Infrastructure.Database.Gateways;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IClienteGateway, ClienteGateway>();
        services.AddScoped<IVeiculoGateway, VeiculoGateway>();
        services.AddScoped<IServicoGateway, ServicoGateway>();

        return services;
    }
}
