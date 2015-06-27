using Newtonsoft.Json;
using System;

namespace WebApi.Example.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }
        public bool IsLocked { get; set; }
        [JsonIgnore]
        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }
    }
}
