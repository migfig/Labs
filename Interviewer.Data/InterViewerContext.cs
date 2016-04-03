using Interviewer.Common;
using System.Data.Entity;

namespace Interviewer.Data
{
    public class InterViewerContext: DbContext
    {
        public InterViewerContext()
            :base(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=Interviewer;Integrated Security=True")
        {
        }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<KnowledgeArea> KnowledgeAreas { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Question> Questions { get; set; }
    }
}
