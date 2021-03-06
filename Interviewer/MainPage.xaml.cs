﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AppUIBasics.Common;
using Interviewer.Data;
using AppUIBasics;
using System.Runtime.CompilerServices;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Interviewer
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private NavigationHelper navigationHelper;
        private IEnumerable<Platform> _platforms;
        
        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public IEnumerable<Platform> Platforms
        {
            get { return _platforms; }
            set { SetProperty(ref _platforms, value); }
        }
       
        public MainPage()
        {
            //Check if groups have already been loaded, and just fire the event if the groups were loaded before the page got navigated to.
            if (NavigationRootPage.Current.Platforms == null || NavigationRootPage.Current.Platforms.Count() == 0)
            {
                NavigationRootPage.Current.PlatformsLoaded += RootPage_PlatformsLoaded;
            }
            else
            {
                RootPage_PlatformsLoaded(NavigationRootPage.Current, new EventArgs());
            }

            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.DataContext = this; //We have to set the DataContext to itself here to ensure the bindings get hooked up correctly in the Hub.
        }

        /// <summary>
        /// Handler for after the groups load.  Since NavigationRootPage and MainPage get constructed at very similar times, there's a 
        /// possibility of a race condition, so we only get the groups on the NavigationRootPage and listen for it to finish on the MainPage.
        /// </summary>
        /// <param name="sender">The NavigationRootPage that has loaded the groups.</param>
        /// <param name="e">Empty event args since we can get the groups directly from the root page.</param>
        private void RootPage_PlatformsLoaded(object sender, EventArgs e)
        {
            var platforms = NavigationRootPage.Current.Platforms;
            Platforms = platforms;            
        }


        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Invoked when a HubSection header is clicked.
        /// </summary>
        /// <param name="sender">The Hub that contains the HubSection whose header was clicked.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        void Hub_SectionHeaderClick(object sender, HubSectionHeaderClickEventArgs e)
        {
            HubSection section = e.Section;
            var platform = section.DataContext;
            this.Frame.Navigate(typeof(SectionPage), ((Platform)platform).Id);
        }

        /// <summary>
        /// Invoked when an item within a section is clicked.
        /// </summary>
        /// <param name="sender">The GridView or ListView
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var itemId = ((KnowledgeArea)e.ClickedItem).Id;
            this.Frame.Navigate(typeof(ItemPage), itemId);
        }

        void GroupView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var itemId = ((Platform)e.ClickedItem).Id;
            this.Frame.Navigate(typeof(SectionPage), itemId);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(SearchResultsPage));
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void pageRoot_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width <= 768)
            {
                VisualStateManager.GoToState(this, "Below768Layout", true);
            }
            else if (e.NewSize.Width <= 1130)
            {
                VisualStateManager.GoToState(this, "Below1130Layout", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "DefaultLayout", true);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void interviewButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(InterviewPage));
        }

        private void addItemButton_Click(object sender, RoutedEventArgs e)
        {
            if(MainViewModel.ViewModel.AddPlatform.CanExecute(null))
            {
                MainViewModel.ViewModel.AddPlatform.Execute(null);
            }
        }

        //private void Element_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    FrameworkElement element = sender as FrameworkElement;
        //    if (element != null)
        //    {
        //        FlyoutBase.ShowAttachedFlyout(element);
        //    }
        //}
    }
}
