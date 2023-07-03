using System.Net.Mime;
using BackendLi.DataAccess;
using BackendLi.DTOs;
using BackendLi.Entities.Attributes;

using BackendLi.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendLi.Services;

[Service(typeof(IStatisticsService))]
public class StatisticsService:IStatisticsService
{
    
    private readonly IRepository _repository;
    private readonly IImageService _imageService;

    public StatisticsService(IRepository repository,IImageService imageService)
    {
        _repository = repository;
        _imageService = imageService;
    }


    public IEnumerable<int> GetNumberOfUsers(DateTime startDate, DateTime endDate)
    {
      
        var userCounts = new List<int>();
        for (DateTime date = startDate.Date.AddDays(1); date.Date <= endDate.Date.AddDays(1); date = date.AddDays(1))
        {
            DateTime nextDate = date.AddDays(1);

            var count = _repository.GetEntities<User>().Count(u => u.RegisterDate >= date && u.RegisterDate < nextDate);
            userCounts.Add(count);
        }

        return userCounts;
    }

    
    public IEnumerable<int> GetNumberOfPhotos(DateTime startDate, DateTime endDate)
    {
      
        var userCounts = new List<int>();
        for (DateTime date = startDate.Date.AddDays(1); date.Date <= endDate.Date.AddDays(1); date = date.AddDays(1))
        {
            DateTime nextDate = date.AddDays(1);

            var count = _repository.GetEntities<  Photo >().Count(u => u.Uploaded >= date && u.Uploaded < nextDate);
            userCounts.Add(count);
        }

        return userCounts;
    }
    public int GetAllUsersNb()
    {
        return _repository.GetEntities<User>().Count();
    }
    public IEnumerable<User> GetAllUsers(int pageNb, int pageSize)
    {

        return _repository.GetEntities<User>().Skip((pageNb - 1) * pageSize).Take(pageSize).ToList();

    }

    public int GetNewUserNb()
    {DateTime endDate = DateTime.Now;
        DateTime startDate = endDate.AddDays(-7);
        return _repository.GetEntities<User>()
            .Count(u => u.RegisterDate >= startDate && u.RegisterDate <= endDate);
    }
    
    public IEnumerable<User> GetNewUsers(int pageNb, int pageSize)
    {
        DateTime endDate = DateTime.Now;
        DateTime startDate = endDate.AddDays(-7);
        return _repository.GetEntities<User>()
            .Where(u => u.RegisterDate >= startDate && u.RegisterDate <= endDate).Skip((pageNb - 1) * pageSize).Take(pageSize).ToList();

    }
    
    
    public IEnumerable<  Photo > GetNewImages(int pageNb, int pageSize)
    {
        DateTime endDate = DateTime.Now;
        DateTime startDate = endDate.AddDays(-7);
        return _repository.GetEntities<  Photo >()
            .Where(u => u.Uploaded >= startDate && u.Uploaded <= endDate).Skip((pageNb - 1) * pageSize).Take(pageSize).ToList();

    }
    

    public IEnumerable<  Photo > GetAllImages(int pageNb, int pageSize)
    {

        return _repository.GetEntities<  Photo >().Skip((pageNb - 1) * pageSize).Take(pageSize).ToList();

    }



    public IEnumerable<StatisticsTypeDto> GetStatisticsByType()
    {
        var statistics = new Dictionary<string, string>
        {
            { "Popular", "Popular" },
            { "Basic", "Basic" },
            { "Fresh", "Fresh" },
            { "Editor's Choice", "EditorChoice" }
        };

        var list = statistics.Select(s => new StatisticsTypeDto(s.Key, _imageService.GetImagesByTypeAndCategory(s.Value, null).Count())).ToList();
        return list;


    }
    
    public IEnumerable<StatisticsTypeDto> GetStatisticsByCategory()
    {
        var categories = Enum.GetNames(typeof(ImageCategory));
        var images = _repository.GetEntities<  Photo >().ToList();

        var result = categories.Select(category =>
        {
            var categoryEnum = (ImageCategory)Enum.Parse(typeof(ImageCategory), category);
            var imageCount = images.Count(image => image.Category == categoryEnum);
            return new StatisticsTypeDto(category, imageCount);
        }).ToList();

        return result;
    }


    public IEnumerable<Photo> GetFirstImages()
    {
        DateTime lastWeek = DateTime.Now.AddDays(-7); // Data și ora acum - 7 zile
        return _repository.GetEntities<Photo>()
            .Where(photo => photo.Uploaded >= lastWeek && photo.Likes.Count > 0)
            .Include(i=>i.Likes)// Selecționează fotografii încărcate în ultima săptămână și cu un număr de like-uri mai mare decât 0
            .OrderByDescending(photo => photo.Likes.Count) // Sortează descrescător după numărul de like-uri
            .Take(10) // Selectează primele 10 fotografii
            .ToList();
    }
}