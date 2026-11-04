using System.Text.Json.Serialization;
using Api.Contracts.Validation;
using Api.Controllers.Staff.Cliente.CreateCliente;
using Api.Controllers.Staff.Cliente.UpdateCliente;
using Api.Controllers.Staff.Servico.CreateServico;
using Api.Controllers.Staff.Servico.UpdateServico;
using Api.Controllers.Staff.Veiculo.CreateVeiculo;
using Api.Controllers.Staff.Veiculo.UpdateVeiculo;
using Api.Presenters.Cliente;
using Api.Presenters.Servico;
using Api.Presenters.Veiculo;
using BuildingBlocks.Http.Auth;
using BuildingBlocks.Http.ProblemDetails;
using Domain.Exceptions;
using Infrastructure.DependencyInjection;

namespace Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);

        services.AddScoped<ClientePresenter>();
        services.AddScoped<VeiculoPresenter>();
        services.AddScoped<ServicoPresenter>();

        services.AddSingleton<IValidator<CreateClienteRequest>, CreateClienteRequestValidator>();
        services.AddSingleton<IValidator<UpdateClienteRequest>, UpdateClienteRequestValidator>();
        services.AddSingleton<IValidator<CreateVeiculoRequest>, CreateVeiculoRequestValidator>();
        services.AddSingleton<IValidator<UpdateVeiculoRequest>, UpdateVeiculoRequestValidator>();
        services.AddSingleton<IValidator<CreateServicoRequest>, CreateServicoRequestValidator>();
        services.AddSingleton<IValidator<UpdateServicoRequest>, UpdateServicoRequestValidator>();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(Application.UseCases.Cliente.Commands.CreateCliente.CreateClienteCommand).Assembly));

        services.AddBuildingBlocksProblemDetails(options =>
        {
            options.Map<DomainNotFoundException>(StatusCodes.Status404NotFound, "Not Found");
            options.Map<KeyNotFoundException>(StatusCodes.Status404NotFound, "Not Found");
            options.Map<BusinessRuleException>(StatusCodes.Status400BadRequest, "Business Rule Violation");
            options.Map<ArgumentException>(StatusCodes.Status400BadRequest, "Bad Request");
            options.Map<UnauthorizedAccessException>(StatusCodes.Status401Unauthorized, "Unauthorized");
        });

        services.AddControllers(options =>
            {
                options.Filters.Add<ProblemDetailsExceptionFilter>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.AddBuildingBlocksJwtAuthentication(configuration);
    }
}
