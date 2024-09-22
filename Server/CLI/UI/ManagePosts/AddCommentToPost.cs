using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class AddCommentToPost
{
    private readonly ICommentRespository commentRespository;
    private readonly IPostRespository postRespository;
    

    public AddCommentToPost(ICommentRespository commentRespository,IPostRespository postRespository)
    {
        
        this.commentRespository = commentRespository;
        this.postRespository = postRespository;
    }


    public async Task AddCommentAsync()
    {
        var posts = postRespository.GetMany().ToList();
        if (!posts.Any())
        {
            Console.WriteLine("no posts found");
            return;
        }
        
        Console.WriteLine("select a post to comment on");
        foreach (var post in posts)
        {
            Console.WriteLine($"post ID: {post.Id}, title: {post.Title},");
        }
        
        Console.WriteLine("enter the post ID to comment on");
        if (!int.TryParse(Console.ReadLine(), out int postId))
        {
            Console.WriteLine("invalid post ID");
            return;
        }

        var postToCommentOn = await postRespository.GetSingleAsync(postId);
        if (postToCommentOn == null)
        {
            Console.WriteLine($"post not found with {postId} ID ");
            return;
        }
        
        
         Console.WriteLine("enter you comment");
         string commentBody = Console.ReadLine() ?? "no comment";
         
         int commentId = commentRespository.GetMany().Any()
             ? commentRespository.GetMany().Max(x => x.Id) + 1
             : 1;
         
         var newcomment = new Comment(commentId, commentBody, postToCommentOn.Id);
         await commentRespository.AddAsync(newcomment);
         
         Console.WriteLine($"comment added to {postToCommentOn.Id}");
         


    }
}