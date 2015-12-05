#region usings
using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Configuration;
using static Common.Extensions;
using www.serviciipeweb.ro.iafblog.ExportDLL;
using Common;
using System.Data.SqlClient;
#endregion usings

namespace RelatedRecords.Data.ViewModels
{
    public partial class MainViewModel
    {
        private void DoBack()
        {
            if (_tableNavigation.Any())
            {
                CurrentTable = _tableNavigation.Pop();
                _goBack.RaiseCanExecuteChanged();
            }
        }

        private void DoCloneAsId(string catalog)
        {
            DoCloneCatalogIdAsId(SelectedDataset.name, catalog);
        }

        private void DoCloneCatalogIdAsId(string srcCatalog, string tgtCatalog)
        {
            var srcDataset = findDataset(srcCatalog);
            if (null == srcDataset) return;

            var tgtDataset = findDataset(tgtCatalog);
            if (null != tgtDataset) return;

            tgtDataset = Extensions.Clone<CDataset>(srcDataset);
            var tgtDatasource = Extensions.Clone<CDatasource>(SelectedConfiguration
                .Datasource
                .First(x => x.name == srcDataset.dataSourceName));
            tgtDataset.name = tgtCatalog;
            tgtDataset.dataSourceName = tgtCatalog;
            tgtDatasource.name = tgtCatalog;
            SelectedConfiguration.Dataset.Add(tgtDataset);
            SelectedConfiguration.Datasource.Add(tgtDatasource);
            SaveConfiguration();
            LoadConfiguration();
        }

        private void DoCloneCatalogId(string catalog)
        {
            var srcDataset = findDataset(catalog);
            if (null == srcDataset) return;

            var tgtCatalog = catalog.RemoveLastNumbers();
            var count = SelectedConfiguration
                .Dataset
                .Where(x => x.name.ToLower().Contains(tgtCatalog.ToLower()))
                .Count();
            DoCloneCatalogIdAsId(catalog, tgtCatalog + count.ToString());
        }

        private void DoClone()
        {
            DoCloneCatalogId(SelectedDataset.name);
        }

        private void DoColumnsInt(string topN)
        {
            if (null != CurrentTable)
            {
                var table = findTable(CurrentTable.Root.ConfigTable.name);

                if (table != null)
                {
                    PushCurrentTable(CurrentTable
                            .Root
                            .ConfigTable
                            .ToColumnsDataTable(int.Parse(topN))
                            .ToDatatableEx(CurrentTable.Root.ConfigTable)
                    );
                }
                else
                {
                    CurrentTable = _tableNavigation.Peek();
                    table = findTable(CurrentTable.Root.ConfigTable.name);

                    if (table != null)
                    {
                        PushCurrentTable(CurrentTable
                                .Root
                                .ConfigTable
                                .ToColumnsDataTable(int.Parse(topN))
                                .ToDatatableEx(CurrentTable.Root.ConfigTable)
                        );
                    }
                }
            }
        }

        private void DoColumns()
        {
            DoColumnsInt("100");
        }

        #region export features

        private string ExportPath
        {
            get
            {
                string basePath = Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "export");

                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }

                return basePath;
            }
        }

        private void DoExportAsHtml(bool includeChildren = true)
        {
            if (null != CurrentTable)
            {
                TraceLog.Information("Exporting tables to Html");

                string templatePath = Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    @"templates\Collection\html_tables.st");
                string basePath = ExportPath;

                string namePart = DateTime.Now.ToString("yyyy-MMM-dd.hh-mm-ss");
                StringBuilder html = new StringBuilder();
                html.Append(File.ReadAllText(templatePath).Replace("$FirstItem.Name;format=\"toUpper\"$", namePart)
                    .Replace("$DateCreated;format=\"yyyy MMM dd\"$", DateTime.Now.ToString("MMM-dd-yyyy hh:mm:ss")));

                StringBuilder tables = new StringBuilder();
                int level = 0;
                fillHtml(CurrentTable, ref tables, ref level, includeChildren);

                string htmlFile = Path.Combine(basePath, string.Format("{0}.html", namePart));
                File.WriteAllText(htmlFile, html.ToString().Replace("<$Tables>", tables.ToString()));

                runProcess(ConfigurationManager.AppSettings["fileExplorer"], htmlFile, -1);
            }
        }

        private void fillHtml(DatatableEx t, ref StringBuilder tables, ref int level, bool includeChildren)
        {
            string[] levels = new string[] {
                "first", "second", "third", "fourth", "fifth", "sixth", "seventh", "eighth", "nineth", "tenth"
            };
            tables.Append(t.Root.Table.ExportAsHtmlFragment()
                .Replace("className", string.Format("{0}LevelTable", levels[level]))
                .Replace("tableName", t.Root.Table.TableName.ToUpper())
                .Replace("columnCount", t.Root.Table.Columns.Count.ToString()));

            if (includeChildren)
            {
                ++level;
                foreach (var child in t.Children)
                    fillHtml(child, ref tables, ref level, includeChildren);
                --level;
            }
        }

        private void DoExportAsJson(bool includeChildren = true)
        {
        }

        private void DoExportAsSql(bool includeChildren = true)
        {
            TraceLog.Information("Exporting tables to SQL Insert");

            string sqlFile = Path.Combine(ExportPath, DateTime.Now.ToString("yyyy-MMM-dd.hh-mm-ss") + ".sql");
            using (var stream = new StreamWriter(sqlFile))
            {
                stream.Write(new StringBuilder().SqlInsert(CurrentTable, includeChildren).ToString());
            }

            runProcess(ConfigurationManager.AppSettings["fileExplorer"], sqlFile, -1);
        }

        private void DoExportIdAsHtml(string tableName, bool includeChildren = true)
        {
            var table = findTable(tableName);
            if (null == table) ThrowError("Invalid table {0}", tableName);

            DoTableId(tableName);
            DoExportAsHtml(includeChildren);
        }

        private void DoExportIdAsJson(string tableName, bool includeChildren = true)
        {
        }

        private void DoExportIdAsSql(string tableName, bool includeChildren = true)
        {
            var table = findTable(tableName);
            if (null == table) ThrowError("Invalid table {0}", tableName);

            DoTableId(tableName);
            DoExportAsSql(includeChildren);
        }

        private void DoExportIdAsXml(string tableName, bool includeChildren = true)
        {
        }

        private void DoExportAsXml(bool includeChildren = true)
        {
        }

        #endregion export features

        private async void DoImportCatalogIdSvrIdUserIdPwdId(string catalog, string server, string user = "", string password = "")
        {
            var connStr = string.Format("data source={0};initial catalog={1};{2}MultipleActiveResultSets=True;asynchronous processing=True;Enlist=false;",
                server, catalog,
                !string.IsNullOrWhiteSpace(user)
                    ? string.Format("User Id={1};Password={1};", user, password)
                    : "Integrated Security=True;");
            var newCfg = XmlHelper<CConfiguration>.Load(
                await Helpers.GetConfigurationFromConnectionString(connStr)
            );

            if (null == newCfg || !newCfg.Dataset.Any()) return;

            newCfg.Dataset.FirstOrDefault().defaultTable = newCfg.Dataset.First().Table.First().name;
            var sourceName = newCfg.Datasource.First().name.RemoveLastNumbers();
            var namesFound = SelectedConfiguration.Datasource.Where(s => s.name.Contains(sourceName)).Count();
            if (namesFound > 0)
            {
                sourceName += namesFound.ToString();
                newCfg.Datasource.First().name = sourceName;
                newCfg.Dataset.First().dataSourceName = sourceName;
                newCfg.Dataset.First().name = sourceName;
            }
            SelectedConfiguration.Datasource.Add(newCfg.Datasource.First());
            SelectedConfiguration.Dataset.Add(newCfg.Dataset.First());

            SaveConfiguration();
            LoadConfiguration();
        }

        private void DoImportCatalogIdUserIdPwdId(string catalog, string user, string password)
        {
            DoImportCatalogIdSvrIdUserIdPwdId(catalog,
                ConfigurationManager.AppSettings["localdb"].ToString(),
                user,
                password);
        }

        private void DoImportCatalogId(string catalog)
        {
            DoImportCatalogIdSvrIdUserIdPwdId(catalog,
                ConfigurationManager.AppSettings["localdb"].ToString());
        }

        private void DoLoadCatalogId(string catalog)
        {
            var ds = findDataset(catalog);
            if (ds == null) ThrowError("Invalid catalog {0}", catalog);

            SelectedDataset = ds;
            DoLoad();
        }

        private async void DoLoad()
        {
            PushCurrentTable(await findTable(SelectedDataset.defaultTable)
                .Query("".ToArray(), "".ToArray(), true));
        }

        private void DoRelateIdToIdOnIdEqId(string srcTblName, string tgtTblName, string srcColName, string tgtColName)
        {
            var srcTable = SelectedDataset
                .Table
                .FirstOrDefault(x =>
                    x.name.ToLower() == srcTblName.ToLower());

            if (null == srcTable) return;

            var tgtTable = SelectedDataset
            .Table
            .FirstOrDefault(x =>
                x.name.ToLower() == tgtTblName.ToLower());

            if (null == tgtTable) return;

            var srcCol = srcTable
                .Column
                .FirstOrDefault(x => x.name.ToLower() == srcColName.ToLower());

            if (null == srcCol) return;

            var tgtCol = tgtTable
                .Column
                .FirstOrDefault(x => x.name.ToLower() == tgtColName.ToLower());

            if (null == tgtCol || srcCol.DbType != tgtCol.DbType) return;

            var relationship = new CRelationship
            {
                name = string.Format("{0}->{1}", srcTable.name, tgtTable.name),
                fromTable = srcTable.name,
                toTable = tgtTable.name,
                fromColumn = srcCol.name,
                toColumn = tgtCol.name
            };

            if (SelectedDataset.Relationship.Any(x => x.name == relationship.name)) return;

            var currentDatasetName = SelectedDataset.name;
            var currentTableName = null != CurrentTable ? CurrentTable.Root.ConfigTable.name : string.Empty;

            SelectedDataset.Relationship.Add(relationship);
            SaveConfiguration();
            LoadConfiguration();
        }

        private void DoRelateToIdOnIdEqId(string tgtTblName, string srcColName, string tgtColName)
        {
            var currentTableName = null != CurrentTable
                ? CurrentTable.Root.ConfigTable.name
                : SelectedDataset.defaultTable;

            DoRelateIdToIdOnIdEqId(currentTableName, tgtTblName, srcColName, tgtColName);
        }

        private void DoRemoveCatalogId(string datasetName)
        {
            if (SelectedConfiguration.Dataset.Count > 1)
            {
                var currentDatasetName = SelectedDataset.name;
                var dataSet = SelectedConfiguration
                    .Dataset
                    .FirstOrDefault(x =>
                    x.name.ToLower() == datasetName.ToLower());
                if (null == dataSet) return;

                SelectedConfiguration.Dataset.Remove(dataSet);
                if (SelectedConfiguration.defaultDataset == currentDatasetName)
                {
                    SelectedConfiguration.defaultDataset = SelectedConfiguration.Dataset.First().name;
                    SelectedConfiguration.defaultDatasource = SelectedConfiguration.Dataset.First().dataSourceName;
                }

                SaveConfiguration();
                LoadConfiguration();
            }
        }

        private void DoRemove()
        {
            DoRemoveCatalogId(SelectedDataset.name);
        }

        private void DoRefresh()
        {
            DoRefreshCatalogId(SelectedDataset.name);
        }

        private async void DoRefreshCatalogId(string catalog)
        {
            var dataset = findDataset(catalog);
            if (null == dataset) return;

            var config = XmlHelper<CConfiguration>.Load(
                await Helpers.GetConfigurationFromConnectionString(
                    SelectedConfiguration.Datasource.First(ds => ds.name == dataset.dataSourceName)
                        .ConnectionString)
                );

            if (null == config || !config.Dataset.Any()) return;

            dataset.Table.Clear();
            config.Dataset.First().Table.ToList().ForEach(t =>
                dataset.Table.Add(t)
            );

            SaveConfiguration();
            LoadConfiguration();
        }

        private void DoRoot()
        {
            _tableNavigation.Clear();
            DoLoad();
        }

        private void DoTableIdDefaultWhereIdOperatorStrLit(string tableName, string column, string value, string @operator = "=")
        {
            var table = findTable(tableName);

            if (null == table) ThrowError("Invalid Table {0}", tableName);

            if (SelectedDataset.defaultTable != table.name)
            {
                ClearState();

                SelectedDataset.defaultTable = table.name;
                SaveConfiguration();
            }
            DoTableIdWhereIdOperatorValue(tableName, column, value.Replace("\"", string.Empty), typeof(string), @operator);
        }

        private void DoTableIdDefault(string tableName)
        {
            var table = findTable(tableName);

            if (null == table) return;

            if (SelectedDataset.defaultTable != table.name)
            {
                ClearState();

                SelectedDataset.defaultTable = table.name;
                SaveConfiguration();
            }
            DoTableId(tableName);
        }

        private async void DoTableIdWhereIdBetweenValueAndValue(string tableName, string columnName,
            string minValue, string maxValue, Type type = null)
        {
            var table = findTable(tableName);
            if (null == table || !table.Column.Any(x => x.name.ToLower() == columnName.ToLower()))
                ThrowError("Invalid table {0}, column: {1}", tableName, columnName);

            PushCurrentTable(await table.Query(">=,<=".ToArray(),
                        "And".ToArray(),
                        false,
                        new SqlParameter(columnName, ParseValue(minValue, type)),
                        new SqlParameter(columnName, ParseValue(maxValue, type))));
        }

        private async void DoTableIdWhereIdOperatorValue(string tableName, string columnName,
            string value, Type type = null, string compOperator = "")
        {
            var table = findTable(tableName);
            if (null == table || !table.Column.Any(x => x.name.ToLower() == columnName.ToLower()))
                ThrowError("Invalid table {0}, column: {1}", tableName, columnName);

            PushCurrentTable(await table.Query(compOperator.ToArray(),
                        "".ToArray(),
                        false,
                        new SqlParameter(columnName, ParseValue(value, type)))
            );
        }

        private async void DoTableId(string tableName, int topN = 1000)
        {
            var table = findTable(tableName);
            if (null == table) ThrowError("Invalid table {0}", tableName);

            Extensions.MaxRowCount = topN;

            PushCurrentTable(await table.Query("".ToArray(), "".ToArray(), true));
        }

        private void DoTopInt(string topN)
        {
            DoTableId(SelectedDataset.defaultTable, int.Parse(topN));
        }

        private void DoTablesInt(string topN)
        {
            DoTables(int.Parse(topN));
        }

        private void DoTables(int topN = 1000)
        {
            PushCurrentTable(SelectedDataset
                .ToDataTable(topN)
                .ToDatatableEx(SelectedDataset.ToTable())
            );
        }

        private void DoUnrelateIdToId(string srcTblName, string tgtTblName)
        {
            var srcTable = SelectedDataset
                .Table
                .FirstOrDefault(x =>
                    x.name.ToLower() == srcTblName.ToLower());

            if (null == srcTable) return;

            var tgtTable = SelectedDataset
            .Table
            .FirstOrDefault(x =>
                x.name.ToLower() == tgtTblName.ToLower());

            if (null == tgtTable || srcTable.name == tgtTable.name) return;

            var relationship = SelectedDataset
                .Relationship.FirstOrDefault(x =>
                    x.name == CRelationship.GetName(srcTable.name, tgtTable.name));
            if (null == relationship) return;

            SelectedDataset.Relationship.Remove(relationship);
            SaveConfiguration();
            LoadConfiguration();
        }

        private void DoUnrelateToId(string tgtTblName)
        {
            var currentTableName = null != CurrentTable
                ? CurrentTable.Root.ConfigTable.name
                : SelectedDataset.defaultTable;
            DoUnrelateIdToId(currentTableName, tgtTblName);
        }

        private void DoChild()
        {
            DoChildInt("0");
        }

        private void DoChildInt(string index)
        {
            var table = findDataTableByIndex(int.Parse(index));
            if (null == table) ThrowError("Invalid table at Index {0}", index);

            PushCurrentTable(table);
        }

        private void DoChildId(string tableName)
        {
            var table = findDataTable(tableName);
            if (null == table) ThrowError("Invalid table {0}", tableName);

            PushCurrentTable(table);
        }

        private void DoHelp()
        {
            var commands = GetCommands();
            var descriptions = GetDescriptions();

            var table = new string[] { commands, descriptions }
                .ToDatatableEx();

            PushCurrentTable(table);
        }
       
        private void DoQueryIdRowInt(string columnName, int row = -1)
        {

        }

        private void DoTransformIdTemplateId(string sqlObject = "", string template = "")
        {

        }

        private void DoRunIdWithParams(string queryName, params QueryParam[] p)
        {

        }
    }
}
