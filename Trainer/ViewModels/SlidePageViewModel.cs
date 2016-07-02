using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Trainer.Domain;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Trainer.ViewModels
{
    public class SlidePageViewModel : ViewModelBase
    {
        private readonly ICodeServices _codeServices;

        public SlidePageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
            }

            _codeServices = new CodeServices();
            MainViewModel.ViewModel.GetPresentations((items) => Presentations = items);
        }

        public string Foother
        {
            get { return string.Format("Slide {0} of {1}", Presentation.Slide.IndexOf(CurrentSlide) + 1, Presentation.Slide.Count); }
        }

        private ObservableCollection<Presentation> _presentations;
        public ObservableCollection<Presentation> Presentations
        {
            get
            {
                return _presentations;
            }

            private set
            {
                Set(ref _presentations, value);
                Presentation = _presentations.FirstOrDefault();
            }
        }


        private Presentation _presentation;
        public Presentation Presentation
        {
            get
            {
                return _presentation;
            }
            private set
            {
                Set(ref _presentation, value);
                CurrentSlide = _presentation.Slide.FirstOrDefault();
            }
        }

        private Slide _currentSlide;
        public Slide CurrentSlide
        {
            get { return _currentSlide; }
            private set { Set(ref _currentSlide, value); }
        }

        public string BackgroundImageUrl
        {
            get { return !string.IsNullOrEmpty(CurrentSlide.Image) ? CurrentSlide.Image : Presentation.Image; }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if(null != parameter)
            {
                CurrentSlide = _presentation.Slide.First(x => x.Title.Equals(parameter.ToString()));
            }

            await Task.CompletedTask;
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {
            if (suspending)
            {
                suspensionState[nameof(CurrentSlide.Title)] = CurrentSlide.Title;
            }
            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }

        public void GotoNextSlide() =>
            NavigationService.Navigate(typeof(Views.SlidePage), GetNextSlideTitle());

        public void GotoPreviousSlide() =>
            NavigationService.Navigate(typeof(Views.SlidePage), GetPreviousSlideTitle());

        public void CopyCode() =>
            _codeServices.CopyCode(CurrentSlide.Component);

        public Visibility CopyCodeVisibility { get { return CurrentSlide.Component.Any() ? Visibility.Visible : Visibility.Collapsed; } }            

        public bool CanGotoPreviousSlide { get { return !_currentSlide.Title.Equals(_presentation.Slide.First().Title); } }

        public bool CanGotoNextSlide { get { return !_currentSlide.Title.Equals(_presentation.Slide.Last().Title); } }

        private string GetNextSlideTitle()
        {
            var index = _presentation.Slide.IndexOf(_currentSlide);
            if(index < _presentation.Slide.Count-1)
            {
                return _presentation.Slide.ElementAt(index + 1).Title;
            }

            return _presentation.Slide.First().Title;
        }

        private string GetPreviousSlideTitle()
        {
            var index = _presentation.Slide.IndexOf(_currentSlide);
            if (index > 0)
            {
                return _presentation.Slide.ElementAt(index - 1).Title;
            }

            return _presentation.Slide.First().Title;
        }
    }
}

