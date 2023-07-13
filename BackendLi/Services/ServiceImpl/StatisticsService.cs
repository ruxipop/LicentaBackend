using BackendLi.DataAccess;
using BackendLi.DTOs;
using BackendLi.Entities;
using BackendLi.Entities.Attributes;
using Microsoft.EntityFrameworkCore;

namespace BackendLi.Services.ServiceImpl;

[Service(typeof(IStatisticsService))]
public class StatisticsService : IStatisticsService
{
    private readonly IImageService _imageService;

    private readonly IRepository _repository;

    public StatisticsService(IRepository repository, IImageService imageService)
    {
        _repository = repository;
        _imageService = imageService;
    }


    public IEnumerable<int> GetNumberOfUsers(DateTime startDate, DateTime endDate)
    {
        var userCounts = new List<int>();
        for (var date = startDate.Date.AddDays(1); date.Date <= endDate.Date.AddDays(1); date = date.AddDays(1))
        {
            var nextDate = date.AddDays(1);

            var count = _repository.GetEntities<User>().Count(u => u.RegisterDate >= date && u.RegisterDate < nextDate);
            userCounts.Add(count);
        }

        return userCounts;
    }


    public IEnumerable<int> GetNumberOfPhotos(DateTime startDate, DateTime endDate)
    {
        var userCounts = new List<int>();
        for (var date = startDate.Date.AddDays(1); date.Date <= endDate.Date.AddDays(1); date = date.AddDays(1))
        {
            var nextDate = date.AddDays(1);

            var count = _repository.GetEntities<Photo>().Count(u => u.Uploaded >= date && u.Uploaded < nextDate);
            userCounts.Add(count);
        }

        return userCounts;
    }

    public int GetAllUsersNb()
    {
        return _repository.GetEntities<User>().Count();
    }
    
    public int GetAllImagesNb()
    {
        return _repository.GetEntities<Photo>().Count();
    }

    public IEnumerable<User> GetAllUsers(int pageNb, int pageSize)
    {
        return _repository.GetEntities<User>().Skip((pageNb - 1) * pageSize).Take(pageSize).ToList();
    }

    public int GetNewUserNb()
    {
        var endDate = DateTime.Now;
        var startDate = endDate.AddDays(-7);
        return _repository.GetEntities<User>()
            .Count(u => u.RegisterDate >= startDate && u.RegisterDate <= endDate);
    }
    
    public int GetNewImagesNb()
    {
        var endDate = DateTime.Now;
        var startDate = endDate.AddDays(-7);
        return _repository.GetEntities<Photo>()
            .Count(u => u.Uploaded >= startDate && u.Uploaded <= endDate);
    }

    public IEnumerable<User> GetNewUsers(int pageNb, int pageSize)
    {
        var endDate = DateTime.Now;
        var startDate = endDate.AddDays(-7);
        return _repository.GetEntities<User>()
            .Where(u => u.RegisterDate >= startDate && u.RegisterDate <= endDate).Skip((pageNb - 1) * pageSize)
            .Take(pageSize).ToList();
    }


    public IEnumerable<Photo> GetNewImages(int pageNb, int pageSize)
    {
        var endDate = DateTime.Now;
        var startDate = endDate.AddDays(-7);
        return _repository.GetEntities<Photo>()
            .Where(u => u.Uploaded >= startDate && u.Uploaded <= endDate).Skip((pageNb - 1) * pageSize).Take(pageSize)
            .ToList();
    }


    public IEnumerable<Photo> GetAllImages(int pageNb, int pageSize)
    {
        return _repository.GetEntities<Photo>().Skip((pageNb - 1) * pageSize).Take(pageSize).ToList();
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

        var list = statistics.Select(s =>
            new StatisticsTypeDto(s.Key, _imageService.GetImagesByTypeAndCategory(s.Value, null).Count())).ToList();
        return list;
    }

    public IEnumerable<StatisticsTypeDto> GetStatisticsByCategory()
    {
        var categories = Enum.GetNames(typeof(ImageCategory));
        var images = _repository.GetEntities<Photo>().ToList();

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
        var lastWeek = DateTime.Now.AddDays(-7);
        return _repository.GetEntities<Photo>()
            .Where(photo => photo.Uploaded >= lastWeek && photo.Likes!.Count > 0)
            .Include(i => i.Likes) 
            .OrderByDescending(photo => photo.Likes!.Count) 
            .Take(10) 
            .ToList();
    }
}