using Microsoft.EntityFrameworkCore;
using Resting.API.Controllers;

namespace Resting.API.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<President> Presidents { get; set; }
  }
}