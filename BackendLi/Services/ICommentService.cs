using BackendLi.Entities;

namespace BackendLi.Services;

public interface ICommentService
{
    void AddComment(Comment comment);
    IEnumerable<Comment> GetComments(int imageId);
}