using Log.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log.Visor.VStudio.ViewModels
{
    public class LogViewModel
    {
        private static LogViewModel _viewModel = new LogViewModel();
        public static LogViewModel ViewModel { get { return _viewModel; } }
        private const string _apiBaseUrl = "http://localhost:3030/api";

        private ObservableCollection<string> _filters;
        public ObservableCollection<string> Filters
        {
            get { return _filters; }
        }

        public IEnumerable<LogEntry> Entries
        {
            get
            {
                using (var client = ApiServiceFactory.CreateService(_apiBaseUrl))
                {
                    return client.GetEntries().GetAwaiter().GetResult();
                }
            }
        }
    }
}
