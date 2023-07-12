using BackendLi.DataAccess;
using BackendLi.Entities;
using BackendLi.Entities.Attributes;
using Microsoft.EntityFrameworkCore;

namespace BackendLi.Services.ServiceImpl;
[Service(typeof(ICommentService))]
public class CommentService : ICommentService
{
    private readonly IRepository _repository;

    public CommentService(IRepository repository)
    {
        _repository = repository;
    }

    public void AddComment(Comment comment)
    {
        comment.CreatedAt = DateTime.Now;
        using (var unitOfWork = _repository.CreateUnitOfWork())
        {
            unitOfWork.Add(comment);
            unitOfWork.SaveChanges();
        }
    }

    public IEnumerable<Comment> GetComments(int imageId)
    {
        return _repository.GetEntities<Comment>().Include(i => i.User)
            .Where(i => i.ImageId == imageId).OrderByDescending(c => c.CreatedAt)
            .ToList();
    }
}