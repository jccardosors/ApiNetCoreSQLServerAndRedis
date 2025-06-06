using Project.Domain.Entities;

namespace Project.Domain
{
    public interface IControlUserAccessRepository : IDisposable
    {
        Task<IEnumerable<ControlUserAccess>> GetAll();
        Task<ControlUserAccess> GetItem(int id);
        Task<ControlUserAccess> GetItem(string email);
        Task<ControlUserAccess> Add(ControlUserAccess entity);
        Task<ControlUserAccess> Update(ControlUserAccess entity);
        Task<bool> GetUserBlocked(string email);
    }
}
