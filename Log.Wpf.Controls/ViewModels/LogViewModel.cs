using Common.Commands;
using Log.Common;
using Log.Common.Services;
using Log.Wpf.Controls.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Windows.ApplicationModel;

namespace Log.Wpf.Controls.ViewModels
{
    #region event args 

    public class ViewCodeArgs: EventArgs
    {
        public string NameSpace { get; private set; }
        public string ClassName { get; private set; }
        public int LineNumber { get; private set; }
        public string ProgId { get; private set; }

        public ViewCodeArgs(string progId, string fullName, int lineNumber)
        {
            ProgId = progId;
            if (!string.IsNullOrWhiteSpace(fullName))
            {
                var parts = fullName.Replace(".cs", string.Empty).Split('\\');
                NameSpace = string.Join("\\", parts.Take(parts.Length - 1));
                ClassName = parts.Last() + ".cs";
            }
            LineNumber = lineNumber;
        }
    }

    #endregion

    public class LogViewModel: INotifyPropertyChanged
    {
        private static LogViewModel _viewModel = new LogViewModel();
        public static LogViewModel ViewModel { get { return _viewModel; } }
        private const string _apiBaseUrl = "http://localhost:3030/api/";

        public StringCollection Filters
        {
            get { return Settings.Default.Filters; }
        }

        public StringCollection FilterModes
        {
            get { return Settings.Default.FilterModes; }
        }

        public StringCollection IgnoreValues
        {
            get { return Settings.Default.IgnoreValues; }
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
                    _entries = (await client.GetEntries(SelectedLevel))
                        .Where(x => !IgnoreValues.Contains(x.Message));
                    
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

        private void FilterEntries()
        {
            if (!string.IsNullOrWhiteSpace(SelectedFilter))
            {
                IsBusy = true;
                switch (SelectedFilterMode)
                {
                    case "StartsWith":
                        _entries = _entries.Where(x => x.Message.ToLower().StartsWith(SelectedFilter));
                        break;
                    case "Contains":
                        _entries = _entries.Where(x => x.Message.ToLower().Contains(SelectedFilter));
                        break;
                    case "NotContains":
                        _entries = _entries.Where(x => !x.Message.ToLower().Contains(SelectedFilter));
                        break;
                    case "EndsWith":
                        _entries = _entries.Where(x => x.Message.ToLower().EndsWith(SelectedFilter));
                        break;
                }
                IsBusy = false;
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

        private string _selectedFilter;
        public string SelectedFilter
        {
            get { return _selectedFilter; }
            set
            {
                _selectedFilter = value;
                FilterEntries();
                _addFilterCommand.RaiseCanExecuteChanged();
                _clearFilterCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        private string _selectedFilterMode = "Contains";
        public string SelectedFilterMode
        {
            get { return _selectedFilterMode; }
            set
            {
                _selectedFilterMode = value;
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
                _levelCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        private RelayCommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                return _refreshCommand ?? (_refreshCommand = new RelayCommand((tag) =>
                {
                    _entries = null;
                    OnPropertyChanged("Entries");
                }));
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
                            return !tag.ToString().Equals(SelectedServer);
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
                            return !tag.ToString().Equals(SelectedService);
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
                            return !tag.ToString().Equals(SelectedSource);
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
                        return !level.Equals(SelectedLevel); 
                    }));
            }
        }

        private RelayCommand _addFilterCommand;
        public ICommand AddFilterCommand
        {
            get
            {
                return _addFilterCommand ?? (_addFilterCommand = new RelayCommand(
                    (tag) =>
                    {
                        Settings.Default.Filters.Insert(0, SelectedFilter);
                        Settings.Default.Save();
                        _addFilterCommand.RaiseCanExecuteChanged();
                        OnPropertyChanged("Filters");
                    },
                    (tag) =>
                    {
                        return !string.IsNullOrWhiteSpace(SelectedFilter)
                            && !Filters.Contains(SelectedFilter);
                    }));
            }
        }

        private RelayCommand _clearFilterCommand;
        public ICommand ClearFilterCommand
        {
            get
            {
                return _clearFilterCommand ?? (_clearFilterCommand = new RelayCommand(
                    (tag) =>
                    {
                        SelectedFilter = string.Empty;
                        RefreshCommand.Execute(null);
                    },
                    (tag) =>
                    {
                        return !string.IsNullOrWhiteSpace(SelectedFilter);
                    }));
            }
        }
        
        private RelayCommand _addToIgnoreValuesCommand;
        public ICommand AddToIgnoreValuesCommand
        {
            get
            {
                return _addToIgnoreValuesCommand ?? (_addToIgnoreValuesCommand = new RelayCommand(
                    (tag) =>
                    {
                        if (!IgnoreValues.Contains(tag.ToString()))
                        {
                            Settings.Default.IgnoreValues.Add(tag.ToString());
                            Settings.Default.Save();
                            RefreshCommand.Execute(null);
                        }
                    },
                    (tag) =>
                    {
                        return !string.IsNullOrWhiteSpace(tag.ToString())
                            && !IgnoreValues.Contains(tag.ToString());
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
