namespace ApiContracts.Comment;

public class CreateCommentDto
{
    public required String Body { get; set; }
    public required int UserId { get; set; }
}