using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using RepositoryContracts;

public class CliApp
{
    private readonly IUserRespository userRespository;
    private readonly ICommentRespository commentRespository;
    private readonly IPostRespository postRespository;
    

    public CliApp(IUserRespository userRespository, ICommentRespository commentRespository, IPostRespository postRespository)
    {
        this.userRespository = userRespository;
        this.commentRespository = commentRespository;
        this.postRespository = postRespository;
    }

    public async Task StartAsync()
    {
        var createUserView = new CreateUserView(userRespository);
        var createPostView = new CreatePostView(postRespository, userRespository);
        var addCommentView = new AddCommentToPost(commentRespository, postRespository);
        var listPostView = new ListPostView(commentRespository, postRespository);

        bool exitApp = false;

        while (!exitApp)
        {
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. Create Post");
            Console.WriteLine("3. Add Comment to Post");
            Console.WriteLine("4. View posts overview");
            Console.WriteLine("5. Exit");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await createUserView.CreateUserAsync();
                    break;
                case "2":
                    await createPostView.CreatePostAsync();
                    break;
                case "3":
                    await addCommentView.AddCommentAsync();
                    break;
                case "4" :
                    await listPostView.ListPostAsync();
                    break;
                    
                case "5":
                    exitApp = true;
                    Console.WriteLine("Exiting application");
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
}