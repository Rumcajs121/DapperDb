using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using MinnimalWebApi_net9.Endpoints;
using MinnimalWebApi_net9.Models;

namespace IntegrationTests;

public class ToDoListIntegrationTests:IClassFixture<SqlContainerFixture>
{
        private readonly SqlWebApplicationFactory _factory;

        public ToDoListIntegrationTests(SqlContainerFixture fixture)
        {
            _factory = new SqlWebApplicationFactory(fixture);
        }
    [Fact]
    public async Task TestDatabaseConnection()
    {
        using (var connection = new SqlConnection(_factory.ConnectionString))
        {
            await connection.OpenAsync();
            Assert.Equal(ConnectionState.Open, connection.State); 
        }
    }
    [Fact]
    public async Task TestGetAllToDoListS()
    {
        // Arrange
        var toDoListData = _factory.Services.GetRequiredService<IToDoListData>();

        // Act
        var result = await toDoListData.GetAllToDoListS();

        // Assert
        Assert.NotNull(result);  
        Assert.NotEmpty(result); 
    }
    [Fact]
    public async Task InsertToDoList_ShouldRollback_OnException()
    {
        var toDoListData = _factory.Services.GetRequiredService<IToDoListData>();

        var faultyAssignment = new AssignmentDto
        {
            Description = "Rollback test",
            EndDate = DateTime.Today
        };
        
        var exceptionThrown = false;
        try
        {
            await _factory.Services.GetRequiredService<ISqlDataAcces>()
                .SaveData("spToDoListsDb_CreateNewTask_Throws", new
                {
                    Description = faultyAssignment.Description,
                    EndDate = faultyAssignment.EndDate
                });
        }
        catch
        {
            exceptionThrown = true;
        }
        
        Assert.True(exceptionThrown);

        var allTasks = await toDoListData.GetAllToDoListS();
        Assert.DoesNotContain(allTasks, t => t.Description == "Rollback test");
    }

}
