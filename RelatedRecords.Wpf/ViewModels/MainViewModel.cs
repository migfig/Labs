using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RelatedRecords.Wpf.Annotations;

namespace RelatedRecords.Wpf
{
    public class MainViewModel : INotifyPropertyChanged
    {
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

        private DataTable _selectedDataTable;
        public DataTable SelectedDataTable
        {
            get { return _selectedDataTable; }
            set
            {
                _selectedDataTable = value;
                OnPropertyChanged();
            }
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
