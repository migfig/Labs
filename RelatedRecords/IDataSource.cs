using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RelatedRecords
{
    public interface IDataSource
    {
        Task<DataTable> Load(string connectionString, string query);
        Task<DataTable> LoadStoreProcedure(string connectionString, CQuery query, params IDbDataParameter[] pars);
        Task<string> LoadXml(string connectionString, string query);
    }
}
