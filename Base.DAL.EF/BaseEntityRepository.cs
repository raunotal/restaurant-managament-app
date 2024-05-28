using Base.Contracts.DAL;
using Base.Contracts.Domain;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;

public class BaseEntityRepository<TDomainEntity, TDalEntity, TDbContext> :
    BaseEntityRepository<Guid, TDomainEntity, TDalEntity,
        TDbContext>, IEntityRepository<TDalEntity>
    where TDomainEntity : class, IDomainEntityId
    where TDalEntity : class, IDomainEntityId
    where TDbContext : DbContext
{
    public BaseEntityRepository(TDbContext repoDbContext, IDalMapper<TDomainEntity, TDalEntity> mapper) : base(
        repoDbContext, mapper)
    {
    }
}

public class BaseEntityRepository<TKey, TDomainEntity, TDalEntity, TDbContext>
    where TKey : IEquatable<TKey>
    where TDomainEntity : class, IDomainEntityId
    where TDalEntity : class, IDomainEntityId
    where TDbContext : DbContext
{
    protected readonly TDbContext RepoDbContext;
    protected readonly DbSet<TDomainEntity> RepoDbSet;
    protected readonly IDalMapper<TDomainEntity, TDalEntity> Mapper;

    public BaseEntityRepository(TDbContext repoDbContext, IDalMapper<TDomainEntity, TDalEntity> mapper)
    {
        RepoDbContext = repoDbContext;
        RepoDbSet = RepoDbContext.Set<TDomainEntity>();
        Mapper = mapper;
    }

    protected virtual IQueryable<TDomainEntity> CreateQuery(TKey? userId, bool noTracking = false)
    {
        var query = RepoDbSet.AsQueryable();

        if (userId != null && !userId.Equals(default) &&
            typeof(IDomainAppUserId<TKey>).IsAssignableFrom(typeof(TDomainEntity)))
        {
            query = query
                .Include("AppUser")
                .Where(entity => ((IDomainAppUserId<TKey>)entity).AppUserId.Equals(userId));
        }

        if (noTracking)
        {
            query = query.AsNoTracking();
        }

        return query;
    }

    public virtual TDalEntity Add(TDalEntity entity)
    {
        return Mapper.MapLR(RepoDbSet.Add(Mapper.MapRL(entity)!).Entity)!;
    }

    public virtual TDalEntity Update(TDalEntity entity)
    {
        return Mapper.MapLR(RepoDbSet.Update(Mapper.MapRL(entity)!).Entity)!;
    }

    public virtual int Delete(TDalEntity entity, TKey? userId = default)
    {
        if (userId == null)
        {
            return RepoDbSet.Where(e => e.Id.Equals(userId)).ExecuteDelete();
        }

        return CreateQuery(userId).Where(e => e.Id.Equals(userId)).ExecuteDelete();
    }

    public virtual int Delete(TKey id, TKey? userId = default)
    {
        if (userId == null)
        {
            return RepoDbSet.Where(e => e.Id.Equals(userId)).ExecuteDelete();
        }

        return CreateQuery(userId).Where(e => e.Id.Equals(userId)).ExecuteDelete();
    }

    public virtual IEnumerable<TDalEntity> GetAll(TKey? userId = default, bool noTracking = false)
    {
        var q = CreateQuery(userId, noTracking);
        return q.ToList().Select(e => Mapper.MapLR(e)).AsEnumerable()!;
    }

    public virtual bool Exists(TKey id, TKey? userId = default)
    {
        return CreateQuery(userId).Any(e => e.Id.Equals(id));
    }


    public virtual async Task<IEnumerable<TDalEntity>> GetAllAsync(TKey? userId = default, bool noTracking = false)
    {
        return (await CreateQuery(userId, noTracking).ToListAsync()).Select(e => Mapper.MapLR(e))!;
    }

    public virtual async Task<bool> ExistsAsync(TKey id, TKey? userId = default)
    {
        return await CreateQuery(userId).AnyAsync(e => e.Id.Equals(id));
    }

    public virtual async Task<int> RemoveAsync(TKey id, TKey? userId = default)
    {
        if (userId == null)
        {
            return await RepoDbSet.Where(e => e.Id.Equals(id)).ExecuteDeleteAsync();
        }

        return await CreateQuery(userId).Where(e => e.Id.Equals(id)).ExecuteDeleteAsync();
    }

    public virtual async Task<int> RemoveAsync(TDalEntity entity, TKey? userId = default)
    {
        if (userId == null)
        {
            return await RepoDbSet.Where(e => e.Id.Equals(entity.Id)).ExecuteDeleteAsync();
        }

        return await CreateQuery(userId).Where(e => e.Id.Equals(entity.Id)).ExecuteDeleteAsync();
    }

    public TDalEntity? FirstOrDefault(TKey id, TKey userId = default, bool noTracking = true)
    {
        return Mapper.MapLR(CreateQuery(userId, noTracking).FirstOrDefault(m => m.Id.Equals(id)));
    }

    public async Task<TDalEntity?> FirstOrDefaultAsync(TKey id, TKey userId = default, bool noTracking = true)
    {
        return Mapper.MapLR(await CreateQuery(userId, noTracking).FirstOrDefaultAsync(m => m.Id.Equals(id)));
    }
}