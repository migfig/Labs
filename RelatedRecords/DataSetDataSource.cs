using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RelatedRecords
{
    public class DataSetDataSource : IDataSource
    {
        private readonly List<DataSet> _dataSets;

        public DataSetDataSource()
        {
            var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dxml");
            _dataSets = new List<DataSet>();
            foreach (var file in files)
            {
                var ds = new DataSet(Path.GetFileNameWithoutExtension(file));
                ds.ReadXml(file);
                _dataSets.Add(ds);
            }
        }

        public Task<DataTable> Load(string connectionString, string query)
        {
            var tableName = Extensions.ParseTableName(query);

            return Task.FromResult(
                _dataSets
                    .First(x => x.DataSetName == Extensions.SelectedDataset.name)
                        .Tables[tableName]);
        }

        public Task<DataTable> LoadStoreProcedure(string connectionString, CQuery query, params IDbDataParameter[] pars)
        {
            throw new NotImplementedException();
        }

        public Task<string> LoadXml(string connectionString, string query)
        {
            throw new NotImplementedException();
        }
    }
}
