using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace RelatedRecords
{
    public class SqlDataSource : IDataSource
    {
        public async Task<DataTable> Load(string connectionString, string query)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var rdr = await connection.ExecuteReaderAsync(query);
                var table = new DataTable(Extensions.ParseTableName(query));
                table.Load(rdr);
                rdr.Close();

                return table;
            }
        }

        public async Task<DataTable> LoadStoreProcedure(string connectionString, CQuery query, params IDbDataParameter[] pars)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var cmd = connection.CreateCommand())
                {
                    cmd.Connection.Open();
                    cmd.CommandText = query.Text;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(pars);
                    var reader = await cmd.ExecuteReaderAsync();
                    var table = new DataTable(query.name);
                    table.Load(reader);
                    reader.Close();

                    return table;
                }
            }
        }

        public async Task<string> LoadXml(string connectionString, string query)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                return (await connection.ExecuteScalarAsync(query)).ToString();
            }
        }
    }
}
