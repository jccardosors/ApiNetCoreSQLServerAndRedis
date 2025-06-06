using Microsoft.EntityFrameworkCore;
using Project.Domain;
using Project.Domain.Entities;

namespace Project.Infra.Data.Repositories
{
    public class LogsRepository : ILogsRepository
    {
        public AppDbContext _context;
        private bool disposedValue;

        public LogsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Logs> Add(Logs entity)
        {
            var entry = await _context.logs.AddAsync(entity);
            _context.SaveChanges();

            return entity;
        }

        public async Task<IEnumerable<Logs>> GetAll()
        {
            var entities = await _context.logs.ToListAsync();
            return entities!;
        }

        public async Task<Logs> GetItem(int id)
        {
            var entity = await _context.logs.FirstOrDefaultAsync(p => p.Id == id);
            return entity!;
        }

        #region Disposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~EntryRepository()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
