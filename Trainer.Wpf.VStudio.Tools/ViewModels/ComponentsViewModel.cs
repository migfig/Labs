using Common;
using System.Collections.ObjectModel;
using System.IO;
using Trainer.Domain;
using Trainer.Wpf.VStudio.Tools.Properties;
using System.Linq;
using System;
using Common.Commands;
using System.Windows.Input;
using Visor.VStudio;
using System.Threading;
using Log.Common.Services;
using System.Collections.Generic;
using Trainer.Wpf.VStudio.Tools.Views;

namespace Trainer.Wpf.VStudio.Tools.ViewModels
{
    public class ComponentsViewModel: BaseModel
    {
        private static ComponentsViewModel _viewModel = new ComponentsViewModel();
        public static ComponentsViewModel ViewModel { get { return _viewModel; } }

        public IPlugableWindow ParentWindow { get; set; }
        private ObservableCollection<Component> items;

        private Timer _timer;

        public ComponentsViewModel()
        {
            _timer = new Timer(OnTimerTick);
            _timer.Change(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10));
        }

        async void OnTimerTick(object state)
        {
            _timer.Change(TimeSpan.FromHours(10), TimeSpan.FromHours(10));

            try
            {
                using (var service = ApiServiceFactory.CreateService<Component>())
                {
                    var items = await service.GetItems("components");
                    foreach(var item in items)
                    {
                        await service.RemoveItem(item, "Id");
                    }
                    AddCodeComponents(items);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                _timer.Change(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10));
            }
        }

        private void AddCodeComponents(IEnumerable<Component> components)
        {
            if (null != components && components.Any())
            {
                foreach (var component in components)
                {
                    var item = items.FirstOrDefault(x => x.Id.Equals(component.Id)) ?? component;
                    if (item.Action.Equals(ComponentAction.Copy))
                        AddCodeCommand.Execute(item);
                    else
                        ViewCodeCommand.Execute(item);
                }
            }
        }

        private ObservableCollection<Component> _items;
        public ObservableCollection<Component> Items
        {
            get
            {
                if (_items == null)
                {
                    items = new ObservableCollection<Component>();
                    var files = Directory.GetFiles(Settings.Default.SourcePath, Settings.Default.FileMask);
                    foreach(var file in files)
                    {
                        var item = XmlHelper<Components>.Load(file);
                        if(null != item)
                        {
                            foreach (var i in item.Component) items.Add(i);
                        }
                    }

                    foreach(var item in items)
                    {
                        if (string.IsNullOrWhiteSpace(item.Code.Value))
                        {
                            foreach (var dep in item.Dependency)
                                item.Code.ComposedValue += items.First(x => x.Id.Equals(dep.Id)).TargetFile
                                    + Environment.NewLine; 
                        }
                    }

                    _items = new ObservableCollection<Component>(items.Where(x => x.IsBrowsable));
                }

                return _items;
            }
        }

        private void SaveComponents(Component component, bool isRemoved = false)
        {
            var file = Directory.GetFiles(Settings.Default.SourcePath, "patterns" + Settings.Default.FileMask)
                .FirstOrDefault();
            if(file != null)
            {
                var item = XmlHelper<Components>.Load(file);
                if (null != item)
                {
                    if(isRemoved)
                        item.Component.Remove(item.Component.First(x => x.Id.Equals(component.Id)));
                    else
                        item.Component.Add(component);
                    XmlHelper<Components>.Save(file, item);
                }
            }
        }

        private void SaveComponents(IEnumerable<Component> components)
        {
            var file = Directory.GetFiles(Settings.Default.SourcePath, "patterns" + Settings.Default.FileMask)
                .FirstOrDefault();
            if (file != null)
            {
                var item = XmlHelper<Components>.Load(file);
                if (null != item)
                {
                    foreach (var component in components)
                    {
                        item.Component.Add(component);
                        component.IsDirty = false;
                    }
                    XmlHelper<Components>.Save(file, item);
                }
            }
        }

        private RelayCommand _saveComponentCommand;
        public ICommand SaveComponentCommand
        {
            get
            {
                return _saveComponentCommand ?? (_saveComponentCommand = new RelayCommand(
                    (tag) =>
                    {
                        var components = Items.Where(x => x.IsDirty.Equals(true));
                        if (null != components && components.Any())
                        {
                            SaveComponents(components);
                        }
                    },
                    (tag) => true));
            }
        }

        private RelayCommand _addComponentCommand;
        public ICommand AddComponentCommand
        {
            get
            {
                return _addComponentCommand ?? (_addComponentCommand = new RelayCommand(
                    (tag) =>
                    {
                        var component = XmlHelper<Component>.LoadFromString(Settings.Default.DefaultComponent);
                        if (null != component)
                        {
                            component.Id = Guid.NewGuid().ToString("N");
                            component.IsDirty = true;
                            items.Add(component);
                            _items.Add(component);
                            OnPropertyChanged("Items");
                        }
                    },
                    (tag) => true));
            }
        }

        private RelayCommand _removeComponentCommand;
        public ICommand RemoveComponentCommand
        {
            get
            {
                return _removeComponentCommand ?? (_removeComponentCommand = new RelayCommand(
                    (tag) =>
                    {
                        var component = tag as Component;
                        if (component != null)
                        {
                            items.Remove(component);
                            _items.Remove(component);
                            SaveComponents(component, isRemoved: true);
                            OnPropertyChanged("Items");
                        }
                    },
                    (tag) => true));
            }
        }

        private RelayCommand _addCodeCommand;
        public ICommand AddCodeCommand
        {
            get
            {
                return _addCodeCommand ?? (_addCodeCommand = new RelayCommand(
                    (tag) =>
                    {
                        var component = tag as Component;
                        if (!string.IsNullOrWhiteSpace(component.Code.ComposedValue))
                        {
                            component = ResolveParameters(component);
                            if (ParentWindow != null) ParentWindow.AddCode(ResolveDependencies(component));
                        }
                    },
                    (tag) =>
                    {
                        return !string.IsNullOrWhiteSpace((tag as Component).Code.ComposedValue);
                    }));
            }
        }

        private RelayCommand _viewCodeCommand;
        public ICommand ViewCodeCommand
        {
            get
            {
                return _viewCodeCommand ?? (_viewCodeCommand = new RelayCommand(
                    (tag) =>
                    {
                        var component = tag as Component;
                        if (!string.IsNullOrWhiteSpace(component.Code.ComposedValue))
                        {
                            if (ParentWindow != null) ParentWindow.ViewCode(ResolveDependencies(component));
                        }
                    },
                    (tag) =>
                    {
                        return !string.IsNullOrWhiteSpace((tag as Component).Code.ComposedValue);
                    }));
            }
        }

        private Component ResolveDependencies(Component component)
        {
            if(component.Dependency.Any())
            {
                foreach(var dep in component.Dependency)
                {
                    dep.Component = items.First(x => x.Id.Equals(dep.Id));
                    if(!string.IsNullOrEmpty(component.SourcePath) && string.IsNullOrEmpty(dep.Component.SourcePath))
                        dep.Component.SourcePath = component.SourcePath;
                    dep.Component = ResolveDependencies(dep.Component);
                }
            }
            return component;
        }

        private Component ResolveParameters(Component component)
        {
            if (!string.IsNullOrWhiteSpace(component.Code.ComposedValue))
            {
                if (component.Parameter.Any())
                {
                    var viewModel = new ComponentVarsViewModel(component.Parameter, ParentWindow.Projects.ToList());
                    var cv = new ComponentVars(viewModel);
                    var result = cv.ShowDialog();
                    if (result.HasValue && result.Value.Equals(true))
                    {
                        var fileParts = component.TargetFile.Split('\\');
                        var suffixFolder = fileParts.Length > 1 ? "." + fileParts[fileParts.Length - 2] : string.Empty;

                        if (!string.IsNullOrEmpty(viewModel.SelectedProject))
                        {
                            component.TargetProject = viewModel.SelectedProject;
                        }

                        foreach (var p in viewModel.Parameters)
                        {
                            if (!string.IsNullOrEmpty(p.Value) && p.IsProjectName)
                            {
                                component.Code.Value = component.Code.Value.Replace(p.Name, viewModel.SelectedProject + suffixFolder);
                            }
                            else if (!string.IsNullOrEmpty(p.Value))
                            {
                                component.Code.Value = component.Code.Value.Replace(p.Name, p.Value);
                                component.TargetFile = component.TargetFile.Replace(p.Name, p.Value);
                            }
                        }
                    }
                }
            }

            return component;
        }
    }
}
