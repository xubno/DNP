using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly IPostRespository postRespository;
    private readonly IUserRespository userRespository;



    public CreatePostView(IPostRespository postRespository,
        IUserRespository userRespository)
    {
        this.postRespository = postRespository;
        this.userRespository = userRespository;
    }


    public async Task CreatePostAsync()
    {
        var users = userRespository.GetMany().ToList();
        if (!users.Any())
        {
            Console.WriteLine("No users available to create a post.");
            return;
        }
        Console.WriteLine("Select a user ID to create a post:");
        foreach (var user in users)
        {
            Console.WriteLine($"User ID: {user.Id}, Username: {user.Username}");
        }
        
        Console.WriteLine("Enter the User ID:");
        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Invalid User ID.");
            return;
        }
        
        
        Console.WriteLine("enter post title:");
        string title = Console.ReadLine() ?? "Untitled Post";
        
        Console.WriteLine("enter post body:");
        string body = Console.ReadLine() ?? "no body";

        int postId = postRespository.GetMany().Any()
            ? postRespository.GetMany().Max(x => x.Id) + 1
            : 1;
        
        var newPost = new Post(postId, title, body, userId);
        await postRespository.AddAsync(newPost);
        
        Console.WriteLine($"post {title} created successfully for user {userId} with post id {newPost.Id}"); 

    }
}