using Job_Post_Website.Model;
using Microsoft.EntityFrameworkCore;

namespace Job_Post_Website.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        public DbSet<Job> Job { get; set; }
        
    }
}
