using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interviewer.Data.Repositories
{
    public interface IDbConnection
    {
        string ConnectionString { get; }
    }
}
