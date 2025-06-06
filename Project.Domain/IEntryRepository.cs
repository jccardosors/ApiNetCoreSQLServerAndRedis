using Project.Domain.Entities;

namespace Project.Domain
{
    public interface IEntryRepository : IDisposable
    {
        Task<IEnumerable<Entry>> GetAll();

        Task<Entry> GetItem(int id);

        Task<Entry> Add(Entry entity);

        Task<Entry> Update(Entry entity);

        Task<int> Delete(int id);

        Task<IEnumerable<Entry>> GetAllByPeriod(DateTime initialDate, DateTime finalDate);
    }
}
