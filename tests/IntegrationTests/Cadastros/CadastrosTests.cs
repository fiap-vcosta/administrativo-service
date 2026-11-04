using System.Net.Http.Json;
using System.Text.Json;
using IntegrationTests.Infrastructure;

namespace IntegrationTests.Cadastros;

[Collection(nameof(IntegrationFixture))]
public class CadastrosTests
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly HttpClient _client;

    public CadastrosTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Cadastros_CrudHappyPath_AsInDemoCollection()
    {
        // Arrange
        AuthHelper.AuthenticateAsStaff(_client);
        const string documento = "92561324354";

        // Act / Assert — cliente
        var createCliente = await _client.PostAsJsonAsync("/administrativo/clientes", new
        {
            Nome = "Cliente Integração",
            Email = "integracao@teste.com",
            TipoDocumento = "Cpf",
            Documento = documento
        });
        createCliente.EnsureSuccessStatusCode();
        var cliente = await createCliente.Content.ReadFromJsonAsync<IdResponse>(JsonOptions);
        Assert.NotNull(cliente);

        var getCliente = await _client.GetAsync($"/administrativo/clientes/{cliente.Id}");
        getCliente.EnsureSuccessStatusCode();

        var updateCliente = await _client.PutAsJsonAsync($"/administrativo/clientes/{cliente.Id}", new
        {
            Nome = "Cliente Integração Atualizado",
            Email = "integracao.atualizado@teste.com",
            TipoDocumento = "Cpf",
            Documento = "29040653186"
        });
        updateCliente.EnsureSuccessStatusCode();

        var createVeiculo = await _client.PostAsJsonAsync("/administrativo/veiculos", new
        {
            Placa = "ABC1D23",
            IdCliente = cliente.Id,
            Modelo = "Gol",
            Marca = "Volkswagen"
        });
        createVeiculo.EnsureSuccessStatusCode();
        var veiculo = await createVeiculo.Content.ReadFromJsonAsync<IdResponse>(JsonOptions);
        Assert.NotNull(veiculo);

        var getVeiculo = await _client.GetAsync($"/administrativo/veiculos/{veiculo.Id}");
        getVeiculo.EnsureSuccessStatusCode();

        var veiculosDoCliente = await _client.GetAsync($"/administrativo/clientes/{cliente.Id}/veiculos");
        veiculosDoCliente.EnsureSuccessStatusCode();

        var getClienteComVeiculos = await _client.GetAsync($"/administrativo/clientes/{cliente.Id}");
        getClienteComVeiculos.EnsureSuccessStatusCode();
        var clienteComVeiculos = await getClienteComVeiculos.Content.ReadFromJsonAsync<ClienteComVeiculosResponse>(JsonOptions);
        Assert.NotNull(clienteComVeiculos);
        Assert.Contains(clienteComVeiculos.Veiculos, v => v.Id == veiculo.Id && v.Placa == "ABC1D23");

        var createServico = await _client.PostAsJsonAsync("/administrativo/servicos", new
        {
            Codigo = "OLE-999",
            Nome = "Serviço Integração",
            PrecoPadrao = 150m,
            Ativo = true
        });
        createServico.EnsureSuccessStatusCode();
        var servico = await createServico.Content.ReadFromJsonAsync<IdResponse>(JsonOptions);
        Assert.NotNull(servico);

        var getServico = await _client.GetAsync($"/administrativo/servicos/{servico.Id}");
        getServico.EnsureSuccessStatusCode();

        using var systemRequest = new HttpRequestMessage(HttpMethod.Get, $"/system/administrativo/veiculos/{veiculo.Id}");
        systemRequest.Headers.Add("X-Service-Key", "integration-service-auth-key");
        var systemVeiculo = await _client.SendAsync(systemRequest);
        systemVeiculo.EnsureSuccessStatusCode();

        using var systemServicoRequest = new HttpRequestMessage(HttpMethod.Get, $"/system/administrativo/servicos/{servico.Id}");
        systemServicoRequest.Headers.Add("X-Service-Key", "integration-service-auth-key");
        var systemServico = await _client.SendAsync(systemServicoRequest);
        systemServico.EnsureSuccessStatusCode();
    }

    private sealed record IdResponse(int Id);

    private sealed record ClienteComVeiculosResponse(IReadOnlyList<VeiculoResumoResponse> Veiculos);

    private sealed record VeiculoResumoResponse(int Id, string Placa);
}
