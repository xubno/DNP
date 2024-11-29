using System.Collections.ObjectModel;
using System.Text.Json;
using ApiContracts.Post;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient _client;
    
    public HttpPostService(HttpClient client)
    {
        this._client = client;
    }
    
    public async Task<PostDto> AddPostAsync(CreatePostDto request)
    {
        var httpResponse = await _client.PostAsJsonAsync("/api/Posts", request);
        var response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<Collection<PostDto>> GetPostsAsync()
    {
        var posts = await _client.GetFromJsonAsync<Collection<PostDto>>("api/Posts?includeComments=false");
        if (posts == null)
        {
            throw new Exception("Failed to retrieve posts.");
        }

        return posts;
    }

    public async Task<PostDto> GetPostAsync(int id)
    {
        var httpResponse = await _client.GetAsync($"/api/Posts/{id}?includeComments=true");
        var response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
}