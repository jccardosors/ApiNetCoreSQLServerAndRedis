using Project.Domain.Entities;

namespace Project.Domain
{
    public interface ILogsRepository : IDisposable
    {
        Task<IEnumerable<Logs>> GetAll();
        Task<Logs> GetItem(int id);
        Task<Logs> Add(Logs entity);
    }
}
