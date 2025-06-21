using MinimalWebApi_net9.Models;

namespace MinimalWebApi_net9.Endpoints;

public interface IToDoListData
{
    Task<IEnumerable<Assignment>> GetAllToDoListS();
    Task <Assignment?> GetToDoList(int id);
    Task InsertToDoList(AssignmentDto assignment);
    Task UpdateTask(int id, AssignmentDto assignment);
    Task DeleteTask(int id);
}