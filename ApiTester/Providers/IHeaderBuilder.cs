using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiTester.Models;

namespace ApiTester.Providers
{
    public interface IHeaderBuilder
    {
        string BuildHeader(BuildHeader builder);
    }

    public class @Default : IHeaderBuilder
    {
        public string BuildHeader(BuildHeader builder)
        {
            foreach(var task in builder.workflow)
            {
                
            }

            return "?";
        }
    }
}
