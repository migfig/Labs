using ApiTester.Models;
using Common;
using Common.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
                _runTests = _runTests ?? new RelayCommand(
                    (parameter) => {
                        runWorkflowForSelectedMethods();
                    }, 
                    x => SelectedConfiguration != null 
                        && SelectedWorkflow != null
                        && SelectedConfiguration.method.Any(m => m.isSelected)
                        && !IsBusy);
                return _runTests;
            }
        }

        RelayCommand _loadConfiguration;
        public ICommand LoadConfiguration
        {
            get
            {
                _loadConfiguration = _loadConfiguration ?? new RelayCommand(
                    (parameter) => {
                        new LoadAssembly().ShowDialog();
                        OnPropertyChanged("Configurations");
                    },
                    x => true);
                return _loadConfiguration;
            }
        }

        RelayCommand _saveLoadedConfiguration;
        public ICommand SaveLoadedConfiguration
        {
            get
            {
                _saveLoadedConfiguration = _saveLoadedConfiguration ?? new RelayCommand(
                    (parameter) => {
                        reflectAndLoadAssembly();
                    },
                    x => SelectedAssembly != null);

                return _saveLoadedConfiguration;
            }
        }

        RelayCommand _toggleSelection;
        public ICommand ToggleSelection
        {
            get
            {
                _toggleSelection = _toggleSelection ?? new RelayCommand(
                    (parameter) => {
                        foreach(var m in SelectedConfiguration.method)
                        {
                            m.isSelected = null != parameter ? Convert.ToBoolean(parameter) : !m.isSelected;
                        }

                        OnPropertyChanged("MethodsTable");
                        _runTests.RaiseCanExecuteChanged();
                    },
                    x => SelectedConfiguration != null);

                return _toggleSelection;
            }
        }

        RelayCommand _buildHeaders;
        public ICommand BuildHeaders
        {
            get
            {
                _buildHeaders = _buildHeaders ?? new RelayCommand(
                    (parameter) => {
                    },
                    x => true);

                return _buildHeaders;
            }
        }

        RelayCommand _goBack;
        public ICommand GoBack
        {
            get
            {
                _goBack = _goBack ?? new RelayCommand(
                    (parameter) => System.Windows.Application.Current.Shutdown()
                    , x => true);
                return _goBack;
            }
        }

        RelayCommand _exitApplication;
        public ICommand ExitApplication
        {
            get
            {
                _exitApplication = _exitApplication ?? new RelayCommand(
                    (parameter) => System.Windows.Application.Current.Shutdown()
                    ,x => !IsBusy);
                return _exitApplication;
            }
        }
        #endregion commands

        #region command methods

        #region reflect and load assembly

        private void reflectAndLoadAssembly()
        {
            var buildWorkflowFile = ConfigurationManager.AppSettings["DefaultBuildWorkflow"];
            Common.Extensions.TraceLog.Information("Running build workflow {buildWorkflowFile}", buildWorkflowFile);

            SelectedBuildWorkflow = XmlHelper<buildWorkflow>.Load(buildWorkflowFile);
            var worker = new BackgroundWorker();
            worker.DoWork += (o, s) =>
            {
                IsBusy = true;

                try
                {
                    foreach (var task in SelectedBuildWorkflow.taskItem)
                    {
                        runTaskItem(task);
                    }
                }
                catch (Exception e)
                {
                    Common.Extensions.ErrorLog.Error(e, "@ reflectAndLoadAssembly");
                }
            };
            worker.RunWorkerCompleted += (o, s) =>
            {
                IsBusy = false;
                SelectedAssembly = null;
                //_saveLoadedConfiguration.RaiseCanExecuteChanged();
            };
            worker.RunWorkerAsync();
        }

        private void runTaskItem(TaskItem task)
        {
            try
            {
                if (!task.foreachType)
                {
                    executeTaskItem(task);
                }
                else
                {
                    var types = SelectedAssembly.GetTypes().Where(x => x.Name.Contains("Controller"));
                    foreach (var type in types)
                    {
                        executeTaskItem(task, type.FullName
                            .Split('.')
                            .Last()
                            .Replace("Controller", string.Empty));
                    }
                }

                foreach (var t in task.taskItem)
                {
                    runTaskItem(t);
                }
            }
            catch (Exception e)
            {
                Common.Extensions.ErrorLog.Error(e, "@ runTaskItem {name}", task.name);
            }
        }

        private void executeTaskItem(TaskItem task, string type = "")
        {
            if(!string.IsNullOrEmpty(type))
            {
                type = Path.Combine(Path.Combine(Path.GetDirectoryName(
                        !string.IsNullOrEmpty(task.commandLine) 
                            ? task.commandLine 
                            : SelectedBuildWorkflow.commandLine), "output"),
                    type + ".xml");
            }
            else
            {
                type = SelectedAssembly.Location;
            }

            var exitCode = runProcess(!string.IsNullOrEmpty(task.commandLine)
                    ? task.commandLine
                    : SelectedBuildWorkflow.commandLine,
                string.Format(task.ToArgs(type)));
        }

        #endregion reflect and load assembly

        #region run workflow

        private void runWorkflowForSelectedMethods()
        {
            Common.Extensions.TraceLog.Information("Running workflow {name}", SelectedWorkflow.name);

            var workflow = XmlHelper<workflow>.Load(
                Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output"), SelectedWorkflow.name));
            var worker = new BackgroundWorker();
            worker.DoWork += (o, s) =>
            {
                IsBusy = true;

                try
                {
                    foreach (var task in workflow.task)
                    {
                        runTask(task);
                    }
                }
                catch (Exception e)
                {
                    Common.Extensions.ErrorLog.Error(e, "@ runWorkflowForSelectedMethods");
                }
            };
            worker.RunWorkerCompleted += (o, s) =>
            {
                ExecutedWorkflow = workflow;

                if (SelectedConfiguration.method.Any(m => m.isValidTest))
                {
                    OnPropertyChanged("MethodsTable");
                }
                IsBusy = false;
            };
            worker.RunWorkerAsync();
        }

        private void runTask(Models.Task task)
        {
            try
            {
                var method = SelectedConfiguration.method.First(m => m.name == task.name);
                if (null != method && method.isSelected)
                {
                    executeMethod(method, task);
                }

                foreach(var t in task.task)
                {
                    t.ParentTask = task;
                    runTask(t);
                }
            }
            catch (Exception e)
            {
                Common.Extensions.ErrorLog.Error(e, "@ runTask {name}", task.name);
            }
        }

        private void executeMethod(Method method, Models.Task task)
        {
            var outFile = method.name + ".json";
            if (File.Exists(outFile)) File.Delete(outFile);

            var exitCode = runProcess(SelectedConfiguration.setup.commandLine,
                string.Format(method.ToArgs(task), 
                    SelectedHost.baseAddress,
                    SelectedConfiguration.setup.ToHeaders(),
                    SelectedHost.ToHeaders()));
            if (File.Exists(outFile))
            {
                var type = getAssemblyType(method.type);
                if (type == null) return;

                task.ResultsObject = loadResults(type, outFile);
                method.isValidTest = task.IsValid;
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

        #endregion run workflow

        private int runProcess(string program, string args)
        {
            Common.Extensions.TraceLog.Information("Running method with args {program} {args}", program, args);
            System.Diagnostics.Debug.WriteLine(program + " " + args);

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
