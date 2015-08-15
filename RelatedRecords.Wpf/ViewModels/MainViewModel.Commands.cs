using Common;
using RelatedRecords.Wpf.Commands;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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
                        x =>
                        {
                            IsBusy = true;
                            SelectedNewConfiguration =
                                XmlHelper<CConfiguration>.Load(
                                    Helpers.GetConfigurationFromConnectionString(SelectedConnectionString)
                                );
                            IsBusy = false;
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
                            SelectedConfiguration.Datasource
                                .Add(SelectedNewConfiguration.Datasource.First());
                            SelectedConfiguration.Dataset
                                .Add(SelectedNewConfiguration.Dataset.First());
                            XmlHelper<CConfiguration>.Save(
                                ConfigurationManager.AppSettings["ConfigurationFile"], SelectedConfiguration);

                            OnPropertyChanged("SelectedConfiguration");

                            SelectedNewConfiguration = null;
                            SelectedConnectionString = string.Empty;
                            _loadDatasourceSchemaCommand.RaiseCanExecuteChanged();
                            _setTableRelationshipsCommand.RaiseCanExecuteChanged();
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
        /// Exit from the application
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

        #region export support

        #region Export to Word

        RelayCommand _export2WordCommand;

        /// <summary>
        /// Exit from the application
        /// </summary>
        public ICommand Export2WordCommand
        {
            get
            {
                if (_export2WordCommand == null)
                {
                    _export2WordCommand = new RelayCommand(
                        x => exportTables(SelectedDataTable, ExportToFormat.Word2007),
                        x => null != SelectedDataTable && SelectedDataTable.Root.Table.Rows.Count > 0);
                }
                return _export2WordCommand;
            }
        }

        #endregion //Export to Word

        #region Export to Excel

        RelayCommand _export2ExcelCommand;

        /// <summary>
        /// Exit from the application
        /// </summary>
        public ICommand Export2ExcelCommand
        {
            get
            {
                if (_export2ExcelCommand == null)
                {
                    _export2ExcelCommand = new RelayCommand(
                        x => exportTables(SelectedDataTable, ExportToFormat.Excel2007),
                        x => null != SelectedDataTable && SelectedDataTable.Root.Table.Rows.Count > 0);
                }
                return _export2ExcelCommand;
            }
        }

        #endregion //Export to Excel

        #region Export to PDF

        RelayCommand _export2PDFCommand;

        /// <summary>
        /// Exit from the application
        /// </summary>
        public ICommand Export2PDFCommand
        {
            get
            {
                if (_export2PDFCommand == null)
                {
                    _export2PDFCommand = new RelayCommand(
                        x => exportTables(SelectedDataTable, ExportToFormat.PDFtextSharpXML),
                        x => null != SelectedDataTable && SelectedDataTable.Root.Table.Rows.Count > 0);
                }
                return _export2PDFCommand;
            }
        }

        #endregion //Export to PDF

        #region Export to Html

        RelayCommand _export2HtmlCommand;

        /// <summary>
        /// Exit from the application
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
                        x => null != SelectedDataTable && SelectedDataTable.Root.Table.Rows.Count > 0);
                }
                return _export2HtmlCommand;
            }
        }

        #endregion //Export to Html

        #endregion //export support

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

        #region Connect 

        RelayCommand _connectCommand;

        /// <summary>
        /// Exit from the application
        /// </summary>
        public ICommand ConnectCommand
        {
            get
            {
                if (_connectCommand == null)
                {
                    _connectCommand = new RelayCommand(
                        x =>
                        {
                            if (null != OnConnect)
                                OnConnect(this, EventArgs.Empty);
                        });
                }
                return _connectCommand;
            }
        }

        #endregion //Connect

        #region Work Offline

        RelayCommand _workOfflineCommand;

        /// <summary>
        /// Exit from the application
        /// </summary>
        public ICommand WorkOfflineCommand
        {
            get
            {
                if (_workOfflineCommand == null)
                {
                    _workOfflineCommand = new RelayCommand(
                        x =>
                        {
                            //dataViewModel.saveDataSetEntities();
                            _workOfflineCommand.RaiseCanExecuteChanged();
                            _clearCacheCommand.RaiseCanExecuteChanged();
                        },
                        x => null != SelectedDataTable
                            && SelectedDataTable.Root.Table.Rows.Count > 0
                            && !File.Exists(CacheFile));
                }
                return _workOfflineCommand;
            }
        }

        #endregion //Work Offline

        #region Clear Cache

        RelayCommand _clearCacheCommand;

        /// <summary>
        /// Exit from the application
        /// </summary>
        public ICommand ClearCacheCommand
        {
            get
            {
                if (_clearCacheCommand == null)
                {
                    _clearCacheCommand = new RelayCommand(
                        x =>
                        {
                            //dataViewModel.clearDataSetEntities();
                            _clearCacheCommand.RaiseCanExecuteChanged();
                            _workOfflineCommand.RaiseCanExecuteChanged();
                        },
                        x => File.Exists(CacheFile));
                }
                return _clearCacheCommand;
            }
        }

        #endregion //Clear Cache

        #region Column Relations

        RelayCommand _setColumnRelationshipsCommand;

        /// <summary>
        /// Exit from the application
        /// </summary>
        public ICommand SetColumnRelationshipsCommand
        {
            get
            {
                if (_setColumnRelationshipsCommand == null)
                {
                    /*setColumnRelationshipsCommand = new RelayCommand(
                        x => OnSetColumnRelationships(),
                        x => SelectedSchemaTable != null && SelectedSchemaTable2 != null);*/
                }
                return _setColumnRelationshipsCommand;
            }
        }

        private void OnSetColumnRelationships()
        {
            MessageBox.Show("Feature not yet Implemented!", "Information",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion //Column Relations

        #region Table Relationships

        RelayCommand _setTableRelationshipsCommand;

        /// <summary>
        /// Exit from the application
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
                            new TableRelationships().ShowDialog();
                        });
                }
                return _setTableRelationshipsCommand;
            }
        }

        #endregion //Table Relationships

        #region Go Back

        RelayCommand _goBackCommand;

        /// <summary>
        /// Delete this person
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
        /// Delete this person
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
                _filterCommand.RaiseCanExecuteChanged();
                _clearFilterCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion //Set Filter

        #region Clear Filter

        RelayCommand _clearFilterCommand;

        /// <summary>
        /// Delete this person
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
            _filterCommand.RaiseCanExecuteChanged();
            _clearFilterCommand.RaiseCanExecuteChanged();
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
        /// Delete this person
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
        /// Delete this person
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
                    _clearSearchHistoryCommand.RaiseCanExecuteChanged();
                }

                if (!IsBusy)
                {
                    Common.Extensions.TraceLog.Information("Searching data for table {name} with {SelectedColumn} {SelectedOperator} {SearchCriteria}",
                        SelectedDataTable.Root.ConfigTable.name,
                        SelectedColumn,
                        SelectedOperator,
                        SearchCriteria
                        );

                    IsBusy = true;
                    var table = await SelectedDataTable.Root.ConfigTable
                        .Query(SelectedOperator.ToArray(),
                            "".ToArray(),
                            true,
                            new SqlParameter(SelectedColumn, SearchCriteria));

                    IsBusy = false;
                    SelectedDataTable = table;
                    _searchCommand.RaiseCanExecuteChanged();
                }
            }
        }

        #endregion //Search

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
            fillHtml(SelectedDataTable, ref tables, ref level);

            string retval = Path.Combine(basePath, string.Format("{0}.html", namePart));
            File.WriteAllText(retval, html.ToString().Replace("<$Tables>", tables.ToString()));

            return retval;
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

        #endregion //public export methods

    }
}
