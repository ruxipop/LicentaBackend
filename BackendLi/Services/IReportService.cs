using BackendLi.DTOs;
using BackendLi.Entities;

namespace BackendLi.Services;

public interface IReportService
{
    public  void CreateReportEmail(ReportEmailDto reportEmailDto);
    public IEnumerable<Report> GetReports();
    public void DeleteReport(int id);
    public void CreateReport(Report report);
    public bool existReport(int imageId);
    public void DeleteReportImage(int imageId);
}