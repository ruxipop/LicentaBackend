using System.Net.Mime;
using BackendLi.DataAccess;
using BackendLi.DataAccess.Enums;
using BackendLi.Entities;
using BackendLi.Entities.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace BackendLi.Services;
[Service(typeof(IImageService))]
public class ImageService:IImageService
{

    private readonly IRepository _repository;

    public ImageService(IRepository repository)
    {
       _repository = repository;
    }

    
    public IEnumerable<Photo> GetImages()
    {
        return _repository.GetEntities<Photo>()
            .Include(i => i.Autor)
            .Include(i => i.Location)
            .ToList();    }

    public IEnumerable<Photo> GetPaginatedImages(int pageNb, int pageSize,string type,List<string> category)
    {
      
        return GetImagesByTypeAndCategory(type,category).Skip((pageNb - 1) * pageSize).Take(pageSize)
           
            .ToList(); 
    }

    public IEnumerable<Like> GetImageLikes(int pageNb, int pageSize, int imageId)
    {
       return  _repository.GetEntities<Like>().Where(l=>l.ImageId==imageId).Include(i=>i.User ).Skip((pageNb - 1) * pageSize).Take(pageSize)
           
            .ToList();
        
    }
    public Photo? GetImageById(int id)
    {
        return _repository.GetEntities<Photo>()
            .Include(i => i.Autor!)
            .Include(i => i.Location!)
            .Include(i => i.Comments!)
            .ThenInclude(c => c.User)
            .Include(i => i.EditorChoice)
            .ThenInclude(ec => ec.Editor)   
            .Include(i => i.Likes)
            .ThenInclude(c => c.User)
            .FirstOrDefault(x => x.Id == id);

    }

    private IEnumerable<Photo> getPopularImages(List<string>? category)
    {
        List<ImageCategory> enumCategories = category?.Select(c => (ImageCategory)Enum.Parse(typeof(ImageCategory), c)).ToList();

        return _repository.GetEntities<Photo>()
            .Include(i => i.EditorChoice)
            .Where(i => i.EditorChoice == null && (category == null || enumCategories.Contains(i.Category)))
            .Include(i => i.Likes).Include(i=>i.Autor)
            
            .Where(i => i.Likes.Count > 50)  
            .OrderByDescending(i => i.Likes.Count)
            .ToList();

    }

    
    private void DeleteExpiredEditorChoices()
    {
        using (IUnitOfWork unitOfWork = _repository.CreateUnitOfWork())
        {
            DateTime currentDate = DateTime.Now;
            DateTime expirationDate = currentDate.AddDays(-5);

            var expiredEditorChoices = unitOfWork.GetEntities<EditorChoice>()
                .Where(ec => ec.Date< expirationDate)
                .ToList();

            foreach (var editorChoice in expiredEditorChoices)
            {
                unitOfWork.Delete(editorChoice);
            }

            unitOfWork.SaveChanges();
        }

    }

    private IEnumerable<Photo> GetEditorChoiceImages(List<String>? category)
    {        List<ImageCategory> enumCategories = category?.Select(c => (ImageCategory)Enum.Parse(typeof(ImageCategory), c)).ToList();


        return _repository.GetEntities<Photo>().Include(i => i.EditorChoice).Include(i=>i.Autor).Include(i=>i.Likes)
            .Where(i => i.EditorChoice !=null  && (enumCategories == null || enumCategories.Contains(i.Category))).ToList();
    }

    private IEnumerable<Photo> GetFreshImages(List<string>? category)
    {


             List<ImageCategory> enumCategories = category?.Select(c => (ImageCategory)Enum.Parse(typeof(ImageCategory), c)).ToList();

             DateTime twoDaysAgo = DateTime.Now.AddDays(-2);

             return _repository.GetEntities<Photo>().Include(i=>i.Autor).Include(i=>i.Likes)
                 .Where(i => i.EditorChoice == null && i.Likes.Count < 50 && (enumCategories == null || enumCategories.Contains(i.Category)) && i.Uploaded > twoDaysAgo)
                 .OrderByDescending(i => i.Uploaded)
                 .ToList();
    }

    public IEnumerable<Photo> GetImagesByTypeAndCategory(string type,List<string>?category)
    { 
        DeleteExpiredEditorChoices();
        
        switch (type)
        {
            case "Popular":
                return getPopularImages(category);
            case "Fresh":
                return GetFreshImages(category);
            case "EditorChoice":
                return GetEditorChoiceImages(category);
            
            case "Basic": 
                List<ImageCategory> enumCategories = category?.Select(c => (ImageCategory)Enum.Parse(typeof(ImageCategory), c)).ToList();

                var freshimages = GetFreshImages(null);
                return _repository.GetEntities<Photo>().Where
                    
                    (i=>i.Likes.Count <50 && !freshimages.Contains(i) && i.EditorChoice==null && (enumCategories == null || enumCategories.Contains(i.Category))).Include(i=>i.Autor).Include(i=>i.Likes).ToList();
            default:
                throw new ArgumentException("Invalid image type.");
        }
    }

    

    public string GetImageType(int id)
    {
        if (IsChoiceByEditor(id)) return TypeImage.EditorChoice.GetDisplayName();
        if (IsImagePopular(id)) return TypeImage.Popular.GetDisplayName();
        if (IsImageFresh(id)) return TypeImage.Fresh.GetDisplayName();
        return TypeImage.Basic.GetDisplayName();
    }
    private bool IsImagePopular(int imageId)
    {
        var popularImages = getPopularImages(null);
        return popularImages.Any(image => image.Id == imageId);
    }


    private bool IsChoiceByEditor(int imageId)
    {
        var popularImages = GetEditorChoiceImages(null);
        return popularImages.Any(image => image.Id == imageId);
    }
    
    private bool IsImageFresh(int imageId)
    {
        var popularImages = GetFreshImages(null);
        return popularImages.Any(image => image.Id == imageId);
    }
    
   public IEnumerable<Photo> GetImagesByAuthorId(int pageNb,int pageSize,int authorId)

    {
        return _repository.GetEntities<Photo>()
            .Where(i => i.Autor.Id == authorId)
            .Skip((pageNb - 1) * pageSize).Take(pageSize)
           
            .ToList();
    }


    public IEnumerable<Photo> GetImagesByUserId(int userId,int pageNb, int pageSize)
    {
        return _repository.GetEntities<Photo>().Include(i=>i.Autor).Include(i=>i.Likes).Where(l=>l.AutorId==userId).Skip((pageNb - 1) * pageSize).Take(pageSize)
           
            .ToList();
    }

    public IEnumerable<Photo> GetLikedImagesByUser(int userId,int pageNb, int pageSize)
    {
        return _repository.GetEntities<Photo>()
            .Include(i=>i.Autor).Include(i=>i.Likes)
            .Where(image => image.Likes.Any(like => like.UserId == userId))
            .Skip((pageNb - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }
    
    
    
    public bool CreateImage(Photo image)
    {
        var existingImage = _repository.GetEntities<Photo>().FirstOrDefault(i =>  image.Title == i.Title);

        if (existingImage != null) return false;
        using (IUnitOfWork unitOfWork = _repository.CreateUnitOfWork())
        {
          
            unitOfWork.Add(image);
            unitOfWork.SaveChanges();
                
        }

        return true;
    }

    public void DeleteImage(int id)
    {
        var image = _repository.GetEntities<Photo>().FirstOrDefault(i => i.Id == id);
        using (IUnitOfWork unitOfWork = _repository.CreateUnitOfWork())
        {
            unitOfWork.Delete(image!);
            unitOfWork.SaveChanges();
                
        }

    }

    public Photo? GetImageByTitle(string title)
    {
        
        return _repository.GetEntities<Photo>().Include(i=>i.Autor).FirstOrDefault(i => i.Title==title);
    }


}