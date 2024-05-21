using F_LocalBrand.Models;

namespace F_LocalBrand.Repository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> FindUserByEmail(string email);
    }
}
