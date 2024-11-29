using ApiContracts;
using ApiContracts.Comment;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("api/Posts/{postId:int}/[controller]")]
public class CommentsController(
    ICommentRespository commentRepository,
    IUserRespository userRepository)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetMany(
        int? postId,
        [FromQuery] int? userId = null,
        [FromQuery] string? userName = null)
    {
        var comments = commentRepository.GetMany();

        if (userId.HasValue)
        {
            comments =
                comments.Where(comment => comment.UserId == userId.Value);
        }

        if (!string.IsNullOrEmpty(userName))
        {
            var users = userRepository.GetMany()
                .Where(user => user.Username.Contains(userName,
                    StringComparison.OrdinalIgnoreCase))
                .Select(user => user.Id);
            comments =
                comments.Where(comment => users.Contains(comment.UserId));
        }

        if (postId.HasValue)
        {
            comments =
                comments.Where(comment => comment.PostId == postId.Value);
        }

        var commentDtos = comments.Select(comment => new CommentDto
        {
            Id = comment.Id,
            Body = comment.Body,
            PostId = comment.PostId,
            UserId = comment.UserId
        }).ToList();

        return Ok(commentDtos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDto>> GetSingle(int id)
    {
        var comment = await commentRepository.GetSingleAsync(id);
        var commentDto = new CommentDto
        {
            Id = comment.Id,
            Body = comment.Body,
            PostId = comment.PostId,
            UserId = comment.UserId
        };

        return Ok(commentDto);
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> Add(
        CreateCommentDto createCommentDto, int postId)
    {
        var comment = new Comment
        {
            Body = createCommentDto.Body,
            PostId = postId,
            UserId = createCommentDto.UserId
        };

        var createdComment = await commentRepository.AddAsync(comment);
        var createdCommentDto = new CommentDto
        {
            Id = createdComment.Id,
            Body = createdComment.Body,
            PostId = createdComment.PostId,
            UserId = createdComment.UserId
        };

        return Ok(createdCommentDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CommentDto>> Update(int id,
        OpdateCommentDto updateCommentDto)
    {
        var comment = new Comment
        {
            Id = id,
            Body = updateCommentDto.Body,
        };

        await commentRepository.UpdateAsync(comment);

        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await commentRepository.DeleteAsync(id);

        return NoContent();
    }
}