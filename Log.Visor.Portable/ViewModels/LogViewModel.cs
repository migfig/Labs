using Log.Common;
using Log.Common.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Log.Visor.Portable.ViewModels
{
    public class LogViewModel: INotifyPropertyChanged
    {
        private static LogViewModel _viewModel = new LogViewModel();
        public static LogViewModel ViewModel { get { return _viewModel; } }
        private const string _apiBaseUrl = "http://localhost:3030/api/";

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

        private IEnumerable<LogSummary> _summary;
        public IEnumerable<LogSummary> Summary
        {
            get
            {
                try
                {
                    if (_summary == null)
                    {
                        GetSummary();
                    }

                    return _summary;
                }
                catch (Exception)
                {
                    return new List<LogSummary>();
                }
            }
        }

        private IEnumerable<LogSummaryByLevel> _summaryByLevel;
        public IEnumerable<LogSummaryByLevel> SummaryByLevel
        {
            get
            {
                try
                {
                    if (_summaryByLevel == null)
                    {
                        GetSummaryByLevel();
                    }

                    return _summaryByLevel;
                }
                catch (Exception)
                {
                    return new List<LogSummaryByLevel>();
                }
            }
        }

        private async void GetEntries()
        {
            using (var client = ApiServiceFactory.CreateService(_apiBaseUrl))
            {
                IsBusy = true;
                _entries = (await client.GetEntries(eEventLevel.All));

                OnPropertyChanged("Entries");
                IsBusy = false;
            }
        }

        private async void GetSummary()
        {
            using (var client = ApiServiceFactory.CreateService(_apiBaseUrl))
            {
                IsBusy = true;
                _summary = await client.GetSummaryEntries();

                OnPropertyChanged("Summary");
                IsBusy = false;
            }
        }

        private async void GetSummaryByLevel()
        {
            using (var client = ApiServiceFactory.CreateService(_apiBaseUrl))
            {
                IsBusy = true;
                _summaryByLevel = await client.GetSummaryEntriesByLevel();

                OnPropertyChanged("SummaryByLevel");
                IsBusy = false;
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
            }
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
