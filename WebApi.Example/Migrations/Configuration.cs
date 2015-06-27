namespace WebApi.Example.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApi.Example.Models.ITContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebApi.Example.Models.ITContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            var groups = new string[] { "Administrators", "Support" };
            var users = new List<Tuple<string, string, string, string>>
            {
                new Tuple<string, string, string, string>("John", "Smith", "john.smith@yoursite.com", groups[0]),
                new Tuple<string, string, string, string>("Mary", "Smith", "mary.smith@yoursite.com", groups[1]),
                new Tuple<string, string, string, string>("Peter", "Baum", "peter.baum@yoursite.com", groups[1])
            };

            var grpNumber = 0;
            groups.ToList().ForEach(g =>
            {
                var group = context.Groups.Add(new Models.Group
                {
                    Id = Guid.NewGuid(),
                    GroupNumber = ++grpNumber,
                    Name = g,
                });

                users.Where(u => u.Item4 == g)
                    .ToList()
                    .ForEach(u =>
                {
                    var user = context.Users.Add(new Models.User
                    {
                        Id = Guid.NewGuid(),
                        FirstName = u.Item1,
                        LastName = u.Item2,
                        EmailAddress = u.Item3,
                        Created = DateTime.Now,
                        Expires = DateTime.Now.AddDays(365),
                        IsLocked = false
                    });

                    group.Users.Add(user);
                });
            });

            context.SaveChanges();
        }
    }
}
