using System;
using System.Collections.Generic;
using System.Linq;
using BackendLi.Controller;
using BackendLi.DTOs;
using BackendLi.Entities;
using BackendLi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace UnitTests;

public class IntegrationTest
{
    private NotificationController _notificationController;
    private Mock<INotificationService> _notificationServiceMock;
    private Mock<IFollowService> _followServiceMock;

    [SetUp]
    public void SetUp()
    {
        // Configurați resursele necesare pentru testul de integrare
        _notificationServiceMock = new Mock<INotificationService>();
        _followServiceMock = new Mock<IFollowService>();
        _notificationController = new NotificationController(_notificationServiceMock.Object, _followServiceMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        // Eliberați resursele utilizate în testul de integrare
    }
    
    [Test]
    public void IntegrationTest_DeleteNotification()
    {
        // Aranjați datele și resursele necesare pentru acest test de integrare

        // Configurați comportamentul obiectelor mock
        int notificationId = 15;
        var notifications = new List<Notification>
        {
            new Notification { Id = 10, Type = NotificationType.LIKE, Content = "Notification 1", SenderId = 1, ReceiverId = 2, Timestamp = DateTime.Now },
            new Notification { Id = 20, Type = NotificationType.FOLLOW, Content = "Notification 2", SenderId = 3, ReceiverId = 4, Timestamp = DateTime.Now },
            new Notification { Id = 30, Type = NotificationType.MESSAGE, Content = "Notification 3", SenderId = 5, ReceiverId = 6, Timestamp = DateTime.Now }
        };


        _notificationServiceMock.Setup(s => s.GetAllNotificationByUserId(1,1,10))
            .Returns(notifications);

        _notificationServiceMock.Setup(s => s.DeleteNotification(notificationId))
            .Callback<int>(id =>
            {
                var notificationToRemove = notifications.FirstOrDefault(n => n.Id == id);
                if (notificationToRemove != null)
                    notifications.Remove(notificationToRemove);
            })
            .Returns(new SuccessResponseDto("Notification deleted successfully"));


        // Executați acțiunea de integrare

        // Verificați rezultatele obținute în urma acțiunii de integrare
        IActionResult result = _notificationController.DeleteNotification(notificationId);

// Verificați rezultatele obținute în urma acțiunii de integrare
        Assert.IsInstanceOf<OkObjectResult>(result);

        OkObjectResult okResult = (OkObjectResult)result;
        SuccessResponseDto successResponse = (SuccessResponseDto)okResult.Value;
        Assert.AreEqual("Notification deleted successfully", successResponse.Message);

// Verificați statusul HTTP returnat de acțiune
        // Assert.AreEqual(200, okResult.StatusCode);


        // Verificați că notificarea a fost ștearsă din lista de notificări
        var remainingNotifications = _notificationServiceMock.Object.GetAllNotificationByUserId(1,1,10);
        Assert.IsNull(remainingNotifications.FirstOrDefault(n => n.Id == notificationId));
    }

}