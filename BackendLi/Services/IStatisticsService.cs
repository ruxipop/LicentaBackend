using BackendLi.DTOs;
using BackendLi.Entities;

namespace BackendLi.Services;

public interface IStatisticsService
{
    public IEnumerable<int> GetNumberOfUsers(DateTime startDate, DateTime endDate);
    
    public IEnumerable<User> GetAllUsers(int pageNb, int pageSize);
    public int GetAllUsersNb();

    public int GetNewUserNb();
    public IEnumerable<User> GetNewUsers(int pageNb, int pageSize);
    public IEnumerable<  Photo > GetNewImages(int pageNb, int pageSize);
    public IEnumerable<  Photo > GetAllImages(int pageNb, int pageSize);
    public IEnumerable<int> GetNumberOfPhotos(DateTime startDate, DateTime endDate);
    public IEnumerable<StatisticsTypeDto> GetStatisticsByType();
    public IEnumerable<StatisticsTypeDto> GetStatisticsByCategory();
    public IEnumerable<Photo> GetFirstImages();



}