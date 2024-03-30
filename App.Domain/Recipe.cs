using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class Recipe : BaseEntityId, IDomainAppUser<AppUser>
{
    [MaxLength(256)]
    public string Name { get; set; } = default!;

    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}