﻿using Dapper;
using Serilog;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
using RelatedRows.Domain;
using System.ComponentModel.Composition;

namespace RelatedRows.Providers.SqlServer
{
    [Export(typeof(IConnectable))]
    public class SqlServerProvider : IDatasourceProvider, IConnectable
    {
        private ILogger _logger;

        public string Name => "SqlServer";
        public bool IsDefault => true;
        public QueryBuilderBase QueryBuilder => new SqlServerQueryBuilder();

        public IConnectable Get() { return this; }
        public void SetLog(ILogger logger) { _logger = logger; }
        public override string ToString() { return Name; }

        public string GetConnectionStringFormat(bool isTrusted = false)
        {
            return isTrusted
                ? @"Data Source={0};Initial Catalog={1};Integrated Security=True;Connect Timeout=120;MultipleActiveResultSets=True;Asynchronous Processing=True;Enlist=false;"
                : @"Data Source={0};Initial Catalog={1};User={2};Password={3};Connect Timeout=120;MultipleActiveResultSets=True;Asynchronous Processing=True;Enlist=false;";
        }

        public async Task<ITabularSource> GetData(CDatasource source, CTable table, string query)
        {
            _logger.Information("Running query {@query} on server {@server}.{@databaseName} for table {@name}", query, source.serverName, source.databaseName, table.name);

            using (var connection = new SqlConnection(source.ConnectionString))
            {
                var rdr = await connection.ExecuteReaderAsync(query);
                var data = new TabularSource(table);
                data.LoadFromReader(rdr);
                rdr.Close();

                return data;
            }
        }

        public async Task<DataTable> GetData(CDatasource source, string name, string query)
        {
            _logger.Information("Running query {@query} on server {@server}.{@databaseName} for table {@name}", query, source.serverName, source.databaseName, name);

            using (var connection = new SqlConnection(source.ConnectionString))
            {
                var rdr = await connection.ExecuteReaderAsync(query);
                var data = new DataTable(name);
                data.Load(rdr);
                rdr.Close();

                if (data.Columns.IndexOf("CountOver$") >= 0)
                {
                    if (data.Rows.Count > 0)
                        data.Namespace = data.Rows[0]["CountOver$"].ToString();
                    data.Columns.Remove("CountOver$");
                }

                return data;
            }
        }

        public async Task<DataTable> GetStoreProcedureData(CDatasource source, string name, params IDbDataParameter[] pars)
        {
            _logger.Information("Running store proc {@name} on server {@server}.{@databaseName} with params {@pars}", name, source.serverName, source.databaseName, pars.Select(p => $", {p.ParameterName}={p.Value}"));

            using (var connection = new SqlConnection(source.ConnectionString))
            {
                using (var cmd = connection.CreateCommand())
                {
                    cmd.Connection.Open();
                    cmd.CommandText = name;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(pars);
                    var reader = await cmd.ExecuteReaderAsync();
                    var table = new DataTable(name);
                    table.Load(reader);
                    reader.Close();

                    return table;
                }
            }
        }

        public async Task<DataTable> GetQueryData(CDatasource source, string query)
        {
            _logger.Information("Running query {@query} on server {@server}.{@databaseName}", query, source.serverName, source.databaseName);

            using (var connection = new SqlConnection(source.ConnectionString))
            {
                using (var cmd = connection.CreateCommand())
                {
                    cmd.Connection.Open();
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    var reader = await cmd.ExecuteReaderAsync();
                    var fromIdx = query.ToUpper().IndexOf("FROM") + 5;
                    var portion = query.Substring(fromIdx).Split(' ');
                    var table = new DataTable(portion.FirstOrDefault().Trim());
                    table.Load(reader);
                    reader.Close();

                    return table;
                }
            }
        }

        public async Task<string> GetSchema(CDatasource source, string tableName = "")
        {
            _logger.Information("Retrieving schema for connection string {0}", source.ConnectionString.SecureString());

            using (var connection = new SqlConnection(source.ConnectionString))
            {
                var resourceName = string.IsNullOrEmpty(tableName) ? "schema" : "schema-table";
                using (var stream = new StreamReader(Assembly.GetAssembly(typeof(SqlServerProvider))
                        .GetManifestResourceStream("RelatedRows.Providers.SqlServer.Properties." + resourceName + ".sql")))
                {
                    var query = await stream.ReadToEndAsync();
                    using (var rdr = await connection.ExecuteReaderAsync(query.Replace("@tableName", tableName), commandTimeout: 120))
                    {
                        var data = string.Empty;
                        if (rdr.Read()) data = rdr.GetValue(0).ToString();
                        rdr.Close();

                        return data;
                    }
                }
            }
        }

        public async Task<string> GetStoreProcsSchema(CDatasource source, string storeProcName = "")
        {
            _logger.Information("Retrieving store procs schema for connection string {0}", source.ConnectionString.SecureString());

            using (var connection = new SqlConnection(source.ConnectionString))
            {
                var resourceName = string.IsNullOrEmpty(storeProcName) ? "store-procs" : "store-procs-name";
                using (var stream = new StreamReader(Assembly.GetAssembly(typeof(SqlServerProvider))
                        .GetManifestResourceStream("RelatedRows.Providers.SqlServer.Properties." + resourceName + ".sql")))
                {
                    var query = await stream.ReadToEndAsync();
                    using (var rdr = await connection.ExecuteReaderAsync(query.Replace("@storeProcName", storeProcName), commandTimeout: 120))
                    {
                        var data = string.Empty;
                        if (rdr.Read()) data = rdr.GetValue(0).ToString();
                        rdr.Close();

                        return data;
                    }
                }
            }
        }
    }
}
