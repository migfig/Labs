using Common;
using RelatedRecords.Parser;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RelatedRecords.Data.ViewModels
{
    public partial class MainViewModel: BaseModel
    {
        #region properties

        private static MainViewModel _viewModel;
        public static MainViewModel ViewModel
        {
            get
            {
                if(null == _viewModel)
                {
                    _viewModel = new MainViewModel(
                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configuration.xml"), 
                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "relatedrecords.cgt"));
                }

                return _viewModel;
            }
        }

        private readonly RRParser _parser;
        private readonly string _configurationFile;
        private IEnumerable<MethodInfo> _commandMethods;
        private readonly State _state;

        public MainViewModel(string configFile, string grammarFile)
        {
            _parser = new RRParser(grammarFile);
            _configurationFile = configFile;
            _commandMethods = GetType()
                .GetMethods()
                .Where(m => m.GetCustomAttribute<CommandAttribute>(false) != null);
            _state = new State(this);
            LoadConfiguration();
        }

        public string Title
        {
            get
            {
                var title = "Related Records";
                if (null != SelectedDataset)
                {
                    title += " - " + Common.Extensions.CapitalizeWords(SelectedDataset.name);
                }

                return title;
            }
        }
        
        public CConfiguration SelectedConfiguration
        {
            get { return Extensions.SelectedConfiguration; }
            set
            {
                Extensions.SelectedConfiguration = value;
                OnPropertyChanged();
                OnPropertyChanged("SelectedDataset");
            }
        }

        public CDataset SelectedDataset
        {
            get { return Extensions.SelectedDataset; }
            set
            {
                Extensions.SelectedDataset = value;
                OnPropertyChanged();
                OnPropertyChanged("SelectedDatasource");
                OnPropertyChanged("Title");
            }
        }

        public CDatasource SelectedDatasource
        {
            get { return Extensions.SelectedDatasource; }
            set
            {
                Extensions.SelectedDatasource = value;
                OnPropertyChanged();
            }
        }

        private DatatableEx _currentTable;
        public DatatableEx CurrentTable
        {
            get { return _currentTable; }
            set
            {
                _currentTable = value;
                OnPropertyChanged();
            }
        }

        private DataRowView _selectedRootDataRowView;
        public DataRowView SelectedRootDataRowView
        {
            get { return _selectedRootDataRowView; }
            set
            {
                if (null != value && !value.AreEqual(_selectedRootDataRowView))
                {
                    var loadChildren = _selectedRootDataRowView != null;
                    _selectedRootDataRowView = value;

                    if (loadChildren && null != _selectedRootDataRowView)
                    {
                        IsBusy = true;
                        CurrentTable.QueryChildren(_selectedRootDataRowView.Row);
                        IsBusy = false;
                    }
                    OnPropertyChanged();
                }
            }
        }

        private Stack<DatatableEx> _tableNavigation = new Stack<DatatableEx>();
        public Stack<DatatableEx> TableNavigation
        {
            get { return _tableNavigation; }
        }

        private bool _isValidCommand;
        public bool IsValidCommand
        {
            get { return _isValidCommand; }
            set
            {
                _isValidCommand = value;
                OnPropertyChanged();
            }
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
            }
        }

        public DataGridSelectionMode SelectionMode
        {
            get
            {
                return (DataGridSelectionMode)Enum.Parse(typeof(DataGridSelectionMode), 
                    Properties.Settings.Default.DefaultSelectionMode);
            }
            set
            {
                if (value.ToString() == Properties.Settings.Default.DefaultSelectionMode) return;
                Properties.Settings.Default.DefaultSelectionMode = value.ToString();
                Properties.Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        public List<DataGridSelectionMode> SelectionModes
        {
            get
            {
                var list = new List<DataGridSelectionMode>();
                foreach (var item in Properties.Settings.Default.SelectionModes)
                    list.Add((DataGridSelectionMode)Enum.Parse(typeof(DataGridSelectionMode), item));

                return list;
            }
        }

        public LogEventLevel LogLevel
        {
            get { return Common.Extensions.LogLevel; }
            set
            {
                Common.Extensions.LogLevel = value;
                OnPropertyChanged();
            }
        }

        public List<LogEventLevel> LogLevels
        {
            get { return Common.Extensions.LogLevels; }
        }

        public DataGridClipboardCopyMode CopyMode
        {
            get
            {
                return (DataGridClipboardCopyMode)Enum.Parse(typeof(DataGridClipboardCopyMode), 
                    Properties.Settings.Default.DefaultCopyMode);
            }
            set
            {
                if (value.ToString() == Properties.Settings.Default.DefaultCopyMode) return;
                Properties.Settings.Default.DefaultCopyMode = value.ToString();
                Properties.Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        public List<DataGridClipboardCopyMode> CopyModes
        {
            get
            {
                var list = new List<DataGridClipboardCopyMode>();
                foreach (var item in Properties.Settings.Default.CopyModes)
                    list.Add((DataGridClipboardCopyMode)Enum.Parse(typeof(DataGridClipboardCopyMode), item));

                return list;
            }
        }

        #endregion properties

        #region methods

        public bool LoadConfiguration()
        {
            var isStateSaved = _state.SaveState();

            var cfg = XmlHelper<CConfiguration>.Load(_configurationFile);
            cfg.Deflate();
            SelectedConfiguration = cfg;

            if (isStateSaved)
            {
                _state.RestoreState();
            }

            return cfg.Dataset.Any();
        }

        public bool SaveConfiguration()
        {
            SelectedConfiguration.Inflate();
            var result = XmlHelper<CConfiguration>.Save(_configurationFile, SelectedConfiguration);
            SelectedConfiguration.Deflate();

            return result;
        }

        private string _command;
        public string Command
        {
            get { return _command; }
            set
            {
                _command = value;
                OnPropertyChanged();
                OnPropertyChanged("Commands");
            }
        }

        private IEnumerable<string> _commands = new List<string>(); 
        public IEnumerable<string> Commands
        {
            get
            {
                if(_commands.Count() == 0)
                {
                    _commands = ExpandCommands();
                }
                return _commands;
            }
        }

        public void ExecuteCommand()
        {
            if (string.IsNullOrWhiteSpace(Command)) return;

            var parseResults = _parser.Parse(Command);
            IsValidCommand = parseResults.isAccepted;
            if(parseResults.isAccepted)
            {
                HandleCommand(parseResults);
            }
        }

        #endregion methods
    }
}
