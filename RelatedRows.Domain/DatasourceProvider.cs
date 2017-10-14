using Serilog;
using System.Data;
using System.Threading.Tasks;

namespace RelatedRows.Domain
{
    public interface IDatasourceProvider
    {
        Task<string> GetSchema(CDatasource source, string tableName = "");
        Task<string> GetStoreProcsSchema(CDatasource source, string storeProcName = "");
        Task<ITabularSource> GetData(CDatasource source, CTable table, string query);
        Task<DataTable> GetData(CDatasource source, string name, string query);
        Task<DataTable> GetStoreProcedureData(CDatasource source, string name, params IDbDataParameter[] pars);
        Task<DataTable> GetQueryData(CDatasource source, string query);
        void SetLog(ILogger logger);
        bool IsDefault { get; }
        string Name { get; }
        QueryBuilderBase QueryBuilder { get; }
        string GetConnectionStringFormat(bool isTrusted = false);
    }    
}
