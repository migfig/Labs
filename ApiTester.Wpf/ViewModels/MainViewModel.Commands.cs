using ApiTester.Models;
using Common;
using Common.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

            try {
                IsBusy = true;

                var workflow = XmlHelper<workflow>.Load(SelectedWorkflow.name);
                foreach (var task in workflow.task)
                {
                    try {
                        var method = SelectedConfiguration.method.First(m => m.name == task.name);
                        if (null != method && method.isSelected)
                        {
                            executeMethod(method, task);
                        }
                    } catch(Exception ex)
                    {
                        Common.Extensions.ErrorLog.Error(ex, "@ runWorkflowForSelectedMethods");
                    }
                }

                if(SelectedConfiguration.method.Any(m => m.isValidTest))
                {
                    OnPropertyChanged("MethodsTable");
                }
            } catch(Exception e)
            {
                Common.Extensions.ErrorLog.Error(e, "@ runWorkflowForSelectedMethods");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void executeMethod(Method method, Models.Task task)
        {
            var outFile = method.name + ".json";
            if (File.Exists(outFile)) File.Delete(outFile);

            var exitCode = runProcess(SelectedConfiguration.setup.commandLine,
                string.Format(method.ToArgs(task), SelectedHost.baseAddress));

            if (File.Exists(outFile))
            {
                var type = getAssemblyType(method.type);
                if (type == null) return;

                task.Results = loadResults(type, outFile);
                if (!(task.Results is Exception))
                    method.isValidTest = true;
            }
        }

        private object loadResults(Type type, string fileName)
        {
            using (var stream = new StreamReader(fileName))
            {
                try
                {
                    return JsonConvert.DeserializeObject(stream.ReadToEnd(), type);
                }
                catch (Exception e)
                {
                    return e;
                }
            }
        }

        private Type getAssemblyType(string typeName)
        {
            Type type = null;

            if (_assemblyTypes.ContainsKey(typeName))
            {
                type = _assemblyTypes[typeName];
            }
            if (null == type)
            {
                foreach (var asm in _assemblies.Values)
                {
                    type = asm.GetType(typeName);
                    if (type != null)
                    {
                        _assemblyTypes.Add(typeName, type);
                        break;
                    }
                }
            }

            return type;
        }

        private int runProcess(string program, string args)
        {
            Common.Extensions.TraceLog.Information("Running method with args {program} {args}", program, args);

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
