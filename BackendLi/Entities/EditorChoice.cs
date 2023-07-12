using System.ComponentModel.DataAnnotations.Schema;

namespace BackendLi.Entities;

[Table("editor")]
public class EditorChoice
{
    public EditorChoice(int editorId, int imageId)
    {
        EditorId = editorId;
        ImageId = imageId;
    }

    public int Id { get; set; }
    public int EditorId { get; set; }

    [ForeignKey("EditorId")] public User? Editor { get; set; }

    public int ImageId { get; set; }

    [ForeignKey("ImageId")] public Photo? Image { get; set; }

    public DateTime Date { get; set; }
}