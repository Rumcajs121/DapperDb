namespace MinimalWebApi_net9.Endpoints;

public interface ISqlDataAcces
{
    Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters, string connetionID = "MinimalWebApi");

    // Task SaveData<T>(string storedProcedure, T parameters, string connetionID = "MinimalWebApi");
    Task SaveData<T>(string storedProcedure, T parameters, string connetionID = "MinimalWebApi");
}