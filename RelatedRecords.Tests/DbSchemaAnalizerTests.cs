using System.Data;
using System.Data.SqlClient;
using System.IO;
using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace RelatedRecords.Tests
{
    [TestClass]
    public class DbSchemaAnalizerTests
    {
        private CConfiguration _config;

        [TestInitialize]
        public void Create_Xml_And_Sample_Tables()
        {
            _config = Helpers.CreateXmlConfiguration();
            Assert.IsNotNull(_config);

            var rows = Helpers.CreateSampleTables(_config);
            Assert.AreEqual(9, rows);
        }

        [TestMethod]
        public void Db_Schema_Analizer_Generator_Test()
        {
            Assert.IsNotNull(_config);

            var generatedFile = Helpers.ConfigurationFromConnectionString(
                _config.Datasource.First().ConnectionString);
            Assert.IsTrue(!string.IsNullOrEmpty(generatedFile));
            Assert.IsTrue(File.Exists(generatedFile));
        }

        [TestCleanup]
        public void Remove_Xml_And_Sample_Tables()
        {
            var rows = Helpers.RemoveSampleTables(_config);
            Assert.AreEqual(-1, rows);
            Helpers.RemoveXmlConfiguration();
            _config = null;
        }
    }
}
