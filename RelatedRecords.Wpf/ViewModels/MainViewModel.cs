using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using RelatedRecords.Wpf.Annotations;
using System.Configuration;
using Common;
using System.Windows;
using System.Collections.ObjectModel;
using System.IO;
using Serilog;

namespace RelatedRecords.Wpf.ViewModels
{
    public partial class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            Logger = new LoggerConfiguration()
                .WriteTo.File(Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, 
                    GetType().ToString() + ".log"))
                .CreateLogger();

            var configuration = ConfigurationManager.AppSettings["ConfigurationFile"];
            SelectedConfiguration = XmlHelper<CConfiguration>.Load(configuration);
        }

        private static MainViewModel _instance = new MainViewModel();
        public static MainViewModel ViewModel {
            get { return _instance; }
        }

        #region Events

        public delegate void QueryingTableCompleted(object sender, EventArgs e);
        public event QueryingTableCompleted OnQueryingTableCompleted;

        public delegate void Connect(object sender, EventArgs e);
        public event Connect OnConnect;

        public delegate void ProgressNotify(string message);
        public event ProgressNotify OnProgressNotify;

        public delegate void ExportCompleted(string fileName);
        public event ExportCompleted OnExportCompleted;

        private Action<int> onProgress;
        private Action<string> onNotify;

        #endregion //Events

        private ILogger Logger;

        public CConfiguration SelectedConfiguration
        {
            get { return Extensions.SelectedConfiguration; }
            set
            {
                Extensions.SelectedConfiguration = value;
                OnPropertyChanged();
            }
        }

        public CDataset SelectedDataset
        {
            get { return Extensions.SelectedDataset; }
            set
            {
                Extensions.SelectedDataset = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<DatatableEx> _dataTablesList;
        public ObservableCollection<DatatableEx> DataTablesList
        {
            get
            {
                if(null == _dataTablesList)
                {
                    _dataTablesList = new ObservableCollection<DatatableEx>(
                        from ds in SelectedConfiguration.Dataset
                        select ds.Table.First().Query("".ToArray(""), "".ToArray(""), true)
                    );

                    OnPropertyChanged();
                    SelectedDataTable = _dataTablesList.First();
                }

                return _dataTablesList;
            }
        }

        private DatatableEx _selectedDataTable;
        public DatatableEx SelectedDataTable
        {
            get { return _selectedDataTable; }
            set
            {
                _selectedDataTable = value;
                OnPropertyChanged();
                SelectedRootDataView = _selectedDataTable.Root.Table.AsDataView();
                SelectedRootDataRowView = SelectedRootDataView[0];
                OnPropertyChanged("ParentVisibility");
                _goBackCommand.RaiseCanExecuteChanged();
            }
        }

        private DataRowView _selectedRootDataRowView;
        public DataRowView SelectedRootDataRowView
        {
            get { return _selectedRootDataRowView; }
            set
            {
                _selectedRootDataRowView = value;
                OnPropertyChanged();

                if (null != _selectedRootDataRowView)
                {
                    SelectedDataTable.QueryChildren(_selectedRootDataRowView.Row);
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

        private Stack<DatatableEx> _tableNavigation = new Stack<DatatableEx>();
        public Stack<DatatableEx> TableNavigation
        {
            get { return _tableNavigation; }
        }

        public Visibility ParentVisibility
        {
            get { return TableNavigation.Count > 0 ? Visibility.Visible : Visibility.Collapsed;  }
        }

        public string CacheFile { get; set; }

        private int _loadProgress = 0;
        public int LoadProgress
        {
            get { return _loadProgress; }
            private set
            {
                //if (loadProgress == value) return;
                _loadProgress = value;
                OnPropertyChanged("LoadProgress");

                if (null != OnProgressNotify)
                    OnProgressNotify(string.Format("{0} %", _loadProgress));
            }
        }

        private void OnReportProgress(int progressPercentage)
        {
            this.LoadProgress = progressPercentage;
        }

        private ObservableCollection<string> _lastErrors = new ObservableCollection<string>();
        public ObservableCollection<string> LastErrors
        {
            get { return _lastErrors; }
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

        public Visibility SprocketVisibility
        {
            get { return (this.IsBusy ? Visibility.Visible : Visibility.Collapsed); }
        }

        public Visibility LastErrorsVisibility
        {
            get { return (LastErrors.Count > 0 ? Visibility.Visible : Visibility.Collapsed); }
        }
        
        private bool _loaded;
        public void Load()
        {
            if (_loaded) return;

            //Helpers.CreateSampleTables(SelectedConfiguration);
            if (null == SelectedDataTable)
            {
                SelectedDataTable = SelectedDataset.Table.First()
                    .Query("".ToArray(""), "".ToArray(""), true);
            }
            _loaded = true;
        }

        #region property changed handler

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion property changed handler
    }
}
