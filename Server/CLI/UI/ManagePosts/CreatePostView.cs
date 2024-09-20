using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly IPostRespository postRespository;



    public CreatePostView(IPostRespository postRespository)
    {
        this.postRespository = postRespository;
    }
    
    
}