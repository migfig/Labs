using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Configuration;
using Common;
using System.Windows;
using System.Collections.ObjectModel;
using Serilog;
using System.Windows.Controls;
using Common.Commands;

namespace RelatedRecords.Wpf.ViewModels
{
    public partial class MainViewModel : BaseModel
    {
        public MainViewModel()
        {
            loadAndSetConfiguration();
        }

        private void loadAndSetConfiguration(string cfgFile = "")
        {
            _tableNavigation.Clear();
            _dataTablesList.Clear();

            if(string.IsNullOrEmpty(cfgFile))
                cfgFile = ConfigurationManager.AppSettings["ConfigurationFile"];
            TraceLog.Information("Loading {cfgFile} file", cfgFile);

            var cfg = XmlHelper<CConfiguration>.Load(cfgFile);
            cfg.Deflate();
            IsBusy = true;
            SelectedConfiguration = cfg;
        }

        private void saveAndReloadConfiguration()
        {
            loadAndSetConfiguration(saveConfiguration());
        }

        private string saveConfiguration()
        {
            var cfgFile = ConfigurationManager.AppSettings["ConfigurationFile"];
            TraceLog.Information("Saving {cfgFile} file", cfgFile);

            SelectedConfiguration.Inflate();
            XmlHelper<CConfiguration>.Save(cfgFile, SelectedConfiguration);
            SelectedConfiguration.Deflate();

            return cfgFile;
        }

        private static MainViewModel _instance = new MainViewModel();
        public static MainViewModel ViewModel {
            get { return _instance; }
        }
        
        public ILogger TraceLog
        {
            get { return Common.Extensions.TraceLog; }
        }

        public ILogger ErrorLog
        {
            get { return Common.Extensions.ErrorLog; }
        }

        public CConfiguration SelectedConfiguration
        {
            get { return Extensions.SelectedConfiguration; }
            set
            {
                Extensions.SelectedConfiguration = value;
                OnPropertyChanged();
                Extensions.MaxRowCount = MaxRowCount;
            }
        }

        public CDataset SelectedDataset
        {
            get { return Extensions.SelectedDataset; }
            set
            {
                _dataTablesList.Clear();
                
                if (null != Extensions.SelectedDataset)
                    Extensions.SelectedDataset.isSelected = false;

                Extensions.SelectedDataset = value;
                if (null != value)
                {
                    Extensions.SelectedDataset.isSelected = true;
                    Extensions.SelectedDataset.isDefault = value.name == Extensions.SelectedConfiguration.defaultDataset;
                    LoadTableList();
                }
                OnPropertyChanged();
            }
        }

        public CDatasource SelectedDatasource
        {
            get { return Extensions.SelectedDatasource; }
            set
            {
                Extensions.SelectedDatasource = value;
                OnPropertyChanged();
                SelectedDatasourceFilter = value;
            }
        }

        private CDatasource _selectedDatasourceFilter;
        public CDatasource SelectedDatasourceFilter {
            get { return _selectedDatasourceFilter; }
            set
            {
                _selectedDatasourceFilter = value;
                OnPropertyChanged();
                if(null != _selectedDatasourceFilter)
                    SelectedConnectionString = _selectedDatasourceFilter.ConnectionString;
            }
        }

        private ObservableCollection<DatatableEx> _dataTablesList = new ObservableCollection<DatatableEx>();
        private void LoadTableList()
        {
            DatatableEx defaultTable = _dataTablesList
                .FirstOrDefault(x => x.Root.ConfigTable.name == SelectedDataset.defaultTable);

            if (defaultTable != null)
            {
                SelectedDataTable = defaultTable;
            }
            else 
            {
                Common.Extensions.TraceLog.Information("Loading Data Tables @ LoadTableList");

                IsBusy = true;
                
                var action = new Action(async () =>
                {
                    var table = await SelectedDataset.Table.First(x => x.name == SelectedDataset.defaultTable)
                        .Query("".ToArray(""),
                            "".ToArray(""),
                            true);
                        _dataTablesList.Add(table);

                    IsBusy = false;
                    SelectedDataTable = _dataTablesList.First(x => x.Root.ConfigTable.name == SelectedDataset.defaultTable);
                });
                action.Invoke();
            }
        }

        private DatatableEx _selectedDataTable;
        public DatatableEx SelectedDataTable
        {
            get { return _selectedDataTable; }
            set
            {
                if (_selectedDataTable != value)
                {
                    _selectedDataTable = value;
                    SelectedRootDataView = _selectedDataTable.Root.Table.AsDataView();
                    if (_selectedDataTable.Root.Table.Rows.Count > 0)
                    {
                        SelectedRootDataRowView = SelectedRootDataView[GetDataRowViewIndex()];
                    }

                    DefaultTable = _selectedDataTable.Root.ConfigTable;
                    OnPropertyChanged();
                    OnPropertyChanged("ParentVisibility");
                    OnPropertyChanged("SelectedDataTableColumns");
                    OnPropertyChanged("RemoveRelationShipVisibility");
                    GoBackCommand.AsRelay().RaiseCanExecuteChanged();
                    Export2HtmlCommand.AsRelay().RaiseCanExecuteChanged();
                    Export2SqlInsertCommand.AsRelay().RaiseCanExecuteChanged();
                    SetAsDefaultTableCommand.AsRelay().RaiseCanExecuteChanged();
                }
            }
        }

        private int GetDataRowViewIndex()
        {
            var filter = WorkingTable.Root.Table.DefaultView.RowFilter;
            if (!string.IsNullOrEmpty(filter))
            {
                var rows = WorkingTable.Root.Table.Select(filter);
                if(null != rows && rows.Any())
                {
                    for(var i=0; i< WorkingTable.Root.Table.Rows.Count; i++)
                    {
                        if(rows[0].Equals(WorkingTable.Root.Table.Rows[i]))
                            return i;
                    }
                }
            }

            return 0;
        }

        private DataRowView _selectedRootDataRowView;
        public DataRowView SelectedRootDataRowView
        {
            get { return _selectedRootDataRowView; }
            set
            {
                if (SelectedViewType == eViewType.Queries) return;

                if (null != value && !value.AreEqual(_selectedRootDataRowView))
                {
                    var loadChildren = _selectedRootDataRowView != null;
                    _selectedRootDataRowView = value;

                    if (loadChildren && null != _selectedRootDataRowView)
                    {
                        IsBusy = true;
                        WorkingTable.QueryChildren(_selectedRootDataRowView.Row);
                        IsBusy = false;
                    }
                    OnPropertyChanged();
                }
            }
        }

        private DataView _selectedRootDataView;
        public DataView SelectedRootDataView
        {
            get { return _selectedRootDataView; }
            set
            {
                _selectedRootDataView = value;
                OnPropertyChanged();
            }
        }

        private bool containsColumnName(CTable source, CTable target)
        {
            foreach (var c in source.Column)
                if (target.Column.Any(x => x.name == c.name))
                    return true;

            return false;
        }

        private string _queryText = string.Empty;
        public string QueryText
        {
            get { return _queryText; }
            set
            {
                _queryText = value;
                OnPropertyChanged();
                RunQueryCommand.AsRelay().RaiseCanExecuteChanged();
                SaveQueryCommand.AsRelay().RaiseCanExecuteChanged();
            }
        }

        private string _queryName = string.Empty;
        public string QueryName
        {
            get { return _queryName; }
            set
            {
                _queryName = value;
                OnPropertyChanged();
                SaveQueryCommand.AsRelay().RaiseCanExecuteChanged();
            }
        }

        public IEnumerable<CTable> AvailableTables
        {
            get
            {
                return (
                    from t in SelectedDataset.Table
                    where (!string.IsNullOrEmpty(_filterTable)
                               ? t.name.ToLower().Contains(_filterTable.ToLower()) : true)
                    select t
                        ).Distinct();
            }
        }

        public IEnumerable<CTable> NonYetRelatedTables
        {
            get
            {
                switch(SelectedTableFilter)
                {
                    case eAutoFilter.MatchingColumnNames:
                        return (
                            from t in SelectedDataset.Table
                            where t.name != SelectedParentTable.name
                                   && (!string.IsNullOrEmpty(_filterTable)
                                       ? t.name.ToLower().Contains(_filterTable.ToLower()) : true)
                                   && containsColumnName(t, SelectedParentTable)
                            select t
                                ).Distinct();
                    case eAutoFilter.TablesWithPrimaryKey:
                        return (
                            from t in SelectedDataset.Table
                            where t.name != SelectedParentTable.name
                                   && (!string.IsNullOrEmpty(_filterTable)
                                       ? t.name.ToLower().Contains(_filterTable.ToLower()) : true)
                                   && t.Column.Any(x => x.isPrimaryKey)
                            select t
                                ).Distinct();
                    default:
                        return (
                            from t in SelectedDataset.Table
                            where t.name != SelectedParentTable.name
                                   && (!string.IsNullOrEmpty(_filterTable)
                                       ? t.name.ToLower().Contains(_filterTable.ToLower()) : true)
                            select t
                                ).Distinct();
                }
            }
        }

        private string _filterTable;
        public string FilterTable {
            get { return _filterTable; }
            set {
                _filterTable = value;
                OnPropertyChanged();
                OnPropertyChanged("NonYetRelatedTables");
                OnPropertyChanged("AvailableTables");
            }
        }

        private CTable _selectedParentTable;
        public CTable SelectedParentTable
        {
            get { return _selectedParentTable; }
            set {
                _selectedParentTable = value;
                OnPropertyChanged();
                if (value == null) return;

                OnPropertyChanged("NonYetRelatedTables");
                SelectedParentColumn = value.Column.FirstOrDefault(x => x.isPrimaryKey);
            }
        }

        private CTable _selectedChildTable;
        public CTable SelectedChildTable
        {
            get { return _selectedChildTable; }
            set
            {
                _selectedChildTable = value;
                OnPropertyChanged();
                if (value == null) return;

                OnPropertyChanged("RemoveRelationShipVisibility");
                SelectedChildColumn = null != SelectedParentColumn && _selectedChildTable.Column.Any(x => x.name == SelectedParentColumn.name) 
                    ? _selectedChildTable.Column.First(x => x.name == SelectedParentColumn.name)
                    : value.Column.FirstOrDefault(x => x.isForeignKey);
            }
        }

        private CColumn _selectedParentColumn;
        public CColumn SelectedParentColumn
        {
            get { return _selectedParentColumn; }
            set
            {
                _selectedParentColumn = value;
                OnPropertyChanged();
                SaveRelationshipCommand.AsRelay().RaiseCanExecuteChanged();
            }
        }

        private CColumn _selectedChildColumn;
        public CColumn SelectedChildColumn
        {
            get { return _selectedChildColumn; }
            set
            {
                _selectedChildColumn = value;
                OnPropertyChanged();
                SaveRelationshipCommand.AsRelay().RaiseCanExecuteChanged();
            }
        }

        private string _selectedConnectionString;
        public string SelectedConnectionString
        {
            get { return _selectedConnectionString; }
            set
            {
                _selectedConnectionString = value;
                OnPropertyChanged();
            }
        }

        private CTable _selectedTable;
        public CTable SelectedTable
        {
            get { return _selectedTable; }
            set
            {
                _selectedTable = value;

                if (null != _selectedTable)
                {
                    IsBusy = true;

                    var action = new Action(async () =>
                    {
                        SelectedRootTable = await _selectedTable
                            .Query("".ToArray(""),
                                "".ToArray(""),
                                true);

                        IsBusy = false;
                    });
                    action.Invoke();
                }

                OnPropertyChanged();
            }
        }

        private CTable _defaultTable;
        public CTable DefaultTable
        {
            get { return _defaultTable; }
            set
            {
                _defaultTable = value;
                OnPropertyChanged();
                SetAsDefaultTableCommand.AsRelay().RaiseCanExecuteChanged();
            }
        }

        private DatatableEx _selectedRootTable;
        public DatatableEx SelectedRootTable
        {
            get { return _selectedRootTable; }
            set
            {
                _selectedRootTable = value;
                OnPropertyChanged();
                OnPropertyChanged("SelectedDataTableColumns");
                OnPropertyChanged("SelectedColumnOperators");
                SelectedRootDataView = _selectedRootTable.Root.Table.AsDataView();
                if (_selectedRootTable.Root.Table.Rows.Count > 0)
                {
                    SelectedRootDataRowView = SelectedRootDataView[GetDataRowViewIndex()];
                }
            }
        }

        private bool _isRefreshing = true;
        private bool _refreshingFromMainCommand = true;

        private CQuery _selectedQuery;
        public CQuery SelectedQuery
        {
            get { return _selectedQuery; }
            set
            {
                _selectedQuery = value;

                if (null != _selectedQuery)
                {
                    var result = true;
                    if(_selectedQuery.Parameter.Any() && _isRefreshing && _refreshingFromMainCommand)
                    {
                        result = new InputParameters().ShowDialog().Value;
                    }
                    if (result)
                    {
                        _isRefreshing = false;
                        IsBusy = true;

                        var action = new Action(async () =>
                        {
                            SelectedRootTable = await _selectedQuery.Query(_selectedQuery.ToParams());

                            IsBusy = false;
                        });
                        action.Invoke();
                    }
                }

                OnPropertyChanged();
                SaveQueryCommand.AsRelay().RaiseCanExecuteChanged();
            }
        }

        public string ClipboardText
        {
            get
            {
                if(!string.IsNullOrEmpty(SelectedSearchCriteria) && null != WorkingTable)
                    return "SELECT * FROM " +
                        WorkingTable.Root.ConfigTable.name +
                        " WHERE " + SelectedSearchCriteria + ";"; ;

                return string.Empty;
            }
        }

        private CConfiguration _selectedNewConfiguration;
        public CConfiguration SelectedNewConfiguration
        {
            get { return _selectedNewConfiguration; }
            set
            {
                _selectedNewConfiguration = value;
                OnPropertyChanged();
                OnPropertyChanged("SelectedNewConfigurationTables");
                SaveDatasourceSchemaCommand.AsRelay().RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<CTable> SelectedNewConfigurationTables
        {
            get
            {
                if(null != _selectedNewConfiguration)
                {
                    return new ObservableCollection<CTable>(
                        from t in _selectedNewConfiguration.Dataset.First().Table
                           select t);
                }

                return new ObservableCollection<CTable>();
            }
        }

        private Stack<DatatableEx> _tableNavigation = new Stack<DatatableEx>();
        public Stack<DatatableEx> TableNavigation
        {
            get { return _tableNavigation; }
        }

        public Visibility ParentVisibility
        {
            get { return TableNavigation.Count > 0 ? Visibility.Visible : Visibility.Collapsed;  }
        }
        
        private ObservableCollection<string> _lastErrors = new ObservableCollection<string>();
        public ObservableCollection<string> LastErrors
        {
            get { return _lastErrors; }
        }

        public string LastErrorsString
        {
            get { return string.Join(". ", LastErrors.ToArray()); }
        }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                OnPropertyChanged("IsBusy");
                OnPropertyChanged("SprocketVisibility");
            }
        }

        public Properties.Settings AppSettings
        {
            get { return Properties.Settings.Default; }
        }

        private TabItem _selectedTabItem;
        public TabItem SelectedTabItem
        {
            get { return _selectedTabItem; }
            set
            {
                _selectedTabItem = value;
                OnPropertyChanged();
            }
        }

        public bool IsDatasetsViewSelected
        {
            get { return SelectedViewType == eViewType.Datasets; }
        }

        public bool IsTablesViewSelected
        {
            get { return SelectedViewType == eViewType.Tables; }
        }

        public bool IsQueriesViewSelected
        {
            get { return SelectedViewType == eViewType.Queries; }
        }

        private eViewType _selectedViewType = eViewType.Datasets;
        public eViewType SelectedViewType
        {
            get { return _selectedViewType; }
            set
            {
                _selectedViewType = value;
                OnPropertyChanged();
                OnPropertyChanged("DatasetsVisibility");
                OnPropertyChanged("HiddenDatasetsVisibility");
                OnPropertyChanged("TablesVisibility");
                OnPropertyChanged("HiddenTablesVisibility");
                OnPropertyChanged("QueriesVisibility");
                OnPropertyChanged("HiddenQueriesVisibility");
                OnPropertyChanged("IsDatasetsViewSelected");
                OnPropertyChanged("IsTablesViewSelected");
                OnPropertyChanged("IsQueriesViewSelected");
                AddQueryCommand.AsRelay().RaiseCanExecuteChanged();
            }
        }

        public Visibility DatasetsVisibility
        {
            get { return SelectedViewType == eViewType.Datasets ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility TablesVisibility
        {
            get { return SelectedViewType == eViewType.Tables ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility QueriesVisibility
        {
            get { return SelectedViewType == eViewType.Queries ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility HiddenDatasetsVisibility
        {
            get { return SelectedViewType == eViewType.Datasets ? Visibility.Collapsed : Visibility.Visible; }
        }

        public Visibility HiddenTablesVisibility
        {
            get { return SelectedViewType == eViewType.Tables ? Visibility.Collapsed : Visibility.Visible; }
        }

        public Visibility HiddenQueriesVisibility
        {
            get { return SelectedViewType == eViewType.Queries ? Visibility.Collapsed : Visibility.Visible; }
        }

        public Visibility SprocketVisibility
        {
            get { return (this.IsBusy ? Visibility.Visible : Visibility.Collapsed); }
        }

        public Visibility LastErrorsVisibility
        {
            get { return (LastErrors.Count > 0 ? Visibility.Visible : Visibility.Collapsed); }
        }

        public IEnumerable<eAutoFilter> AutoFilterTables
        {
            get
            {
                yield return eAutoFilter.Everything;
                yield return eAutoFilter.MatchingColumnNames;
                yield return eAutoFilter.TablesWithPrimaryKey;
            }
        }

        private eAutoFilter _selectedTableFilter = eAutoFilter.Everything;
        public eAutoFilter SelectedTableFilter
        {
            get { return _selectedTableFilter; }
            set
            {
                _selectedTableFilter = value;
                OnPropertyChanged();
                OnPropertyChanged("NonYetRelatedTables");
            }
        }
    }
}
