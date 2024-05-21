using F_LocalBrand.Repository;

namespace F_LocalBrand.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository User { get; }
        int Complete();
    }

}
