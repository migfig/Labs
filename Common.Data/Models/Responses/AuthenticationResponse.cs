using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data.Models.Responses
{
    public class AuthenticationResponse
    {
        public string Token { get; set; }
        public string ExpiretionDate { get; set; }
    }
}
