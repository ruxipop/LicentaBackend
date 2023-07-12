using BackendLi.DataAccess;
using BackendLi.DTOs;
using BackendLi.Entities;
using BackendLi.Entities.Attributes;
using Microsoft.EntityFrameworkCore;

namespace BackendLi.Services.ServiceImpl;

[Service(typeof(IEditorService))]
public class EditorService : IEditorService
{
    private readonly IRepository _repository;

    public EditorService(IRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<EditorChoice> GetVotedImages(int userId, int pageNb, int pageSize)
    {
        return _repository.GetEntities<EditorChoice>().Where(ec => ec.EditorId == userId).Include(ec => ec.Image)
            .ThenInclude(i => i.Autor).Include(ec => ec.Image).ThenInclude(l => l.Likes)
            .Skip((pageNb - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public SuccessResponseDto AddImagedToVoted(EditorChoice editorChoice)
    {
        var vote = _repository.GetEntities<EditorChoice>()
            .FirstOrDefault(ec => ec.EditorId == editorChoice.EditorId && ec.ImageId == editorChoice.ImageId);
        if (vote != null) return new SuccessResponseDto("You have already voted this image!");


        using (var unitOfWork = _repository.CreateUnitOfWork())
        {
            unitOfWork.Add(editorChoice);
            unitOfWork.SaveChanges();
        }

        return new SuccessResponseDto("Image voted successfully!");
    }

    public SuccessResponseDto DeleteVote(int id)
    {
        using (var unitOfWork = _repository.CreateUnitOfWork())
        {
            var vote = unitOfWork.GetEntities<EditorChoice>().FirstOrDefault(i => i.Id == id);

            if (vote != null)
            {
                unitOfWork.Delete(vote);
                unitOfWork.SaveChanges();
                return new SuccessResponseDto("Vote removed successfully.");
            }
        }

        return new SuccessResponseDto("No vote found for that image");
    }

    public EditorChoice GetEditorChoice(int userId, int imageId)
    {
        return _repository.GetEntities<EditorChoice>()
            .FirstOrDefault(i => i.EditorId == userId && i.ImageId == imageId)!;
    }

    public bool IsImageVoted(int imageId)
    {
        var vote = _repository.GetEntities<EditorChoice>().FirstOrDefault(ec => ec.ImageId == imageId);
        if (vote != null)
            return true;

        return false;
    }


    public int GetNbOfVotes(int userId)
    {
        return _repository.GetEntities<EditorChoice>().Where(ec => ec.EditorId == userId).ToList().Count;
    }
}