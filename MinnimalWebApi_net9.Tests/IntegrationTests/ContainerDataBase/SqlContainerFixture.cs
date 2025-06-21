using Testcontainers.MsSql;

namespace IntegrationTests;

public class SqlContainerFixture:IAsyncLifetime
{
    private readonly MsSqlContainer _sqlServerContainer;
    private DatabaseInitializer? _dataBaseInitializer;
    public SqlContainerFixture()
    {
        _sqlServerContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server")
            .WithCleanUp(true)
            .Build();
        
    }
    public string ConnectionString => _sqlServerContainer.GetConnectionString();

    public async Task InitializeAsync()
    {
        await _sqlServerContainer.StartAsync();
        _dataBaseInitializer = new DatabaseInitializer(ConnectionString);
        await _dataBaseInitializer.InitializeDatabaseAsync();
    }

    public async Task DisposeAsync()
    {
        await _sqlServerContainer.StopAsync();
    }
}