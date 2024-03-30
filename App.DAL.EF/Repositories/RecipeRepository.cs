using App.Contracts.DAL.Repositories;
using App.Domain;
using Base.Contracts.DAL;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class RecipeRepository : BaseEntityRepository<Recipe, Recipe, AppDbContext>, IRecipeRepository
{
    public RecipeRepository(AppDbContext repoDbContext) : base(repoDbContext, new DalDummyMapper<Recipe, Recipe>())
    {
    }
}