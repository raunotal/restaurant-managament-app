using Microsoft.AspNetCore.Identity;

public interface IDomainAppUser<TUser> : IDomainAppUser<Guid, TUser> 
    where TUser : IdentityUser<Guid>
{
}

public interface IDomainAppUser<TKey, TUser>
    where TKey : IEquatable<TKey>
    where TUser : IdentityUser<TKey>
{
    public TKey AppUserId { get; }
    public TUser? AppUser { get; set; }
} 