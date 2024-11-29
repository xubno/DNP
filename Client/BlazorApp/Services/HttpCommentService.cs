using System.Text.Json;
using ApiContracts.Comment;

namespace BlazorApp.Services;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient _client;

    public HttpCommentService(HttpClient httpClient)
    {
        _client = httpClient;
    }

    public async Task<CommentDto> AddCommentAsync(CreateCommentDto request,
        int postId)
    {
        var httpResponse =
            await _client.PostAsJsonAsync($"/api/Posts/{postId}/Comments",
                request);
        var response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<CommentDto>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }
}