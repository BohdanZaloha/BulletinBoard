using BulletinBoardApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoardApi.Data
{
    public class BulletinBoardDbContext : DbContext
    {
        public BulletinBoardDbContext(DbContextOptions<BulletinBoardDbContext> options)
            : base(options)
        {
        }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
      
    }
}
