using F_LocalBrand.Models;
using F_LocalBrand.Repository;

namespace F_LocalBrand.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SWD_FLocalBrandContext _context;
        public IUserRepository User { get; private set; }

        public UnitOfWork(SWD_FLocalBrandContext context)
        {
            _context = context;
            User = new UserRepository(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
