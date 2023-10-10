using BulkyWeb_book_temp.Model;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb_book_temp.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) 
        {
        }
        public  DbSet<Category> Categories { get;set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 1, Name = "Scifi", DisplayOrder = 2 },
                new Category { Id = 1, Name = "History", DisplayOrder = 3 }
                );
        }
    }
}
