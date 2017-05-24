using Common.Commands;
using VStudio.Extensions.Path2Improve.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
//using Windows.ApplicationModel;
using Newtonsoft.Json;
using System.IO;
using System.Collections.ObjectModel;

namespace VStudio.Extensions.Path2Improve.ViewModels
{    
    public class MainViewModel : INotifyPropertyChanged
    {
        private static MainViewModel  _viewModel = new MainViewModel ();
        public static MainViewModel  ViewModel { get { return _viewModel; } }

        public StringCollection Filters
        {
            get { return Settings.Default.Filters; }
        }

        public StringCollection FilterModes
        {
            get { return Settings.Default.FilterModes; }
        }

        public StringCollection IgnoreValues
        {
            get { return Settings.Default.IgnoreValues; }
        }

        private readonly string _fileName;

        public MainViewModel()
        {
            _fileName = Path.Combine(Visor.VStudio.Controls.Properties.Settings.Default.ExtensionsDirectory, "stories.json");
        }

        public IEnumerable<StoryStatus> StoryStatuses => Enum.GetValues(typeof(StoryStatus)).Cast<StoryStatus>();

        private ObservableCollection<Story> _stories;

        public IEnumerable<Story> Stories
        {
            get
            {
                try
                {
                    if (_stories == null)
                    {
                        GetStories();                        
                    }

                    return _stories;
                } catch(Exception)
                {
                    return new ObservableCollection<Story> { Story.New() };
                }
            }
        }

        private void GetStories()
        {
            //if (!DesignMode.DesignModeEnabled)
            //{
                _stories = JsonConvert.DeserializeObject<ObservableCollection<Story>>(File.ReadAllText(_fileName));
            //}
            //else
            //{
            //    #region dummy entries

            //    _stories = new ObservableCollection<Story>
            //        {
            //            new Story
            //            {
            //                Name = "ID-1001",
            //                Title ="Create cloud items",
            //                Description = "Create items from provided values",
            //                DateStarted = DateTime.UtcNow,
            //                DateEnded = DateTime.MinValue,
            //                Status = StoryStatus.NotStarted,
            //                Url = Story.NewUri(),
            //                ParentStoryUrl = Story.NewUri(),
            //                Attachments = new ObservableCollection<StringValue> { Story.NewAttachment() },
            //                KeyIdentifiers = new ObservableCollection<Keyidentifier>{ Keyidentifier.New() },
            //                Checkups = new ObservableCollection<Checkup> { Checkup.New() },
            //                Queries = new ObservableCollection<StringValue>{ Story.NewQuery() },
            //                Scripts = new ObservableCollection<StringValue>{ Story.NewScript() },
            //                TestCases = new ObservableCollection<Testcase>{ Testcase.New() },
            //                Issues = new ObservableCollection<Issue>{ Issue.New() },
            //                AcceptanceCriteria = new ObservableCollection<StringValue>{ Story.NewAcceptanceCriteria() },
            //                DeveloperCriteria = new ObservableCollection<StringValue>{ Story.NewDeveloperCriteria() },
            //                SubTasks = new ObservableCollection<SubTask>{ SubTask.New() }
            //            },
            //        };

            //    _stories.First().TestCases.First().KeyIdentifierIds.Add(new StringValue(_stories.First().KeyIdentifiers.First().Id.ToString(), "KeyIdentifierId"));
            //    #endregion

            //    SetStories();
            //    OnPropertyChanged("Stories");
            //}
        }

        private void SetStories()
        {
            File.WriteAllText(_fileName, JsonConvert.SerializeObject(_stories));
            IsDirty = false;
        }        

        private void FilterEntries()
        {
            if (!string.IsNullOrWhiteSpace(SelectedFilter))
            {
                IsBusy = true;
                switch (SelectedFilterMode)
                {
                    case "StartsWith":
                        _stories = new ObservableCollection<Story>( _stories.Where(x => x.Description.ToLower().StartsWith(SelectedFilter)));
                        break;
                    case "Contains":
                        _stories = new ObservableCollection<Story>(_stories.Where(x => x.Description.ToLower().Contains(SelectedFilter)));
                        break;
                    case "NotContains":
                        _stories = new ObservableCollection<Story>(_stories.Where(x => !x.Description.ToLower().Contains(SelectedFilter)));
                        break;
                    case "EndsWith":
                        _stories = new ObservableCollection<Story>(_stories.Where(x => x.Description.ToLower().EndsWith(SelectedFilter)));
                        break;
                }
                IsBusy = false;
                OnPropertyChanged("Entries");
            }
        }

        private Story _selectedStory;
        public Story SelectedStory
        {
            get { return _selectedStory; }
            set
            {
                _selectedStory = value;
                OnPropertyChanged();
            }
        }

        private Keyidentifier _selectedKeyidentifier;
        public Keyidentifier SelectedKeyidentifier
        {
            get { return _selectedKeyidentifier; }
            set
            {
                _selectedKeyidentifier = value;
                OnPropertyChanged();
            }
        }

        private Question _selectedQuestion;
        public Question SelectedQuestion
        {
            get { return _selectedQuestion; }
            set
            {
                _selectedQuestion = value;
                OnPropertyChanged();
            }
        }

        private Testcase _selectedTestcase;
        public Testcase SelectedTestcase
        {
            get { return _selectedTestcase; }
            set
            {
                _selectedTestcase = value;
                OnPropertyChanged();
            }
        }

        private string _selectedFilter;
        public string SelectedFilter
        {
            get { return _selectedFilter; }
            set
            {
                _selectedFilter = value;
                FilterEntries();
                _addFilterCommand.RaiseCanExecuteChanged();
                _clearFilterCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        private string _selectedFilterMode = "Contains";
        public string SelectedFilterMode
        {
            get { return _selectedFilterMode; }
            set
            {
                _selectedFilterMode = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                return _refreshCommand ?? (_refreshCommand = new RelayCommand((tag) =>
                {
                    _stories = null;
                    OnPropertyChanged("Entries");
                }));
            }
        }        
        
        private RelayCommand _addFilterCommand;
        public ICommand AddFilterCommand
        {
            get
            {
                return _addFilterCommand ?? (_addFilterCommand = new RelayCommand(
                    (tag) =>
                    {
                        Settings.Default.Filters.Insert(0, SelectedFilter);
                        Settings.Default.Save();
                        _addFilterCommand.RaiseCanExecuteChanged();
                        OnPropertyChanged("Filters");
                    },
                    (tag) =>
                    {
                        return !string.IsNullOrWhiteSpace(SelectedFilter)
                            && !Filters.Contains(SelectedFilter);
                    }));
            }
        }

        private RelayCommand _clearFilterCommand;
        public ICommand ClearFilterCommand
        {
            get
            {
                return _clearFilterCommand ?? (_clearFilterCommand = new RelayCommand(
                    (tag) =>
                    {
                        SelectedFilter = string.Empty;
                        RefreshCommand.Execute(null);
                    },
                    (tag) =>
                    {
                        return !string.IsNullOrWhiteSpace(SelectedFilter);
                    }));
            }
        }
        
        private RelayCommand _addToIgnoreValuesCommand;
        public ICommand AddToIgnoreValuesCommand
        {
            get
            {
                return _addToIgnoreValuesCommand ?? (_addToIgnoreValuesCommand = new RelayCommand(
                    (tag) =>
                    {
                        if (!IgnoreValues.Contains(tag.ToString()))
                        {
                            Settings.Default.IgnoreValues.Add(tag.ToString());
                            Settings.Default.Save();
                            RefreshCommand.Execute(null);
                        }
                    },
                    (tag) =>
                    {
                        return !string.IsNullOrWhiteSpace(tag.ToString())
                            && !IgnoreValues.Contains(tag.ToString());
                    }));
            }
        }
        
        private RelayCommand _addStoryCommand;
        public ICommand AddStoryCommand
        {
            get
            {
                return _addStoryCommand ?? (_addStoryCommand = new RelayCommand(
                    (tag) =>
                    {
                        var newStory = Story.New();
                        _stories.Add(newStory);
                        SelectedStory = newStory;
                        OnPropertyChanged("Stories");
                        IsDirty = true;
                    },
                    (tag) => true));
            }
        }

        private bool _isDirty = false;
        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                _isDirty = value;
                OnPropertyChanged();
                _saveStoryCommand.RaiseCanExecuteChanged();
            }
        }

        private RelayCommand _saveStoryCommand;
        public ICommand SaveStoryCommand
        {
            get
            {
                return _saveStoryCommand ?? (_saveStoryCommand = new RelayCommand(
                    (tag) =>
                    {
                        if (Stories != null && Stories.Any())
                        {
                            SetStories();
                            RefreshCommand.Execute(null);
                        }
                    },
                    (tag) =>
                    {
                        return /*IsDirty &&*/ Stories != null && Stories.Any();
                    }));
            }
        }
        
        
        private RelayCommand _addActionCommand;
        public ICommand AddActionCommand
        {
            get
            {
                return _addActionCommand ?? (_addActionCommand = new RelayCommand(
                    (tag) =>
                    {
                        if (null != SelectedStory)
                        {
                            switch(tag.ToString())
                            {
                                case "Testcase":
                                    SelectedStory.TestCases.Add(Testcase.New());
                                    break;
                                case "Question":
                                    SelectedKeyidentifier.Questions.Add(Question.New());
                                    break;
                                case "Checkup":
                                    SelectedStory.Checkups.Add(Checkup.New());
                                    break;
                                case "KeyIdentifier":
                                    SelectedStory.KeyIdentifiers.Add(Keyidentifier.New());
                                    break;
                                case "Step":
                                    SelectedTestcase.Steps.Add(Testcase.NewStep());
                                    break;
                                case "KeyIdentifierId":
                                    SelectedTestcase.KeyIdentifierIds.Add(Testcase.NewKeyIdentifierId(SelectedStory.KeyIdentifiers.First().Id));
                                    break;
                                case "Attachment":
                                    SelectedStory.Attachments.Add(Story.NewAttachment());
                                    break;
                                case "Query":
                                    SelectedStory.Queries.Add(Story.NewQuery());
                                    break;
                                case "Script":
                                    SelectedStory.Scripts.Add(Story.NewScript());
                                    break;
                                case "Issue":
                                    SelectedStory.Issues.Add(Issue.New());
                                    break;
                                case "AcceptanceCriteria":
                                    SelectedStory.AcceptanceCriteria.Add(Story.NewAcceptanceCriteria());
                                    break;
                                case "DeveloperCriteria":
                                    SelectedStory.DeveloperCriteria.Add(Story.NewDeveloperCriteria());
                                    break;
                                case "Url":
                                    SelectedQuestion.Urls.Add(Question.NewUrl());
                                    break;
                            }

                            IsDirty = true;
                        }
                    },
                    (tag) =>
                    {
                        return null != SelectedStory;
                    }));
            }
        }

        private RelayCommand _removeActionCommand;
        public ICommand RemoveActionCommand
        {
            get
            {
                return _removeActionCommand ?? (_removeActionCommand = new RelayCommand(
                    (tag) =>
                    {
                        if (null != SelectedStory)
                        {
                            if (tag is Story)
                                _stories.Remove(tag as Story);
                            else if (tag is Testcase && SelectedStory.TestCases.Count > 1)
                                SelectedStory.TestCases.Remove(tag as Testcase);
                            else if (tag is Keyidentifier && SelectedStory.KeyIdentifiers.Count > 1)
                                SelectedStory.KeyIdentifiers.Remove(tag as Keyidentifier);
                            else if (tag is Checkup && SelectedStory.Checkups.Count > 1)
                                SelectedStory.Checkups.Remove(tag as Checkup);
                            else if (tag is Issue && SelectedStory.Issues.Count > 1)
                                SelectedStory.Issues.Remove(tag as Issue);
                            else if (tag is Question && SelectedKeyidentifier.Questions.Count > 1)
                                SelectedKeyidentifier.Questions.Remove(tag as Question);
                            else if (tag is StringValue)
                            {
                                var item = tag as StringValue;

                                if (item.Type.Equals("Attachment") && SelectedStory.Attachments.Count > 1)
                                    SelectedStory.Attachments.Remove(item);
                                else if (item.Type.Equals("Query") && SelectedStory.Queries.Count > 1)
                                    SelectedStory.Queries.Remove(item);
                                else if (item.Type.Equals("Script") && SelectedStory.Scripts.Count > 1)
                                    SelectedStory.Scripts.Remove(item);
                                else if (item.Type.Equals("AcceptanceCriteria") && SelectedStory.AcceptanceCriteria.Count > 1)
                                    SelectedStory.AcceptanceCriteria.Remove(item);
                                else if (item.Type.Equals("DeveloperCriteria") && SelectedStory.DeveloperCriteria.Count > 1)
                                    SelectedStory.DeveloperCriteria.Remove(item);
                                else if (item.Type.Equals("Url") && SelectedQuestion.Urls.Count > 1)
                                    SelectedQuestion.Urls.Remove(item);
                                else if (item.Type.Equals("Step") && SelectedTestcase.Steps.Count > 1)
                                    SelectedTestcase.Steps.Remove(item);
                                else if (item.Type.Equals("KeyIdentifierId") && SelectedTestcase.KeyIdentifierIds.Count > 1)
                                    SelectedTestcase.KeyIdentifierIds.Remove(item);
                            }

                            IsDirty = true;
                        }
                    },
                    (tag) =>
                    {
                        return null != SelectedStory;
                    }));
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
                OnPropertyChanged("BusyVisibility");
            }
        }

        public Visibility BusyVisibility
        {
            get { return _isBusy ? Visibility.Visible : Visibility.Collapsed; }
        }

        #region property changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion 
    }
}
