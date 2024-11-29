using ApiContracts;
using ApiContracts.Comment;
using ApiContracts.Post;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController(
    IPostRespository postRepository,
    ICommentRespository commentRepository,
    IUserRespository userRepository)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetMany(
        [FromQuery] string? titleContains = null, 
        [FromQuery] int? userId = null, 
        [FromQuery] string? userName = null,
        [FromQuery] bool includeComments = false)
    {
        var posts = postRepository.GetMany();

        if (!string.IsNullOrEmpty(titleContains))
        {
            posts = posts.Where(post => post.Title.Contains(titleContains, StringComparison.OrdinalIgnoreCase));
        }

        if (userId.HasValue)
        {
            posts = posts.Where(post => post.UserId == userId.Value);
        }

        if (!string.IsNullOrEmpty(userName))
        {
            var users = userRepository.GetMany().Where(user => user.Username.Contains(userName, StringComparison.OrdinalIgnoreCase)).Select(user => user.Id);
            posts = posts.Where(post => users.Contains(post.UserId));
        }
        List<PostDto> postDtos;
        if (includeComments)
        {
            postDtos = posts.Select(post => new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId,
                Comments = commentRepository.GetMany()
                    .Where(comment => comment.PostId == post.Id)
                    .Select(comment => new CommentDto
                    {
                        Id = comment.Id,
                        Body = comment.Body,
                        PostId = comment.PostId,
                        UserId = comment.UserId
                    }).ToList()
            }).ToList();
        }
        else
        {
            postDtos = posts.Select(post => new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId,
                Comments = new List<CommentDto> { }
            }).ToList();
        }

        return Ok(postDtos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDto>> GetSingle(
        int id,
        [FromQuery] bool includeComments = false)
    {
        var post = await postRepository.GetSingleAsync(id);
        PostDto postDto;
        if (!includeComments)
        {
            postDto = new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId,
                Comments = (List<CommentDto>) []
            };
        }
        else
        {
            postDto = new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId,
                Comments = commentRepository.GetMany()
                    .Where(comment => comment.PostId == post.Id)
                    .Select(comment => new CommentDto
                    {
                        Id = comment.Id,
                        Body = comment.Body,
                        PostId = comment.PostId,
                        UserId = comment.UserId
                    }).ToList()
            };
        }

        return Ok(postDto);
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> Create(CreatePostDto createPostDto)
    {
        var post = new Post
        {
            Title = createPostDto.Title,
            Body = createPostDto.Body,
            UserId = createPostDto.UserId
        };

        var createdPost = await postRepository.AddAsync(post);

        return Ok(createdPost);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdatePostDto updatePostDto)
    {
        var post = new Post
        {
            Id = id,
            Title = updatePostDto.Title,
            Body = updatePostDto.Body,
        };

        await postRepository.UpdateAsync(post);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await postRepository.DeleteAsync(id);
        return NoContent();
    }
}