using Microsoft.EntityFrameworkCore;
using Project.Domain;
using Project.Domain.Entities;

namespace Project.Infra.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        public AppDbContext _context;
        private bool disposedValue;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> Add(User entity)
        {
            var entry = await _context.users.AddAsync(entity);
            _context.SaveChanges();

            return entity;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var entities = await _context.users.ToListAsync();
            return entities!;
        }

        public async Task<User> GetItemByEmail(string email)
        {
            var entity = await _context.users.FirstOrDefaultAsync(p => p.Email == email);
            return entity!;
        }

        public async Task<User> GetItemByEmail(string email, string password)
        {
            var entity = await _context.users.FirstOrDefaultAsync(p => p.Email == email && p.Password == password);
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
