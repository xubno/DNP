using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRespository
{
    private readonly string filePath = "users.json";
        
    public UserFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }
    public async Task<User> AddAsync(User user)
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        int maxId = users.Count > 0 ? users.Max(c => c.Id) : 1;
        user.Id = maxId + 1;
        users.Add(user);
        usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath, usersAsJson);
        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        int maxId = users.Count > 0 ? users.Max(c => c.Id) : 1;
        user.Id = maxId + 1;
        users.Remove(user);
        users.Add(user);
        usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath, usersAsJson);
        return user;
    }

    public async Task DeleteAsync(int id)
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        User? userToDelete = users.FirstOrDefault(u => u.Id == id);
        if (userToDelete != null)
        {
            users.Remove(userToDelete);
            
            usersAsJson = JsonSerializer.Serialize(users);
            await File.WriteAllTextAsync(filePath, usersAsJson);
        }
    }

    public async Task<User> GetSingleAsync(int id)
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        User? user = users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            throw new Exception($"User with id {id} not found.");
        }

        return user;
    }

    public IQueryable<User> GetMany()
    {
        var usersAsJson = File.ReadAllTextAsync(filePath).Result;
        return JsonSerializer.Deserialize<List<User>>(usersAsJson)!.AsQueryable();
    }
}