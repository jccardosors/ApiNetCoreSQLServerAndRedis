using Project.Domain.Entities;

namespace Project.Domain
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetItemByEmail(string email);
        Task<User> GetItemByEmail(string email, string password);
        Task<User> Add(User entity);
    }
}
