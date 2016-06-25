using Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Template10.Common;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Trainer.Models;
using Windows.UI.Xaml.Navigation;

namespace Trainer.ViewModels
{
    public class SlidePageViewModel : ViewModelBase
    {
        public SlidePageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
            }
        }

        public string Header
        {
            get { return Presentation.Title + " - " + CurrentSlide.Title; }
        }

        private Presentation _presentation;
        public Presentation Presentation
        {
            get
            {
                if(_presentation == null)
                {
                    GetPresentation();
                }
                return _presentation;
            }
            set
            {
                Set(ref _presentation, value);
            }
        }

        private void GetPresentation()
        {
            #region dummy data
            _presentation = XmlHelper<Presentation>.LoadFromString(                
@"<?xml version='1.0' encoding='utf - 8' ?>
<Presentation Title='Trainer Assistant'>
 <Slide Title='Maintenable Applications'>
   <RichTextBlock FontSize='20' FontWeight='DemiBold'>
     <Paragraph>
       <Bold>Static vs Dynamic Components</Bold>
     </Paragraph>
   </RichTextBlock>
 </Slide>
 <Slide Title='Extensibility with MEF'>
   <RichTextBlock FontSize='20' FontWeight='DemiBold'>
     <Paragraph>
       <Bold>What is MEF</Bold>
     </Paragraph>
     <Paragraph>
       <Bold>How does MEF works</Bold>
     </Paragraph>
   </RichTextBlock>
 </Slide>
 <Slide Title='Integration with Visual Studio'>
   <RichTextBlock FontSize='20' FontWeight='DemiBold'>
     <Paragraph>
       <Bold>Visual Studio Add - ins</Bold>
     </Paragraph>
     <Paragraph>
       <Bold>A Generic place holder</Bold>
     </Paragraph>
   </RichTextBlock>
 </Slide>
 <Slide Title='Integration with Universal Windows Platform Applications'>
   <RichTextBlock FontSize='20' FontWeight='DemiBold'>
     <Paragraph>
       <Bold>UWP Applications</Bold>
     </Paragraph>
     <Paragraph>
       <Bold>Template 10 Extension</Bold>
     </Paragraph>
   </RichTextBlock>
 </Slide>
 <Slide Title='Sample Paragraph'>
   <RichTextBlock SelectionHighlightColor='Green'>
          <Paragraph>
            RichTextBlock provides a rich text display container that supports
            <Run FontStyle='Italic' FontWeight='Bold'>formatted text</Run>,
            <Hyperlink NavigateUri='http://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.xaml.documents.hyperlink.aspx'>hyperlinks</Hyperlink>, inline images, and other rich content.
          </Paragraph>
          <Paragraph>RichTextBlock also supports a built-in overflow model.</Paragraph>
        </RichTextBlock>
        </Slide>
</Presentation>".Replace("'","\""));
            #endregion 

            CurrentSlide = _presentation.Slide.First();
        }

        private Slide _currentSlide;
        public Slide CurrentSlide
        {
            get { return _currentSlide; }
            set { Set(ref _currentSlide, value); }
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

        public bool CanGotoPreviousSlide
        { 
            get
            {
                return !_currentSlide.Title.Equals(_presentation.Slide.First().Title);
            }
        }

        public bool CanGotoNextSlide
        {
            get
            {
                return !_currentSlide.Title.Equals(_presentation.Slide.Last().Title);
            }
        }

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

