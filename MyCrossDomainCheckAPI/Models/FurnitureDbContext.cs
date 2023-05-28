using Microsoft.EntityFrameworkCore;

namespace MyCrossDomainCheckAPI.Models
{
    public class FurnitureDbContext: DbContext
    {
        public FurnitureDbContext(DbContextOptions<FurnitureDbContext> options) : base(options)
        {

        }
        public DbSet<Furniture> Furnitures { get; set; }
    }
}
