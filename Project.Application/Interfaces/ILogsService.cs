using Project.Application.Utils;
using Project.Application.ViewModels;

namespace Project.Application.Interfaces
{
    public interface ILogsService
    {
        Task<CustomResult<LogsVM>> Add(string email, string classe, string method, string messageError);
    }
}
