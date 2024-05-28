using App.Contracts.DAL.Repositories;
using App.Domain.Identity;
using Base.Contracts.DAL;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class AppUserRepository : BaseEntityRepository<AppUser, AppUser, AppDbContext>, IAppUserRepository
{
    public AppUserRepository(AppDbContext repoDbContext) : base(repoDbContext,
        new DalDummyMapper<AppUser, AppUser>())
    {
    }
}