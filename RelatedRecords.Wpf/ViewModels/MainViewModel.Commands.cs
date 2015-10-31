using Common;
using Common.Commands;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using www.serviciipeweb.ro.iafblog.ExportDLL;

namespace RelatedRecords.Wpf.ViewModels
{
    public partial class MainViewModel
    {
        #region load schema commands

        RelayCommand _loadDatasourceSchemaCommand;
        public ICommand LoadDatasourceSchemaCommand
        {
            get
            {
                if (_loadDatasourceSchemaCommand == null)
                {
                    _loadDatasourceSchemaCommand = new RelayCommand(
                        async x =>
                        {
                            IsBusy = true;
                            SelectedNewConfiguration =
                                XmlHelper<CConfiguration>.Load(
                                    await Helpers.GetConfigurationFromConnectionString(SelectedConnectionString)
                                );
                            IsBusy = false;
                            SelectedParentTable = SelectedNewConfiguration
                                .Dataset.FirstOrDefault()
                                .Table.FirstOrDefault();
                        },
                        x => !string.IsNullOrEmpty(SelectedConnectionString));
                }
                return _loadDatasourceSchemaCommand;
            }
        }

        RelayCommand _saveDatasourceSchemaCommand;
        public ICommand SaveDatasourceSchemaCommand
        {
            get
            {
                if (_saveDatasourceSchemaCommand == null)
                {
                    _saveDatasourceSchemaCommand = new RelayCommand(
                        x =>
                        {                            
                            SelectedNewConfiguration.Dataset.FirstOrDefault().defaultTable = SelectedParentTable.name;
                            SelectedConfiguration.Datasource
                                .Add(SelectedNewConfiguration.Datasource.First());
                            SelectedConfiguration.Dataset
                                .Add(SelectedNewConfiguration.Dataset.First());

                            saveConfiguration();
                            SelectedNewConfiguration = null;
                            SelectedConnectionString = string.Empty;
                            LoadDatasourceSchemaCommand.AsRelay().RaiseCanExecuteChanged();
                            SetTableRelationshipsCommand.AsRelay().RaiseCanExecuteChanged();
                        },
                        x => null != SelectedNewConfiguration);
                }
                return _saveDatasourceSchemaCommand;
            }
        }

        #endregion

        #region drill down command
        RelayCommand _drillDown;
        /// <summary>
        /// drill down navigation
        /// </summary>
        public ICommand DrillDownCommand
        {
            get
            {
                if (_drillDown == null)
                {
                    _drillDown = new RelayCommand(
                        x =>
                        {
                            MainViewModel.ViewModel.TableNavigation.Push(MainViewModel.ViewModel.SelectedDataTable);
                            MainViewModel.ViewModel.SelectedDataTable = x as DatatableEx;
                        },
                        x => null != SelectedDataTable
                            && SelectedDataTable.Root.ConfigTable.Children.Count > 0);
                }
                return _drillDown;
            }
        }
        #endregion drill down command

        #region refresh command
        RelayCommand _refresh;
        /// <summary>
        /// Refresh data
        /// </summary>
        public ICommand RefreshCommand
        {
            get
            {
                if (_refresh == null)
                {
                    _refresh = new RelayCommand(
                        x =>
                        {
                            _refreshingFromMainCommand = (x == null);
                            switch (SelectedViewType)
                            {
                                case eViewType.Datasets:
                                    var tname = SelectedDataTable.Root.ConfigTable.name;
                                    _dataTablesList.Remove(_dataTablesList.First(t => t.Root.ConfigTable.name == tname));

                                    IsBusy = true;

                                    var action = new Action(async () =>
                                    {
                                        var table = await SelectedDataset.Table.First(t => t.name == tname)
                                            .Query("".ToArray(""),
                                                "".ToArray(""),
                                                true);
                                        _dataTablesList.Add(table);

                                        IsBusy = false;
                                        SelectedDataTable = table;
                                    });
                                    action.Invoke();                                    
                                    break;
                                case eViewType.Tables:
                                    var tableName = _selectedTable.name;
                                    SelectedTable = null;
                                    SelectedTable = SelectedDataset.Table.First(t => t.name == tableName);
                                    break;
                                case eViewType.Queries:
                                    var queryName = _selectedQuery.name;
                                    SelectedQuery = null;
                                    _isRefreshing = true;
                                    SelectedQuery = SelectedDataset.Query.First(q => q.name == queryName);
                                    break;
                            }
                        },
                        x => true);
                }
                return _refresh;
            }
        }
        #endregion

        #region export support

        #region Export to Word

        RelayCommand _export2WordCommand;

        /// <summary>
        /// Export to ms word
        /// </summary>
        public ICommand Export2WordCommand
        {
            get
            {
                if (_export2WordCommand == null)
                {
                    _export2WordCommand = new RelayCommand(
                        x => exportTables(WorkingTable, ExportToFormat.Word2007),
                        x => null != WorkingTable && WorkingTable.Root.Table.Rows.Count > 0);
                }
                return _export2WordCommand;
            }
        }

        #endregion //Export to Word

        #region Export to Excel

        RelayCommand _export2ExcelCommand;

        /// <summary>
        /// Export to excel
        /// </summary>
        public ICommand Export2ExcelCommand
        {
            get
            {
                if (_export2ExcelCommand == null)
                {
                    _export2ExcelCommand = new RelayCommand(
                        x => exportTables(WorkingTable, ExportToFormat.Excel2007),
                        x => null != WorkingTable && WorkingTable.Root.Table.Rows.Count > 0);
                }
                return _export2ExcelCommand;
            }
        }

        #endregion //Export to Excel

        #region Export to PDF

        RelayCommand _export2PDFCommand;

        /// <summary>
        /// export to pdf
        /// </summary>
        public ICommand Export2PDFCommand
        {
            get
            {
                if (_export2PDFCommand == null)
                {
                    _export2PDFCommand = new RelayCommand(
                        x => exportTables(WorkingTable, ExportToFormat.PDFtextSharpXML),
                        x => null != WorkingTable && WorkingTable.Root.Table.Rows.Count > 0);
                }
                return _export2PDFCommand;
            }
        }

        #endregion //Export to PDF

        #region Export to Html

        RelayCommand _export2HtmlCommand;

        /// <summary>
        /// export to html
        /// </summary>
        public ICommand Export2HtmlCommand
        {
            get
            {
                if (_export2HtmlCommand == null)
                {
                    _export2HtmlCommand = new RelayCommand(
                        x =>
                        {
                            exportTablesToHtml();
                        },
                        x => null != WorkingTable && WorkingTable.Root.Table.Rows.Count > 0);
                }
                return _export2HtmlCommand;
            }
        }

        #endregion //Export to Html

        #region Export to sql insert

        RelayCommand _export2SqlInsertCommand;

        /// <summary>
        /// export to sql script
        /// </summary>
        public ICommand Export2SqlInsertCommand
        {
            get
            {
                if (_export2SqlInsertCommand == null)
                {
                    _export2SqlInsertCommand = new RelayCommand(
                        x => exportTablesToSqlInsert(),
                        x => WorkingTable != null && WorkingTable.Root.Table.Rows.Count > 0);
                }
                return _export2SqlInsertCommand;
            }
        }

        #endregion //Export to sql insert

        #endregion //export support

        #region views command

        RelayCommand _viewDatasetsCommand;
        /// <summary>
        /// select the datasets view
        /// </summary>
        public ICommand ViewDatasetsCommand
        {
            get
            {
                if (_viewDatasetsCommand == null)
                {
                    _viewDatasetsCommand = new RelayCommand(
                        x => SelectedViewType = eViewType.Datasets,
                        x => true);
                }
                return _viewDatasetsCommand;
            }
        }

        RelayCommand _viewTablesCommand;
        /// <summary>
        /// select the tables view
        /// </summary>
        public ICommand ViewTablesCommand
        {
            get
            {
                if (_viewTablesCommand == null)
                {
                    _viewTablesCommand = new RelayCommand(
                        x => SelectedViewType = eViewType.Tables,
                        x => SelectedViewType != eViewType.Tables);
                }
                return _viewTablesCommand;
            }
        }

        RelayCommand _viewQueriesCommand;
        /// <summary>
        /// select the queries view
        /// </summary>
        public ICommand ViewQueriesCommand
        {
            get
            {
                if (_viewQueriesCommand == null)
                {
                    _viewQueriesCommand = new RelayCommand(
                        x => SelectedViewType = eViewType.Queries,
                        x => SelectedViewType != eViewType.Queries);
                }
                return _viewQueriesCommand;
            }
        }

        #endregion views command

        #region Exit

        RelayCommand _exitCommand;

        /// <summary>
        /// Exit from the application
        /// </summary>
        public ICommand ExitCommand
        {
            get
            {
                if (_exitCommand == null)
                {
                    _exitCommand = new RelayCommand(x => System.Windows.Application.Current.Shutdown());
                }
                return _exitCommand;
            }
        }

        #endregion //Exit

        #region Save query

        RelayCommand _saveQueryCommand;

        /// <summary>
        /// save query
        /// </summary>
        public ICommand SaveQueryCommand
        {
            get
            {
                if (_saveQueryCommand == null)
                {
                    _saveQueryCommand = new RelayCommand(
                        x =>
                        {
                            SelectedDataset.Query.Add(SelectedQuery);
                            saveConfiguration();
                        },
                        x => QueryText.Length > 0 && QueryName.Length > 0 && SelectedQuery != null);
                }
                return _saveQueryCommand;
            }
        }

        #endregion 

        #region Run Query Command

        RelayCommand _runQueryCommand;

        /// <summary>
        /// run query
        /// </summary>
        public ICommand RunQueryCommand
        {
            get
            {
                if (_runQueryCommand == null)
                {
                    _runQueryCommand = new RelayCommand(
                        x => SelectedQuery = buildQuery(),
                        x => QueryText.Length > 0);
                }
                return _runQueryCommand;
            }
        }

        private CQuery buildQuery()
        {
            var query = new CQuery
            {
                isStoreProcedure = false,
                name = QueryName,
                Text = QueryText
            };
            
            var match = new Regex(@"(?<columnfilter>(?<column>[\[]?\w+[\]]?)\s*=\s*(?<idvar>\{\{(?<id>\w+)\}\}))").Match(query.Text);
            while(null != match && match.Success)
            {
                var column = match.Groups["column"].Value;
                var paramName = match.Groups["id"].Value;
                if(!string.IsNullOrEmpty(column) && !string.IsNullOrEmpty(paramName))
                {
                    var col = (from t in SelectedDataset.Table
                              from c in t.Column
                              where c.name.ToLower() == column.ToLower()
                              select c).First();
                    query.Parameter.Add(new CParameter
                    {
                        name = paramName,
                        type = col.DbType,
                        defaultValue = ""
                    });
                }

                match = match.NextMatch();
            }

            return query;
        }

        #endregion

        #region Add Query

        RelayCommand _addQueryCommand;

        /// <summary>
        /// add query
        /// </summary>
        public ICommand AddQueryCommand
        {
            get
            {
                if (_addQueryCommand == null)
                {
                    _addQueryCommand = new RelayCommand(
                        x =>
                        {
                            SelectedParentTable = SelectedDataset.Table.FirstOrDefault();
                            QueryName = "qry" + SelectedParentTable.name.Replace("[", string.Empty).Replace("]", string.Empty);
                            QueryText = SelectedParentTable.ToSelectWhereString("".ToArray(), "".ToArray(), true);
                            var result = new AddQuery().ShowDialog();
                            if (result.HasValue && result.Value)
                                loadAndSetConfiguration();
                        },
                        x => SelectedViewType == eViewType.Queries);
                }
                return _addQueryCommand;
            }
        }
        
        #endregion //Add Query

        #region Table Relationships

        RelayCommand _setTableRelationshipsCommand;

        /// <summary>
        /// setup table relationships
        /// </summary>
        public ICommand SetTableRelationshipsCommand
        {
            get
            {
                if (_setTableRelationshipsCommand == null)
                {
                    _setTableRelationshipsCommand = new RelayCommand(
                        x =>
                        {
                            SelectedConnectionString = SelectedDatasource.ConnectionString;
                            var result = new TableRelationships().ShowDialog();
                            if (result.HasValue && result.Value)
                                loadAndSetConfiguration();
                        });
                }
                return _setTableRelationshipsCommand;
            }
        }

        #endregion //Table Relationships

        #region Add Table Relationship

        RelayCommand _addTableRelationshipCommand;

        /// <summary>
        /// Add table to relationship
        /// </summary>
        public ICommand AddTableRelationshipCommand
        {
            get
            {
                if (_addTableRelationshipCommand == null)
                {
                    _addTableRelationshipCommand = new RelayCommand(
                        x =>
                        {
                            LastErrors.Clear();
                            OnPropertyChanged("LastErrorsString");
                            var result = new AddTableRelationship().ShowDialog();
                            if (result.HasValue && result.Value)
                                loadAndSetConfiguration();
                        });
                }
                return _addTableRelationshipCommand;
            }
        }

        #endregion //Add Table Relationship

        #region Save Relationship

        RelayCommand _saveRelationshipCommand;

        /// <summary>
        /// Save current relationship
        /// </summary>
        public ICommand SaveRelationshipCommand
        {
            get
            {
                if (_saveRelationshipCommand == null)
                {
                    _saveRelationshipCommand = new RelayCommand(
                        x =>
                        {
                            var relationship = new CRelationship
                            {
                                name = string.Format("{0}->{1}", SelectedParentTable.name, SelectedChildTable.name),
                                fromTable = SelectedParentTable.name,
                                toTable = SelectedChildTable.name,
                                fromColumn = SelectedParentColumn.name,
                                toColumn = SelectedChildColumn.name
                            };
                            if (!SelectedDataset.Relationship.Any(r => r.name == relationship.name))
                            {
                                SelectedDataset.Relationship.Add(relationship);
                                saveConfiguration();
                                LastErrors.Clear();
                                LastErrors.Add("Relationship Saved");
                            }
                            else
                            {
                                LastErrors.Clear();
                                LastErrors.Add(string.Format("Table {0} is already related to {1} on columns {2}->{3}", relationship.fromTable, 
                                    relationship.toTable, 
                                    relationship.fromColumn,
                                    relationship.toColumn));
                            }
                            OnPropertyChanged("LastErrorsString");
                            SelectedChildTable = null;
                            SelectedChildColumn = null;
                        },
                        x =>
                        {
                            return SelectedParentTable != null
                                && SelectedParentColumn != null
                                && SelectedChildTable != null
                                && SelectedChildColumn != null;
                        });
                }
                return _saveRelationshipCommand;
            }
        }

        #endregion //Save Relationship

        #region Go Back

        RelayCommand _goBackCommand;

        /// <summary>
        /// go back
        /// </summary>
        public ICommand GoBackCommand
        {
            get
            {
                if (_goBackCommand == null)
                {
                    _goBackCommand = new RelayCommand(
                        x =>
                        {
                            if (TableNavigation.Count > 0)
                            {
                                SelectedDataTable = TableNavigation.Pop();
                            }
                            _goBackCommand.RaiseCanExecuteChanged();
                        },
                        x => TableNavigation.Count > 0);

                }
                return _goBackCommand;
            }
        }

        #endregion //Go Back

        #region Set Filter

        RelayCommand _filterCommand;

        /// <summary>
        /// apply the filter
        /// </summary>
        public ICommand FilterCommand
        {
            get
            {
                if (_filterCommand == null)
                {
                    _filterCommand = new RelayCommand(
                        x => filterMasterData(),
                        x => (null != SelectedRootDataView
                                && string.IsNullOrEmpty(SelectedRootDataView.RowFilter)
                                && !string.IsNullOrEmpty(SelectedCriteria)));

                }
                return _filterCommand;
            }
        }

        void filterMasterData()
        {
            if (null != SelectedRootDataView)
            {
                SelectedRootDataView.RowFilter = SelectedCriteria;
                OnPropertyChanged();
                FilterCommand.AsRelay().RaiseCanExecuteChanged();
                ClearFilterCommand.AsRelay().RaiseCanExecuteChanged();
            }
        }

        #endregion //Set Filter

        #region Clear Filter

        RelayCommand _clearFilterCommand;

        /// <summary>
        /// clear filter
        /// </summary>
        public ICommand ClearFilterCommand
        {
            get
            {
                if (_clearFilterCommand == null)
                {
                    _clearFilterCommand = new RelayCommand(
                        x => clearFilterMasterData(),
                        x => null != SelectedRootDataView
                            && !string.IsNullOrEmpty(SelectedRootDataView.RowFilter));

                }
                return _clearFilterCommand;
            }
        }

        void clearFilterMasterData()
        {
            SelectedRootDataView.RowFilter = string.Empty;
            OnPropertyChanged("SelectedRootDataView");
            FilterCommand.AsRelay().RaiseCanExecuteChanged();
            ClearFilterCommand.AsRelay().RaiseCanExecuteChanged();
        }

        #endregion //Clear Filter

        #region Remove Default SelectedDataSource

        RelayCommand _removeDefaultDataSourceCommand;

        public ICommand RemoveDefaultDataSourceCommand
        {
            get
            {
                if (_removeDefaultDataSourceCommand == null)
                {
                    _removeDefaultDataSourceCommand = new RelayCommand(
                        x =>
                        {
                            AppSettings.DefaultDatasource = string.Empty;
                            AppSettings.Save();
                            _removeDefaultDataSourceCommand.RaiseCanExecuteChanged();
                        },
                        x => !string.IsNullOrEmpty(AppSettings.DefaultDatasource));

                }
                return _removeDefaultDataSourceCommand;
            }
        }

        #endregion //Remove Default SelectedDataSource

        #region Clear Last Errors

        RelayCommand _clearLastErrorsCommand;

        public ICommand ClearLastErrorsCommand
        {
            get
            {
                if (_clearLastErrorsCommand == null)
                {
                    _clearLastErrorsCommand = new RelayCommand(
                        x =>
                        {
                            LastErrors.Clear();
                            OnPropertyChanged("LastErrorsVisibility");
                            _clearLastErrorsCommand.RaiseCanExecuteChanged();
                        },
                        x => LastErrors.Count > 0);

                }
                return _clearLastErrorsCommand;
            }
        }

        #endregion //Clear Last Errors

        #region Clear Search History

        RelayCommand _clearSearchHistoryCommand;

        /// <summary>
        /// clear search history
        /// </summary>
        public ICommand ClearSearchHistoryCommand
        {
            get
            {
                if (_clearSearchHistoryCommand == null)
                {
                    _clearSearchHistoryCommand = new RelayCommand(
                        x =>
                        {
                            AppSettings.SearchValues.Clear();
                            AppSettings.Save();
                            _clearSearchHistoryCommand.RaiseCanExecuteChanged();
                        },
                        x => AppSettings.SearchValues.Count > 0);

                }
                return _clearSearchHistoryCommand;
            }
        }

        #endregion //Clear Search History

        #region Search

        RelayCommand _searchCommand;

        /// <summary>
        /// Search from selected filter
        /// </summary>
        public ICommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                {
                    _searchCommand = new RelayCommand(
                        x => searchDataForRootTable(),
                        x => (null != SelectedRootDataView
                                && !string.IsNullOrEmpty(SelectedSearchCriteria)));

                }
                return _searchCommand;
            }
        }

        async void searchDataForRootTable()
        {
            if (null != SelectedRootDataView)
            {
                if (!AppSettings.SearchValues.Contains(SelectedSearchCriteria))
                {
                    AppSettings.SearchValues.Add(SelectedSearchCriteria);
                    AppSettings.Save();
                    OnPropertyChanged("AppSettings");
                    ClearSearchHistoryCommand.AsRelay().RaiseCanExecuteChanged();
                }

                if (!IsBusy)
                {
                    Common.Extensions.TraceLog.Information("Searching data for table {name} with {SelectedColumn} {SelectedOperator} {SearchCriteria}",
                        WorkingTable.Root.ConfigTable.name,
                        SelectedColumn,
                        SelectedOperator,
                        SearchCriteria
                        );

                    IsBusy = true;
                    var table = await WorkingTable.Root.ConfigTable
                        .Query(SelectedOperator.ToArray(),
                            "".ToArray(),
                            true,
                            new SqlParameter(SelectedColumn, SearchCriteria));

                    IsBusy = false;
                    WorkingTable = table;
                    SearchCommand.AsRelay().RaiseCanExecuteChanged();
                    CopyCommand.AsRelay().RaiseCanExecuteChanged();
                }
            }
        }

        #endregion //Search

        #region copy command

        RelayCommand _copyCommand;

        /// <summary>
        /// Copy current select sql text
        /// </summary>
        public ICommand CopyCommand
        {
            get
            {
                if (_copyCommand == null)
                {
                    _copyCommand = new RelayCommand(
                        x => Clipboard.SetText(ClipboardText, TextDataFormat.Text),
                        x => (null != SelectedRootDataView
                                && !string.IsNullOrEmpty(SelectedSearchCriteria)));

                }
                return _copyCommand;
            }
        }


        #endregion

        #region public export methods

        private void exportTables(DatatableEx root, ExportToFormat format)
        {
            TraceLog.Information("Exporting tables to {format}", format.ToString());

            var fileExtension = ".xlsx";
            switch (format)
            {
                case ExportToFormat.Word2007:
                    fileExtension = ".docx";
                    break;
                case ExportToFormat.HTML:
                    fileExtension = ".html";
                    break;
                case ExportToFormat.PDFtextSharpXML:
                    fileExtension = ".pdf";
                    break;
            }
            var fileName = Path.Combine(ExportPath, root.Root.Table.TableName + fileExtension);
            root.Root.Table.ExportTo(format, fileName);
            foreach (var t in root.Children)
                exportTables(t, format);
        }

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

        public string exportTablesToHtml()
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
            //t.ExportTo(ExportToFormat.HTML, Path.Combine(basePath, string.Format("{0}.html", t.TableName)));
            int level = 0;
            fillHtml(WorkingTable, ref tables, ref level);

            string htmlFile = Path.Combine(basePath, string.Format("{0}.html", namePart));
            File.WriteAllText(htmlFile, html.ToString().Replace("<$Tables>", tables.ToString()));

            Common.Extensions.runProcess(ConfigurationManager.AppSettings["fileExplorer"], htmlFile, -1);
            return htmlFile;
        }

        private void fillHtml(DatatableEx t, ref StringBuilder tables, ref int level)
        {
            string[] levels = new string[] {
                "first", "second", "third", "fourth", "fifth", "sixth", "seventh", "eighth", "nineth", "tenth"
            };
            tables.Append(t.Root.Table.ExportAsHtmlFragment()
                .Replace("className", string.Format("{0}LevelTable", levels[level]))
                .Replace("tableName", t.Root.Table.TableName.ToUpper())
                .Replace("columnCount", t.Root.Table.Columns.Count.ToString()));

            ++level;
            foreach (var child in t.Children)
                fillHtml(child, ref tables, ref level);
            --level;
        }

        public string exportTablesToSqlInsert()
        {
            TraceLog.Information("Exporting tables to SQL Insert");

            string sqlFile = Path.Combine(ExportPath, DateTime.Now.ToString("yyyy-MMM-dd.hh-mm-ss") + ".sql");
            using(var stream = new StreamWriter(sqlFile))
            {
                stream.Write(new StringBuilder().SqlInsert(WorkingTable).ToString());
            }

            Common.Extensions.runProcess(ConfigurationManager.AppSettings["fileExplorer"], sqlFile, -1);

            return sqlFile;
        }        

        #endregion //public export methods

    }
}
