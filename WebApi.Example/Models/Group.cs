using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WebApi.Example.Models
{
    public class Group
    {
        public Guid Id { get; set; }
        public int GroupNumber { get; set; }
        public string Name { get; set; }
        public virtual IList<User> Users { get; set; }
        public Group()
        {
            Users = new List<User>();
        }
    }
}
