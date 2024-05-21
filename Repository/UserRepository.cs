using F_LocalBrand.Models;
using Microsoft.EntityFrameworkCore;

namespace F_LocalBrand.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(SWD_FLocalBrandContext context) : base(context)
        {

        }

        public async Task<User?> FindUserByEmail(string email)
        {
            return await _context.Users.Where(u => u.Email.Equals(email)).FirstOrDefaultAsync();
        }
    }
}
