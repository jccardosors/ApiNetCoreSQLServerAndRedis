using Microsoft.EntityFrameworkCore;
using Project.Domain;
using Project.Domain.Entities;

namespace Project.Infra.Data.Repositories
{
    public class ControlUserAccessRepository : IControlUserAccessRepository
    {
        public AppDbContext _context;
        private bool disposedValue;

        public ControlUserAccessRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ControlUserAccess> Add(ControlUserAccess entity)
        {
            var entry = await _context.controlUsers.AddAsync(entity);
            _context.SaveChanges();

            return entity;
        }

        public async Task<ControlUserAccess> Update(ControlUserAccess entity)
        {
            var registro = await Task.FromResult(_context.Set<ControlUserAccess>().Update(entity));
            registro.State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<ControlUserAccess>> GetAll()
        {
            var entities = await _context.controlUsers.ToListAsync();
            return entities!;
        }

        public async Task<ControlUserAccess> GetItem(int id)
        {
            var entity = await _context.controlUsers.FirstOrDefaultAsync(p => p.Id == id);
            return entity!;
        }

        public async Task<ControlUserAccess> GetItem(string email)
        {
            var userAccess = await _context.controlUsers.FirstOrDefaultAsync(p => p.UserEmail == email);
            return userAccess!;
        }

        public async Task<bool> GetUserBlocked(string email)
        {
            var userAccess = await _context.controlUsers.FirstOrDefaultAsync(p => p.UserEmail.Trim() == email);
            if (userAccess is not null && userAccess.Blocked)
            {
                return false;
            }
            else
            {
                return true;
            }
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
