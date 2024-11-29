using System.Collections.ObjectModel;
using ApiContracts.Post;

namespace BlazorApp.Services;

public interface IPostService
{
    public Task<PostDto> AddPostAsync(CreatePostDto request);
    public Task<Collection<PostDto>> GetPostsAsync();
    public Task<PostDto> GetPostAsync(int id);
}