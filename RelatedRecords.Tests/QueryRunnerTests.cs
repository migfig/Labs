using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace RelatedRecords.Tests
{
    [TestClass]
    public class QueryRunnerTests
    {
        private CConfiguration _config;

        [TestInitialize]
        public void Create_Xml_And_Sample_Tables()
        {
            _config = Helpers.CreateXmlConfiguration();
            Assert.IsNotNull(_config);
            Extensions.SelectedConfiguration = _config;

            var rows = Helpers.CreateSampleTables(_config);
            Assert.AreEqual(18, rows);
        }

        [TestMethod]
        public void Single_Table_Query_Run_Test()
        {
            Assert.IsNotNull(_config);

            var t = _config.Dataset.First().Table.First();
            var query = t
                .ToSelectWhereString("=|>=".Split('|'),
                    "And".Split('|'),
                    true,
                    new SqlParameter { ParameterName = "StatusCodeId", Value = 1 },
                    new SqlParameter { ParameterName = "PriorityId", Value = 1 }
                );

            using (var connection = new SqlConnection(_config.Datasource.First().ConnectionString))
            {
                var reader = connection.ExecuteReader(query);
                Assert.IsNotNull(reader);
                var table = new DataTable(t.name);
                table.Load(reader);
                Assert.AreEqual(1, table.Rows.Count);
                Assert.AreEqual(6, table.Columns.Count);
                Assert.AreEqual("PriorityId", table.Columns[4].ColumnName);
                Assert.AreEqual(1, table.Rows[0][4]);
            }
        }

        [TestMethod]
        public void Related_Tables_Query_Run_Test()
        {
            Assert.IsNotNull(_config);

            var t = _config.Dataset.First().Table.First();
            var query = t
                .ToSelectWhereString("=|>=".Split('|'),
                    "And".Split('|'),
                    true,
                    new SqlParameter { ParameterName = "StatusCodeId", Value = 1 },
                    new SqlParameter { ParameterName = "PriorityId", Value = 1 }
                );

            using (var connection = new SqlConnection(_config.Datasource.First().ConnectionString))
            {
                var reader = connection.ExecuteReader(query);
                Assert.IsNotNull(reader);
                var table = new DataTable(t.name);
                table.Load(reader);

                var queries = t
                    .RelatedTablesSelect(table.Rows[0]);
                Assert.IsTrue(!string.IsNullOrEmpty(queries));
                queries.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries)
                    .ToList()
                    .ForEach(q =>
                {
                    var rdr = connection.ExecuteReader(q);
                    Assert.IsNotNull(rdr);
                    var tbl = new DataTable("");
                    tbl.Load(rdr);
                    Assert.AreEqual(1, tbl.Rows.Count);
                    Assert.IsTrue(tbl.Columns.Count > 1);
                    Assert.IsTrue(tbl.Columns[0].ColumnName.EndsWith("Id"));
                    Assert.IsTrue((int)tbl.Rows[0][0] >= 1);

                });
            }
        }

        [TestMethod]
        public void Single_Call_Query_Table_And_Related_Tables_Test()
        {
            Assert.IsNotNull(_config);

            var result = _config.Dataset.First().Table.First()
                .Query("".ToArray("=",">="),
                    "".ToArray("And"),
                    true,
                    new SqlParameter { ParameterName = "StatusCodeId", Value = 1 },
                    new SqlParameter { ParameterName = "PriorityId", Value = 1 }
                );
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Root);
            Assert.IsNotNull(result.Root.Table);
            Assert.AreEqual(1, result.Root.Table.Rows.Count);
            Assert.IsNotNull(result.Children);
            Assert.AreEqual(3, result.Children.Count());
            Assert.AreEqual(1, result.Children[0].Table.Rows.Count);
            Assert.AreEqual(1, result.Children[1].Table.Rows.Count);
        }

        [TestCleanup]
        public void Remove_Xml_And_Sample_Tables()
        {
            Assert.IsNotNull(_config);
            Helpers.RemoveXmlConfiguration();
            var rows = Helpers.RemoveSampleTables(_config);
            Assert.AreEqual(-1, rows);

            _config = null;
        }
    }
}
