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

namespace Visor.Wpf.TodoCoder.ViewModels
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
                    var item = items.FirstOrDefault(x => x.Id.Equals(component.Id));
                    if (component.Action.Equals(ComponentAction.Copy))
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
    }
}
