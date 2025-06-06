using Microsoft.EntityFrameworkCore;
using Project.Domain;
using Project.Domain.Entities;

namespace Project.Infra.Data.Repositories
{
    public class EntryRepository : IEntryRepository
    {
        public AppDbContext _context;

        public EntryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Entry> Add(Entry entity)
        {
            var entry = await _context.entries.AddAsync(entity);
            _context.SaveChanges();

            return entity;
        }

        public async Task<Entry> Update(Entry entity)
        {
            var registro = await Task.FromResult(_context.Set<Entry>().Update(entity));
            registro.State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<Entry>> GetAll()
        {
            var entities = await _context.entries.ToListAsync();
            return entities!;
        }

        public async Task<Entry> GetItem(int id)
        {
            var entity = await _context.entries.FirstOrDefaultAsync(p => p.Id == id);
            return entity!;
        }

        public async Task<int> Delete(int id)
        {
            var entity = await _context.entries.FirstOrDefaultAsync(p => p.Id == id);

            _context.Set<Entry>().Remove(entity);
            var result = await _context.SaveChangesAsync();

            return result;
        }

        public async Task<IEnumerable<Entry>> GetAllByPeriod(DateTime initialDate, DateTime finalDate)
        {
            var entities = await _context.entries.Where(p => p.DateEntry.Date >= initialDate.Date || p.DateEntry <= finalDate.Date).ToListAsync();
            return entities!;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }


    }
}
