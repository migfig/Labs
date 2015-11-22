using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RelatedRecords.Data.ViewModels;
using System.IO;

namespace RelatedRecords.Data.Tests
{
    [TestClass]
    public class DataTests
    {
        [TestInitialize]
        public void Initialize()
        {
            MainViewModel.ViewModel.LoadConfiguration();
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        [TestMethod, Ignore]
        public void Configuration_Is_Valid_Test()
        {
            Assert.IsNotNull(MainViewModel.ViewModel.SelectedConfiguration);
            Assert.IsTrue(MainViewModel.ViewModel.SelectedConfiguration.Dataset.Any());
            Assert.IsNotNull(MainViewModel.ViewModel.SelectedConfiguration);
            Assert.IsNotNull(MainViewModel.ViewModel.SelectedDataset);
            Assert.IsNotNull(MainViewModel.ViewModel.SelectedDatasource);
        }

        [TestMethod, Ignore]
        public void Create_DataSets_Test()
        {
            Assert.IsNotNull(MainViewModel.ViewModel.SelectedConfiguration);
            Assert.IsTrue(MainViewModel.ViewModel.SelectedConfiguration.Dataset.Any());
            foreach(var ds in MainViewModel.ViewModel.SelectedConfiguration.Dataset)
            {
                var dataset = ds.ToDataSet();
                Assert.IsNotNull(dataset);
                Assert.IsTrue(dataset.Tables.Count > 0);
                var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
                    dataset.DataSetName + ".dxml");
                if(File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                dataset.WriteXml(fileName, System.Data.XmlWriteMode.WriteSchema);
                Assert.IsTrue(File.Exists(fileName));
            }
        }

        [TestMethod, Ignore]
        public void DataProvider_Test()
        {
            var table = DataSourceProvider.Data.Source.Load("sample", "Select * from Tickets")
                .GetAwaiter()
                .GetResult();
            Assert.IsNotNull(table);
            Assert.IsTrue(table.Rows.Count > 0);

            table = DataSourceProvider.Data.Source.Load("sample-remote", "Select * from TicketsStatusCodes")
                .GetAwaiter()
                .GetResult();
            Assert.IsNotNull(table);
            Assert.IsTrue(table.Rows.Count > 0);
        }

        [TestMethod, Ignore]
        public void Back_Test()
        {
            Tables_Test();
            Table_Id_Test();
            MainViewModel.ViewModel.Command = "back";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
            Assert.IsNotNull(MainViewModel.ViewModel.CurrentTable);
            Assert.IsTrue(MainViewModel.ViewModel.CurrentTable.Root.ConfigTable.name == "Tickets");
        }
        [TestMethod, Ignore]
        public void Invalid_Back_Test()
        {
            MainViewModel.ViewModel.Command = "_back";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod, Ignore]
        public void Clone_As_Id_Test()
        {
            MainViewModel.ViewModel.Command = "clone as SomeOtherName";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
            Assert.IsTrue(MainViewModel.ViewModel.SelectedConfiguration
                .Dataset.Any(x => x.name == "SomeOtherName"));
        }
        [TestMethod, Ignore]
        public void Invalid_Clone_As_Id_Test()
        {
            MainViewModel.ViewModel.Command = "_clone as SomeOtherName";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod, Ignore]
        public void Clone_Catalog_Id_As_Id_Test()
        {
            MainViewModel.ViewModel.Command = "clone catalog sample as SomeOther12_sample";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
            Assert.IsTrue(MainViewModel.ViewModel.SelectedConfiguration
                .Dataset.Any(x => x.name == "SomeOther12_sample"));
        }
        [TestMethod, Ignore]
        public void Invalid_Clone_Catalog_Id_As_Id_Test()
        {
            MainViewModel.ViewModel.Command = "_clone catalog MyCatalog as SomeOther12_catName";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod, Ignore]
        public void Clone_Catalog_Id_Test()
        {
            MainViewModel.ViewModel.Command = "clone catalog sample";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
            Assert.IsTrue(MainViewModel.ViewModel.SelectedConfiguration
                .Dataset.Any(x => x.name == "sample2"));
        }
        [TestMethod, Ignore]
        public void Invalid_Clone_Catalog_Id_Test()
        {
            MainViewModel.ViewModel.Command = "_clone catalog SampleCat";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod, Ignore]
        public void Clone_Test()
        {
            MainViewModel.ViewModel.Command = "clone";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
            Assert.IsTrue(MainViewModel.ViewModel.SelectedConfiguration
                .Dataset.Any(x => x.name == "sample2"));
        }
        [TestMethod, Ignore]
        public void Invalid_Clone_Test()
        {
            MainViewModel.ViewModel.Command = "_clone";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Columns_Int_Test()
        {
            MainViewModel.ViewModel.Command = "columns 2";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Columns_Int_Test()
        {
            MainViewModel.ViewModel.Command = "_columns 2";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Columns_Test()
        {
            MainViewModel.ViewModel.Command = "columns";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Columns_Test()
        {
            MainViewModel.ViewModel.Command = "_columns";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Export_As_Html_Test()
        {
            MainViewModel.ViewModel.Command = "export as html";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Export_As_Html_Test()
        {
            MainViewModel.ViewModel.Command = "_export as html";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Export_As_Json_Test()
        {
            MainViewModel.ViewModel.Command = "export as json";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Export_As_Json_Test()
        {
            MainViewModel.ViewModel.Command = "_export as json";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Export_As_Sql_Test()
        {
            MainViewModel.ViewModel.Command = "export as sql";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Export_As_Sql_Test()
        {
            MainViewModel.ViewModel.Command = "_export as sql";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Export_Id_As_Html_Test()
        {
            MainViewModel.ViewModel.Command = "export Html_Table as html";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Export_Id_As_Html_Test()
        {
            MainViewModel.ViewModel.Command = "_export Html_Table as html";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Export_Id_As_Json_Test()
        {
            MainViewModel.ViewModel.Command = "export _Tablename as json";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Export_Id_As_Json_Test()
        {
            MainViewModel.ViewModel.Command = "_export _Tablename as json";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Export_Id_As_Sql_Test()
        {
            MainViewModel.ViewModel.Command = "export _Table_Name_12 as sql";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Export_Id_As_Sql_Test()
        {
            MainViewModel.ViewModel.Command = "_export _Table_Name_12 as sql";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Export_Id_As_Xml_Test()
        {
            MainViewModel.ViewModel.Command = "export ThisTable as xml";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Export_Id_As_Xml_Test()
        {
            MainViewModel.ViewModel.Command = "_export ThisTable as xml";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Export_As_Xml_Test()
        {
            MainViewModel.ViewModel.Command = "export as xml";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Export_As_Xml_Test()
        {
            MainViewModel.ViewModel.Command = "_export as xml";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Import_Catalog_Id_Svr_Id_User_Id_Pwd_Id_Test()
        {
            MainViewModel.ViewModel.Command = "import catalog SampleZ server localhostz user devz password pwdz";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Import_Catalog_Id_Svr_Id_User_Id_Pwd_Id_Test()
        {
            MainViewModel.ViewModel.Command = "_import catalog SampleZ server localhostz user devz password pwdz";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Import_Catalog_Id_User_Id_Pwd_Id_Test()
        {
            MainViewModel.ViewModel.Command = "import catalog SampleY user devy password pwdy";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Import_Catalog_Id_User_Id_Pwd_Id_Test()
        {
            MainViewModel.ViewModel.Command = "_import catalog SampleY user devy password pwdy";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Import_Catalog_Id_Test()
        {
            MainViewModel.ViewModel.Command = "import catalog SampleX";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Import_Catalog_Id_Test()
        {
            MainViewModel.ViewModel.Command = "_import catalog SampleX";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod, Ignore]
        public void Load_Catalog_Id_Test()
        {
            MainViewModel.ViewModel.Command = "load catalog sample_remote";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
            Assert.IsTrue(MainViewModel.ViewModel.SelectedConfiguration
                .Dataset.Any(x => x.name == "sample_remote"));
            Assert.IsNotNull(MainViewModel.ViewModel.CurrentTable);
            Assert.IsTrue(MainViewModel.ViewModel.CurrentTable.Root.ConfigTable.name == "Tickets");
        }
        [TestMethod, Ignore]
        public void Invalid_Load_Catalog_Id_Test()
        {
            MainViewModel.ViewModel.Command = "_load catalog CatalogName";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod, Ignore]
        public void Load_Test()
        {
            MainViewModel.ViewModel.Command = "load";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
            Assert.IsNotNull(MainViewModel.ViewModel.CurrentTable);
            Assert.IsTrue(MainViewModel.ViewModel.CurrentTable.Root.ConfigTable.name == "Tickets");
        }
        [TestMethod, Ignore]
        public void Invalid_Load_Test()
        {
            MainViewModel.ViewModel.Command = "_load";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Relate_Id_To_Id_On_Id_Eq_Id_Test()
        {
            MainViewModel.ViewModel.Command = "relate ThisTable to OtherTable12 on Column1 = column_2";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Relate_Id_To_Id_On_Id_Eq_Id_Test()
        {
            MainViewModel.ViewModel.Command = "_relate ThisTable to OtherTable12 on Column1 = column_2";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Relate_To_Id_On_Id_Eq_Id_Test()
        {
            MainViewModel.ViewModel.Command = "relate to OtherTable12 on Column1 = column_2";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Relate_To_Id_On_Id_Eq_Id_Test()
        {
            MainViewModel.ViewModel.Command = "_relate to OtherTable12 on Column1 = column_2";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Remove_Catalog_Id_Test()
        {
            MainViewModel.ViewModel.Command = "remove catalog CatalogName";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Remove_Catalog_Id_Test()
        {
            MainViewModel.ViewModel.Command = "_remove catalog CatalogName";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Remove_Test()
        {
            MainViewModel.ViewModel.Command = "remove";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Remove_Test()
        {
            MainViewModel.ViewModel.Command = "_remove";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Refresh_Test()
        {
            MainViewModel.ViewModel.Command = "refresh";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Refresh_Test()
        {
            MainViewModel.ViewModel.Command = "_refresh";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Refresh_Catalog_Id_Test()
        {
            MainViewModel.ViewModel.Command = "refresh catalog My_Catalog";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Refresh_Catalog_Id_Test()
        {
            MainViewModel.ViewModel.Command = "_refresh catalog My_Catalog";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Root_Test()
        {
            MainViewModel.ViewModel.Command = "root";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Root_Test()
        {
            MainViewModel.ViewModel.Command = "_root";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Table_Id_Default_Where_Id_Eq_StrLit_Test()
        {
            MainViewModel.ViewModel.Command = "table Test21 default where col1 = \"1.34\"";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Table_Id_Default_Where_Id_Eq_StrLit_Test()
        {
            MainViewModel.ViewModel.Command = "_table Test21 default where col1 = \"1.34\"";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Table_Id_Default_Test()
        {
            MainViewModel.ViewModel.Command = "table Test21 default";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Table_Id_Default_Test()
        {
            MainViewModel.ViewModel.Command = "_table Test21 default";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Table_Id_Where_Id_Between_Int_And_Int_Test()
        {
            MainViewModel.ViewModel.Command = "table Test21 where col1 between 1 and 10";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Table_Id_Where_Id_Between_Int_And_Int_Test()
        {
            MainViewModel.ViewModel.Command = "_table Test21 where col1 between 1 and 10";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Table_Id_Where_Id_Between_Dec_And_Dec_Test()
        {
            MainViewModel.ViewModel.Command = "table Test21 where col1 between 1.34 and 245.234";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Table_Id_Where_Id_Between_Dec_And_Dec_Test()
        {
            MainViewModel.ViewModel.Command = "_table Test21 where col1 between 1.34 and 245.234";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Table_Id_Where_Id_GtEq_Minus_Int_Test()
        {
            MainViewModel.ViewModel.Command = "table Test21 where col1 >= -1";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Table_Id_Where_Id_GtEq_Minus_Int_Test()
        {
            MainViewModel.ViewModel.Command = "_table Test21 where col1 >= -1";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Table_Id_Where_Id_GtEq_Minus_Dec_Test()
        {
            MainViewModel.ViewModel.Command = "table Test21 where col1 >= -121.340";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Table_Id_Where_Id_GtEq_Minus_Dec_Test()
        {
            MainViewModel.ViewModel.Command = "_table Test21 where col1 >= -121.340";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Table_Id_Where_Id_GtEq_Int_Test()
        {
            MainViewModel.ViewModel.Command = "table Test21 where col1 >= 1";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Table_Id_Where_Id_GtEq_Int_Test()
        {
            MainViewModel.ViewModel.Command = "_table Test21 where col1 >= 1";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Table_Id_Where_Id_GtEq_Dec_Test()
        {
            MainViewModel.ViewModel.Command = "table Test21 where col1 >= 121.340";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Table_Id_Where_Id_GtEq_Dec_Test()
        {
            MainViewModel.ViewModel.Command = "_table Test21 where col1 >= 121.340";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Table_Id_Where_Id_Gt_Minus_Int_Test()
        {
            MainViewModel.ViewModel.Command = "table Test21 where col1 > -1";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Table_Id_Where_Id_Gt_Minus_Int_Test()
        {
            MainViewModel.ViewModel.Command = "_table Test21 where col1 > -1";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Table_Id_Where_Id_Gt_Minus_Dec_Test()
        {
            MainViewModel.ViewModel.Command = "table Test21 where col1 > -121.340";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Table_Id_Where_Id_Gt_Minus_Dec_Test()
        {
            MainViewModel.ViewModel.Command = "_table Test21 where col1 > -121.340";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Table_Id_Where_Id_Gt_Int_Test()
        {
            MainViewModel.ViewModel.Command = "table Test21 where col1 > 1";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Table_Id_Where_Id_Gt_Int_Test()
        {
            MainViewModel.ViewModel.Command = "_table Test21 where col1 > 1";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Table_Id_Where_Id_Gt_Dec_Test()
        {
            MainViewModel.ViewModel.Command = "table Test21 where col1 > 121.340";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Table_Id_Where_Id_Gt_Dec_Test()
        {
            MainViewModel.ViewModel.Command = "_table Test21 where col1 > 121.340";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Table_Id_Where_Id_LtEq_Minus_Int_Test()
        {
            MainViewModel.ViewModel.Command = "table Test21 where col1 <= -1";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Table_Id_Where_Id_LtEq_Minus_Int_Test()
        {
            MainViewModel.ViewModel.Command = "_table Test21 where col1 <= -1";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Table_Id_Where_Id_LtEq_Minus_Dec_Test()
        {
            MainViewModel.ViewModel.Command = "table Test21 where col1 <= -121.340";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Table_Id_Where_Id_LtEq_Minus_Dec_Test()
        {
            MainViewModel.ViewModel.Command = "_table Test21 where col1 <= -121.340";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Table_Id_Where_Id_LtEq_Int_Test()
        {
            MainViewModel.ViewModel.Command = "table Test21 where col1 <= 1";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Table_Id_Where_Id_LtEq_Int_Test()
        {
            MainViewModel.ViewModel.Command = "_table Test21 where col1 <= 1";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Table_Id_Where_Id_LtEq_Dec_Test()
        {
            MainViewModel.ViewModel.Command = "table Test21 where col1 <= 121.340";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Table_Id_Where_Id_LtEq_Dec_Test()
        {
            MainViewModel.ViewModel.Command = "_table Test21 where col1 <= 121.340";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Table_Id_Where_Id_LtGt_Int_Test()
        {
            MainViewModel.ViewModel.Command = "table Test21 where col1 <> 1";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Table_Id_Where_Id_LtGt_Int_Test()
        {
            MainViewModel.ViewModel.Command = "_table Test21 where col1 <> 1";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Table_Id_Where_Id_LtGt_Dec_Test()
        {
            MainViewModel.ViewModel.Command = "table Test21 where col1 <> 121.340";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Table_Id_Where_Id_LtGt_Dec_Test()
        {
            MainViewModel.ViewModel.Command = "_table Test21 where col1 <> 121.340";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Table_Id_Where_Id_LtGt_Minus_Int_Test()
        {
            MainViewModel.ViewModel.Command = "table Test21 where col1 <> -1";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Table_Id_Where_Id_LtGt_Minus_Int_Test()
        {
            MainViewModel.ViewModel.Command = "_table Test21 where col1 <> -1";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Table_Id_Where_Id_LtGt_Minus_Dec_Test()
        {
            MainViewModel.ViewModel.Command = "table Test21 where col1 <> -121.340";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Table_Id_Where_Id_LtGt_Minus_Dec_Test()
        {
            MainViewModel.ViewModel.Command = "_table Test21 where col1 <> -121.340";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Table_Id_Where_Id_Eq_Minus_Int_Test()
        {
            MainViewModel.ViewModel.Command = "table Test21 where col1 = -1";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Table_Id_Where_Id_Eq_Minus_Int_Test()
        {
            MainViewModel.ViewModel.Command = "_table Test21 where col1 = -1";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Table_Id_Where_Id_Eq_Minus_Dec_Test()
        {
            MainViewModel.ViewModel.Command = "table Test21 where col1 = -121.340";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Table_Id_Where_Id_Eq_Minus_Dec_Test()
        {
            MainViewModel.ViewModel.Command = "_table Test21 where col1 = -121.340";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Table_Id_Where_Id_Eq_Int_Test()
        {
            MainViewModel.ViewModel.Command = "table Test21 where col1 = 1";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Table_Id_Where_Id_Eq_Int_Test()
        {
            MainViewModel.ViewModel.Command = "_table Test21 where col1 = 1";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Table_Id_Where_Id_Eq_Dec_Test()
        {
            MainViewModel.ViewModel.Command = "table Test21 where col1 = 121.340";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Table_Id_Where_Id_Eq_Dec_Test()
        {
            MainViewModel.ViewModel.Command = "_table Test21 where col1 = 121.340";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Table_Id_Where_Id_Is_Not_Null_Test()
        {
            MainViewModel.ViewModel.Command = "table Test21 where col_some12 is not null";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Table_Id_Where_Id_Is_Not_Null_Test()
        {
            MainViewModel.ViewModel.Command = "_table Test21 where col_some12 is not null";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Table_Id_Where_Id_Is_Null_Test()
        {
            MainViewModel.ViewModel.Command = "table Test21 where col12xo is null";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Table_Id_Where_Id_Is_Null_Test()
        {
            MainViewModel.ViewModel.Command = "_table Test21 where col12xo is null";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod, Ignore]
        public void Table_Id_Test()
        {
            MainViewModel.ViewModel.Command = "table Tickets";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
            Assert.IsNotNull(MainViewModel.ViewModel.CurrentTable);
            Assert.IsTrue(MainViewModel.ViewModel.CurrentTable.Root.ConfigTable.name == "Tickets");
        }
        [TestMethod, Ignore]
        public void Invalid_Table_Id_Test()
        {
            MainViewModel.ViewModel.Command = "_table Test21";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);

            MainViewModel.ViewModel.CurrentTable = null;
            MainViewModel.ViewModel.Command = "table Test21";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
            Assert.IsNull(MainViewModel.ViewModel.CurrentTable);
        }

        [TestMethod, Ignore]
        public void Tables_Int_Test()
        {
            MainViewModel.ViewModel.Command = "tables 2";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
            Assert.IsNotNull(MainViewModel.ViewModel.CurrentTable);
            Assert.IsTrue(MainViewModel.ViewModel.CurrentTable.Root.Table.Rows.Count == 2);
        }
        [TestMethod, Ignore]
        public void Invalid_Tables_Int_Test()
        {
            MainViewModel.ViewModel.Command = "_tables 10";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
            Assert.IsNull(MainViewModel.ViewModel.CurrentTable);
        }

        [TestMethod, Ignore]
        public void Tables_Test()
        {
            MainViewModel.ViewModel.Command = "tables";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
            Assert.IsNotNull(MainViewModel.ViewModel.CurrentTable);
            Assert.IsTrue(MainViewModel.ViewModel.CurrentTable.Root.ConfigTable.name == "sample");
        }
        [TestMethod, Ignore]
        public void Invalid_Tables_Test()
        {
            MainViewModel.ViewModel.Command = "_tables";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod, Ignore]
        public void Top_Int_Test()
        {
            MainViewModel.ViewModel.SelectedDataset.name = "sample";
            MainViewModel.ViewModel.Command = "top 10";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
            Assert.IsNotNull(MainViewModel.ViewModel.CurrentTable);
            Assert.IsTrue(MainViewModel.ViewModel.CurrentTable.Root.Table.Rows.Count == 10);
        }
        [TestMethod, Ignore]
        public void Invalid_Top_Int_Test()
        {
            MainViewModel.ViewModel.Command = "_top 100";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
            Assert.IsNull(MainViewModel.ViewModel.CurrentTable);
        }

        [TestMethod]
        public void Unrelate_Id_To_Id_Test()
        {
            MainViewModel.ViewModel.Command = "unrelate This_Table12 to OtherTable12";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Unrelate_Id_To_Id_Test()
        {
            MainViewModel.ViewModel.Command = "_unrelate This_Table12 to OtherTable12";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Unrelate_To_Id_Test()
        {
            MainViewModel.ViewModel.Command = "unrelate to OtherTable12";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Unrelate_To_Id_Test()
        {
            MainViewModel.ViewModel.Command = "_unrelate to OtherTable12";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Child_Test()
        {
            MainViewModel.ViewModel.Command = "child";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Child_Test()
        {
            MainViewModel.ViewModel.Command = "_child";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Child_Int_Test()
        {
            MainViewModel.ViewModel.Command = "child 2";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Child_Int_Test()
        {
            MainViewModel.ViewModel.Command = "_child 2";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Child_Id_Test()
        {
            MainViewModel.ViewModel.Command = "child MyTable";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Child_Id_Test()
        {
            MainViewModel.ViewModel.Command = "_child MyTable";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

        [TestMethod]
        public void Help_Test()
        {
            MainViewModel.ViewModel.Command = "help";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsTrue(MainViewModel.ViewModel.IsValidCommand);
        }
        [TestMethod]
        public void Invalid_Help_Test()
        {
            MainViewModel.ViewModel.Command = "_help";
            MainViewModel.ViewModel.ExecuteCommand();
            Assert.IsFalse(MainViewModel.ViewModel.IsValidCommand);
        }

    }
}
