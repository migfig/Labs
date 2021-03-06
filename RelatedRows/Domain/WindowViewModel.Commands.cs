﻿using DynamicData;
using MaterialDesignThemes.Wpf;
using RelatedRows.Controls;
using RelatedRows.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Windows;
using System.Windows.Input;

namespace RelatedRows.Domain
{
    public partial class WindowViewModel
    {
        private ICommand _runStoreProcCommand;
        public ICommand RunStoreProcCommand {
            get
            {
                return _runStoreProcCommand ?? (_runStoreProcCommand = new Command(() => {
                    IsBusy = true;

                    _schedulerProvider.Background.Schedule(async () =>
                    {
                        if (SelectedQuery != null)
                        {
                            try
                            {
                                SelectedQuery.DataTable =
                                    await _dataSourceProvider
                                        .GetStoreProcedureData(
                                            SelectedDatasource,
                                            SelectedQuery.name,
                                            SelectedQuery.GetParameters());

                                if (!SelectedDataset.QueryHistory.Any(qh => qh.key.Equals(SelectedQuery.key)))
                                {
                                    var newQuery = CQuery.Clone(SelectedQuery);
                                    if (!File.Exists(DefaultStoreProcsHistoryFile))
                                    {
                                        var histConfig = new CConfiguration();
                                        histConfig.defaultDataset = Configuration.defaultDataset;
                                        histConfig.defaultDatasource = Configuration.defaultDatasource;
                                        histConfig.Datasource.AddRange(Configuration.Datasource);
                                        foreach (var ds in Configuration.Dataset)
                                            histConfig.Dataset.Add(new CDataset
                                            {
                                                name = ds.name,
                                                dataSourceName = ds.dataSourceName,
                                                defaultTable = ds.defaultTable
                                            });

                                        histConfig.Dataset.FirstOrDefault(d => d.name.Equals(SelectedDataset.name))
                                            .Query.Add(newQuery);
                                        XmlHelper<CConfiguration>.Save(DefaultStoreProcsHistoryFile, histConfig);

                                        _schedulerProvider.MainThread.Schedule(() =>
                                        {
                                            SelectedDataset.QueryHistory.Add(newQuery);
                                        });
                                    }
                                    else
                                    {
                                        var histConfig = XmlHelper<CConfiguration>.Load(DefaultStoreProcsHistoryFile);
                                        histConfig.Dataset.FirstOrDefault(d => d.name.Equals(SelectedDataset.name))
                                            .Query.Add(newQuery);

                                        foreach (var dsrc in Configuration.Datasource)
                                            if (!histConfig.Datasource.Any(dsource => dsource.name.Equals(dsrc.name)))
                                                histConfig.Datasource.Add(new CDatasource {
                                                    name = dsrc.name,
                                                    ConnectionString = dsrc.ConnectionString
                                                });

                                        foreach (var ds in Configuration.Dataset)
                                            if(!histConfig.Dataset.Any(d => d.name.Equals(ds.name)))
                                                histConfig.Dataset.Add(new CDataset
                                                {
                                                    name = ds.name,
                                                    dataSourceName = ds.dataSourceName,
                                                    defaultTable = ds.defaultTable
                                                });

                                        XmlHelper<CConfiguration>.Save(DefaultStoreProcsHistoryFile, histConfig);

                                        _schedulerProvider.MainThread.Schedule(() =>
                                        {
                                            SelectedDataset.QueryHistory.Add(newQuery);
                                        });
                                    }
                                }
                            } catch(Exception e)
                            {
                                Logger.Log.Error(e, "Exception while running store proc {@name}", SelectedQuery.name);
                                MessageQueue.Enqueue(e.Message);
                            }
                            finally
                            {
                                _schedulerProvider.MainThread.Schedule(() => IsBusy = false);
                            }
                        }
                    });
                }));
            }               
        }

        private ICommand _runQueryCommand;
        public ICommand RunQueryCommand
        {
            get
            {
                return _runQueryCommand ?? (_runQueryCommand = new Command<CQuery>((query) => {
                    IsBusy = true;

                    _schedulerProvider.Background.Schedule(async () =>
                    {
                        if (!string.IsNullOrEmpty(query.Text))
                        {
                            try
                            {
                                var children = new List<DataTable>();
                                foreach(var subQuery in query.GetQueries())
                                    if(subQuery.Replace(Environment.NewLine, "").Trim().Length > 0)
                                        children.Add(
                                            await _dataSourceProvider
                                                .GetQueryData(
                                                    SelectedDatasource,
                                                    subQuery));

                                _schedulerProvider.MainThread.Schedule(() => {
                                    query.Children.Clear();
                                    query.Children.AddRange(children);
                                });
                            }
                            catch (Exception e)
                            {
                                Logger.Log.Error(e, "Exception while running query {@name}", query.name);
                                MessageQueue.Enqueue(e.Message);
                            }
                            finally
                            {
                                _schedulerProvider.MainThread.Schedule(() => IsBusy = false);
                            }
                        }
                    });
                }));
            }
        }

        private ICommand _addQueryCommand;
        public ICommand AddQueryCommand
        {
            get
            {
                return _addQueryCommand ?? (_addQueryCommand = new Command<string>((action) => {
                    var query = new CQuery
                    {
                        catalog = SelectedDataset.Table.First().catalog,
                        schemaName = SelectedDataset.Table.First().schemaName,
                        name = $"{SelectedTable.name}-{DateTime.Now.ToString("yyyyMMMdd-hhmmss")}",
                        type = "Q",
                        isStoreProcedure = false,
                        Text = $"SELECT * FROM {SelectedTable.catalog}.{SelectedTable.schemaName}.{SelectedTable.name} "
                            + SelectedTable.Column.Aggregate("WHERE", (seed, c) => seed + $" AND {c.name} = {c.QuoteParam()}")
                                .Replace("WHERE AND", "WHERE ") + ";",
                        Parameter = new ObservableCollection<CParameter>
                        {
                            SelectedTable.Column.Select(c => 
                                new CParameter
                                {
                                    type = c.DbType,
                                    name = $"@{c.name.UnQuoteName()}",
                                    defaultValue = c.DefaultValue()
                                })
                        }
                    };
                    if (action == "add" || SelectedScriptQuery == null)
                    {
                        SelectedDataset.Query.Add(query);
                        SelectedScriptQuery = query;
                    }
                    else
                    {
                        SelectedScriptQuery.Text +=                            
                           $"{(!SelectedScriptQuery.Text.TrimEnd().EndsWith(";") ? ";" : "")}{Environment.NewLine}{Environment.NewLine}{query.Text}";

                        var currParNames = SelectedScriptQuery.Parameter.Select(p => p.name);
                        SelectedScriptQuery.Parameter.AddRange(
                            query.Parameter.Where(p => !currParNames.Contains(p.name)));
                        OnPropertyChanged("SelectedScriptQuery");
                    }
                }));
            }
        }

        private ICommand _refreshQueryCommand;
        public ICommand RefreshQueryCommand
        {
            get
            {
                return _refreshQueryCommand ?? (_refreshQueryCommand = new Command(() => {
                }));
            }
        }

        private ICommand _removeParametersCommand;
        public ICommand RemoveParametersCommand
        {
            get
            {
                return _removeParametersCommand ?? (_removeParametersCommand = new Command(() => {
                    var selectedParams = SelectedScriptQuery.SelectedParameters ?? SelectedScriptQuery.Parameter;
                    selectedParams.ToList().ForEach((p) =>
                    {
                        SelectedScriptQuery.Text = 
                            SelectedScriptQuery.Text.RestoreText(p);
                    });

                    for(var i=selectedParams.Count()-1;i>=0;i--)
                        SelectedScriptQuery.Parameter.Remove(selectedParams.ElementAt(i));
                    OnPropertyChanged("SelectedScriptQuery");                    
                }));
            }
        }

        private ICommand _actionParameterCommand;
        public ICommand ActionParameterCommand
        {
            get
            {
                return _actionParameterCommand ?? (_actionParameterCommand = new Command<CComboTrick>((trick) => {
                    var p = trick.owner;

                    if (trick.action == "Remove Parameter")
                    {                        
                         SelectedScriptQuery.Text = SelectedScriptQuery.Text.RestoreText(p);
                        
                        SelectedScriptQuery.Parameter.Remove(p);
                        OnPropertyChanged("SelectedScriptQuery");
                    }
                    else
                    {
                        SelectedScriptQuery.Parameter.Add(new CParameter
                        {
                            name = $"{p.name}{SelectedScriptQuery.Parameter.Count + 1}",
                            type = p.type,
                            customType = p.customType,
                            length = p.length,
                            defaultValue = p.defaultValue
                        });
                    }
                }));
            }
        }

        private ICommand _refreshSchemaCommand;
        public ICommand RefreshSchemaCommand
        {
            get
            {
                return _refreshSchemaCommand ?? (_refreshSchemaCommand = new Command<CDataset>((dataSet) =>
                {
                    new Action(async () => {
                        var proceed = (bool)await DialogHost.Show(new NotificationDialog
                        {
                            DataContext = "Running an Schema refresh can take some minute(s) to complete. \r\nDo you want to proceed?"
                        }, "RootDialog", ClosingEventHandler);

                        if (proceed) ReLoadAndSaveConfiguration(dataSet);
                    })
                    .Invoke();                    
                }));
            }
        }

        private ICommand _refreshTableSchemaCommand;
        public ICommand RefreshTableSchemaCommand
        {
            get
            {
                return _refreshTableSchemaCommand ?? (_refreshTableSchemaCommand = new Command<CTable>((table) => {
                    ReLoadAndSaveTableConfiguration(SelectedDataset, table);
                }));
            }
        }

        private ICommand _copyQueryCommand;
        public ICommand CopyQueryCommand
        {
            get
            {
                return _copyQueryCommand ?? (_copyQueryCommand = new Command<CQuery>((query) => {
                    Clipboard.SetText(query.FriendlyText, TextDataFormat.Text);
                    query.name.PersistAndRunText(query.FriendlyText);
                }));
            }
        }

        private ICommand _saveQueryCommand;
        public ICommand SaveQueryCommand
        {
            get
            {
                return _saveQueryCommand ?? (_saveQueryCommand = new Command<CQuery>((query) => {
                    var config = XmlHelper<CConfiguration>.Load(DefaultStoreProcsConfigFile);
                    config.Dataset.FirstOrDefault(d => d.name.Equals(SelectedDataset.name))
                        .Query.Add(query);

                    foreach (var dsrc in Configuration.Datasource)
                        if (!config.Datasource.Any(dsource => dsource.name.Equals(dsrc.name)))
                            config.Datasource.Add(new CDatasource
                            {
                                name = dsrc.name,
                                ConnectionString = dsrc.ConnectionString
                            });

                    foreach (var ds in Configuration.Dataset)
                        if (!config.Dataset.Any(d => d.name.Equals(ds.name)))
                            config.Dataset.Add(new CDataset
                            {
                                name = ds.name,
                                dataSourceName = ds.dataSourceName,
                                defaultTable = ds.defaultTable
                            });

                    XmlHelper<CConfiguration>.Save(DefaultStoreProcsConfigFile, config);
                }));
            }
        }        

        private ICommand _setTargetTableCommand;
        public ICommand SetTargetTableCommand
        {
            get
            {
                return _setTargetTableCommand ?? (_setTargetTableCommand = new Command<CRelationship>((r) => {
                    SelectedTargetTable = SelectedDataset.Table.FirstOrDefault(t => t.name.Equals(r.toTable));
                }));
            }
        }

        private ICommand _exportToSqlCommand;
        public ICommand ExportToSqlCommand
        {
            get
            {
                return _exportToSqlCommand ?? (_exportToSqlCommand = new Command<CTable>((table) =>
                {
                    table.name.PersistAndRunText(_dataSourceProvider.QueryBuilder.SqlInsert(table, true));
                }));
            }
        }

        private ICommand _exportToCsvCommand;
        public ICommand ExportToCsvCommand
        {
            get
            {
                return _exportToCsvCommand ?? (_exportToCsvCommand = new Command<DataTable>((table) => {
                    table.TableName.PersistAndRunText(table.CsvExport(), "csv");
                }));
            }
        }

        private ICommand _setQueryCommand;
        public ICommand SetQueryCommand
        {
            get
            {
                return _setQueryCommand ?? (_setQueryCommand = new Command<CQuery>((item) => {
                    var query = SelectedDataset.Query.FirstOrDefault(q => q.name.Equals(item.name));
                    if(query != null)
                    {
                        SelectedQuery = query;
                        item.Parameter.ToList().ForEach(p =>
                            SelectedQuery.Parameter
                                .FirstOrDefault(par => par.name.Equals(p.name))
                                    .defaultValue = p.defaultValue
                        );
                    }
                }));
            }
        }

        private ICommand _deleteQueryHistoryCommand;
        public ICommand DeleteQueryHistoryCommand
        {
            get
            {
                return _deleteQueryHistoryCommand ?? (_deleteQueryHistoryCommand = new Command<CQuery>((item) => {
                    var query = SelectedDataset.QueryHistory.FirstOrDefault(q => q.key.Equals(item.key));
                    if (query != null)
                    {
                        var histConfig = XmlHelper<CConfiguration>.Load(DefaultStoreProcsHistoryFile);
                        var ds = histConfig.Dataset.FirstOrDefault(d => d.name.Equals(SelectedDataset.name));
                        var remQuery = ds.Query.FirstOrDefault(q => q.key.Equals(item.key));
                        ds.Query.Remove(remQuery);

                        XmlHelper<CConfiguration>.Save(DefaultStoreProcsHistoryFile, histConfig);

                        SelectedDataset.QueryHistory.Remove(query);
                    }
                }));
            }
        }

        private ICommand _openFileCommand;
        public ICommand OpenFileCommand
        {
            get
            {
                return _openFileCommand ?? (_openFileCommand = new Command(OpenFile));
            }
        }

        private ICommand _showInGitHubCommand;
        public ICommand ShowInGitHubCommand
        {
            get
            {
                return _showInGitHubCommand ?? 
                    (_showInGitHubCommand = new Command<object>((url) => 
                        Process.Start(url is Uri ? (url as Uri).ToString() : "https://github.com/migfig")));
            }
        }

        private ICommand _refreshCommand;
        public ICommand RefreshCommand {
            get {
                return _refreshCommand ?? (_refreshCommand = new Command<CTable>((table) =>
                {
                    if (_viewMode == eViewMode.Data && table != null)
                    {
                        IsBusy = true;
                        _schedulerProvider.Background.Schedule(async () =>
                        {
                            try
                            {
                                table.DataTable =
                                    await _dataSourceProvider
                                        .GetData(SelectedDatasource, table.name, 
                                            _dataSourceProvider.QueryBuilder.GetQuery(table, Settings.RowsPerPage));

                                table.Pager = new CPager(table.DataTable.RowsCount(), Settings.RowsPerPage);

                                if (table.DataTable != null && table.DataTable.Rows.Count > 0)
                                    table.Children.ToList().ForEach(async (child) =>
                                    {
                                        child.DataTable =
                                            await _dataSourceProvider
                                                .GetData(SelectedDatasource, child.name, 
                                                    _dataSourceProvider.QueryBuilder.GetQuery(child, Settings.RowsPerPage, parent: table));
                                    });

                                if (table == SelectedTable)
                                    OnPropertyChanged("SelectedTable");
                            }
                            catch (Exception e)
                            {
                                Logger.Log.Error(e, "Exception while loading table {@name}", table.name);
                                MessageQueue.Enqueue(e.Message);
                            }
                            finally
                            {
                                _schedulerProvider.MainThread.Schedule(() => IsBusy = false);
                            }
                        });
                    }
                }));
            }
        }

        private ICommand _exitCommmand;
        public ICommand ExitCommmand
        {
            get
            {
                return _exitCommmand ?? (_exitCommmand = new Command(() =>
                {
                    Application.Current.Shutdown();
                }));
            }
        }

        private ICommand _zoomInCommand;
        public ICommand ZoomInCommand
        {
            get
            {
                return _zoomInCommand ?? (_zoomInCommand = new Command(() => { Settings.Scale = Settings.Scale - 5; }));
            }
        }

        private ICommand _zoomOutCommand;
        public ICommand ZoomOutCommand
        {
            get
            {
                return _zoomOutCommand ?? (_zoomOutCommand = new Command(() => { Settings.Scale = Settings.Scale + 5; }));
            }
        }

        private ICommand _setDataset;
        public ICommand SetDataset
        {
            get
            {
                return _setDataset ?? (_setDataset = new Command<CDataset>((o) =>
                {
                    if (SelectedDataset == null || SelectedDataset.name != o.name)
                    {
                        Logger.Log.Verbose("SetDataset [{@name}]", o.name);

                        SelectedDataset = o;

                        var dSrc = Configuration.Datasource.FirstOrDefault(ds => ds.name.Equals(o.dataSourceName));
                        _dataSourceProvider = _dataSourceProviders.FirstOrDefault(p => p.Name.Equals(dSrc.providerName));
                        _dataSourceProvider.SetLog(Logger.Log);

                        Configuration.Dataset.ToList().ForEach((i) => i.isSelected = false);
                        SelectedDataset.isSelected = true;
                        SelectedTable = SelectedDataset.Table.FirstOrDefault(t => t.name.Equals(SelectedDataset.defaultTable));

                        if (SelectedDataset.Query.Any())
                        {
                            var config = XmlHelper<CConfiguration>.Load(DefaultStoreProcsConfigFile);

                            var defaultQuery = config.Dataset.FirstOrDefault(d => d.name.Equals(SelectedDataset.name)).defaultTable;
                            SelectedQuery = SelectedDataset.Query.FirstOrDefault(q => q.name.Equals(defaultQuery));
                            SelectedScriptQuery = SelectedDataset.Query.LastOrDefault(q => q.isScript);
                        }
                    }
                }));
            }
        }

        private ICommand _setDefaultTable;
        public ICommand SetDefaultTable
        {
            get
            {
                return _setDefaultTable ?? (_setDefaultTable = new Command<CTable>((o) =>
                {
                    if (SelectedDataset.defaultTable != o.name)
                    {
                        Logger.Log.Verbose("SetDefaultTable [{@name}]", o.name);

                        var config = XmlHelper<CConfiguration>.Load(DefaultConfigFile);
                        config.Dataset.FirstOrDefault(d => d.name.Equals(SelectedDataset.name))
                            .defaultTable = o.name;
                        XmlHelper<CConfiguration>.Save(DefaultConfigFile, config);
                    }
                }));
            }
        }

        private ICommand _setDefaultStoreProc;
        public ICommand SetDefaultStoreProc
        {
            get
            {
                return _setDefaultStoreProc ?? (_setDefaultStoreProc = new Command<CQuery>((o) =>
                {
                    Logger.Log.Verbose("SetDefaultStoreProc [{@name}]", o.name);

                    var config = XmlHelper<CConfiguration>.Load(DefaultStoreProcsConfigFile);
                    config.Dataset.FirstOrDefault(d => d.name.Equals(SelectedDataset.name))
                        .defaultTable = o.name;
                    XmlHelper<CConfiguration>.Save(DefaultStoreProcsConfigFile, config);
                }));
            }
        }

        private ICommand _relateColumnsCommand;
        public ICommand RelateColumnsCommand
        {
            get
            {
                return _relateColumnsCommand ?? (_relateColumnsCommand = new Command(() =>
                {
                    ApplicationErrors = SelectedTable == null || SelectedTargetTable == null
                        ? "Please select Source and Destination tables"
                        : string.Empty;

                    if (string.IsNullOrEmpty(ApplicationErrors))
                    {
                        if (SelectedColumn == null || SelectedTargetColumn == null)
                            ApplicationErrors = "Please select the Columns to be Related";
                        else if(SelectedColumn != null && SelectedTargetColumn != null
                                && SelectedColumn.DbType != SelectedTargetColumn.DbType)
                            ApplicationErrors = "Selcted Columns must be of the same type";

                        if (string.IsNullOrEmpty(ApplicationErrors))
                        {
                            var relationship = new CRelationship
                            {
                                fromTable = SelectedTable.name,
                                toTable = SelectedTargetTable.name,
                                ColumnRelationship = new ObservableCollection<CColumnRelationShip>
                                {
                                    SelectedTable.Column.Where(c => c.name.Equals(SelectedColumn.name))
                                        .Select(c => new CColumnRelationShip
                                    {
                                        fromColumn = c.name,
                                        toColumn = SelectedTargetTable.Column.Where(tc => tc.name.Equals(SelectedTargetColumn.name)).First().name
                                    })
                                }
                            };

                            if (relationship.ColumnRelationship.Any()
                                && relationship.ColumnRelationship.All(cr => !string.IsNullOrEmpty(cr.fromColumn)
                                    && !string.IsNullOrEmpty(cr.toColumn)))
                            {
                                relationship.name = relationship.GetName();
                                SelectedDataset.Relationship.Add(relationship);
                                SelectedTable.Relationship.Add(SelectedDataset.Relationship.Last());
                                AppendChildren(SelectedTable, SelectedDataset);

                                SelectedColumn = null;
                                SelectedTargetColumn = null;

                                _newRelationships.Add(relationship);
                                OnPropertyChanged("HasRelationshipChanges");
                            }
                        }
                    }
                }, () => true /*SelectedTable != null && SelectedTargetTable != null*/));
            }
        }

        private ICommand _unrelateColumnCommand;
        public ICommand UnrelateColumnCommand
        {
            get
            {
                return _unrelateColumnCommand ?? (_unrelateColumnCommand = new Command<CRelationship>((relationship) =>
                {
                    _removedRelationships.Add(SelectedDataset.Relationship.FirstOrDefault(r => r.fromTable.Equals(SelectedTable.name)
                                && r.toTable.Equals(relationship.toTable)));

                    SelectedTable.Relationship.Remove(SelectedTable.Relationship.FirstOrDefault(r => r.name.Equals(relationship.name)));
                    SelectedTable.Children.Remove(SelectedTable.Children.FirstOrDefault(c => c.name.Equals(relationship.toTable)));
                    SelectedDataset.Relationship.Remove(
                            SelectedDataset.Relationship.FirstOrDefault(r => r.fromTable.Equals(SelectedTable.name)
                                && r.toTable.Equals(relationship.toTable)));
                    OnPropertyChanged("HasRelationshipChanges");
                }));
            }
        }

        private ICommand _removeRelationshipsCommand;
        public ICommand RemoveRelationshipsCommand
        {
            get
            {
                return _removeRelationshipsCommand ?? (_removeRelationshipsCommand = new Command(() =>
                {
                    _removedRelationships.Add(SelectedTable.Relationship);
                    var relatedTables = SelectedTable.Relationship.Select(r => r.toTable);
                    SelectedTable.Relationship.Clear();
                    foreach (var toTable in relatedTables)
                    {
                        SelectedDataset.Relationship.Remove(
                            SelectedDataset.Relationship.FirstOrDefault(r => r.fromTable.Equals(SelectedTable.name)
                                && r.toTable.Equals(toTable)));
                        SelectedTable.Children.Remove(
                            SelectedTable.Children.FirstOrDefault(c => c.name.Equals(toTable)));
                    }
                }));
            }
        }

        private ICommand _copyRowCommand;
        public ICommand CopyRowCommand
        {
            get
            {
                return _copyRowCommand ?? (_copyRowCommand = new Command<CTable>((table) =>
                {
                    Clipboard
                            .SetText(table.CopyTooltip, TextDataFormat.Text);
                    table.name.PersistAndRunText(table.CopyTooltip);
                }));
            }
        }

        private ICommand _setMainViewCommand;
        public ICommand SetMainViewCommand
        {
            get
            {
                return _setMainViewCommand ?? (_setMainViewCommand = new Command<CTable>((table) =>
                {
                    Logger.Log.Verbose("SetMainViewCommand [{@name}]", table.name);

                    _navigationStack.Push(_selectedTable);
                    SelectedTable = table;
                    OnPropertyChanged("GoHomeVisibility");
                }));
            }
        }

        private ICommand _goHomeCommand;
        public ICommand GoHomeCommand
        {
            get
            {
                return _goHomeCommand ?? (_goHomeCommand = new Command(() =>
                {
                    Logger.Log.Verbose("GoHomeCommand");

                    var table = _navigationStack.Pop();
                    SelectedTable = table;
                    OnPropertyChanged("GoHomeVisibility");
                }));
            }
        }

        private ICommand _pageCommand;
        public ICommand PageCommand
        {
            get
            {
                return _pageCommand ?? (_pageCommand = new Command<string>((direction) =>
                {
                    Logger.Log.Verbose("PageCommand [{@direction}]", direction);

                    SelectedTable.Pager.Navigate(direction);
                    OnSelectedTableChange();
                }));
            }
        }

        private ICommand _child1stPageCommand;
        public ICommand Child1stPageCommand
        {
            get
            {
                return _child1stPageCommand ?? (_child1stPageCommand = new Command<CTable>((table) =>
                {
                    Logger.Log.Verbose("Child PageCommand [{@direction}]", "first");

                    table.Pager.Navigate("first");
                    OnChildTableChange(table);
                }));
            }
        }

        private ICommand _childNextPageCommand;
        public ICommand ChildNextPageCommand
        {
            get
            {
                return _childNextPageCommand ?? (_childNextPageCommand = new Command<CTable>((table) =>
                {
                    Logger.Log.Verbose("Child PageCommand [{@direction}]", "next");

                    table.Pager.Navigate("next");
                    OnChildTableChange(table);
                }));
            }
        }

        private ICommand _childPrevPageCommand;
        public ICommand ChildPrevPageCommand
        {
            get
            {
                return _childPrevPageCommand ?? (_childPrevPageCommand = new Command<CTable>((table) =>
                {
                    Logger.Log.Verbose("Child PageCommand [{@direction}]", "prev");

                    table.Pager.Navigate("prev");
                    OnChildTableChange(table);
                }));
            }
        }

        private ICommand _childLastPageCommand;
        public ICommand ChildLastPageCommand
        {
            get
            {
                return _childLastPageCommand ?? (_childLastPageCommand = new Command<CTable>((table) =>
                {
                    Logger.Log.Verbose("Child PageCommand [{@direction}]", "last");

                    table.Pager.Navigate("last");
                    OnChildTableChange(table);
                }));
            }
        }

        private ICommand _collectMemoryCommand;
        public ICommand CollectMemoryCommand
        {
            get
            {
                return _collectMemoryCommand ?? (_collectMemoryCommand = new Command(() =>
                {
                    Logger.Log.Verbose("CollectMemoryCommand");

                    //Diagnostics [useful for memory testing]
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }));
            }
        }

        private ICommand _maximizeRestoreChildSizeCommand;
        public ICommand MaximizeRestoreChildSizeCommand
        {
            get
            {
                return _maximizeRestoreChildSizeCommand ?? (_maximizeRestoreChildSizeCommand = new Command<bool>((isChecked) =>
                {
                    _defaultMaxChildSize = isChecked ? 1.0: 0.4;
                    OnPropertyChanged("MaxChildSize");
                }));
            }
        }       
    }
}
