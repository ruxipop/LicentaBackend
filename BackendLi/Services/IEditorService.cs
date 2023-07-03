using BackendLi.DTOs;
using BackendLi.Entities;

namespace BackendLi.Services;

public interface IEditorService
{
    public IEnumerable<EditorChoice> GetVotedImages(int userId, int pageNb, int pageSize);
    public SuccessResponseDto AddImagedToVoted(EditorChoice editorChoice);
    public SuccessResponseDto DeleteVote(int id);
    public EditorChoice GetEditorChoice(int userId, int imageId);
    public bool IsImageVoted(int imageId);
    public int GetNbOfVotes(int userId);



}