﻿using AppUIBasics.Common;
using Interviewer.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AppUIBasics
{
    public sealed partial class PageHeader : UserControl
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(object), typeof(PageHeader), new PropertyMetadata(null));
        public object Title
        {
            get { return GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty WideLayoutThresholdProperty = DependencyProperty.Register("WideLayoutThreshold", typeof(double), typeof(PageHeader), new PropertyMetadata(600));
        public double WideLayoutThreshold
        {
            get { return (double)GetValue(WideLayoutThresholdProperty); }
            set
            {
                SetValue(WideLayoutThresholdProperty, value);
                WideLayoutTrigger.MinWindowWidth = value;
            }
        }

        public PageHeader()
        {
            this.InitializeComponent();
        }

        private async void controlsSearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var config = await InterviewerDataSource.GetConfiguration();
                var platforms = config.Platform;
                var suggestions = new List<KnowledgeArea>();

                foreach (var platform in platforms)
                {
                    var matchingItems = platform.KnowledgeArea.Where(
                        item => item.Name.IndexOf(sender.Text, StringComparison.CurrentCultureIgnoreCase) >= 0);

                    foreach (var item in matchingItems)
                    {
                        suggestions.Add(item);
                    }
                }
                if (suggestions.Count > 0)
                    controlsSearchBox.ItemsSource = suggestions.OrderByDescending(i => i.Name.StartsWith(sender.Text, StringComparison.CurrentCultureIgnoreCase)).ThenBy(i => i.Name);
                else
                    controlsSearchBox.ItemsSource = new string[] { "No results found" };
            }
        }

        private void controlsSearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null && args.ChosenSuggestion is KnowledgeArea)
            {
                var itemId = (args.ChosenSuggestion as KnowledgeArea).Id;
                NavigationRootPage.RootFrame.Navigate(typeof(ItemPage), itemId);
            }
            else if (!string.IsNullOrEmpty(args.QueryText))
            {
                //NavigationRootPage.RootFrame.Navigate(typeof(SearchResultsPage), args.QueryText);
            }
        }

        private void splitViewToggle_Click(object sender, RoutedEventArgs e)
        {
            NavigationRootPage.RootSplitView.IsPaneOpen = !NavigationRootPage.RootSplitView.IsPaneOpen;
        }
    }
}
