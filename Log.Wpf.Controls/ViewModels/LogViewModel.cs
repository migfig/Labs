using Common.Commands;
using Log.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Windows.ApplicationModel;

namespace Log.Wpf.Controls.ViewModels
{
    public class LogViewModel: INotifyPropertyChanged
    {
        private static LogViewModel _viewModel = new LogViewModel();
        public static LogViewModel ViewModel { get { return _viewModel; } }
        private const string _apiBaseUrl = "http://localhost:3030/api/";

        private ObservableCollection<string> _filters = new ObservableCollection<string>();
        public ObservableCollection<string> Filters
        {
            get { return _filters; }
        }

        private IEnumerable<LogEntry> _entries;

        public IEnumerable<LogEntry> Entries
        {
            get
            {
                try
                {
                    if (_entries == null)
                    {
                        GetEntries();                        
                    }

                    return _entries;
                } catch(Exception)
                {
                    return new List<LogEntry>();
                }
            }
        }

        private async void GetEntries()
        {
            if (!DesignMode.DesignModeEnabled)
            {
                using (var client = ApiServiceFactory.CreateService(_apiBaseUrl))
                {
                    IsBusy = true;
                    _entries = await client.GetEntries(SelectedLevel);
                    OnPropertyChanged("Entries");
                    IsBusy = false;
                }
            }
            else
            {
                #region dummy entries

                _entries = new List<LogEntry>
                            {
                                new LogEntry
                                {
                                    TimeStamp = DateTime.UtcNow,
                                    Message = "Error Can't serialize item. Error Exception at System.Xml.Serialization.XmlClass.cs line number: 170",
                                    Computer = Environment.MachineName,
                                    EventLevel = eEventLevel.Error,
                                    User = Environment.UserName,
                                    Source = "System"
                                },
                                new LogEntry
                                {
                                    TimeStamp = DateTime.UtcNow,
                                    Message = "Item serialized.",
                                    Computer = Environment.MachineName,
                                    EventLevel = eEventLevel.Information,
                                    User = Environment.UserName,
                                    Source = "System"
                                },
                                new LogEntry
                                {
                                    TimeStamp = DateTime.UtcNow,
                                    Message = "Item partially serialized. Some properties were not expected.",
                                    Computer = Environment.MachineName,
                                    EventLevel = eEventLevel.Warning,
                                    User = Environment.UserName,
                                    Source = "System"
                                }
                            };
                #endregion

                OnPropertyChanged("Entries");
            }
        }

        private string _selectedServer;
        public string SelectedServer
        {
            get { return _selectedServer; }
            set
            {
                _selectedServer = value;
                OnPropertyChanged();
            }
        }

        private string _selectedService;
        public string SelectedService
        {
            get { return _selectedService; }
            set
            {
                _selectedService = value;
                OnPropertyChanged();
            }
        }

        private string _selectedSource;
        public string SelectedSource
        {
            get { return _selectedSource; }
            set
            {
                _selectedSource = value;
                OnPropertyChanged();
            }
        }

        private eEventLevel _selectedLevel = eEventLevel.All;
        public eEventLevel SelectedLevel
        {
            get { return _selectedLevel; }
            set
            {
                _selectedLevel = value;
                RefreshCommand.Execute(null);
            }
        }

        private RelayCommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                _refreshCommand = _refreshCommand ?? (_refreshCommand = new RelayCommand((tag) =>
                {
                    _entries = null;
                    OnPropertyChanged("Entries");
                }));

                return _refreshCommand;
            }
        }

        private RelayCommand _serverCommand;
        public ICommand ServerCommand
        {
            get
            {
                return _serverCommand ?? (_serverCommand = new RelayCommand((tag) => SelectedServer = tag.ToString(),
                        (tag) =>
                        {
                            return tag.ToString().Equals(SelectedServer);
                        }));
            }
        }

        private RelayCommand _servicesCommand;
        public ICommand ServicesCommand
        {
            get
            {
                return _servicesCommand ?? (_servicesCommand = new RelayCommand((tag) => SelectedService = tag.ToString(),
                        (tag) =>
                        {
                            return tag.ToString().Equals(SelectedService);
                        }));
            }
        }

        private RelayCommand _sourceCommand;
        public ICommand SourceCommand
        {
            get
            {
                return _sourceCommand ?? (_sourceCommand = new RelayCommand((tag) => SelectedSource = tag.ToString(),
                        (tag) =>
                        {
                            return tag.ToString().Equals(SelectedSource);
                        }));
            }
        }

        private RelayCommand _levelCommand;
        public ICommand LevelCommand
        {
            get
            {
                return _levelCommand ?? (_levelCommand = new RelayCommand((tag) => 
                    SelectedLevel = (eEventLevel)Enum.Parse(typeof(eEventLevel), tag.ToString()),
                    (tag) =>
                    {
                        var level = (eEventLevel)Enum.Parse(typeof(eEventLevel), tag.ToString());
                        return level.Equals(SelectedLevel); 
                    }));
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
                OnPropertyChanged("BusyVisibility");
            }
        }

        public Visibility BusyVisibility
        {
            get { return _isBusy ? Visibility.Visible : Visibility.Collapsed; }
        }


        #region property changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion 
    }
}
