using Project.Application.Utils;
using Project.Application.ViewModels;

namespace Project.Application.Interfaces
{
    public interface IConsolidatedReportService
    {
        Task<CustomResult<ConsolidatedReportResultVM>> GenerateReport(string email, ConsolidatedReportVM parameters);
    }
}
