using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.EF.Repositories;
using Base.DAL.EF;

namespace App.DAL.EF;

public class AppUnitOfWork : BaseUnitOfWork<AppDbContext>, IAppUnitOfWork
{
    private IRecipeRepository? _recipes;
    private IAppUserRepository? _appUsers;

    public AppUnitOfWork(AppDbContext dbContext) : base(dbContext)
    {
    }

    public IRecipeRepository Recipes => _recipes ??= new RecipeRepository(UowDbContext);
    public IAppUserRepository AppUsers => _appUsers ??= new AppUserRepository(UowDbContext);
}