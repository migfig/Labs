using ApiTester.Models;
using Common;
using Common.Commands;
using FluentTesting;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
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

        RelayCommand _runWorkflowTests;
        public ICommand RunWorkflowTests
        {
            get
            {
                _runWorkflowTests = _runWorkflowTests ?? new RelayCommand(
                    (parameter) => {
                        ToggleSelection.Execute(true);
                        runWorkflowForSelectedMethods(SelectedWorkflowDef);
                    },
                    x => SelectedConfiguration != null
                        && SelectedWorkflowDef != null
                        && SelectedWorkflowDef.task.Any()
                        && !IsBusy);
                return _runWorkflowTests;
            }
        }

        RelayCommand _saveWorkflow;
        public ICommand SaveWorkflow
        {
            get
            {
                _saveWorkflow = _saveWorkflow ?? new RelayCommand(
                    (parameter) => {
                        if(null != WorkflowParametersTable)
                        {
                            SelectedWorkflowTask.resultValue.Clear();
                            foreach(DataRow r in WorkflowResultsTable.Rows)
                            {
                                SelectedWorkflowTask.resultValue.Add(new ResultValue
                                {
                                    propertyName = r["propertyName"].ToString(),
                                    condition = (eCondition)Enum.Parse(typeof(eCondition), r["condition"].ToString()),
                                    @operator = (eOperator)Enum.Parse(typeof(eOperator), r["operator"].ToString()),
                                    value = r["value"].ToString()
                                });
                            }

                            SaveConfigurationWorkflow();
                            _saveWorkflow.RaiseCanExecuteChanged();
                        }
                        isEditingWorkflow = false;
                    },
                    x => SelectedConfiguration != null
                        && SelectedWorkflow != null && EditingWorkflow != null
                        && !IsBusy);
                return _saveWorkflow;
            }
        }

        RelayCommand _editWorkflow;
        public ICommand EditWorkflow
        {
            get
            {
                _editWorkflow = _editWorkflow ?? new RelayCommand(
                    (parameter) => {
                        isEditingWorkflow = true;
                    },
                    x => SelectedConfiguration != null
                        && SelectedWorkflow != null
                        && !IsBusy);
                return _editWorkflow;
            }
        }

        RelayCommand _addTask;
        public ICommand AddTask
        {
            get
            {
                _addTask = _addTask ?? new RelayCommand(
                    (parameter) => {
                        SelectedWorkflowDef.task.Add(new Task
                        {
                            name = SelectedMethod.name,
                            parameter = new ObservableCollection<Parameter>( SelectedMethod.parameter.ToArray()),
                            resultValue = new ObservableCollection<ResultValue>
                            {
                               new ResultValue
                               {
                                   condition = eCondition.And,
                                   propertyName = "Length",
                                   @operator = eOperator.isGreaterThan,
                                   value = "0"
                               }
                            }
                        });
                    },
                    x => SelectedConfiguration != null
                        && SelectedWorkflowDef != null
                        && SelectedMethod != null
                        && !IsBusy);
                return _addTask;
            }
        }

        RelayCommand _removeTask;
        public ICommand RemoveTask
        {
            get
            {
                _removeTask = _removeTask ?? new RelayCommand(
                    (parameter) => {
                        SelectedWorkflowDef.task.Remove(parameter as Task);
                        SaveWorkflow.AsRelay().RaiseCanExecuteChanged();
                    },
                    x => SelectedConfiguration != null
                        && SelectedWorkflowDef != null
                        && SelectedMethod != null
                        && !IsBusy);
                return _removeTask;
            }
        }

        RelayCommand _saveTask;
        public ICommand SaveTask
        {
            get
            {
                _saveTask = _saveTask ?? new RelayCommand(
                    (parameter) => {
                        var updatedTask = parameter as Task;
                        var task = SelectedWorkflowDef.task.First(x => x.name == updatedTask.name);
                        var pos = SelectedWorkflowDef.task.IndexOf(task);
                        SelectedWorkflowDef.task.RemoveAt(pos);
                        SelectedWorkflowDef.task.Insert(pos, updatedTask);

                        SaveWorkflow.AsRelay().RaiseCanExecuteChanged();
                    },
                    x => SelectedConfiguration != null
                        && SelectedWorkflowDef != null
                        && SelectedMethod != null
                        && !IsBusy);
                return _saveTask;
            }
        }

        RelayCommand _addWorkflow;
        public ICommand AddWorkflow
        {
            get
            {
                _addWorkflow = _addWorkflow ?? new RelayCommand(
                    (parameter) =>
                    {
                        var workflow = new workflow
                        {
                            name = SelectedConfiguration.setup.workflow.Last().name.Replace(".",  SelectedConfiguration.setup.workflow.Count.ToString() + "."),
                            task = new ObservableCollection<Task>()
                        };
                        SaveWorkflowItem(workflow);
                        SelectedConfiguration.setup.workflow.Add(workflow);
                        SelectedWorkflow = workflow;
                    },
                    x => SelectedConfiguration != null
                        && !IsBusy);
                return _addWorkflow;
            }
        }

        RelayCommand _saveWorkflowItems;
        public ICommand SaveWorkflowItems
        {
            get
            {
                _saveWorkflowItems = _saveWorkflowItems ?? new RelayCommand(
                    (parameter) =>
                    {
                        var currWorkflowName = SelectedWorkflowDef.name;
                        SaveWorkflowItem(SelectedWorkflowDef);
                        var otherWorkflows = SelectedConfiguration.setup.workflow.Where(x => !x.name.Equals(currWorkflowName));
                        foreach (var workflow in otherWorkflows)
                        {
                            SelectedWorkflowDef = workflow;
                            SaveWorkflowItem(SelectedWorkflowDef);
                        }
                        SaveConfiguration();
                        _saveWorkflowItems.RaiseCanExecuteChanged();
                    },
                    x => SelectedConfiguration != null
                        && SelectedWorkflow != null
                        && SelectedWorkflowDef != null
                        && !IsBusy);
                return _saveWorkflowItems;
            }
        }


        RelayCommand _loadConfiguration;
        public ICommand LoadConfiguration
        {
            get
            {
#if API_REFLECTOR
                _loadConfiguration = _loadConfiguration ?? new RelayCommand(
                    (parameter) => {
                        new LoadAssembly().ShowDialog();
                        OnPropertyChanged("Configurations");
                    },
                    x => true);
#else
                _loadConfiguration = _loadConfiguration ?? new RelayCommand(
                    (parameter) => {
                        //new LoadAssembly().ShowDialog();
                        //OnPropertyChanged("Configurations");
                    },
                    x => true);
#endif
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
                            m.isValidTest = eValidTest.Undefined;
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
                    foreach (var task in SelectedBuildWorkflow.taskItem.Where(x => !x.isDisabled))
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
                    foreach (var type in SelectedTypes)
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

            var exitCode = Common.Extensions.runProcess(!string.IsNullOrEmpty(task.commandLine)
                    ? task.commandLine
                    : SelectedBuildWorkflow.commandLine,
                string.Format(task.ToArgs(type)));
        }

#endregion reflect and load assembly

#region run workflow

        private void runWorkflowForSelectedMethods()
        {
            var workflow = XmlHelper<workflow>.Load(
                Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output"), SelectedWorkflow.name));

            runWorkflowForSelectedMethods(workflow);
        }

        private void runWorkflowForSelectedMethods(workflow workflow)
        {
            Common.Extensions.TraceLog.Information("Running workflow {name}", SelectedWorkflow.name);

            var worker = new BackgroundWorker();
            worker.DoWork += (o, s) =>
            {
                IsBusy = true;

                try
                {
                    foreach (var task in workflow.task.Where(x => !x.isDisabled))
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

                if (SelectedConfiguration.method
                    .Any(m => m.isValidTest == eValidTest.Passed || m.isValidTest == eValidTest.Failed))
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
            var outFile = "output\\" + method.name + ".json";
            if (File.Exists(outFile)) File.Delete(outFile);

            var exitCode = Common.Extensions.runProcess(SelectedConfiguration.setup.commandLine,
                string.Format(method.ToArgs(task), 
                    SelectedHost.baseAddress,
                    SelectedConfiguration.setup.ToHeaders(),
                    SelectedHost.ToHeaders()));
            if (File.Exists(outFile))
            {
                var type = getAssemblyType(method.type);
                if (type == null) return;

                task.ResultsObject = loadResults(type, outFile);
                method.isValidTest = task.IsValidTest ? eValidTest.Passed : eValidTest.Failed;
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

#endregion command methods
    }
}
