using ApiTester.Models;
using Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ApiTester.Wpf.ViewModels
{
    public partial class MainViewModel: BaseModel
    {
        private static MainViewModel _viewModel = new MainViewModel();
        public static MainViewModel ViewModel {
            get { return _viewModel; }
        }

        public MainViewModel()
        {
            var configuration = ConfigurationManager.AppSettings["DefaultConfiguration"];
            Common.Extensions.TraceLog.Information("Running with {configuration} file", configuration);

            SelectedConfiguration = XmlHelper<apiConfiguration>.Load(configuration);
            OnPropertyChanged("Configurations");
        }

        public IEnumerable<apiConfiguration> Configurations
        {
            get
            {
                return new List<apiConfiguration>
                {
                    SelectedConfiguration
                };
            }
        }

        private apiConfiguration _selectedConfiguration;
        public apiConfiguration SelectedConfiguration
        {
            get { return _selectedConfiguration; }
            set
            {
                _selectedConfiguration = value;
                if (_selectedConfiguration != null)
                {
                    SelectedHost = _selectedConfiguration.setup.host.FirstOrDefault();
                    SelectedWorkflow = _selectedConfiguration.setup.workflow.FirstOrDefault();
                }
                OnPropertyChanged();
                OnPropertyChanged("HeadersTable");
                OnPropertyChanged("MethodsTable");
            }
        }

        public DataTable HeadersTable
        {
            get
            {
                var table = new DataTable("Headers");
                table.Columns.Add(new DataColumn("Name", typeof(string)));
                table.Columns.Add(new DataColumn("Value", typeof(string)));
                foreach(var h in SelectedConfiguration.setup.header)
                {
                    var row = table.NewRow();
                    row["Name"] = h.name;
                    row["Value"] = h.value;
                    table.Rows.Add(row);
                }

                foreach(var h in SelectedConfiguration.setup.buildHeader)
                {
                    table.Rows.Add(buildHeaderRow(h, table));
                }

                return table;
            }
        }

        private DataRow buildHeaderRow(BuildHeader header, DataTable table)
        {
            var row = table.NewRow();
            foreach(var t in header.workflow)
            {        
                //todo: build actual header value from tasks        
            }

            row["Name"] = header.name;
            row["Value"] = "?";
            return row;
        }

        public DataTable MethodsTable
        {
            get { return SelectedConfiguration.ToTable(); }
        }

        private DataRowView _selectedDataRowView;
        public DataRowView SelectedDataRowView
        {
            get { return _selectedDataRowView; }
            set
            {
                _selectedDataRowView = value;
                OnPropertyChanged();
                if(null != _selectedDataRowView)
                {
                    SelectedMethod = SelectedConfiguration.method
                        .First(x => x.name == _selectedDataRowView["name"].ToString());
                }
            }
        }

        private Method _selectedMethod;
        public Method SelectedMethod
        {
            get { return _selectedMethod; }
            set {
                _selectedMethod = value;
                OnPropertyChanged();
                OnPropertyChanged("ParametersTable");
            }
        }

        public DataTable ParametersTable
        {
            get {
                if(null != SelectedMethod && SelectedMethod.parameter.Any())
                    return  SelectedMethod.ToTable();

                return null;
            }
        }

        private Host _selectedHost;
        public Host SelectedHost
        {
            get { return _selectedHost; }
            set
            {
                _selectedHost = value;
                OnPropertyChanged();
            }
        }

        private workflow _selectedWorkflow;
        public workflow SelectedWorkflow
        {
            get { return _selectedWorkflow; }
            set
            {
                _selectedWorkflow = value;
                OnPropertyChanged();
            }
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

        public Visibility SprocketVisibility
        {
            get { return (this.IsBusy ? Visibility.Visible : Visibility.Collapsed); }
        }

        public Visibility LastErrorsVisibility
        {
            get { return (LastErrors.Count > 0 ? Visibility.Visible : Visibility.Collapsed); }
        }
    }
}
