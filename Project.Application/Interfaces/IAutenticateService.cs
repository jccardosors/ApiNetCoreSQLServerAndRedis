using Project.Application.Utils;
using Project.Application.ViewModels;

namespace Project.Application.Interfaces
{
    public interface IAutenticateService
    {
        Task<CustomResult<AutenticateVM>> Autenticate(string email, string password);
    }
}
