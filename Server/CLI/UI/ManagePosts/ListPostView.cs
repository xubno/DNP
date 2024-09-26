using RepositoryContracts;
using Entities;  // Tilføj dette for at sikre, at Post-klassen er fundet

namespace CLI.UI.ManagePosts;

public class ListPostView
{
    private readonly ICommentRespository commentRespository;
    private readonly IPostRespository postRespository;
      

    public ListPostView(ICommentRespository commentRespository, IPostRespository postRespository )
    {
        this.commentRespository = commentRespository;
        this.postRespository = postRespository;
    }

    public async Task ListPostAsync()
    {
        var posts = postRespository.GetMany().ToList();
        if (!posts.Any())
        {
            Console.WriteLine("no posts found");
            return;
        }

        foreach (var post in posts)
        {
            Console.WriteLine($"[{post.Title}, {post.Id}]");
        }
        
        Console.WriteLine("if you wanted to see a specific post, enter post ID");
        if (int.TryParse(Console.ReadLine(), out int postId))
        {
            try
            {
                // Hent posten med det angivne ID
                Post selectedPost = await postRespository.GetSingleAsync(postId);

                // Vis postens detaljer
                Console.WriteLine($"Post ID: {selectedPost.Id}");
                Console.WriteLine($"Title: {selectedPost.Title}");
                Console.WriteLine($"Body: {selectedPost.Body}");

                // Hent kommentarer til posten
                var comments = commentRespository.GetMany()
                    .Where(c => c.PostId == selectedPost.Id)
                    .ToList();

                if (comments.Any())
                {
                    Console.WriteLine("Comments:");
                    foreach (var comment in comments)
                    {
                        Console.WriteLine($"- Comment ID: {comment.Id}, Body: {comment.Body}");
                    }
                }
                else
                {
                    Console.WriteLine("No comments for this post.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
