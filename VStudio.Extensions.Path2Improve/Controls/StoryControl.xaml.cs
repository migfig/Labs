using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VStudio.Extensions.Path2Improve.ViewModels;
using Common.Services;
using System.Collections.Generic;
using VStudio.Extensions.Path2Improve.Views;

namespace VStudio.Extensions.Path2Improve.Controls
{
    /// <summary>
    /// Interaction logic for StoryControl.xaml
    /// </summary>
    public partial class StoryControl : UserControl
    {
        public StoryControl()
        {
            InitializeComponent();
        }

        private void OnExpanderDetailsCollapsed(object sender, RoutedEventArgs e)
        {
            if (((Control)sender).Name.Equals(expanderStory.Name))
            {
                this.Width = 490;
                this.Height = 25;
            }
            e.Handled = true;
        }

        private void OnExpanderDetailsExpanded(object sender, RoutedEventArgs e)
        {
            var expander = sender as Expander;
            if (expander.Name.Equals(expanderStory.Name))
            {
                this.Width = 490;
                this.Height = 500;
            }
            if (MainViewModel.ViewModel.SelectedStory == null)
                MainViewModel.ViewModel.SelectedStory = expander.DataContext as Story;

            e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).Tag;
            if (MainViewModel.ViewModel.AddActionCommand.CanExecute(item))
            {
                MainViewModel.ViewModel.AddActionCommand.Execute(item);
            }
        }        

        private void RtbSummary_Loaded(object sender, RoutedEventArgs e)
        {
            var rtb = sender as RichTextBox;
            rtb.Document = (rtb.Tag as Story).Document;
            rtb.MouseDoubleClick += Rtb_MouseDoubleClick;
        }

        private void Rtb_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var rtb = sender as RichTextBox;
            var ptr = rtb.GetPositionFromPoint(e.GetPosition(rtb), true);
            if(null != ptr)
            {
                var url = ptr.GetTextInRun(System.Windows.Documents.LogicalDirection.Backward);
                url += ptr.GetTextInRun(System.Windows.Documents.LogicalDirection.Forward);
                //MessageBox.Show("[" + url + "]", "Go", MessageBoxButton.OK, MessageBoxImage.Information);
                if(url.ToLower().Contains("http"))
                    Common.Extensions.runProcess(@"C:\Program Files\Internet Explorer\iexplore.exe", url, -1);
                else if (url.ToLower().EndsWith(".cs") || url.ToLower().EndsWith(".sql"))
                    Common.Extensions.runProcess(@"C:\Windows\explorer.exe", url, -1);

            }
            e.Handled = true;
        }

        private void Button_RemoveItemClick(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).Tag;
            if (MessageBox.Show("Are you sure you want to remove this " + item.GetType().Name +"?", "Remove Item", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                if (MainViewModel.ViewModel.RemoveActionCommand.CanExecute(item))
                {
                    MainViewModel.ViewModel.RemoveActionCommand.Execute(item);
                }
            }
        }

        private void SearchStory(object sender, RoutedEventArgs e)
        {
            var story = ((sender as Button).Tag as Story);
            LoadStory(story);
        }

        private async void LoadStory(Story story)
        {
            if (!string.IsNullOrEmpty(story.Name))
            {
                try
                {
                    var baseUrl = Story.New().Url.ToString();
                    var flatter = Flatter.Get();
                    if(string.IsNullOrEmpty(flatter.Value))
                    {
                        var creds = new Credentials();
                        if(creds.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            flatter = Flatter.Get(creds.User, creds.Password);
                        }
                    }

                    var headers = new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("Authorization", "Basic " + flatter.Value)
                    };

                    using (var client = ApiServiceFactory.CreateService<string>(baseUrl, headers))
                    {
                        var json = await client.GetItem("/" + story.Name).ConfigureAwait(false);
                        var jiraStory = JsonConvert.DeserializeObject<JiraStory>(json);

                        var description = jiraStory.fields.description;
                        var acceptCrit = string.Empty;
                        var developCrit = string.Empty;
                        var descParts = description.Split(new[] { "Development Criteria", "Acceptance Criteria" },   StringSplitOptions.None);

                        if (descParts.Length == 3)
                        {
                            story.Description = descParts[0];
                            developCrit = descParts[1];
                            acceptCrit = descParts[2];
                        }
                        else if(descParts.Length == 2)
                        {
                            var acceptCritIdx = -1;
                            var developCritIdx = -1;
                            if (description.Contains("Acceptance Criteria") && (acceptCritIdx = description.IndexOf("Acceptance Criteria")) >= 0)
                            {
                                story.Description = descParts[acceptCritIdx > 0 ? 0 : 1];
                                acceptCrit = descParts[acceptCritIdx > 0 ? 1 : 0];
                            }
                            else if (description.Contains("Development Criteria") && (developCritIdx = description.IndexOf("Development Criteria")) >= 0)
                            {
                                story.Description = descParts[developCritIdx > 0 ? 0 : 1];
                                developCrit = descParts[developCritIdx > 0 ? 1 : 0];
                            }
                        }
                        else
                        {
                            acceptCrit = description.Contains("Acceptance Criteria") ? descParts[0]: string.Empty;
                            developCrit = description.Contains("Development Criteria") ? descParts[0] : string.Empty;
                            story.Description = !description.Contains("Acceptance Criteria") && !description.Contains("Development Criteria") ? descParts[0] : string.Empty;
                        }

                        if(!string.IsNullOrEmpty(acceptCrit))
                            story.AcceptanceCriteria = new ObservableCollection<StringValue>(
                                acceptCrit.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                                            .Select(v => new StringValue(v, "AcceptanceCriteria")));

                        if (!string.IsNullOrEmpty(developCrit))
                            story.DeveloperCriteria = new ObservableCollection<StringValue>(
                            developCrit.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                                        .Select(v => new StringValue(v, "DevelopmentCriteria")));

                        story.Title = jiraStory.fields.summary;
                        story.Url = new Uri(jiraStory.self);
                        story.ParentStoryUrl = new Uri(jiraStory.fields.project.self);

                        try
                        {
                            story.Attachments.Clear();
                        }
                        catch (Exception) {; }

                        foreach (var att in jiraStory.fields.attachment)
                        {
                            try
                            {
                                story.Attachments.Add(new StringValue(att.content, "Attachment"));
                            }
                            catch (Exception) {;}
                        }

                        try
                        {
                            story.SubTasks.Clear();
                        }
                        catch (Exception) {; }

                        foreach (var task in jiraStory.fields.subtasks)
                        {
                            try
                            {
                                story.SubTasks.Add(new SubTask {
                                    Name = task.key,
                                    Title = task.fields.summary,
                                    Url = new Uri(task.self),
                                    Status = task.fields.status.name
                                });
                            }
                            catch (Exception) {; }
                        }

                        try
                        {
                            MainViewModel.ViewModel.IsDirty = true;
                        }
                        catch (Exception) {;}

                        try
                        {
                            MainViewModel.ViewModel.SaveStoryCommand.Execute(null);
                        }
                        catch (Exception) {; }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }        
    }
}
