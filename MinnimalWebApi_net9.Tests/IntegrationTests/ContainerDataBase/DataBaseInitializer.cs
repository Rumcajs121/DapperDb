using System.Data;
using Microsoft.Data.SqlClient;

namespace IntegrationTests;

public class DatabaseInitializer
{
    private readonly string _connectionString;

    public DatabaseInitializer(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task InitializeDatabaseAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await ExecuteNonQueryAsync(connection, @"
            IF OBJECT_ID('dbo.Assignment', 'U') IS NULL
            CREATE TABLE dbo.Assignment (
                Id INT PRIMARY KEY IDENTITY(1,1),
                Description NVARCHAR(255) NOT NULL,
                IsCompleted BIT NOT NULL,
                EndDate DATE NOT NULL
            );");

        await ExecuteNonQueryAsync(connection, @"
            INSERT INTO dbo.Assignment (Description, IsCompleted, EndDate)
            VALUES
            ('Task 1', 0, '2025-06-30'),
            ('Task 2', 1, '2025-07-15'),
            ('Task 3', 0, '2025-08-01');");

        await ExecuteNonQueryAsync(connection, @"
            IF OBJECT_ID('dbo.spToDoListsDb_GettAll', 'P') IS NULL
            EXEC('
                CREATE PROCEDURE dbo.spToDoListsDb_GettAll
                AS
                BEGIN
                    SELECT * FROM dbo.Assignment;
                END
            ')");
        await ExecuteNonQueryAsync(connection, @"
IF OBJECT_ID('dbo.spToDoListsDb_CreateNewTask_Throws', 'P') IS NULL
EXEC('
    CREATE PROCEDURE dbo.spToDoListsDb_CreateNewTask_Throws
        @Description NVARCHAR(255),
        @EndDate DATE
    AS
    BEGIN
        INSERT INTO dbo.Assignment (Description, IsCompleted, EndDate)
        VALUES (@Description, 0, @EndDate);

        THROW 50000, ''Simulated error after insert.'', 1;
    END
')");
    }


    private async Task ExecuteNonQueryAsync(SqlConnection connection, string query)
    {
        using var command = new SqlCommand(query, connection);
        await command.ExecuteNonQueryAsync();
    }
}