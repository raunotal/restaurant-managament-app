using App.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF;

public class AppDbContext : DbContext
{
    public DbSet<Recipe> Recipes { get; set; } = default!;
}