using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Data.SqlClient;

namespace RelatedRecords.Tests
{
    [TestClass]
    public class ConfigurationTests
    {
        private CConfiguration _config;

        [TestInitialize]
        public void Create_Xml_Configuration()
        {
            _config = Helpers.CreateXmlConfiguration();
        }

        #region configuration load/saving

        [TestMethod]
        public void Xml_Configuration_Loading_Test()
        {            
            Assert.IsNotNull(_config);
            Assert.AreEqual(2, _config.Datasource.Count);
            Assert.AreEqual("local", _config.Datasource.First().name);

            Assert.AreEqual(2, _config.Dataset.Count);
            Assert.AreEqual("sample", _config.Dataset.First().name);

            Assert.AreEqual(6, _config.Dataset.First().Table.Count);
            Assert.AreEqual("Tickets", _config.Dataset.First().Table.First().name);
            Assert.AreEqual(6, _config.Dataset.First().Table.First().Column.Count);

            Assert.AreEqual("Id", _config.Dataset.First().Table.First().Column.First().name);
            Assert.AreEqual(eDbType.guid, _config.Dataset.First().Table.First().Column.First().DbType);

            Assert.AreEqual(5, _config.Dataset.First().Relationship.Count);
            Assert.AreEqual("Tickets->TicketsStatusCodes", _config.Dataset.First().Relationship.First().name);
            Assert.AreEqual("Tickets", _config.Dataset.First().Relationship.First().fromTable);
            Assert.AreEqual("TicketsStatusCodes", _config.Dataset.First().Relationship.First().toTable);
            Assert.AreEqual("StatusCodeId", _config.Dataset.First().Relationship.First().fromColumn);
            Assert.AreEqual("StatusCodeId", _config.Dataset.First().Relationship.First().toColumn);
        }

        [TestMethod]
        public void Xml_Configuration_Saving_Test()
        {
            _config.Dataset.Last().name = "updated-source";
            var result = XmlHelper<CConfiguration>.Save(Helpers.XmlFile, _config);
            Assert.AreEqual(true, result);

            _config = XmlHelper<CConfiguration>.Load(Helpers.XmlFile);
            Assert.AreEqual("updated-source", _config.Dataset.Last().name);
        }

        #endregion configuration load/saving

        #region query builder

        [TestMethod]
        public void Table_To_Select_Query_String_Test()
        {
            Assert.IsNotNull(_config);

            var query = _config.Dataset.First().Table.First().ToSelectString(true);
            Assert.AreEqual("SELECT * FROM Tickets", query.Trim());

            query = _config.Dataset.First().Table.First().ToSelectString();
            Assert.AreEqual("SELECT [Id],[TicketNumber],[Title],[StatusCodeId],[PriorityId],[UserId] FROM Tickets",
                query.Trim());
        }

        [TestMethod]
        public void Table_To_Where_Query_String_Test()
        {
            Assert.IsNotNull(_config);
            //handling of between and like % left or right and other operators need to be also implemented
            var id = Guid.NewGuid();
            var query = _config.Dataset.First().Table.First()
                .ToWhereString("=|>=|<".Split('|'),
                    "And|And".Split('|'),
                    new SqlParameter { ParameterName = "Id", Value = id.ToString() },
                    new SqlParameter { ParameterName = "StatusCodeId", Value = 1 },
                    new SqlParameter { ParameterName = "PriorityId", Value = 10 }
                );
            Assert.AreEqual("WHERE Id = '" + id.ToString() + "' And StatusCodeId >= 1 And PriorityId < 10",
                query.Trim());

            query = _config.Dataset.First().Table.First()
                .ToSelectWhereString("=|>=|<".Split('|'),
                    "And|And".Split('|'),
                    true,
                    new SqlParameter { ParameterName = "Id", Value = id.ToString() },
                    new SqlParameter { ParameterName = "StatusCodeId", Value = 1 },
                    new SqlParameter { ParameterName = "PriorityId", Value = 10 }
                );
            Assert.AreEqual("SELECT * FROM Tickets WHERE Id = '" 
                + id.ToString() 
                + "' And StatusCodeId >= 1 And PriorityId < 10", query);
        }

        [TestMethod]
        public void Column_To_Select_Query_String_Test()
        {
            Assert.IsNotNull(_config);

            var query = _config.Dataset.First().Table.First().Column.First().ToSelectString(true);
            Assert.AreEqual("[Id]", query);
        }

        [TestMethod]
        public void Column_To_Where_Query_String_Test()
        {
            Assert.IsNotNull(_config);

            var id = Guid.NewGuid();
            var query = _config.Dataset.First().Table.First().Column.First()
                .ToWhereString(id.ToString(), true);
            Assert.AreEqual("Id = '" + id.ToString() + "'", query.Trim());

            query = _config.Dataset.First().Table.First().Column.Last()
                .ToWhereString(new SqlParameter { ParameterName = "UserId", Value = 100 },
                    false, ">=");
            Assert.AreEqual("And UserId >= 100", query.Trim());
        }

        [TestMethod]
        public void Build_Related_Tables_Queries()
        {
            Assert.IsNotNull(_config);

            var table = _config.Dataset.First().Table.First().ToDataTable();
            Assert.IsNotNull(table);
            Assert.AreEqual(1, table.Rows.Count);

            var queries = _config.Dataset.First().Table.First()
                .RelatedTablesSelect(_config.Dataset.First(), table.Rows[0]);
            Assert.AreEqual(3, queries.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Count());
            Assert.AreEqual("SELECT * FROM TicketsStatusCodes WHERE StatusCodeId = 1;SELECT * FROM TicketsPriorities WHERE PriorityId = 1;SELECT * FROM Users WHERE UserId = 1;", 
                queries);
        }

        #endregion query builder

        #region schema builder

        [TestMethod]
        public void Build_Table_Schema()
        {
            Assert.IsNotNull(_config);
            var query = _config.Dataset.First().Table.First()
                .ToSchemaString();
            Assert.AreEqual("create Table [dbo].[Tickets] ([Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,[TicketNumber] BIGINT NOT NULL,[Title] VARCHAR(250) NOT NULL,[StatusCodeId] INT NOT NULL,[PriorityId] INT NOT NULL,[UserId] INT NOT NULL);", query);
        }

        [TestMethod]
        public void Build_Foreign_Constraint_Table_Schema()
        {
            Assert.IsNotNull(_config);
            var query = _config.Dataset.First().Relationship.First()
                .ToConstraint();
            Assert.AreEqual("Alter Table [dbo].[Tickets] Add Constraint FK_TicketsStatusCodes_StatusCodeId Foreign Key (StatusCodeId) References [dbo].[TicketsStatusCodes](StatusCodeId);", query);
        }

        [TestMethod]
        public void Build_Dataset_Schema()
        {
            Assert.IsNotNull(_config);
            var query = _config.Dataset.First().ToSchemaString();

            Assert.AreEqual("create Table [dbo].[Tickets] ([Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,[TicketNumber] BIGINT NOT NULL,[Title] VARCHAR(250) NOT NULL,[StatusCodeId] INT NOT NULL,[PriorityId] INT NOT NULL,[UserId] INT NOT NULL);create Table [dbo].[TicketsStatusCodes] ([StatusCodeId] INT NOT NULL PRIMARY KEY,[Description] VARCHAR(250) NOT NULL);create Table [dbo].[TicketsPriorities] ([PriorityId] INT NOT NULL PRIMARY KEY,[Description] VARCHAR(250) NOT NULL);create Table [dbo].[Departments] ([DepartmentId] INT NOT NULL PRIMARY KEY,[DepartmentName] VARCHAR(250) NOT NULL);create Table [dbo].[Users] ([UserId] INT NOT NULL PRIMARY KEY,[GroupId] INT NOT NULL,[LastName] VARCHAR(250) NOT NULL,[FirstName] VARCHAR(250) NOT NULL,[EmailAddress] VARCHAR(250) NOT NULL);create Table [dbo].[Groups] ([GroupId] INT NOT NULL PRIMARY KEY,[GroupName] VARCHAR(250) NOT NULL,[DepartmentId] INT NOT NULL);Alter Table [dbo].[Tickets] Add Constraint FK_TicketsStatusCodes_StatusCodeId Foreign Key (StatusCodeId) References [dbo].[TicketsStatusCodes](StatusCodeId);Alter Table [dbo].[Tickets] Add Constraint FK_TicketsPriorities_PriorityId Foreign Key (PriorityId) References [dbo].[TicketsPriorities](PriorityId);Alter Table [dbo].[Tickets] Add Constraint FK_Users_UserId Foreign Key (UserId) References [dbo].[Users](UserId);Alter Table [dbo].[Users] Add Constraint FK_Groups_GroupId Foreign Key (GroupId) References [dbo].[Groups](GroupId);Alter Table [dbo].[Groups] Add Constraint FK_Departments_DepartmentId Foreign Key (DepartmentId) References [dbo].[Departments](DepartmentId);", 
                query);

            var sqlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                _config.Dataset.First().name + ".sql");
            if (File.Exists(sqlFile))
            {
                File.Delete(sqlFile);
            }
            using (
                var stream =
                    new StreamWriter(sqlFile))
            {
                stream.Write(query.Replace(";", $";{Environment.NewLine}"));
            }
        }

        #endregion schema builder

        #region sample data builder

        [TestMethod]
        public void Build_Table_Sample_Data()
        {
            Assert.IsNotNull(_config);
            var query = _config.Dataset.First().Table.First()
                .ToInsertString();
            Assert.AreEqual("Insert Into [dbo].[Tickets] ([Id],[TicketNumber],[Title],[StatusCodeId],[PriorityId],[UserId]) Values ('00100000-0000-0000-0000-000000000000',1,'Tickets 1',1,1,1);", 
                query);
        }

        [TestMethod]
        public void Build_Dataset_Sample_Data()
        {
            Assert.IsNotNull(_config);
            var query = _config.Dataset.First().ToInsertString(3);

            Assert.AreEqual("Insert Into [dbo].[TicketsStatusCodes] ([StatusCodeId],[Description]) Values (1,'TicketsStatusCodes 1');Insert Into [dbo].[TicketsStatusCodes] ([StatusCodeId],[Description]) Values (2,'TicketsStatusCodes 2');Insert Into [dbo].[TicketsStatusCodes] ([StatusCodeId],[Description]) Values (3,'TicketsStatusCodes 3');Insert Into [dbo].[TicketsPriorities] ([PriorityId],[Description]) Values (1,'TicketsPriorities 1');Insert Into [dbo].[TicketsPriorities] ([PriorityId],[Description]) Values (2,'TicketsPriorities 2');Insert Into [dbo].[TicketsPriorities] ([PriorityId],[Description]) Values (3,'TicketsPriorities 3');Insert Into [dbo].[Departments] ([DepartmentId],[DepartmentName]) Values (1,'Departments 1');Insert Into [dbo].[Departments] ([DepartmentId],[DepartmentName]) Values (2,'Departments 2');Insert Into [dbo].[Departments] ([DepartmentId],[DepartmentName]) Values (3,'Departments 3');Insert Into [dbo].[Groups] ([GroupId],[GroupName],[DepartmentId]) Values (1,'Groups 1',1);Insert Into [dbo].[Groups] ([GroupId],[GroupName],[DepartmentId]) Values (2,'Groups 2',2);Insert Into [dbo].[Groups] ([GroupId],[GroupName],[DepartmentId]) Values (3,'Groups 3',3);Insert Into [dbo].[Users] ([UserId],[GroupId],[LastName],[FirstName],[EmailAddress]) Values (1,1,'Users 1','Users 1','Users 1');Insert Into [dbo].[Users] ([UserId],[GroupId],[LastName],[FirstName],[EmailAddress]) Values (2,2,'Users 2','Users 2','Users 2');Insert Into [dbo].[Users] ([UserId],[GroupId],[LastName],[FirstName],[EmailAddress]) Values (3,3,'Users 3','Users 3','Users 3');Insert Into [dbo].[Tickets] ([Id],[TicketNumber],[Title],[StatusCodeId],[PriorityId],[UserId]) Values ('00100000-0000-0000-0000-000000000000',1,'Tickets 1',1,1,1);Insert Into [dbo].[Tickets] ([Id],[TicketNumber],[Title],[StatusCodeId],[PriorityId],[UserId]) Values ('00200000-0000-0000-0000-000000000000',2,'Tickets 2',2,2,2);Insert Into [dbo].[Tickets] ([Id],[TicketNumber],[Title],[StatusCodeId],[PriorityId],[UserId]) Values ('00300000-0000-0000-0000-000000000000',3,'Tickets 3',3,3,3);",
                query);

            var sqlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                _config.Dataset.First().name + "-insert.sql");
            if (File.Exists(sqlFile))
            {
                File.Delete(sqlFile);
            }
            using (
                var stream =
                    new StreamWriter(sqlFile))
            {
                stream.Write(query.Replace(";", $";{Environment.NewLine}"));
            }
        }

        #endregion sample data builder

        [TestCleanup]
        public void Remove_Xml_Configuration()
        {
            _config = null;
            Helpers.RemoveXmlConfiguration();
        }
    }
}
