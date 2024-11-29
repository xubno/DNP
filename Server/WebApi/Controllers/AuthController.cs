using ApiContracts.User;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRespository userRepository;

        public AuthController(IUserRespository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginUserResponse>> Login(
            [FromBody] LoginUserRequest request)
        {
            var user = userRepository.GetMany()
                .FirstOrDefault(user => user.Username == request.Username);
            if (user == null)
            {
                return Unauthorized();
            }

            if (user.Password != request.Password)
            {
                return Unauthorized();
            }

            return Ok(new LoginUserResponse
            {
                Id = user.Id,
                Username = user.Username
            });
        }
    }
