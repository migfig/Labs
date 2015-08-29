using ApiTester.Models;
using Common;
using Common.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApiTester.Wpf.ViewModels
{
    public partial class MainViewModel
    {
        #region commands

        RelayCommand _runTests;
        public ICommand RunTests
        {
            get
            {
                return _runTests ?? new RelayCommand(
                    (parameter) => {
                        runWorkflowForSelectedMethods();
                    }, 
                    x => SelectedConfiguration != null 
                        && SelectedWorkflow != null
                        && SelectedConfiguration.method.Any(m => m.isSelected)
                        && !IsBusy);
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
                        foreach(var m in SelectedConfiguration.method)
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
                    (parameter) => System.Windows.Application.Current.Shutdown()
                    ,x => !IsBusy);
            }
        }
        #endregion commands

        #region command methods

        private void runWorkflowForSelectedMethods()
        {
            Common.Extensions.TraceLog.Information("Running workflow {name}", SelectedWorkflow.name);

            var workflow = XmlHelper<workflow>.Load(SelectedWorkflow.name);
            foreach(var task in workflow.task)
            {
                var method = SelectedConfiguration.method.First(m => m.name == task.name);
                if(null != method && method.isSelected)
                {
                    executeMethod(method, task);
                }
            }
        }

        private void executeMethod(Method method, Models.Task task)
        {
            var exitCode = runProcess(SelectedConfiguration.setup.commandLine, buildArguments(method, task));
            if(exitCode == 0)
            {
                method.isValidTest = true;
            }
        }

        private string buildArguments(Method method, Models.Task task)
        {
            var args = new StringBuilder();
            args.AppendFormat(method.ToArgs(task), 
                SelectedHost.baseAddress);

            return args.ToString();
        }

        private int runProcess(string program, string args)
        {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo
            {
                FileName = program,
                Arguments = args,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            p.Start();
            p.WaitForExit(30000);

            return p.ExitCode;
        }

        #endregion command methods
    }
}
