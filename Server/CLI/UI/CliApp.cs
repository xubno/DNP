using CLI.UI.ManageUsers;
using RepositoryContracts;

public class CliApp
{
    private readonly IUserRespository userRespository;

    public CliApp(IUserRespository userRespository, ICommentRespository commentRespository, IPostRespository postRespository)
    {
        this.userRespository = userRespository;
    }

    public async Task StartAsync()
    {
        var createUserView = new CreateUserView(userRespository);

        bool ExitApp = false;
        
        while (!ExitApp)
        {
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. Exit");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await createUserView.CreateUserAsync();
                    
                    break;
                case "2":
                    ExitApp = true;
                    Console.WriteLine("Exiting application");
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
}