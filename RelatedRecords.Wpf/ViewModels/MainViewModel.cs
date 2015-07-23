using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RelatedRecords.Wpf.Annotations;
using System.Configuration;

namespace RelatedRecords.Wpf
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private static MainViewModel _instance = new MainViewModel();
        public static MainViewModel Instance {
            get { return _instance; }
        }

        private CConfiguration _selectedConfiguration;
        public CConfiguration SelectedConfiguration
        {
            get { return _selectedConfiguration; }
            set
            {
                _selectedConfiguration = value;
                OnPropertyChanged();
            }
        }

        private CDataset _selectedDataset;
        public CDataset SelectedDataset
        {
            get { return _selectedDataset; }
            set
            {
                _selectedDataset = value;
                OnPropertyChanged();
            }
        }

        private DataTable _selectedDataTableDt;
        public DataTable SelectedDataTableDt
        {
            get { return _selectedDataTableDt; }
            set
            {
                _selectedDataTableDt = value;
                OnPropertyChanged();
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

                var children = _selectedDataTable.QueryChildren(SelectedDataset,
                    SelectedConfiguration.Datasource.First().ConnectionString,
                    _selectedRootDataRowView.Row);
                _selectedDataTable.Children.Clear();
                foreach(var t in children)
                {
                    _selectedDataTable.Children.Add(t);
                }
            }
        }

        private bool _loaded;
        public void Load()
        {
            if (_loaded) return;

            var configuration = ConfigurationManager.AppSettings["ConfigurationFile"];
            SelectedConfiguration = XmlHelper<CConfiguration>.Load(configuration);
            //Helpers.CreateSampleTables(SelectedConfiguration);
            SelectedDataset = SelectedConfiguration.Dataset.First();
            SelectedDataTable = SelectedDataset.Table.First()
                .Query(SelectedDataset,
                    SelectedConfiguration.Datasource.First().ConnectionString,
                    "".ToArray(""),
                    "".ToArray(""),
                    true);
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
