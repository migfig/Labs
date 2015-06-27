using System.Data.Entity;

namespace WebApi.Sample.Models
{
    public class ITContext: DbContext
    {
        public DbSet<Group> Groups { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
