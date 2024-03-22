using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class Recipe : BaseEntityIdMetadata, IDomainAppUser<AppUser>
{
    [MaxLength(256)]
    public string Name { get; set; } = default!;

    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}