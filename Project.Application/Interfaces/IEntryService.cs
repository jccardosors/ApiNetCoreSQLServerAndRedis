using Project.Application.Utils;
using Project.Application.ViewModels;

namespace Project.Application.Interfaces
{
    public interface IEntryService
    {
        Task<CustomResult<IEnumerable<EntryVM>>> GetAll(string email);
        Task<CustomResult<EntryVM>> AddEntry(string email, EntryVM entryVM);
        Task<CustomResult<EntryVM>> UpdateEntry(string email, EntryVM entryVM);
        Task<CustomResult<EntryVM>> GetItem(string email, int id);
        Task<CustomResult<int>> DeleteEntry(string email, int id);
    }
}
