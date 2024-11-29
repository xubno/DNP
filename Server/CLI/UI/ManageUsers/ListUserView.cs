using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUserView (IUserRespository userRepository) : IConsoleView

{
    
    private readonly IUserRespository _userRepository = userRepository;

    private Task ListUsers()
    {
        string output = "";
        foreach(var user in _userRepository.GetMany())
        {
            PrintUser(user);
        }
        return Task.CompletedTask;
    }

    private static void PrintUser(User user)
    {
        Console.WriteLine($"User:{user.Id} - with username : {user.Username} and password : {user.Password}");
    }


    public Task ShowConsoleContent()
    {
        return ListUsers();
    }
}