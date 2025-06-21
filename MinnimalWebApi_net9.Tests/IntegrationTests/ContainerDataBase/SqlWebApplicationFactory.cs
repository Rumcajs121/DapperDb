using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinnimalWebApi_net9.Endpoints;
using Testcontainers.MsSql;

namespace IntegrationTests;

public class SqlWebApplicationFactory:WebApplicationFactory<Program>
{
    private readonly SqlContainerFixture _fixture;

    public SqlWebApplicationFactory(SqlContainerFixture fixture)
    {
        _fixture = fixture;
    }
    public string ConnectionString => _fixture.ConnectionString;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddSingleton<ISqlDataAcces>(new SqlDataAcces(new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "ConnectionStrings:MinimalWebApi", _fixture.ConnectionString}
                })
                .Build()));
            services.AddSingleton<IToDoListData, ToDoListData>();
        });
    }
}