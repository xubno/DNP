using System.Text.Json;
using ApiContracts.User;

namespace BlazorApp.Services;

public class HttpUserService : IUserService
{
    private readonly HttpClient _client;

    public HttpUserService(HttpClient client)
    {
        this._client = client;
    }

    public async Task<UserDto> AddUserAsync(CreateUserDto request)
    {
        var httpResponse = await _client.PostAsJsonAsync("/api/Users", request);
        var response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<UserDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public Task UpdateUserAsync(int id, CreateUserDto request)
    {
        throw new NotImplementedException();
    }
}