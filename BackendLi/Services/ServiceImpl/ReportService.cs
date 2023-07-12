using BackendLi.DataAccess;
using BackendLi.DTOs;
using BackendLi.Entities;
using BackendLi.Entities.Attributes;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace BackendLi.Services;

[Service(typeof(IReportService))]
public class ReportService:IReportService
{
    private readonly IRepository _repository;

    public ReportService(IRepository repository)
    {
        _repository = repository;
    }

    public void CreateReportEmail(ReportEmailDto reportEmailDto)
    {
        Report report = Report
            .Create()
            .WithName(reportEmailDto.Name)
            .WithEmail(reportEmailDto.Email)
            .WithMessage(reportEmailDto.Message);
   

        using (var unitOfWork = _repository.CreateUnitOfWork())
        {
            unitOfWork.Add(report);
            unitOfWork.SaveChanges();
        }
    }

    public void DeleteReportImage(int imageId)
    {
        var exist = _repository.GetEntities<Report>().Where(r => r.ImageId == imageId).ToList();
        using (var unitOfWork = _repository.CreateUnitOfWork())
        {
            unitOfWork.DeleteRange(exist);
            unitOfWork.SaveChanges();
        }
    }
    public bool existReport(int imageId)
    {
        var exist = _repository.GetEntities<Report>().Where(r => imageId == imageId).ToList();
        if (exist.Count>0)
        {
            return true;
        }

        return false;
    }
    public void CreateReport(Report report)
    {
       

        using (var unitOfWork = _repository.CreateUnitOfWork())
        {
            unitOfWork.Add(report);
            unitOfWork.SaveChanges();
        }
    }
    public IEnumerable<Report> GetReports()
    {
        return _repository.GetEntities<Report>().Include(r=>r.User).Include(r=>r.Image).ToList();
    }

    public void DeleteReport(int id)
    {
        using (var unitOfWork = _repository.CreateUnitOfWork())
        {
            var like = unitOfWork.GetEntities<Report>().FirstOrDefault(i => i.Id==id );

            if (like != null)
            {
                unitOfWork.Delete(like);
                unitOfWork.SaveChanges();
            }
        }
    }
}