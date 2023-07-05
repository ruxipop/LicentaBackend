using System.Collections.Generic;
using System.Linq;
using BackendLi.DataAccess;
using BackendLi.Entities;
using BackendLi.Services;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using NUnit.Framework;

namespace UnitTests;

public class GalleryServiceTest
{
    private List<Gallery> _expectedGalleries;
    private Mock<IRepository> _mockRepository;
    private GalleryService _myService;
    private Mock<IUnitOfWork> _mockUnitOfWork;

    [SetUp]
    public void SetUp()
    {
        //Arrange
        _mockRepository = new Mock<IRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _myService = new GalleryService(_mockRepository.Object);
        _expectedGalleries = new List<Gallery>
        {
            new() { Id = 1, Name = "gallery 1", UserId = 1,Images = new List<Photo>(),Description = "First Gallery ",IsPrivate = true},
            new() { Id = 2, Name = "gallery 2",UserId = 2,Images = new List<Photo>(),Description = "Second Gallery",IsPrivate = false}
        };

        _mockRepository.Setup(r => r.GetEntities<Gallery>()).Returns(_expectedGalleries.AsQueryable());
    }
    
    [TearDown]
    public void TearDown()
    {
      _mockUnitOfWork.Object.DeleteRange(_expectedGalleries);
    }

    [Test]
    public void TestGetGalleryById()
    {
        //Act
        var result = _myService.GetGalleryById(1);
        //Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Id);
    }

    [Test]
    public void TestGetAllGalleriesForAUser()
    {
        //Arrange
        var userId = 1;
        var nbPage = 1;
        var pageSize = 10;
        //Act
        var result = _myService.GetAllGalleries(userId, nbPage, pageSize, null);
        //Assert
        Assert.AreEqual(1, result.Count());

    }
    
    [Test]
    public void TestGetAllGalleriesWithSearchTerm()
    {
        //Arrange
        var userId = 1;
        var nbPage = 1;
        var pageSize = 10;
        var searchTerm = "G";
        //Act
        var result = _myService.GetAllGalleries(userId, nbPage, pageSize, searchTerm);
        //Assert
        Assert.AreEqual(1, result.Count());

    }
}