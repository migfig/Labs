using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Dapper;
using Common;
using System.Threading.Tasks;

namespace RelatedRecords
{
    public static class Helpers
    {
        public static string XmlFile => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.xml");

        public static CConfiguration CreateXmlConfiguration()
        {
            #region xml string

            var xml = @"<?xml version='1.0' encoding='utf-8' ?>
<Configuration defaultDatasource='local' defaultDataset='sample'>
  < Datasource name='local'>
    <ConnectionString>Data Source=localhost\development;Initial Catalog=development;Integrated Security=True</ConnectionString>
  </Datasource>
  <Datasource name='remote'>
    <ConnectionString>Data Source=remote\development;Initial Catalog=development;Integrated Security=True</ConnectionString>
  </Datasource>

  <Dataset name='sample' dataSourceName='local' defaultTable='Tickets'>
    < Table name='Tickets'>
      <Column name='Id' DbType='guid' isPrimaryKey='true' isForeignKey='false' isNullable='false' defaultValue=''/>
      <Column name='TicketNumber' DbType='long' isPrimaryKey='false' isForeignKey='false' isNullable='false' defaultValue=''/>
      <Column name='Title' DbType='string' isPrimaryKey='false' isForeignKey='false' isNullable='false' defaultValue=''/>
      <Column name='StatusCodeId' DbType='int' isPrimaryKey='false' isForeignKey='true' isNullable='false' defaultValue='0'/>
      <Column name='PriorityId' DbType='int' isPrimaryKey='false' isForeignKey='true' isNullable='false' defaultValue='0'/>
      <Column name='UserId' DbType='int' isPrimaryKey='false' isForeignKey='true' isNullable='false' defaultValue='0'/>
    </Table>
    <Table name='TicketsStatusCodes'>
      <Column name='StatusCodeId' DbType='int' isPrimaryKey='true' isForeignKey='false' isNullable='false' defaultValue=''/>
      <Column name='Description' DbType='string' isPrimaryKey='false' isForeignKey='false' isNullable='false' defaultValue=''/>
    </Table>
    <Table name='TicketsPriorities'>
      <Column name='PriorityId' DbType='int' isPrimaryKey='true' isForeignKey='false' isNullable='false' defaultValue=''/>
      <Column name='Description' DbType='string' isPrimaryKey='false' isForeignKey='false' isNullable='false' defaultValue=''/>
    </Table>
    <Table name='Users'>
      <Column name='UserId' DbType='int' isPrimaryKey='true' isForeignKey='false' isNullable='false' defaultValue=''/>
      <Column name='GroupId' DbType='int' isPrimaryKey='false' isForeignKey='true' isNullable='false' defaultValue=''/>
      <Column name='LastName' DbType='string' isPrimaryKey='false' isForeignKey='false' isNullable='false' defaultValue=''/>
      <Column name='FirstName' DbType='string' isPrimaryKey='false' isForeignKey='false' isNullable='false' defaultValue=''/>
      <Column name='EmailAddress' DbType='string' isPrimaryKey='false' isForeignKey='false' isNullable='false' defaultValue=''/>
    </Table>
    <Table name='Groups'>
      <Column name='GroupId' DbType='int' isPrimaryKey='true' isForeignKey='false' isNullable='false' defaultValue=''/>
      <Column name='GroupName' DbType='string' isPrimaryKey='false' isForeignKey='false' isNullable='false' defaultValue=''/>
      <Column name='DepartmentId' DbType='int' isPrimaryKey='false' isForeignKey='true' isNullable='false' defaultValue=''/>
    </Table>
    <Table name='Departments'>
      <Column name='DepartmentId' DbType='int' isPrimaryKey='true' isForeignKey='false' isNullable='false' defaultValue=''/>
      <Column name='DepartmentName' DbType='string' isPrimaryKey='false' isForeignKey='false' isNullable='false' defaultValue=''/>
    </Table>

    <Relationship name='Tickets->TicketsStatusCodes' fromTable='Tickets' toTable='TicketsStatusCodes' fromColumn='StatusCodeId' toColumn='StatusCodeId'/>
    <Relationship name='Tickets->TicketsPriorities' fromTable='Tickets' toTable='TicketsPriorities' fromColumn='PriorityId' toColumn='PriorityId'/>
    <Relationship name='Tickets->Users' fromTable='Tickets' toTable='Users' fromColumn='UserId' toColumn='UserId'/>
    <Relationship name='Users->Groups' fromTable='Users' toTable='Groups' fromColumn='GroupId' toColumn='GroupId'/>
    <Relationship name='Groups->Departments' fromTable='Groups' toTable='Departments' fromColumn='DepartmentId' toColumn='DepartmentId'/>
  </Dataset>

  <Dataset name='sample-remote' dataSourceName='remote' defaultTable='Tickets'>
    <Table name='Tickets'>
      <Column name='Id' DbType='guid' isPrimaryKey='true' isForeignKey='false' isNullable='false' defaultValue=''/>
      <Column name='TicketNumber' DbType='long' isPrimaryKey='false' isForeignKey='false' isNullable='false' defaultValue=''/>
      <Column name='Title' DbType='string' isPrimaryKey='false' isForeignKey='false' isNullable='false' defaultValue=''/>
      <Column name='StatusCodeId' DbType='int' isPrimaryKey='false' isForeignKey='true' isNullable='false' defaultValue='0'/>
      <Column name='PriorityId' DbType='int' isPrimaryKey='false' isForeignKey='true' isNullable='false' defaultValue='0'/>
    </Table>
    <Table name='TicketsStatusCodes'>
      <Column name='StatusCodeId' DbType='int' isPrimaryKey='true' isForeignKey='false' isNullable='false' defaultValue=''/>
      <Column name='Description' DbType='string' isPrimaryKey='false' isForeignKey='false' isNullable='false' defaultValue=''/>
    </Table>
    <Table name='TicketsPriorities'>
      <Column name='PriorityId' DbType='int' isPrimaryKey='true' isForeignKey='false' isNullable='false' defaultValue=''/>
      <Column name='Description' DbType='string' isPrimaryKey='false' isForeignKey='false' isNullable='false' defaultValue=''/>
    </Table>

    <Relationship name='Tickets->TicketsStatusCodes' fromTable='Tickets' toTable='TicketsStatusCodes' fromColumn='StatusCodeId' toColumn='StatusCodeId'/>
    <Relationship name='Tickets->TicketsPriorities' fromTable='Tickets' toTable='TicketsPriorities' fromColumn='PriorityId' toColumn='PriorityId'/>
  </Dataset>
</Configuration>";

            #endregion xml string

            using (var stream = File.CreateText(XmlFile))
            {
                stream.Write(xml);
            }

            return XmlHelper<CConfiguration>.Load(XmlFile);
        }

        public static void RemoveXmlConfiguration()
        {
            File.Delete(XmlFile);
        }

        public static async Task<string> ConfigurationFromConnectionString(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString.Deflated()))
            {
                var xml = await connection.ExecuteScalarAsync(ConfigurationManager.AppSettings["schemaQuery"]
                    .Replace("&quot;", "\""));

                var fileName = Helpers.XmlFile.Replace(".xml", "-generated.xml");
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                var schemafileName = Helpers.XmlFile.Replace(".xml", "-schema.xml");
                if (File.Exists(schemafileName))
                {
                    File.Delete(schemafileName);
                }

                using (var schemaStream = File.CreateText(schemafileName))
                {
                    schemaStream.Write(xml.ToString());
                }

                using (var stream = new XmlTextWriter(fileName, Encoding.UTF8))
                {
                    var newDoc = BuildElements(connectionString, XDocument.Load(new StringReader(xml.ToString()))); 
                    newDoc.WriteTo(stream);
                }

                return fileName;
            }
        }

        public static async Task<XElement> GetConfigurationFromConnectionString(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString.Deflated()))
            {
                try {
                    var xml = await connection.ExecuteScalarAsync(ConfigurationManager.AppSettings["schemaQuery"]
                        .Replace("&quot;", "\""));

                    return BuildElements(connectionString, XDocument.Load(new StringReader(xml.ToString())));
                } catch(Exception e)
                {
                    Common.Extensions.ErrorLog.Error(e, "@ Get config from connstr");
                    return new XElement("void");
                } 
            }
        }

        private static XElement BuildElements(string connectionString, XDocument doc)
        {
            var regEx = new Regex(ConfigurationManager.AppSettings["dataSourceNameRegEx"]
                .Replace("&lt;", "<")
                .Replace("&gt;", ">")
                , RegexOptions.IgnoreCase);

            var dataSourceName = regEx.Match(connectionString).Groups["value"].Value.Split('\\').Last() 
                + DateTime.Now.ToString("HHmmss");

            return new XElement("Configuration",
                        new XAttribute("defaultDatasource", dataSourceName),
                        new XAttribute("defaultDataset", dataSourceName),
                    new XElement("Datasource",
                        new XAttribute("name", dataSourceName),
                        new XElement("ConnectionString", connectionString.Inflated())),
                    new XElement("Dataset",
                        new XAttribute("name", dataSourceName),
                        new XAttribute("dataSourceName", dataSourceName),
                        new XAttribute("defaultTable", doc.Root.Elements("Table").First().Attribute("name").Value),

                        from t in doc.Root.Elements("Table")
                        select
                            new XElement("Table", new XAttribute("name", t.Attribute("name").Value),
                                from c in t.Elements("Column")
                                select
                                    new XElement("Column",
                                        from a in c.Attributes()
                                        select
                                            new XAttribute(a.Name.LocalName,
                                                a.Value
                                                    .Replace("0", "false")
                                                    .Replace("1", "true")
                                                    .Replace("NO", "false")
                                                    .Replace("YES", "true"))
                                        )
                                ),

                        from t in doc.Root.Elements("Table")
                        from c in t.Elements("Column")
                        let isForeignKey = c.Attribute("isForeignKey").Value != "0"
                        where isForeignKey
                            && HasRelationships(doc.Root,
                                t.Attribute("name").Value,
                                c.Attribute("name").Value,
                                isForeignKey,
                                c.Attribute("Constraint").Value)
                        select
                            BuildRelationship(doc.Root,
                                t.Attribute("name").Value,
                                c.Attribute("name").Value,
                                isForeignKey,
                                c.Attribute("Constraint").Value)
                        )
                    );
        }

        private static bool HasRelationships(XElement root,
            string tableName,
            string columnName,
            bool isForeignKey,
            string constraint)
        {
            var toTable = tableName;
            if (isForeignKey)
            {
                toTable = (from t in root.Elements("Table")
                           from c in t.Elements("Column")
                           where c.Attribute("name").Value == columnName
                                && c.Attribute("isPrimaryKey").Value != "0"
                           select t.Attribute("name").Value).FirstOrDefault();
            }

            if(null == toTable && !string.IsNullOrEmpty(constraint))
            {
                var parts = constraint.Split('_');
                if(parts.Length == 2)
                {
                    if(Extensions.SelectedDataset.Table.Any(x => x.name == parts.First()))
                        toTable = parts.First();
                }
                else if(parts.Length == 3)
                {
                    if (Extensions.SelectedDataset.Table.Any(x => x.name == parts.First() + "_" + parts[1]))
                        toTable = parts.First() + "_" + parts[1];
                }
            }

            return null != toTable;
        }

        private static XElement BuildRelationship(XElement root, 
            string tableName, 
            string columnName, 
            bool isForeignKey,
            string constraint)
        {
            var fromTable = tableName;
            var toTable = tableName;
            var toColumn = columnName;

            if (isForeignKey)
            {
                toTable = (from t in root.Elements("Table")
                           from c in t.Elements("Column")
                           where c.Attribute("name").Value == columnName
                                && c.Attribute("isPrimaryKey").Value != "0"
                           select t.Attribute("name").Value).FirstOrDefault();

                if (null == toTable && !string.IsNullOrEmpty(constraint))
                {
                    var parts = constraint.Split('_');
                    if (parts.Length == 2)
                    {
                        var table = Extensions.SelectedDataset.Table.First(x => x.name == parts.First());
                        if (null != table && table.Column.Any(x => x.name == parts.Last()))
                        {
                            toTable = parts.First();
                            toColumn = parts.Last();
                        }
                    }
                    else if (parts.Length == 3)
                    {
                        var table = Extensions.SelectedDataset.Table
                            .First(x => x.name == parts.First() + "_" + parts[1]);
                        if (null != table && table.Column.Any(x => x.name == parts.Last()))
                        {
                            toTable = parts.First() + "_" + parts[1];
                            toColumn = parts.Last();
                        }
                    }
                }
            }

            if (null != fromTable && null != toTable)
            {
                return new XElement("Relationship",
                    new XAttribute("name", string.Format("{0}->{1}", fromTable, toTable)),
                    new XAttribute("fromTable", fromTable),
                    new XAttribute("toTable", toTable),
                    new XAttribute("fromColumn", columnName),
                    new XAttribute("toColumn", toColumn));
            }

            return new XElement("void");
        }

        public static int CreateSampleTables(CConfiguration configuration)
        {
            using (var connection = new SqlConnection(configuration.Datasource.First().ConnectionString.Deflated()))
            {
                var rows = connection.Execute(configuration.Dataset.First().ToSchemaString());
                return connection.Execute(configuration.Dataset.First().ToInsertString(3));
            }
        }

        public static int RemoveSampleTables(CConfiguration configuration)
        {
            using (var connection = new SqlConnection(configuration.Datasource.First().ConnectionString.Deflated()))
            {
                return connection.Execute(configuration.Dataset.First().ToSchemaDropString());
            }
        }
    }
}
