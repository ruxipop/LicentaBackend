using BackendLi.Entities;

namespace BackendLi.Services;

public interface ICommentService
{
    public void AddComment(Comment comment);
    IEnumerable<Comment> GetComments(int imageId);
}