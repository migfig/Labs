using System.Data.Entity;

namespace WebApi.Example.Models
{
    public class ITContext: DbContext
    {
        public ITContext()
            :base("name=DefaultConnection")
        {
        }
        public DbSet<Group> Groups { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
