using Common.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApiTester.Wpf.ViewModels
{
    public partial class MainViewModel
    {
        RelayCommand _runTests;
        public ICommand RunTests
        {
            get
            {
                return _runTests ?? new RelayCommand(
                    (parameter) => {
                    }, 
                    x => SelectedConfiguration != null && !IsBusy);
            }
        }

        RelayCommand _loadConfiguration;
        public ICommand LoadConfiguration
        {
            get
            {
                return _loadConfiguration ?? new RelayCommand(
                    (parameter) => {
                    },
                    x => true);
            }
        }

        RelayCommand _toggleSelection;
        public ICommand ToggleSelection
        {
            get
            {
                return _toggleSelection ?? new RelayCommand(
                    (parameter) => {
                        foreach(var m in SelectedConfiguration.setup.method)
                        {
                            m.isSelected = !m.isSelected;
                        }

                        OnPropertyChanged("MethodsTable");
                    },
                    x => SelectedConfiguration != null);
            }
        }

        RelayCommand _buildHeaders;
        public ICommand BuildHeaders
        {
            get
            {
                return _buildHeaders ?? new RelayCommand(
                    (parameter) => {
                    },
                    x => true);
            }
        }

        RelayCommand _viewTestResults;
        public ICommand ViewTestResults 
        {
            get
            {
                return _viewTestResults ?? new RelayCommand(
                    (parameter) => {
                    },
                    x => true);
            }
        }

        RelayCommand _exitApplication;
        public ICommand ExitApplication
        {
            get
            {
                return _exitApplication ?? new RelayCommand(
                    (parameter) => {
                    },
                    x => true);
            }
        }
    }
}
