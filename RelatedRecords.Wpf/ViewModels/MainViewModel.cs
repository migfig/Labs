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
using Common;
using System.Windows;

namespace RelatedRecords.Wpf
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private static MainViewModel _instance = new MainViewModel();
        public static MainViewModel Instance {
            get { return _instance; }
        }

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

        private DatatableEx _selectedDataTable;
        public DatatableEx SelectedDataTable
        {
            get { return _selectedDataTable; }
            set
            {
                _selectedDataTable = value;
                OnPropertyChanged();
                SelectedRootDataRowView = _selectedDataTable.Root.Table.AsDataView()[0];
                OnPropertyChanged("ParentVisibility");
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

        private Stack<DatatableEx> _tableNavigation = new Stack<DatatableEx>();
        public Stack<DatatableEx> TableNavigation
        {
            get { return _tableNavigation; }
        }

        public Visibility ParentVisibility
        {
            get { return TableNavigation.Count > 0 ? Visibility.Visible : Visibility.Collapsed;  }
        }

        private bool _loaded;
        public void Load()
        {
            if (_loaded) return;

            var configuration = ConfigurationManager.AppSettings["ConfigurationFile"];
            SelectedConfiguration = XmlHelper<CConfiguration>.Load(configuration);
            //Helpers.CreateSampleTables(SelectedConfiguration);
            SelectedDataTable = SelectedDataset.Table.First()
                .Query("".ToArray(""), "".ToArray(""), true);
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
