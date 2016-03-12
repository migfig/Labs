//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
using AppUIBasics.Common;
using Interviewer;
using Interviewer.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AppUIBasics
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NavigationRootPage : Page
    {
        public static NavigationRootPage Current;
        public static Frame RootFrame = null;

        private IEnumerable<Interviewer.Data.Platform> _platforms;
        public IEnumerable<Interviewer.Data.Platform> Platforms
        {
            get { return _platforms; }
        }

        public event EventHandler PlatformsLoaded;

        public static SplitView RootSplitView
        {
            get { return Current.rootSplitView; }
        }

        private RootFrameNavigationHelper rootFrameNavigationHelper;

        public NavigationRootPage()
        {
            this.InitializeComponent();
            this.rootFrameNavigationHelper = new RootFrameNavigationHelper(rootFrame);
            LoadPlatforms();
            Current = this;
            RootFrame = rootFrame;
        }

        private async void LoadPlatforms()
        {
            var config = await InterviewerDataSource.GetConfiguration();
            _platforms = config.Platform;
            if (PlatformsLoaded != null)
                PlatformsLoaded(this, new EventArgs());
        }

        private void ControlGroupItems_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (var item in groupsItemControl.Items)
            {
                var container = groupsItemControl.ContainerFromItem(item);
                var panel = VisualTreeHelper.GetChild(container, 0) as StackPanel;
                if (panel != null)
                {
                    var button = panel.Children[0] as ToggleButton;
                    if (button.IsChecked.Value)
                    {
                        var finished = VisualStateManager.GoToState(button, "Normal", true);
                        button.IsChecked = false;
                    }
                }
            }
            this.rootFrame.Navigate(typeof(ItemPage), (e.ClickedItem as Interviewer.Data.KnowledgeArea).Id);
            rootSplitView.IsPaneOpen = false;
        }

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            this.rootFrame.Navigate(typeof(MainPage));
            rootSplitView.IsPaneOpen = false;
        }
    }
}
