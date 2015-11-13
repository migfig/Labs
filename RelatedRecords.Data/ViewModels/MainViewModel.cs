using Common;
using RelatedRecords.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelatedRecords.Data.ViewModels
{
    public class MainViewModel: BaseModel
    {
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

        public MainViewModel(string configFile, string grammarFile)
        {
            _parser = new RRParser(grammarFile);
            _configurationFile = configFile;
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

        public bool LoadConfiguration()
        {
            var cfg = XmlHelper<CConfiguration>.Load(_configurationFile);
            cfg.Deflate();
            SelectedConfiguration = cfg;

            return cfg.Dataset.Any();
        }

        public bool SaveConfiguration()
        {
            SelectedConfiguration.Inflate();
            var result = XmlHelper<CConfiguration>.Save(_configurationFile, SelectedConfiguration);
            SelectedConfiguration.Deflate();

            return result;
        }
    }
}
