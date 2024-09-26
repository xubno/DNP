using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly IUserRespository userRespository;

    public CreateUserView(IUserRespository userRespository)
    {
        this.userRespository = userRespository;
    }
    
    public async Task CreateUserAsync()
    {
        bool addUser = true;

        while (addUser)
        {
           
            string username;
            do
            {
                Console.WriteLine("Enter username: ");
                username = Console.ReadLine();

                
                var existingUser = userRespository.GetMany().FirstOrDefault(u => u.Username == username);
                if (existingUser != null)
                {
                    Console.WriteLine("Username is already taken. Please choose another one.");
                }
                else
                {
                    break; 
                }
            } while (true);

           
            Console.WriteLine("Enter password: ");
            string password = Console.ReadLine();
            
            int newId = userRespository.GetMany().Any() 
                ? userRespository.GetMany().Max(u => u.Id) + 1 
                : 1;
            
            var newUser = new User(username, password, newId);
            await userRespository.AddAsync(newUser);

            Console.WriteLine($"User '{username}' created successfully with ID: {newUser.Id}");
            
            Console.WriteLine("Would you like to create a new user? (y/n)");
            string response = Console.ReadLine();

            if (response.ToLower() != "y")
            {
                addUser = false;
                Console.WriteLine("Exiting user creation");
            }
        }
    }
}