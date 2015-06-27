using System;
using System.Collections.Generic;

namespace WebApi.Sample.Models
{
    public class Group
    {
        public Guid Id { get; set; }
        public int GroupNumber { get; set; }
        public string Name { get; set; }
        public virtual IList<User> Users { get; set; }
    }
}
