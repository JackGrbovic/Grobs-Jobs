using Job_Post_Website.Model;
using Microsoft.EntityFrameworkCore;

namespace Job_Post_Website.Data
{
    public class MessageDbContext : DbContext
    {
        public MessageDbContext(DbContextOptions<MessageDbContext> options) : base(options) { }

        public DbSet<Message> Message { get; set; }
    }
}
