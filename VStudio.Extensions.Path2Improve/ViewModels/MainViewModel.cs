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
using Windows.ApplicationModel;
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
            _fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "stories.json");
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
                    return new ObservableCollection<Story>();
                }
            }
        }

        private void GetStories()
        {
            if (!DesignMode.DesignModeEnabled)
            {
                _stories = JsonConvert.DeserializeObject<ObservableCollection<Story>>(File.ReadAllText(_fileName));
            }
            else
            {
                #region dummy entries

                _stories = new ObservableCollection<Story>
                    {
                        new Story
                        {
                            Name = "ID-1001",
                            Title ="Create cloud items",
                            Description = "Create items from provided values",
                            DateStarted = DateTime.UtcNow,
                            DateEnded = DateTime.MinValue,
                            Status = StoryStatus.NotStarted,
                            Url = new Uri("http://localhost:3033/jira/story/1001", UriKind.Absolute),
                            ParentStoryUrl = new Uri("http://localhost:3033/jira/story/1000", UriKind.Absolute),
                            Attachments = new ObservableCollection<StringValue>
                            {
                                new StringValue(new Uri("http://localhost:3033/jira/story/1001/attachment/1", UriKind.Absolute).ToString())
                            },
                            KeyIdentifiers = new ObservableCollection<Keyidentifier>
                            {
                                new Keyidentifier
                                {
                                    Id = Guid.NewGuid(),
                                    Description = "Create cloud item",
                                    Category = IdentifierCategory.Deliverable,
                                    Questions = new ObservableCollection<Question>
                                    {
                                        new Question
                                        {
                                            Ask = "What's the exact url route where created item will be placed",
                                            Answer = "Goes on the cloud items endpoint",
                                            Urls = new ObservableCollection<StringValue>
                                            {
                                                new StringValue(new Uri("http://localhost:3033/cloud/items", UriKind.Absolute).ToString())
                                            }
                                        }
                                    }
                                }
                            },
                            Checkups = new ObservableCollection<Checkup>
                            {
                                new Checkup
                                {
                                    Description = "Is delivered code clear/clean/object oriented",
                                    Applied = false,
                                    DateApplied = DateTime.MinValue
                                },
                                new Checkup
                                {
                                    Description = "Has delivered code being tested against production data variations/volume",
                                    Applied = false,
                                    DateApplied = DateTime.MinValue
                                }
                            },
                            Queries = new ObservableCollection<StringValue>
                            {
                                new StringValue(new Uri("file://C:/code/data/test.sql", UriKind.Absolute).ToString())
                            },
                            Scripts = new ObservableCollection<StringValue>
                            {
                                new StringValue(new Uri("file://C:/code/scripts/test.cs", UriKind.Absolute).ToString())
                            },
                            TestCases = new ObservableCollection<Testcase>
                            {
                                new Testcase
                                {
                                    Description = "Validate creation of cloud item with incomplete data",
                                    Applied = false,
                                    DateApplied = DateTime.MinValue,
                                    Status = TestcaseStatus.Pending,
                                    Steps = new ObservableCollection<StringValue>
                                    {
                                        new StringValue("Provide cloud item payload with incomplete properties")
                                    },
                                    KeyIdentifierIds = new ObservableCollection<StringValue>()
                                }
                            },
                            Issues = new ObservableCollection<Issue>(),
                            AcceptanceCriteria = new ObservableCollection<StringValue>(),
                            DeveloperCriteria = new ObservableCollection<StringValue>()
                        },
                    };

                _stories.First().TestCases.First().KeyIdentifierIds.Add(new StringValue(_stories.First().KeyIdentifiers.First().Id.ToString()));
                #endregion

                SetStories();
                OnPropertyChanged("Stories");
            }
        }

        private void SetStories()
        {
            File.WriteAllText(_fileName, JsonConvert.SerializeObject(_stories));
            IsDirty = false;
        }

        private Story GetDefaultStory()
        {
            var id = Guid.NewGuid();
            return new Story
            {
                Name = "",
                Title = "",
                Description = "",
                DateStarted = DateTime.UtcNow,
                DateEnded = DateTime.MinValue,
                Status = StoryStatus.NotStarted,
                Url = new Uri("http://localhost:8080/story/0001", UriKind.Absolute),
                ParentStoryUrl = new Uri("http://localhost:8080/story/0000", UriKind.Absolute),
                Attachments = new ObservableCollection<StringValue> { new StringValue("http://localhost") },
                KeyIdentifiers = new ObservableCollection<Keyidentifier>
                {
                    new Keyidentifier
                    {
                        Category = IdentifierCategory.Deliverable,
                        Description = "",
                        Id = id,
                        Questions = new ObservableCollection<Question>
                        {
                            new Question
                            {
                                Ask = "",
                                Answer = "",
                                Urls = new ObservableCollection<StringValue>{new StringValue("http://localhost")}
                            }
                        }
                    }
                },
                Checkups = new ObservableCollection<Checkup>
                    {
                        new Checkup
                        {
                            Description = "Is delivered code clear/clean/object oriented",
                            DateApplied = DateTime.MinValue
                        },
                        new Checkup
                        {
                            Description = "Has delivered code being tested against production data variations/volume",
                            DateApplied = DateTime.MinValue
                        }
                    },
                Queries = new ObservableCollection<StringValue> { new StringValue("file://c:\\") },
                Scripts = new ObservableCollection<StringValue> { new StringValue("file://c:\\") },
                TestCases = new ObservableCollection<Testcase>
                {
                    new Testcase
                    {
                        Description = "",
                        DateApplied = DateTime.MinValue,
                        Status = TestcaseStatus.Pending,
                        Steps = new ObservableCollection<StringValue>{ new StringValue("") },
                        KeyIdentifierIds = new ObservableCollection<StringValue>{new StringValue(id.ToString())}
                    }
                },
                Issues = new ObservableCollection<Issue>
                {
                    new Issue
                    {
                        Description = "",
                        DateClosed = DateTime.MinValue,
                        IsOpen = true
                    }
                },
                AcceptanceCriteria = new ObservableCollection<StringValue> { new StringValue("") },
                DeveloperCriteria = new ObservableCollection<StringValue>{ new StringValue("") }
            };
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
                        var newStory = GetDefaultStory();
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
                        return IsDirty && Stories != null && Stories.Any();
                    }));
            }
        }

        private RelayCommand _addKeyIdentifierCommand;
        public ICommand AddKeyIdentifierCommand
        {
            get
            {
                return _addKeyIdentifierCommand ?? (_addKeyIdentifierCommand = new RelayCommand(
                    (tag) =>
                    {
                        if (null != SelectedStory)
                        {
                            SelectedStory.KeyIdentifiers.Add(new Keyidentifier {
                                Id = Guid.NewGuid(),
                                Description = "",
                                Questions = new ObservableCollection<Question>(),
                                Category = IdentifierCategory.Deliverable
                            });
                        }
                    },
                    (tag) =>
                    {
                        return null != SelectedStory;
                    }));
            }
        }

        private RelayCommand _addKeyIdentifierIdCommand;
        public ICommand AddKeyIdentifierIdCommand
        {
            get
            {
                return _addKeyIdentifierIdCommand ?? (_addKeyIdentifierIdCommand = new RelayCommand(
                    (tag) =>
                    {
                        if (null != SelectedTestcase)
                        {
                            SelectedTestcase.KeyIdentifierIds.Add(new StringValue(SelectedStory.KeyIdentifiers.FirstOrDefault().Id.ToString()));
                        }
                    },
                    (tag) =>
                    {
                        return null != SelectedTestcase;
                    }));
            }
        }

        private RelayCommand _addTestcaseCommand;
        public ICommand AddTestcaseCommand
        {
            get
            {
                return _addTestcaseCommand ?? (_addTestcaseCommand = new RelayCommand(
                    (tag) =>
                    {
                        if (null != SelectedStory)
                        {
                            SelectedStory.TestCases.Add(new Testcase
                            {
                                DateApplied = DateTime.MinValue,
                                Description = "",
                                KeyIdentifierIds = new ObservableCollection<StringValue>(SelectedStory.KeyIdentifiers.Select(i => new StringValue(i.Id.ToString()))),
                                Steps = new ObservableCollection<StringValue>(),
                                Status = TestcaseStatus.Pending
                            });
                        }
                    },
                    (tag) =>
                    {
                        return null != SelectedStory;
                    }));
            }
        }

        private RelayCommand _addCheckupCommand;
        public ICommand AddCheckupCommand
        {
            get
            {
                return _addCheckupCommand ?? (_addCheckupCommand = new RelayCommand(
                    (tag) =>
                    {
                        if (null != SelectedStory)
                        {
                            SelectedStory.Checkups.Add(new Checkup {
                                DateApplied = DateTime.MinValue,
                                Description = ""
                            });
                        }
                    },
                    (tag) =>
                    {
                        return null != SelectedStory;
                    }));
            }
        }

        private RelayCommand _addQuestionCommand;
        public ICommand AddQuestionCommand
        {
            get
            {
                return _addQuestionCommand ?? (_addQuestionCommand = new RelayCommand(
                    (tag) =>
                    {
                        if (null != SelectedKeyidentifier)
                        {
                            SelectedKeyidentifier.Questions.Add(new Question());
                        }
                    },
                    (tag) =>
                    {
                        return null != SelectedKeyidentifier;
                    }));
            }
        }

        private RelayCommand _addStepCommand;
        public ICommand AddStepCommand
        {
            get
            {
                return _addStepCommand ?? (_addStepCommand = new RelayCommand(
                    (tag) =>
                    {
                        if (null != SelectedTestcase)
                        {
                            SelectedTestcase.Steps.Add(new StringValue(""));
                        }
                    },
                    (tag) =>
                    {
                        return null != SelectedTestcase;
                    }));
            }
        }

        private RelayCommand _addAttachmentCommand;
        public ICommand AddAttachmentCommand
        {
            get
            {
                return _addAttachmentCommand ?? (_addAttachmentCommand = new RelayCommand(
                    (tag) =>
                    {
                        if (null != SelectedStory)
                        {
                            SelectedStory.Attachments.Add(new StringValue(new Uri("http://localhost").ToString()));
                        }
                    },
                    (tag) =>
                    {
                        return null != SelectedStory;
                    }));
            }
        }

        private RelayCommand _addScriptCommand;
        public ICommand AddScriptCommand
        {
            get
            {
                return _addScriptCommand ?? (_addScriptCommand = new RelayCommand(
                    (tag) =>
                    {
                        if (null != SelectedStory)
                        {
                            SelectedStory.Scripts.Add(new StringValue(new Uri("file://").ToString()));
                        }
                    },
                    (tag) =>
                    {
                        return null != SelectedStory;
                    }));
            }
        }

        private RelayCommand _addQueryCommand;
        public ICommand AddQueryCommand
        {
            get
            {
                return _addQueryCommand ?? (_addQueryCommand = new RelayCommand(
                    (tag) =>
                    {
                        if (null != SelectedStory)
                        {
                            SelectedStory.Queries.Add(new StringValue(new Uri("file://").ToString()));
                        }
                    },
                    (tag) =>
                    {
                        return null != SelectedStory;
                    }));
            }
        }

        private RelayCommand _addIssueCommand;
        public ICommand AddIssueCommand
        {
            get
            {
                return _addIssueCommand ?? (_addIssueCommand = new RelayCommand(
                    (tag) =>
                    {
                        if (null != SelectedStory)
                        {
                            SelectedStory.Issues.Add(new Issue
                            {
                                IsOpen = true,
                                DateClosed = DateTime.MinValue
                            });
                        }
                    },
                    (tag) =>
                    {
                        return null != SelectedStory;
                    }));
            }
        }

        private RelayCommand _addAcceptanceCommand;
        public ICommand AddAcceptanceCommand
        {
            get
            {
                return _addAcceptanceCommand ?? (_addAcceptanceCommand = new RelayCommand(
                    (tag) =>
                    {
                        if (null != SelectedStory)
                        {
                            SelectedStory.AcceptanceCriteria.Add(new StringValue(""));
                        }
                    },
                    (tag) =>
                    {
                        return null != SelectedStory;
                    }));
            }
        }

        private RelayCommand _addDeveloperCommand;
        public ICommand AddDeveloperCommand
        {
            get
            {
                return _addDeveloperCommand ?? (_addDeveloperCommand = new RelayCommand(
                    (tag) =>
                    {
                        if (null != SelectedStory)
                        {
                            SelectedStory.DeveloperCriteria.Add(new StringValue(""));
                        }
                    },
                    (tag) =>
                    {
                        return null != SelectedStory;
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
                                    AddTestcaseCommand.Execute(tag);
                                    break;
                                case "Question":
                                    AddQuestionCommand.Execute(tag);
                                    break;
                                case "Checkup":
                                    AddCheckupCommand.Execute(tag);
                                    break;
                                case "KeyIdentifier":
                                    AddKeyIdentifierCommand.Execute(tag);
                                    break;
                                case "Step":
                                    AddStepCommand.Execute(tag);
                                    break;
                                case "KeyIdentifierId":
                                    AddKeyIdentifierIdCommand.Execute(tag);
                                    break;
                                case "Attachment":
                                    AddAttachmentCommand.Execute(tag);
                                    break;
                                case "Query":
                                    AddQueryCommand.Execute(tag);
                                    break;
                                case "Script":
                                    AddScriptCommand.Execute(tag);
                                    break;
                                case "Issue":
                                    AddIssueCommand.Execute(tag);
                                    break;
                                case "Acceptance":
                                    AddAcceptanceCommand.Execute(tag);
                                    break;
                                case "Developer":
                                    AddDeveloperCommand.Execute(tag);
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
                            switch (tag.ToString())
                            {
                                case "Testcase":
                                    AddTestcaseCommand.Execute(tag);
                                    break;
                                case "Question":
                                    AddQuestionCommand.Execute(tag);
                                    break;
                                case "Checkup":
                                    AddCheckupCommand.Execute(tag);
                                    break;
                                case "KeyIdentifier":
                                    AddKeyIdentifierCommand.Execute(tag);
                                    break;
                                case "Step":
                                    AddStepCommand.Execute(tag);
                                    break;
                                case "KeyIdentifierId":
                                    AddKeyIdentifierIdCommand.Execute(tag);
                                    break;
                                case "Attachment":
                                    AddAttachmentCommand.Execute(tag);
                                    break;
                                case "Query":
                                    AddQueryCommand.Execute(tag);
                                    break;
                                case "Script":
                                    AddScriptCommand.Execute(tag);
                                    break;
                                case "Issue":
                                    AddIssueCommand.Execute(tag);
                                    break;
                                case "Acceptance":
                                    AddAcceptanceCommand.Execute(tag);
                                    break;
                                case "Developer":
                                    AddDeveloperCommand.Execute(tag);
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
