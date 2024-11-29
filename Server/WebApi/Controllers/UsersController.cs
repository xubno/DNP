
using ApiContracts.User;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserRespository userRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetMany([FromQuery] string? nameContains = null)
    {
        var users = userRepository.GetMany();

        if (!string.IsNullOrEmpty(nameContains))
        {
            users = users.Where(user => user.Username.Contains(nameContains, StringComparison.OrdinalIgnoreCase));
        }

        var userDtos = users.Select(user => new UserDto()
        {
            Id = user.Id,
            Name = user.Username,
            Password = user.Password
        }).ToList();

        return Ok(userDtos);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetSingle(int id)
    {
        var user = await userRepository.GetSingleAsync(id);
        var userDto = new UserDto
        {
            Id = user.Id,
            Name = user.Username,
            Password = user.Password,
        };
        
        return Ok(userDto);
    }
    
    [HttpPost]
    public async Task<ActionResult<UserDto>> Add(CreateUserDto createUserDto)
    {
        var user = new User
        {
            Username = createUserDto.Username,
            Password = createUserDto.Password,
        };
        
        var createdUser = await userRepository.AddAsync(user);
        var createdUserDto = new UserDto
        {
            Id = createdUser.Id,
            Name = createdUser.Username,
            Password = createdUser.Password,
        };
        
        return Ok(createdUserDto);
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDto>> Update(int id, UpdateUserDto updateUserDto)
    {
        var user = new User
        {
            Id = id,
            Password = updateUserDto.Password,
        };
        
        await userRepository.UpdateAsync(user);
        
        return Ok(user);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await userRepository.DeleteAsync(id);
        
        return NoContent();
    }
    
    
}