using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

        public Task<DataTable> Load(string dataSetName, string query)
        {
            var tableName = Extensions.ParseTableName(query);
            dataSetName = dataSetName.ToLower().Contains("catalog") 
                ? Extensions.SelectedDataset.name 
                : dataSetName;

            var whereParts = query.Split(new string[] { "WHERE" }, StringSplitOptions.RemoveEmptyEntries);
            var where = whereParts.Length == 2
                ? whereParts.Last()
                : string.Empty;

            var topRows = int.Parse(new Regex(@"TOP[\s]*(?<topn>\d{1,})[\s]*").Match(query).Groups["topn"].Value);
            var table = _dataSets
                    .First(x => x.DataSetName == (!string.IsNullOrWhiteSpace(dataSetName)
                            ? dataSetName
                            : Extensions.SelectedDataset.name))
                        .Tables[tableName];
            var rows = table.Select(where).Take(Math.Min(topRows, table.Rows.Count));
            var value = table.Clone();
            foreach (var r in rows)
            {
                value.Rows.Add(r.ItemArray);
            }
            
            return Task.FromResult(value);
        }

        public Task<DataTable> LoadStoreProcedure(string dataSetName, CQuery query, 
            params IDbDataParameter[] pars)
        {
            throw new NotImplementedException();
        }

        public Task<string> LoadXml(string dataSetName, string query)
        {
            throw new NotImplementedException();
        }
    }
}
