﻿using Dragablz;
using DynamicData;
using DynamicData.Binding;
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using RelatedRows.Controls;
using RelatedRows.Helpers;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace RelatedRows.Domain
{
    public partial class WindowViewModel : AbstractNotifyPropertyChanged
    {
        private readonly PlugableProviders _plugableProviders;
        private IEnumerable<IDatasourceProvider> _dataSourceProviders;
        private IDatasourceProvider _dataSourceProvider;
        private IEnumerable<IDatasetConsumer> _dataSetConsumers;
        private IEnumerable<ITableConsumer> _tableConsumers;
        private readonly ISchedulerProvider _schedulerProvider;
        private bool _menuIsOpen;

        public string Version { get; }

        public string Title
        {
            get { return $"Related Rows"; } //[{WindowSize.Width}:{WindowSize.Height}]"; }
        }

        private Size _windowSize;
        public Size WindowSize
        {
            get { return _windowSize; }
            set
            {
                SetAndRaise(ref _windowSize, value);
                OnPropertyChanged("Title");
                OnPropertyChanged("MaxChildSize");
            }
        }

        private double _defaultMaxChildSize = 0.4;
        public double MaxChildSize
        {
            get { return WindowSize.Width * _defaultMaxChildSize; }
        }

        private ApplicationOptions _settings;
        public ApplicationOptions Settings { get { return _settings; } }

        private string DefaultConfigFile
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configuration.xml"); }
        }

        private string DefaultStoreProcsConfigFile
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configuration-store-procs.xml"); }
        }

        private string DefaultStoreProcsHistoryFile
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configuration-store-procs-hist.xml"); }
        }        

        private Stack<CTable> _navigationStack = new Stack<CTable>();

        private static WindowViewModel _viewModel;
        public static WindowViewModel GetViewModel(ISchedulerProvider provider)
        {
            if (_viewModel == null)
            {
                _viewModel = new WindowViewModel(provider);
            }
            return _viewModel;
        }

        public WindowViewModel(ISchedulerProvider schedulerProvider)
        {
            _settings = ApplicationOptions.Get();
            Logger.SetLevel(_settings.LogLevel);
            _settings.ApplyThemeCommand.Execute(_settings.Theme.Equals(Theme.Dark));
            _settings.ApplyTheme();
            _isEmpty = true;
            _plugableProviders = new PlugableProviders();
            _plugableProviders.Load();
            _dataSourceProviders = (from p in _plugableProviders.Plugins
                                   let item = p as IDatasourceProvider
                                   where item != null
                                   select item).ToList();
            _dataSetConsumers = (from p in _plugableProviders.Plugins
                                 let item = p as IDatasetConsumer
                                 where item != null
                                 select item).ToList();
            _tableConsumers = (from p in _plugableProviders.Plugins
                               let item = p as ITableConsumer
                               where item != null
                               select item).ToList();
            _dataSourceProvider = _dataSourceProviders.FirstOrDefault(p => p.IsDefault);
            _dataSourceProvider.SetLog(Logger.Log);
            _schedulerProvider = schedulerProvider;

            Version = $"v{Assembly.GetEntryAssembly().GetName().Version.ToString(3)}";

            if (File.Exists(DefaultConfigFile))
                OpenFiles(DefaultConfigFile.Split());
        }

        private void OpenFile()
        {
            if (IsBusy) return;

            var dialog = new OpenFileDialog { Filter = "Xml files (*.xml)|*.xml" };
            var result = dialog.ShowDialog();
            if (result != true) return;

            OpenFile(new FileInfo(dialog.FileName));
        }

        public void OpenFiles(IEnumerable<string> files = null)
        {
            if (files == null) return;

            foreach (var file in files)
                OpenFile(new FileInfo(file));
        }

        private void OpenFile(FileInfo file)
        {
            IsBusy = true;
            Logger.Log.Verbose("OpenFile({@FullName})", file.FullName);

            _schedulerProvider.MainThread.Schedule(() =>
            {
                try
                {
                    Configuration = InitConfiguration(XmlHelper<CConfiguration>.Load(file.FullName));
                    IsEmpty = false;
                    _dataSetConsumers.ToList().ForEach(c => c.OnConnected(Configuration, Logger.Log));
                    SetDataset.Execute(Configuration.Dataset.FirstOrDefault());

                    var qryConfig = XmlHelper<CConfiguration>.Load(file.FullName.Replace(".xml", "-store-procs.xml"));
                    if (qryConfig.Dataset.Any())
                    {
                        foreach (var ds in qryConfig.Dataset)
                            Configuration.Dataset.FirstOrDefault(d => d.name.Equals(ds.name))
                                .Query.AddRange(ds.Query);
                        SelectedQuery = qryConfig.Dataset.FirstOrDefault()
                            .Query.FirstOrDefault(q => q.name.Equals(
                                qryConfig.Dataset.FirstOrDefault().defaultTable));
                        SelectedScriptQuery = qryConfig.Dataset.LastOrDefault()
                            .Query.FirstOrDefault(q => q.isScript);
                    }

                    if (File.Exists(file.FullName.Replace(".xml", "-store-procs-hist.xml")))
                    {
                        var histConfig = XmlHelper<CConfiguration>.Load(file.FullName.Replace(".xml", "-store-procs-hist.xml"));
                        if (histConfig.Dataset.Any())
                        {
                            foreach (var ds in histConfig.Dataset)
                                Configuration.Dataset.FirstOrDefault(d => d.name.Equals(ds.name))
                                    .QueryHistory.AddRange(ds.Query);
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Log.Error(e, "Exception while opening {@fullname}", file.FullName);
                    MessageQueue.Enqueue(e.Message);
                }
                finally
                {
                    IsBusy = false;
                }
            });
        }

        public void OnWindowClosing()
        {
        }

        private void ClosingTabItemHandlerImpl(ItemActionCallbackArgs<TabablzControl> args)
        {
        }

        private SnackbarMessageQueue _messageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(6));
        public SnackbarMessageQueue MessageQueue
        {
            get { return _messageQueue; }
        }

        private CDataset _selectedDataset;
        public CDataset SelectedDataset
        {
            get { return _selectedDataset; }
            set {
                SetAndRaise(ref _selectedDataset, value);
                _dataSetConsumers.ToList().ForEach(c => c.OnDatasetUpdated(value));
            }
        }

        public CDatasource SelectedDatasource
        {
            get { return Configuration.Datasource.FirstOrDefault(ds => ds.name.Equals(SelectedDataset.dataSourceName)); }
        }

        private bool _isEmpty = true;
        public bool IsEmpty
        {
            get { return _isEmpty; }
            set { SetAndRaise(ref _isEmpty, value); }
        }

        public bool MenuIsOpen
        {
            get { return _menuIsOpen; }
            set { SetAndRaise(ref _menuIsOpen, value); }
        }

        private CConfiguration _configuration;
        public CConfiguration Configuration
        {
            get { return _configuration; }
            private set { SetAndRaise(ref _configuration, value); }
        }

        private CQuery _selectedQuery;
        public CQuery SelectedQuery
        {
            get { return _selectedQuery; }
            set { SetAndRaise(ref _selectedQuery, value); }
        }

        private CQuery _selectedScriptQuery;
        public CQuery SelectedScriptQuery
        {
            get { return _selectedScriptQuery; }
            set { SetAndRaise(ref _selectedScriptQuery, value); }
        }

        private CTable _selectedTable;
        public CTable SelectedTable
        {
            get { return _selectedTable; }
            set
            {
                SetAndRaise(ref _selectedTable, value);
                if (_viewMode == eViewMode.Data && SelectedTable != null)
                {
                    OnSelectedTableChange();
                }
                _tableConsumers.ToList().ForEach(t => {
                    t.OnConnected(Configuration, Logger.Log);
                    t.OnTableUpdated(value);
                });
            }
        }

        private void OnSelectedTableChange()
        {
            IsBusy = true;
            Logger.Log.Verbose("OnSelectedTableChange({@name})", SelectedTable.name);

            _schedulerProvider.Background.Schedule(async () =>
            {
                try
                {
                    if (SelectedTable.DataTable == null)
                    {
                        SelectedTable.DataTable =
                        await _dataSourceProvider
                            .GetData(SelectedDatasource, SelectedTable.name,
                                _dataSourceProvider.QueryBuilder.GetQuery(SelectedTable, Settings.RowsPerPage));

                        SelectedTable.Pager = new CPager(SelectedTable.DataTable.RowsCount(), Settings.RowsPerPage);

                        if (SelectedTable.DataTable != null && SelectedTable.DataTable.Rows.Count > 0)
                            foreach (var child in SelectedTable.Children)
                            {
                                child.DataTable =
                                    await _dataSourceProvider
                                        .GetData(SelectedDatasource, child.name, 
                                            _dataSourceProvider.QueryBuilder.GetQuery(child, Settings.RowsPerPage, parent: SelectedTable));

                                if (child.requiresPagination)
                                    child.Pager = new CPager(child.DataTable.RowsCount(), Settings.RowsPerPage);
                            }
                    } else
                    {
                        SelectedTable.DataTable =
                            await _dataSourceProvider
                                .GetData(SelectedDatasource, SelectedTable.name, 
                                    _dataSourceProvider.QueryBuilder.GetQuery(SelectedTable, Settings.RowsPerPage, skip: SelectedTable.Pager.Skip));

                        var rows = SelectedTable.DataTable.RowsCount();
                        if (SelectedTable.Pager.RowsCount != rows)
                            SelectedTable.Pager.Reset(rows, Settings.RowsPerPage);

                        if (SelectedTable.DataTable != null && SelectedTable.DataTable.Rows.Count > 0)
                            foreach (var child in SelectedTable.Children)
                            {
                                child.DataTable =
                                    await _dataSourceProvider
                                        .GetData(SelectedDatasource, child.name, 
                                           _dataSourceProvider.QueryBuilder.GetQuery(child, Settings.RowsPerPage, parent: SelectedTable));

                                if (child.requiresPagination)
                                    child.Pager = new CPager(child.DataTable.RowsCount(), Settings.RowsPerPage);
                            }
                    }
                }
                catch (Exception e)
                {
                    Logger.Log.Error(e, "Exception while loading selected table {@name}", SelectedTable.name);
                    MessageQueue.Enqueue(e.Message);
                }
                finally
                {
                    _schedulerProvider.MainThread.Schedule(() => IsBusy = false);
                }
            });
        }

        private void OnChildTableChange(CTable table)
        {
            IsBusy = true;
            Logger.Log.Verbose("OnChildTableChange({@name})", table.name);

            _schedulerProvider.Background.Schedule(async () =>
            {
                try
                {
                    table.DataTable =
                            await _dataSourceProvider
                                .GetData(SelectedDatasource, table.name, 
                                   _dataSourceProvider.QueryBuilder.GetQuery(table, Settings.RowsPerPage, skip: table.Pager.Skip));

                    var rows = table.DataTable.RowsCount();
                    if (table.Pager.RowsCount != rows)
                        table.Pager.Reset(rows, Settings.RowsPerPage);
                }
                catch (Exception e)
                {
                    Logger.Log.Error(e, "Exception while loading child table {@name}", table.name);
                    MessageQueue.Enqueue(e.Message);
                }
                finally
                {
                    _schedulerProvider.MainThread.Schedule(() => IsBusy = false);
                }
            });
        }

        private CTable _selectedChildTable;
        public CTable SelectedChildTable
        {
            get { return _selectedChildTable; }
            set
            {
                SetAndRaise(ref _selectedChildTable, value);
            }
        }

        private DataRowView _selectedRow;
        public DataRowView SelectedRow
        {
            get { return _selectedRow; }
            set
            {
                SetAndRaise(ref _selectedRow, value);
                if (SelectedTable != null)
                    SelectedTable.SelectedRow = value;

                if (_selectedRow != null && _selectedRow.Row != null)
                    Logger.Log.Verbose("SelectedRow [{@tableName}]", _selectedRow.Row.Table.TableName);

                if (_selectedRow != null
                    && _viewMode == eViewMode.Data
                    && SelectedTable != null && SelectedTable.DataTable != null && SelectedTable.DataTable.Rows.Count > 0)
                {
                    IsBusy = true;

                    _schedulerProvider.Background.Schedule(() =>
                    {
                        try
                        {
                            SelectedTable.Children.ToList().ForEach(async (child) =>
                            {
                                var filter = SelectedTable.Relationship
                                    .Where(r => r.toTable.Equals(child.name))
                                    .SelectMany(r => r.ColumnRelationship)
                                    .Aggregate("",
                                        (seed, cr) => seed + $" AND {cr.toColumn.UnQuoteName()} = {_selectedRow.Row.Value(cr.fromColumn.UnQuoteName())}");
                                filter = filter.Length > 5 ? filter.Substring(5) : filter;

                                Logger.Log.Verbose("Set filter {@filter} on child table {@name}", filter, child.name);
                                if (child.DataTable != null)
                                {
                                    var rows = child.DataTable.Select(filter);
                                    if (!rows.Any())
                                    {
                                        var newData = await _dataSourceProvider
                                                    .GetData(SelectedDatasource, child.name,
                                                        _dataSourceProvider.QueryBuilder.GetQuery(child, Settings.RowsPerPage, parent: SelectedTable, row: _selectedRow.Row));

                                        foreach (DataRow row in newData.Rows)
                                            child.DataTable.Rows.Add(row.ItemArray);
                                    }

                                    _schedulerProvider.MainThread.Schedule(() =>
                                        child.DataTable.DefaultView.RowFilter = filter);
                                }
                            });
                        }
                        catch (Exception e)
                        {
                            Logger.Log.Error(e, "Exception while loading children for table {@name}", SelectedTable.name);
                            MessageQueue.Enqueue(e.Message);
                        }
                        finally
                        {
                            _schedulerProvider.MainThread.Schedule(() => IsBusy = false);
                        }
                    });
                }
            }
        }    
 
        private CTable _selectedTargetTable;
        public CTable SelectedTargetTable
        {
            get { return _selectedTargetTable; }
            set { SetAndRaise(ref _selectedTargetTable, value); }
        }

        private CColumn _selectedColumn;
        public CColumn SelectedColumn
        {
            get { return _selectedColumn; }
            set { SetAndRaise(ref _selectedColumn, value); }
        }

        private CColumn _selectedTargetColumn;
        public CColumn SelectedTargetColumn
        {
            get { return _selectedTargetColumn; }
            set { SetAndRaise(ref _selectedTargetColumn, value); }
        }

        private eViewMode _viewMode = eViewMode.Data;
        public eViewMode ViewMode
        {
            get { return _viewMode; }
            set
            {
                SetAndRaise(ref _viewMode, value);
                OnPropertyChanged("SchemaVisibility");
                OnPropertyChanged("DataVisibility");
                OnPropertyChanged("IsDataMode");

                Logger.Log.Verbose("ViewMode [{@value}]", value);

                if (_viewMode == eViewMode.Data && SelectedTable != null && SelectedTable.DataTable == null)
                {
                    IsBusy = true;
                    _schedulerProvider.Background.Schedule(async () =>
                    {
                        try
                        {
                            SelectedTable.DataTable =
                                await _dataSourceProvider
                                    .GetData(SelectedDatasource, SelectedTable.name,
                                        _dataSourceProvider.QueryBuilder.GetQuery(SelectedTable, Settings.RowsPerPage));

                            SelectedTable.Pager = new CPager(SelectedTable.DataTable.RowsCount(), Settings.RowsPerPage);

                            if (SelectedTable.DataTable != null && SelectedTable.DataTable.Rows.Count > 0)
                                SelectedTable.Children.ToList().ForEach(async (child) =>
                                {
                                    child.DataTable =
                                        await _dataSourceProvider
                                            .GetData(SelectedDatasource, child.name, 
                                                _dataSourceProvider.QueryBuilder.GetQuery(child, Settings.RowsPerPage, parent: SelectedTable));
                                });
                        }
                        catch (Exception e)
                        {
                            Logger.Log.Error(e, "Exception while switching view mode {@viewMode} for table {@name}", _viewMode, SelectedTable.name);
                            MessageQueue.Enqueue(e.Message);
                        }
                        finally
                        {
                            _schedulerProvider.MainThread.Schedule(() => IsBusy = false);
                        }
                    });
                }
            }
        }

        public IEnumerable<eViewMode> ViewModes
        {
            get { return Enum.GetValues(typeof(eViewMode)).Cast<eViewMode>(); }
        }

        public IEnumerable<eDbType> DbTypes
        {
            get { return Enum.GetValues(typeof(eDbType)).Cast<eDbType>(); }
        }

        public bool IsDataMode
        {
            get { return ViewMode.Equals(eViewMode.Data); }
            set
            {
                ViewMode = ViewMode.Equals(eViewMode.Data) ? eViewMode.Schema : eViewMode.Data;
            }
        }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                SetAndRaise(ref _isBusy, value);
            }
        }

        private string _applicationErrors;
        public string ApplicationErrors
        {
            get { return _applicationErrors; }
            set
            {
                SetAndRaise(ref _applicationErrors, value);
            }
        }

        public Visibility SchemaVisibility
        {
            get { return ViewMode == eViewMode.Schema ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility DataVisibility
        {
            get { return ViewMode == eViewMode.Data ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility GoHomeVisibility
        {
            get { return _navigationStack.Count > 0 ? Visibility.Visible : Visibility.Collapsed; }
        }

        public IEnumerable<Actionable<CDataset>> DatasetMenuActions
        {
            get
            {
                return _dataSetConsumers.SelectMany(a => a.Actions);
            }
        }
        public Visibility DatasetActionsMenuVisibility
        {
            get { return _dataSetConsumers.Any() ? Visibility.Visible : Visibility.Collapsed; }
        }

        public IEnumerable<Actionable<CTable>> TableMenuActions
        {
            get
            {
                return _tableConsumers.SelectMany(a => a.Actions);
            }
        }
        public Visibility TableActionsMenuVisibility
        {
            get { return _tableConsumers.Any() ? Visibility.Visible : Visibility.Collapsed; }
        }

        public SolidColorBrush AlternatingRowBackground
        {
            get { return new SolidColorBrush(Settings.SelectedPrimaryColor.PrimaryHues.First().Color); }
        }

        public LogEventLevel LogLevel
        {
            get { return Settings.LogLevel; }
            set
            {
                Settings.LogLevel = value;
                Logger.SetLevel(value);
            }
        }

        private IEnumerable<LogEventLevel> _logLevels;
        public IEnumerable<LogEventLevel> LogLevels
        {
            get
            {
                if(_logLevels == null)
                {
                    _logLevels = Enum.GetValues(typeof(LogEventLevel)).Cast<LogEventLevel>();
                }                

                return _logLevels;
            }
        }

        private void AppendChildren(CTable table, CDataset dataset)
        {
            table.Children = new ObservableCollection<CTable>(
                from r in dataset.Relationship
                where r.fromTable == table.name
                select dataset.Table.First(x => x.name == r.toTable)
                );
        }

        private CConfiguration 
            InitConfiguration(CConfiguration config)
        {
            foreach (var ds in config.Dataset)
            {
                foreach (var table in ds.Table)
                {
                    AppendChildren(table, ds);
                    table.Relationship = new ObservableCollection<CRelationship>
                    {
                        ds.Relationship.Where(r => table.name.Equals(r.fromTable))
                    };
                }
            }

            return config;
        }

        public ICommand NotificationCommand => new AnotherCommandImplementation(ExecNotificationDialog);
        private async void ExecNotificationDialog(object o)
        {
            var view = new NotificationDialog
            {
                DataContext = o
            };

            await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
        }

        public ICommand NewDatasourceCommand => new AnotherCommandImplementation(ExecDatasourceDialog);
        private async void ExecDatasourceDialog(object o)
        {
            if (IsBusy) return;

            if (o == null)
            {
                var providers = _dataSourceProviders.Select(p => p.Name);
                var defaultProvider = _dataSourceProviders.FirstOrDefault(p => p.IsDefault).Name;
                var view = new DatasourceDialog
                {
                    DataContext = new CDatasource {
                        Providers = _dataSourceProviders,
                        providerName = providers.FirstOrDefault(p => p.Equals(defaultProvider))
                    }
                };
                o = new object[] { view.DataContext, false };

                (o as object[])[1] = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
            }

            if ((bool)(o as object[])[1])
            {
                var ds = (o as object[])[0] as CDatasource;
                LoadAndSaveConfiguration(ds);
            }
        }

        private void LoadAndSaveConfiguration(CDatasource ds)
        {
            IsBusy = true;
            _schedulerProvider.Background.Schedule(async () =>
            {
                try
                {
                    _dataSourceProvider = _dataSourceProviders.FirstOrDefault(p => p.Name.Equals(ds.providerName));
                    _dataSourceProvider.SetLog(Logger.Log);

                    var schema = await _dataSourceProvider.GetSchema(ds);
                    if (!string.IsNullOrEmpty(schema))
                    {
                        var builder = XmlBuilder.BuildElements(ds, schema);
                        if (null != builder)
                        {
                            var config = XmlHelper<CConfiguration>.LoadFromString(builder.ToString());
                            if (!File.Exists(DefaultConfigFile))
                            {
                                //fix relationship names
                                foreach (var dset in config.Dataset)
                                    foreach (var rel in dset.Relationship)
                                        rel.name = rel.GetName();

                                XmlHelper<CConfiguration>.Save(DefaultConfigFile, config);

                                Configuration = Configuration ?? InitConfiguration(config);
                                SetDataset.Execute(Configuration.Dataset.FirstOrDefault());
                                IsEmpty = false;

                                var queryConfig = await LoadAndSaveQueries(ds, SelectedDataset.name);

                                _schedulerProvider.MainThread.Schedule(() =>
                                {
                                    SelectedDataset.Query.AddRange(queryConfig.Dataset.FirstOrDefault().Query);
                                    SelectedQuery = SelectedDataset.Query.FirstOrDefault(q => q.isStoreProcedure);
                                    SelectedScriptQuery = SelectedDataset.Query.LastOrDefault(q => q.isScript);
                                });
                            }
                            else
                            {
                                if (Configuration.Dataset.Any(cds => cds.name.Equals(config.defaultDataset)))
                                {
                                    for (var i = 1; i < 10000; i++)
                                        if (!Configuration.Dataset.Any(cds => cds.name.Equals(config.defaultDataset + i)))
                                        {
                                            var newName = config.Dataset.FirstOrDefault().name + i;
                                            config.Dataset.FirstOrDefault().name = newName;
                                            config.Dataset.FirstOrDefault().dataSourceName = newName;
                                            config.Datasource.FirstOrDefault().name = newName;
                                            break;
                                        }
                                }

                                _schedulerProvider.MainThread.Schedule(() =>
                                {
                                    Configuration.Datasource.Add(config.Datasource.FirstOrDefault());
                                    Configuration.Dataset.Add(config.Dataset.FirstOrDefault());
                                    foreach (var dset in Configuration.Dataset)
                                        dset.Query.Clear();

                                    XmlHelper<CConfiguration>.Save(DefaultConfigFile, Configuration);

                                    _dataSetConsumers.ToList().ForEach(c => c.OnConnected(Configuration, Logger.Log));
                                });

                                var queryConfig = await LoadAndSaveQueries(ds, config.Dataset.FirstOrDefault().name);
                                _schedulerProvider.MainThread.Schedule(() => {
                                    foreach (var dset in queryConfig.Dataset)
                                        Configuration.Dataset.FirstOrDefault(d => d.name.Equals(dset.name))
                                            .Query.AddRange(dset.Query);
                                });
                            }
                        }
                    }
                } catch(Exception e)
                {
                    Logger.Log.Error(e, "Exception while loading and saving configuration for {0}", ds.ConnectionString.SecureString());
                    MessageQueue.Enqueue(e.Message);
                }
                finally
                {
                    _schedulerProvider.MainThread.Schedule(() => IsBusy = false);
                }
            });
        }

        private async Task<CConfiguration> LoadAndSaveQueries(CDatasource ds, string datasetName)
        {
            var schema = await _dataSourceProvider.GetStoreProcsSchema(ds);
            if (!string.IsNullOrEmpty(schema))
            {
                var builder = XmlBuilder.BuildQueryElements(ds, schema);
                if (null != builder)
                {
                    var config = XmlHelper<CConfiguration>.LoadFromString(builder.ToString());

                    if (!File.Exists(DefaultStoreProcsConfigFile))
                    {
                        XmlHelper<CConfiguration>.Save(DefaultStoreProcsConfigFile, config);
                    }
                    else
                    {
                        config.Dataset.FirstOrDefault().dataSourceName = datasetName;
                        config.Dataset.FirstOrDefault().name = datasetName;
                        config.Datasource.FirstOrDefault().name = datasetName;

                        var currentConfig = XmlHelper<CConfiguration>.Load(DefaultStoreProcsConfigFile);
                        currentConfig.Datasource.Add(config.Datasource.FirstOrDefault());
                        currentConfig.Dataset.Add(config.Dataset.FirstOrDefault());

                        XmlHelper<CConfiguration>.Save(DefaultStoreProcsConfigFile, currentConfig);

                        return currentConfig;
                    }

                    return config;
                }
            }

            return new CConfiguration();
        }

        private void ReLoadAndSaveConfiguration(CDataset dataSet)
        {
            IsBusy = true;
            _schedulerProvider.Background.Schedule(async () =>
            {
                try
                {
                    var ds = Configuration.Datasource.FirstOrDefault(dsrc => dsrc.name.Equals(dataSet.dataSourceName));

                    var schema = await _dataSourceProvider.GetSchema(ds);
                    if (!string.IsNullOrEmpty(schema))
                    {
                        var builder = XmlBuilder.BuildElements(ds, schema);
                        if (null != builder)
                        {
                            var config = InitConfiguration(XmlHelper<CConfiguration>.LoadFromString(builder.ToString()));
                            _schedulerProvider.MainThread.Schedule(() =>
                            {
                                dataSet.Table.Clear();
                                dataSet.Table.AddRange(config.Dataset.FirstOrDefault().Table);

                                //fix relationship names
                                foreach (var dset in config.Dataset)
                                    foreach (var rel in dset.Relationship)
                                        rel.name = rel.GetName();

                                var tables = dataSet.Table.Select(tbl => tbl.name);
                                var invalidRels = from r in dataSet.Relationship
                                                  where !tables.Contains(r.fromTable)
                                                    || !tables.Contains(r.toTable)
                                                  select r;

                                foreach (var invRel in invalidRels)
                                {
                                    var relToRem = dataSet.Relationship.FirstOrDefault(r => r.name.Equals(invRel.name));
                                    dataSet.Relationship.Remove(relToRem);
                                }

                                foreach (var dset in Configuration.Dataset)
                                    dset.Query.Clear();

                                XmlHelper<CConfiguration>.Save(DefaultConfigFile, Configuration);
                                SelectedTable = dataSet.Table.FirstOrDefault(t => t.name.Equals(dataSet.defaultTable));
                            });

                            var queryConfig = await ReLoadAndSaveQueries(dataSet);
                            _schedulerProvider.MainThread.Schedule(() =>
                            {
                                foreach (var dset in queryConfig.Dataset)
                                    Configuration.Dataset.FirstOrDefault(d => d.name.Equals(dset.name))
                                        .Query.AddRange(dset.Query);

                                SelectedQuery = dataSet.Query.FirstOrDefault(q => q.isStoreProcedure);
                                SelectedScriptQuery = dataSet.Query.LastOrDefault(q => q.isScript);
                            });                            
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Log.Error(e, "Exception while reloading and saving configuration for {0}", dataSet.name);
                    MessageQueue.Enqueue(e.Message);
                }
                finally
                {
                    _schedulerProvider.MainThread.Schedule(() => IsBusy = false);
                }
            });
        }

        private async Task<CConfiguration> ReLoadAndSaveQueries(CDataset dataSet)
        {
            var ds = Configuration.Datasource.FirstOrDefault(dsrc => dsrc.name.Equals(dataSet.dataSourceName));

            var schema = await _dataSourceProvider.GetStoreProcsSchema(ds);
            if (!string.IsNullOrEmpty(schema))
            {
                var builder = XmlBuilder.BuildQueryElements(ds, schema);
                if (null != builder)
                {
                    var config = XmlHelper<CConfiguration>.LoadFromString(builder.ToString());
                    var currentConfig = XmlHelper<CConfiguration>.Load(DefaultStoreProcsConfigFile);

                    _schedulerProvider.MainThread.Schedule(() =>
                    {
                        var currentDataset = currentConfig.Dataset.FirstOrDefault(d => d.name.Equals(dataSet.name));
                        var scripts = currentDataset.Query.Where(q => q.isScript).ToList();
                        currentDataset.Query.Clear();
                        currentDataset.Query.AddRange(config.Dataset.FirstOrDefault().Query);
                        currentDataset.Query.AddRange(scripts);

                        XmlHelper<CConfiguration>.Save(DefaultStoreProcsConfigFile, currentConfig);
                    });                    

                    return currentConfig;
                }
            }

            return new CConfiguration();
        }

        private void ReLoadAndSaveTableConfiguration(CDataset dataSet, CTable table)
        {
            IsBusy = true;
            _schedulerProvider.Background.Schedule(async () =>
            {
                try
                {
                    var ds = Configuration.Datasource.FirstOrDefault(dsrc => dsrc.name.Equals(dataSet.dataSourceName));

                    var schema = await _dataSourceProvider.GetSchema(ds, table.name.UnQuoteName());
                    if (!string.IsNullOrEmpty(schema))
                    {
                        var builder = XmlBuilder.BuildElements(ds, schema);
                        if (null != builder)
                        {
                            var config = XmlHelper<CConfiguration>.LoadFromString(builder.ToString());
                            _schedulerProvider.MainThread.Schedule(() =>
                            {
                                var currentTable = dataSet.Table.FirstOrDefault(t => t.name.Equals(table.name));
                                dataSet.Table.Remove(currentTable);
                                var newTable = config.Dataset.FirstOrDefault().Table.FirstOrDefault();
                                dataSet.Table.Add(newTable);

                                foreach (var dset in Configuration.Dataset)
                                    dset.Query.Clear();

                                XmlHelper<CConfiguration>.Save(DefaultConfigFile, Configuration);

                                AppendChildren(newTable, dataSet);
                                newTable.Relationship = new ObservableCollection<CRelationship>
                                {
                                    dataSet.Relationship.Where(r => newTable.name.Equals(r.fromTable))
                                };

                                var currentConfig = XmlHelper<CConfiguration>.Load(DefaultStoreProcsConfigFile);
                                foreach (var dset in currentConfig.Dataset)
                                    Configuration.Dataset.FirstOrDefault(d => d.name.Equals(dset.name))
                                        .Query.AddRange(dset.Query);

                                SelectedTable = newTable;
                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Log.Error(e, "Exception while reloading and saving configuration for table {0} at data set {1}", table.name, dataSet.name);
                    MessageQueue.Enqueue(e.Message);
                }
                finally
                {
                    _schedulerProvider.MainThread.Schedule(() => IsBusy = false);
                }
            });
        }

        public bool HasRelationshipChanges
        {
            get { return _newRelationships.Any() || _removedRelationships.Any(); }
        }

        private IList<CRelationship> _newRelationships = new List<CRelationship>();
        private IList<CRelationship> _removedRelationships = new List<CRelationship>();
        public ICommand SetRelationsCommand => new AnotherCommandImplementation(ExecSetRelationsDialog);
        private async void ExecSetRelationsDialog(object o)
        {
            ApplicationErrors = string.Empty;
            _removedRelationships.Clear();
            _newRelationships.Clear();

            var view = new RelationsGridDialog
            {
                DataContext = this
            };

            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
            if ((bool)result)
            {
                if (_newRelationships.Any() || _removedRelationships.Any())
                {
                    try
                    {
                        var config = XmlHelper<CConfiguration>.Load(DefaultConfigFile);

                        var ds = config.Dataset.FirstOrDefault(d => d.name.Equals(SelectedDataset.name));
                        foreach (var rel in _removedRelationships)
                            ds.Relationship.Remove(ds.Relationship.FirstOrDefault(r => r.name.Equals(rel.name)));
                        if(_newRelationships.Any())
                            ds.Relationship.AddRange(_newRelationships);

                        XmlHelper<CConfiguration>.Save(DefaultConfigFile, config);
                    }
                    catch (Exception e)
                    {
                        Logger.Log.Error(e, "Exception while saving relationships to file {0}", DefaultConfigFile);
                        MessageQueue.Enqueue(e.Message);
                    }
                }

                if (_newRelationships.Any())
                {
                    IsBusy = true;
                    
                    if (_viewMode == eViewMode.Data 
                        && SelectedTable != null && SelectedTable.DataTable != null && SelectedTable.DataTable.Rows.Count > 0)
                    {                       
                        _schedulerProvider.Background.Schedule(() =>
                        {
                            try
                            {
                                _newRelationships.ToList().ForEach(async (r) =>
                                {
                                    var child = SelectedTable.Children.FirstOrDefault(t => t.name.Equals(r.toTable));
                                    if (child != null)
                                        child.DataTable =
                                                await _dataSourceProvider
                                                    .GetData(SelectedDatasource, child.name, 
                                                        _dataSourceProvider.QueryBuilder.GetQuery(child, Settings.RowsPerPage, parent: SelectedTable));
                                });

                                _newRelationships.Clear();
                            }
                            catch (Exception e)
                            {
                                Logger.Log.Error(e, "Exception while loading new relationship for table {@name}"
                        , SelectedTable.name);
                                MessageQueue.Enqueue(e.Message);
                            }
                            finally
                            {
                                _schedulerProvider.MainThread.Schedule(() =>
                                {
                                    IsBusy = false;
                                    OnPropertyChanged("SelectedTable");
                                });
                            }
                        });
                    } else
                    {
                        _newRelationships.ToList().ForEach((r) =>
                        {
                            var child = SelectedTable.Children.FirstOrDefault(t => t.name.Equals(r.toTable));
                            if (child != null)
                                child.DataTable = child.ToDataTable();
                        });

                        OnPropertyChanged("SelectedTable");
                        _newRelationships.Clear();
                        IsBusy = false;
                    }
                }
            }
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
        }
    }
}
