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

namespace RelatedRecords
{
    public static class Helpers
    {
        public static string XmlFile => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.xml");

        public static CConfiguration CreateXmlConfiguration()
        {
            #region xml string

            var xml = @"<?xml version='1.0' encoding='utf-8' ?>
<Configuration>
  <Datasource name='local'>
    <ConnectionString>Data Source=localhost\development;Initial Catalog=development;Integrated Security=True</ConnectionString>
  </Datasource>
  <Datasource name='remote'>
    <ConnectionString>Data Source=remote\development;Initial Catalog=development;Integrated Security=True</ConnectionString>
  </Datasource>

  <Dataset name='sample' dataSourceName='local'>
    <Table name='Tickets'>
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

  <Dataset name='sample-remote' dataSourceName='remote'>
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

        public static string ConfigurationFromConnectionString(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var xml = connection.ExecuteScalar(ConfigurationManager.AppSettings["schemaQuery"]
                    .Replace("&quot;", "\""));
                var doc = XDocument.Load(new StringReader(xml.ToString()));

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

                var regEx = new Regex(ConfigurationManager.AppSettings["dataSourceNameRegEx"]
                        .Replace("&lt;", "<")
                        .Replace("&gt;", ">")
                    , RegexOptions.IgnoreCase);
                var dataSourceName = regEx.Match(connectionString).Groups["value"].Value;

                using (var stream = new XmlTextWriter(fileName, Encoding.UTF8))
                {
                    var newDoc = new XElement("Configuration",
                        new XElement("Datasource",
                            new XAttribute("name", dataSourceName),
                            new XElement("ConnectionString", connectionString)),
                        new XElement("Dataset",
                            new XAttribute("name", dataSourceName.Split('\\').Last()),
                            new XAttribute("dataSourceName", dataSourceName),

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
                                    isForeignKey)
                            select 
                                BuildRelationship(doc.Root, 
                                    t.Attribute("name").Value, 
                                    c.Attribute("name").Value, 
                                    isForeignKey)
                            )
                        );

                    newDoc.WriteTo(stream);
                }

                return fileName;
            }
        }

        public static XElement GetConfigurationFromConnectionString(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var xml = connection.ExecuteScalar(ConfigurationManager.AppSettings["schemaQuery"]
                    .Replace("&quot;", "\""));
                var doc = XDocument.Load(new StringReader(xml.ToString()));

                var regEx = new Regex(ConfigurationManager.AppSettings["dataSourceNameRegEx"]
                        .Replace("&lt;", "<")
                        .Replace("&gt;", ">")
                    , RegexOptions.IgnoreCase);
                var dataSourceName = regEx.Match(connectionString).Groups["value"].Value;

                return new XElement("Configuration",
                    new XElement("Datasource",
                        new XAttribute("name", dataSourceName),
                        new XElement("ConnectionString", connectionString)),
                    new XElement("Dataset",
                        new XAttribute("name", dataSourceName.Split('\\').Last()),
                        new XAttribute("dataSourceName", dataSourceName),

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
                                isForeignKey)
                        select
                            BuildRelationship(doc.Root,
                                t.Attribute("name").Value,
                                c.Attribute("name").Value,
                                isForeignKey)
                        )
                    );
            }
        }

        private static bool HasRelationships(XElement root,
            string tableName,
            string columnName,
            bool isForeignKey)
        {
            var toTable = tableName;
            if (isForeignKey)
            {
                toTable = (from t in root.Elements("Table")
                           from c in t.Elements("Column")
                           where c.Attribute("name").Value == columnName
                                && c.Attribute("isPrimaryKey").Value == "1"
                           select t.Attribute("name").Value).FirstOrDefault();
            }

            return null != toTable;
        }

        private static XElement BuildRelationship(XElement root, 
            string tableName, 
            string columnName, 
            bool isForeignKey)
        {
            var fromTable = tableName;
            var toTable = tableName;

            if (isForeignKey)
            {
                toTable = (from t in root.Elements("Table")
                           from c in t.Elements("Column")
                           where c.Attribute("name").Value == columnName
                                && c.Attribute("isPrimaryKey").Value == "1"
                           select t.Attribute("name").Value).FirstOrDefault();
            }

            if (null != fromTable && null != toTable)
            {
                return new XElement("Relationship",
                    new XAttribute("name", string.Format("{0}->{1}", fromTable, toTable)),
                    new XAttribute("fromTable", fromTable),
                    new XAttribute("toTable", toTable),
                    new XAttribute("fromColumn", columnName),
                    new XAttribute("toColumn", columnName));
            }

            return new XElement("void");
        }

        public static int CreateSampleTables(CConfiguration configuration)
        {
            using (var connection = new SqlConnection(configuration.Datasource.First().ConnectionString))
            {
                var rows = connection.Execute(configuration.Dataset.First().ToSchemaString());
                return connection.Execute(configuration.Dataset.First().ToInsertString(3));
            }
        }

        public static int RemoveSampleTables(CConfiguration configuration)
        {
            using (var connection = new SqlConnection(configuration.Datasource.First().ConnectionString))
            {
                return connection.Execute(configuration.Dataset.First().ToSchemaDropString());
            }
        }
    }
}
