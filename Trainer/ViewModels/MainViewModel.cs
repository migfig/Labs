using Log.Common.Services;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Template10.Mvvm;
using Trainer.Domain;

namespace Trainer.ViewModels
{
    public class MainViewModel: ViewModelBase
    {
        private static MainViewModel _viewModel = new MainViewModel();
        public static MainViewModel ViewModel { get { return _viewModel; } }

        private ObservableCollection<Presentation> _presentations;
        public void GetPresentations(Action<ObservableCollection<Presentation>> action)
        {
            if (null == _presentations)
            {
                LoadPresentations(action);
            }
            else
            {
                action.Invoke(_presentations);
            }  
        }

        private async void LoadPresentations(Action<ObservableCollection<Presentation>> action)
        {
            using (var service = ApiServiceFactory.CreateService<Presentation>())
            {
                _presentations = new ObservableCollection<Presentation>(await service.GetItems("presentations"));
                action.Invoke(_presentations);
            }
        }
    }
}
