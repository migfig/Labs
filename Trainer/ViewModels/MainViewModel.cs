using Log.Common.Services;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Template10.Mvvm;
using Trainer.Domain;
using Windows.Storage;

namespace Trainer.ViewModels
{
    public class MainViewModel: ViewModelBase
    {
        private static MainViewModel _viewModel = new MainViewModel();
        public static MainViewModel ViewModel { get { return _viewModel; } }

        private ObservableCollection<Presentation> _presentations;
        public void GetPresentations(Action<ObservableCollection<Presentation>> action = null)
        {
            if (null == _presentations)
            {
                LoadPresentations(action);
            }
            else if(action != null)
            {
                action.Invoke(_presentations);
            }  
        }

        private async void LoadPresentations(Action<ObservableCollection<Presentation>> action = null)
        {
            using (var service = ApiServiceFactory.CreateService<Presentation>())
            {
                _presentations = new ObservableCollection<Presentation>(await service.GetItems("presentations"));
                if(!_presentations.Any())
                {
                    var defaultPresentation = await LoadDefaultPresentation();
                    if(defaultPresentation != null)
                    {
                        _presentations.Add(defaultPresentation);
                    }
                }

                if(action != null) action.Invoke(_presentations);
            }
        }

        public string SelectedPresentationTitle { get; set; }

        private async Task<Presentation> LoadDefaultPresentation()
        {
            try
            {
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Presentations/default.xml"));
                string xml = await FileIO.ReadTextAsync(file);
                return Common.XmlHelper2<Presentation>.LoadFromString(xml);
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
