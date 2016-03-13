using Interviewer.Common;
using Interviewer.Data;
using Interviewer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Interviewer
{
    public class MainViewModel : BaseModel
	{
		private class RunTestCommand : ICommand
		{
			public event EventHandler CanExecuteChanged;

			public bool CanExecute(object parameter)
			{
				return !MainViewModel.ViewModel.IsRunning;
			}

			public void Execute(object parameter)
			{
				MainViewModel.ViewModel.RunQuestions(int.Parse(parameter.ToString()));
			}
		}

		private class RunQuestionCommand : ICommand
		{
			private readonly int _rating;

			public event EventHandler CanExecuteChanged;

			public RunQuestionCommand(int rating)
			{
				_rating = rating;
			}

			public bool CanExecute(object parameter)
			{
				return MainViewModel.ViewModel.SelectedQuestion != null;
			}

			public void Execute(object parameter)
			{
				MainViewModel.ViewModel.QuestionAnswered(_rating, parameter as Question);
			}
		}

        private class RunShowAddQuestionsCommand : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public RunShowAddQuestionsCommand()
            {
            }

            public bool CanExecute(object parameter)
            {
                return null != MainViewModel.ViewModel.SelectedArea;
            }

            public void Execute(object parameter)
            {
                //new AddQuestions().ShowDialog();
            }
        }

		private static MainViewModel _viewModel;

		private configuration _selectedConfiguration;
		private Profile _selectedProfile;
		private Platform _selectedPlatform;
		private KnowledgeArea _selectedKnowledgeArea;
		private Area _selectedArea;
		private Question _selectedQuestion;

		private int _maxQuestionsCount = 10;
		private int _questionsCount = 5;
		private bool _isLoaded = false;
		private bool _isRunning = false;

		private int _passedCount = 0;
		private int _failedCount = 0;
		private int _undefinedCount = 0;
        private int _totalQuestions = 0;

		private string _interviewedPerson;
        private string _apiBaseUrl = "http://localhost:52485/api/";

        public string ServicesUrl
        {
            get { return _apiBaseUrl; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Uri uri = null;
                    if (Uri.TryCreate(value, UriKind.Absolute, out uri))
                    {
                        _apiBaseUrl = value;
                        OnPropertyChanged();
                        SetServicesSetting(value);
                    }
                }
            }
        }

        private void SetServicesSetting(string url)
        {
            var settings = ApplicationData.Current.RoamingSettings;
            if (null == settings.Values["Services.Url"])
            {
                settings.Values.Add("Services.Url", url);
            }
            else
            {
                settings.Values["Services.Url"] = url;
            }
        }

        public MainViewModel()
        {
            var settings = ApplicationData.Current.RoamingSettings;
            if(null == settings.Values["Services.Url"])
            {
                settings.Values.Add("Services.Url", _apiBaseUrl);
            }
            _apiBaseUrl = (string)settings.Values["Services.Url"];
        }

        public static MainViewModel ViewModel
		{
			get
			{
			    if (null == _viewModel)
			    {
			        _viewModel = new MainViewModel();                    
                }
                return _viewModel;
			}
		}

        public override bool IsValid()
        {
            return SelectedConfiguration != null && SelectedConfiguration.IsValid();
        }

        public async Task<configuration> LoadConfiguration()
        {
            if (!_isLoaded)
            {
                using (var client = ApiServiceFactory.CreateService(_apiBaseUrl))
                {
                    SelectedConfiguration = await client.GetConfiguration();
                }

                RunQuestionsCommand.Execute(1);

                _isLoaded = true;
            }

            return SelectedConfiguration;
        }

        public async void SavePendingChanges()
        {
            if (SelectedConfiguration == null) return;

            var result = 0;
            foreach(var p in this.Platforms)
            {
                if(p.IsDirty && p.IsValid())
                {
                    if(p.Id == 0)
                    {
                        result = await InsertPlatform(p);
                        p.Id = result;
                    }
                    else
                    {
                        result = await UpdatePlatform(p);

                    }
                    p.IsDirty = result == 0;
                }

                foreach (var ka in p.KnowledgeArea)
                {
                    if(ka.IsDirty && ka.IsValid())
                    {
                        if(ka.Id == 0)
                        {
                            result = await InsertKnowledgeArea(ka);
                            ka.Id = result;
                        }
                        else
                        {
                            result = await UpdateKnowledgeArea(ka);
                        }
                        ka.IsDirty = result == 0;
                    }

                    foreach(var a in ka.Area)
                    {
                        if(a.IsDirty && a.IsValid())
                        {
                            if(a.Id == 0)
                            {
                                result = await InsertArea(a);
                                a.Id = result;
                            }
                            else
                            {
                                result = await UpdateArea(a);
                            }
                            a.IsDirty = result == 0;
                        }

                        foreach (var q in a.Question)
                        {
                            if(q.IsDirty && q.IsValid())
                            {
                                if(q.Id == 0)
                                {
                                    result = await InsertQuestion(q);
                                    q.Id = result;
                                }
                                else
                                {
                                    result = await UpdateQuestion(q);
                                }
                                q.IsDirty = result == 0;
                            }
                        }
                    }
                }
            }
        }
        
        public async Task<int> InsertPlatform(Platform item)
        {
            using (var client = ApiServiceFactory.CreateService(_apiBaseUrl))
            {
                return await client.AddItem(item);
            }
        }

        public async Task<int> UpdatePlatform(Platform item)
        {
            using (var client = ApiServiceFactory.CreateService(_apiBaseUrl))
            {
                return await client.UpdateItem(item);
            }
        }

        public async Task<int> DeletePlatform(Platform item)
        {
            using (var client = ApiServiceFactory.CreateService(_apiBaseUrl))
            {
                return await client.AddItem(item);
            }
        }

        public async Task<int> InsertKnowledgeArea(KnowledgeArea item)
        {
            using (var client = ApiServiceFactory.CreateService(_apiBaseUrl))
            {
                return await client.AddItem(item);
            }
        }

        public async Task<int> UpdateKnowledgeArea(KnowledgeArea item)
        {
            using (var client = ApiServiceFactory.CreateService(_apiBaseUrl))
            {
                return await client.UpdateItem(item);
            }
        }

        public async Task<int> DeleteKnowledgeArea(KnowledgeArea item)
        {
            using (var client = ApiServiceFactory.CreateService(_apiBaseUrl))
            {
                return await client.AddItem(item);
            }
        }

        public async Task<int> InsertArea(Area item)
        {
            using (var client = ApiServiceFactory.CreateService(_apiBaseUrl))
            {
                return await client.AddItem(item);
            }
        }

        public async Task<int> UpdateArea(Area item)
        {
            using (var client = ApiServiceFactory.CreateService(_apiBaseUrl))
            {
                return await client.UpdateItem(item);
            }
        }

        public async Task<int> DeleteArea(Area item)
        {
            using (var client = ApiServiceFactory.CreateService(_apiBaseUrl))
            {
                return await client.AddItem(item);
            }
        }

        public async Task<int> InsertQuestion(Question item)
        {
            using (var client = ApiServiceFactory.CreateService(_apiBaseUrl))
            {
                return await client.AddItem(item);
            }
        }

        public async Task<int> UpdateQuestion(Question item)
        {
            using (var client = ApiServiceFactory.CreateService(_apiBaseUrl))
            {
                return await client.UpdateItem(item);
            }
        }

        public async Task<int> DeleteQuestion(Question item)
        {
            using (var client = ApiServiceFactory.CreateService(_apiBaseUrl))
            {
                return await client.AddItem(item);
            }
        }

        public configuration SelectedConfiguration
		{
			get
			{
				return _selectedConfiguration;
			}
			set
			{
				_selectedConfiguration = value;

				OnPropertyChanged("SelectedConfiguration");
				OnPropertyChanged("Platforms");
				OnPropertyChanged("Profiles");
				OnPropertyChanged("QuestionsCountRange");
			}
		}

		public IEnumerable<Profile> Profiles
		{
			get
			{
                var profiles = SelectedConfiguration.Profile.Any()
                    ? SelectedConfiguration.Profile
                    : from p in SelectedConfiguration.Platform
                      from prof in p.Profile
                      from req in prof.Requirement
                      where req.PlatformId == p.Id
                      select prof;
                SelectedProfile = profiles.FirstOrDefault();
				return profiles.Distinct();
			}
		}

		public Profile SelectedProfile
		{
			get
			{
				return _selectedProfile;
			}
			set
			{
				_selectedProfile = value;
				OnPropertyChanged("SelectedProfile");
			}
		}

		public IEnumerable<Platform> Platforms
		{
			get
			{
                SelectedPlatform = SelectedConfiguration.Platform.FirstOrDefault();
				return SelectedConfiguration.Platform;
			}
		}

		public Platform SelectedPlatform
		{
			get
			{
				return _selectedPlatform;
			}
			set
			{
				_selectedPlatform = value;
				OnPropertyChanged("SelectedPlatform");
			    if (value == null) return;

				SelectedKnowledgeArea = value.KnowledgeArea.FirstOrDefault();
			}
		}

		public KnowledgeArea SelectedKnowledgeArea
		{
			get
			{
				return _selectedKnowledgeArea;
			}
			set
			{
				_selectedKnowledgeArea = value;
				OnPropertyChanged("SelectedKnowledgeArea");
			    if (value == null) return;

				SelectedArea = value.Area.FirstOrDefault();
			}
		}

		public Area SelectedArea
		{
			get
			{
				return _selectedArea;
			}
			set
			{
				_selectedArea = value;
				OnPropertyChanged("SelectedArea");
                if (value == null) return;
                
                SelectedQuestion = value.Question.FirstOrDefault();
			}
		}

		public Question SelectedQuestion
		{
			get
			{
				return _selectedQuestion;
			}
			set
			{
				_selectedQuestion = value;
				OnPropertyChanged("SelectedQuestion");
			}
		}

        #region get items

        public async Task<Platform> GetPlatform(int id)
        {
            return (await _viewModel.LoadConfiguration())
                .Platform.FirstOrDefault(x => x.Id == id);
        }

        public async Task<Profile> GetProfile(int id)
        {
            return (await _viewModel.LoadConfiguration())
                .Profile.FirstOrDefault(x => x.Id == id);
        }

        public async Task<KnowledgeArea> GetKnowledgeArea(int id)
        {
            return (from p in (await _viewModel.LoadConfiguration()).Platform
                    from ka in p.KnowledgeArea
                    where ka.Id == id
                    select ka).FirstOrDefault();
        }

        public async Task<Area> GetArea(int id)
        {
            return (from p in (await _viewModel.LoadConfiguration()).Platform
                    from ka in p.KnowledgeArea
                    from a in ka.Area
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public async Task<Question> GetQuestion(int id)
        {
            return (from p in (await _viewModel.LoadConfiguration()).Platform
                    from ka in p.KnowledgeArea
                    from a in ka.Area
                    from q in a.Question
                    where q.Id == id
                    select q).FirstOrDefault();
        }

        #endregion

        public int QuestionsCount
		{
			get
			{
				return _questionsCount;
			}
			set
			{
				_questionsCount = value;
				OnPropertyChanged("QuestionsCount");
			}
		}

		public IEnumerable<int> QuestionsCountRange
		{
			get
			{
				var list = new List<int>();
				for (var i = 1; i <= _maxQuestionsCount; i++)
				{
					list.Add(i);
				}
				return list;
			}
		}

		public bool IsRunning
		{
			get
			{
				return _isRunning;
			}
			set
			{
				_isRunning = value;
				OnPropertyChanged("IsRunning");
			}
		}

        private ICommand _runQuestionsCommand;
        public ICommand RunQuestionsCommand
		{
			get
			{
				return _runQuestionsCommand ?? (_runQuestionsCommand = new MainViewModel.RunTestCommand());
			}
		}

        private ICommand _questionUpCommand;
        public ICommand QuestionUpCommand
		{
			get
			{
				return _questionUpCommand ?? (_questionUpCommand = new MainViewModel.RunQuestionCommand(1));
			}
		}

        private ICommand _questionDownCommand;
        public ICommand QuestionDownCommand
		{
			get
			{
                return _questionDownCommand ?? (_questionDownCommand = new MainViewModel.RunQuestionCommand(0));
			}
		}

        private ICommand _questionUndefCommand;
        public ICommand QuestionUndefCommand
		{
			get
			{
                return _questionUndefCommand ?? (_questionUndefCommand = new MainViewModel.RunQuestionCommand(-1));
			}
		}

        private ICommand _showAddQuestionsCommand;
        public ICommand ShowAddQuestionsCommand
        {
            get
            {
                return _showAddQuestionsCommand ?? (_showAddQuestionsCommand = new MainViewModel.RunShowAddQuestionsCommand());
            }
        }

        private RelayCommand _addPlatform;
        public RelayCommand AddPlatform
        {
            get
            {
                return _addPlatform ?? (_addPlatform = new RelayCommand(
                    (object p) => {
                        MainViewModel.ViewModel.SelectedConfiguration.Platform
                            .Add(new Platform
                            {
                                Id = 0,
                                Name = "Undefined",
                                KnowledgeArea = new ObservableCollection<KnowledgeArea>(),
                                Profile = new ObservableCollection<Profile>(),
                                IsDirty = true
                            });
                    },
                    (object p) => MainViewModel.ViewModel.SelectedConfiguration != null
                    ));
            }
        }

        private RelayCommand _addKnowdlegeArea;
        public RelayCommand AddKnowledgeArea
        {
            get
            {
                return _addKnowdlegeArea ?? (_addKnowdlegeArea = new RelayCommand(
                    (object p) => {
                        MainViewModel.ViewModel.Platforms.First(x => x.Id == (int)p)
                            .KnowledgeArea
                            .Add(new KnowledgeArea {
                                PlatformId = MainViewModel.ViewModel.SelectedPlatform.Id,
                                Id = 0,
                                Name = "Undefined",
                                Area = new ObservableCollection<Area>(),
                                IsDirty = true                       
                            });
                    },
                    (object p) => MainViewModel.ViewModel.Platforms.Any(x => x.Id == (int)p)
                    ));
            }
        }

        private RelayCommand _addArea;
        public RelayCommand AddArea
        {
            get
            {
                return _addArea ?? (_addArea = new RelayCommand(
                    (object p) =>  {
                        (from pf in MainViewModel.ViewModel.Platforms
                         from ka in pf.KnowledgeArea
                         where ka.Id == (int)p
                         select ka)
                            .First().Area
                            .Add(new Area
                            {
                                KnowledgeAreaId = MainViewModel.ViewModel.SelectedKnowledgeArea.Id,
                                Id = 0,
                                Name = "Undefined",
                                Question = new ObservableCollection<Question>(),
                                IsDirty = true
                            });
                    },
                    (object p) => (int)p > 0
                    ));
            }
        }

        private RelayCommand _addQuestion;
        public RelayCommand AddQuestion
        {
            get
            {
                return _addQuestion ?? (_addQuestion = new RelayCommand(
                    (object p) => {
                        (from pf in MainViewModel.ViewModel.Platforms
                         from ka in pf.KnowledgeArea
                         from a in ka.Area
                         where a.Id == (int)p
                         select a)
                            .First().Question
                            .Add(new Question
                            {
                                AreaId = MainViewModel.ViewModel.SelectedArea.Id,
                                Id = 0,
                                Name = "Undefined",
                                Value = "Undefined",
                                Level = 1,
                                Weight = 1,
                                IsDirty = true
                            });
                    },
                    (object p) => (int)p > 0
                    ));
            }
        }

        private bool _isEditingPlatformProps = false;
        public bool IsEditingPlatformProps {
            get { return _isEditingPlatformProps; }
            set
            {
                _isEditingPlatformProps = value;
                OnPropertyChanged();
                OnPropertyChanged("PlatformEditVisibility");
            }
        }
        public Visibility PlatformEditVisibility
        {
            get { return _isEditingPlatformProps ? Visibility.Visible : Visibility.Collapsed; }
        }

        private RelayCommand _editingPlatformProps;
        public RelayCommand EditingPlatformProps
        {
            get
            {
                return _editingPlatformProps ?? (_editingPlatformProps = new RelayCommand(
                    (object p) => {
                        MainViewModel.ViewModel.IsEditingPlatformProps = !MainViewModel.ViewModel.IsEditingPlatformProps;
                    },
                    (object p) => MainViewModel.ViewModel.SelectedPlatform != null
                    ));
            }
        }

        private bool _isEditingKnowledgeAreaProps = false;
        public bool IsEditingKnowledgeAreaProps
        {
            get { return _isEditingKnowledgeAreaProps; }
            set
            {
                _isEditingKnowledgeAreaProps = value;
                OnPropertyChanged();
                OnPropertyChanged("KnowledgeAreaEditVisibility");
            }
        }
        public Visibility KnowledgeAreaEditVisibility
        {
            get { return _isEditingKnowledgeAreaProps ? Visibility.Visible : Visibility.Collapsed; }
        }

        private RelayCommand _editingKnowledgeAreaProps;
        public RelayCommand EditingKnowledgeAreaProps
        {
            get
            {
                return _editingKnowledgeAreaProps ?? (_editingKnowledgeAreaProps = new RelayCommand(
                    (object p) => {
                        MainViewModel.ViewModel.IsEditingKnowledgeAreaProps= !MainViewModel.ViewModel.IsEditingKnowledgeAreaProps;
                    },
                    (object p) => MainViewModel.ViewModel.SelectedKnowledgeArea != null
                    ));
            }
        }

        private bool _isEditingAreaProps = false;
        public bool IsEditingAreaProps
        {
            get { return _isEditingAreaProps; }
            set
            {
                _isEditingAreaProps = value;
                OnPropertyChanged();
                OnPropertyChanged("AreaEditVisibility");
            }
        }
        public Visibility AreaEditVisibility
        {
            get { return _isEditingAreaProps ? Visibility.Visible : Visibility.Collapsed; }
        }

        private RelayCommand _editingAreaProps;
        public RelayCommand EditingAreaProps
        {
            get
            {
                return _editingAreaProps ?? (_editingAreaProps = new RelayCommand(
                    (object p) => {
                        MainViewModel.ViewModel.IsEditingAreaProps = !MainViewModel.ViewModel.IsEditingAreaProps;
                    },
                    (object p) => MainViewModel.ViewModel.SelectedArea != null
                    ));
            }
        }

        private bool _isEditingQuestionProps = false;
        public bool IsEditingQuestionProps
        {
            get { return _isEditingQuestionProps; }
            set
            {
                _isEditingQuestionProps = value;
                OnPropertyChanged();
                OnPropertyChanged("QuestionEditVisibility");
            }
        }
        public Visibility QuestionEditVisibility
        {
            get { return _isEditingQuestionProps ? Visibility.Visible : Visibility.Collapsed; }
        }

        private RelayCommand _editingQuestionProps;
        public RelayCommand EditingQuestionProps
        {
            get
            {
                return _editingQuestionProps ?? (_editingQuestionProps = new RelayCommand(
                    (object p) => {
                        MainViewModel.ViewModel.IsEditingQuestionProps = !MainViewModel.ViewModel.IsEditingQuestionProps;
                    },
                    (object p) => MainViewModel.ViewModel.SelectedQuestion != null
                    ));
            }
        }

        public int PassedCount
		{
			get
			{
				return _passedCount;
			}
			set
			{
				_passedCount = value;
				OnPropertyChanged("PassedCount");
                OnPropertyChanged("AppliedQuestions");
            }
		}

		public int FailedCount
		{
			get
			{
				return _failedCount;
			}
			set
			{
				_failedCount = value;
				OnPropertyChanged("FailedCount");
                OnPropertyChanged("AppliedQuestions");
            }
		}

		public int UndefinedCount
		{
			get
			{
				return _undefinedCount;
			}
			set
			{
				_undefinedCount = value;
				OnPropertyChanged("UndefinedCount");
                OnPropertyChanged("AppliedQuestions");
            }
		}

        public int TotalQuestions
        {
            get
            {
                return _totalQuestions;
            }
            set
            {
                _totalQuestions = value;
                PassedCount = 0;
                FailedCount = 0;
                UndefinedCount = 0;
                OnPropertyChanged("TotalQuestions");
                OnPropertyChanged("AppliedQuestions");
            }
        }

        public int AppliedQuestions
        {
            get
            {
                return Math.Min(TotalQuestions, PassedCount + FailedCount + UndefinedCount + 1);
            }
        }

        public string InterviewedPerson
		{
			get
			{
				return _interviewedPerson;
			}
			set
			{
				_interviewedPerson = value;
				OnPropertyChanged("InterviewedPerson");
			}
		}		

		private void QuestionAnswered(int rating, Question question)
		{
            SelectedQuestion = question;
            SelectedQuestion.AlreadyAnswered = true;
            SelectedQuestion.rating = rating;
			switch (rating)
			{
			case -1:
				UndefinedCount++;
				break;
			case 0:
				FailedCount++;
				break;
			case 1:
				PassedCount++;
				break;
			}

            SelectedQuestion = GetNextQuestion();
		}

		private void RunQuestions(int mode)
		{
			IsRunning = true;
			try
			{
				if (mode == 1) //get random questions
				{
					ToogleAnsweredFlag(true);

				    var questions = from p in Platforms
				        from ka in p.KnowledgeArea
				        from a in ka.Area
				        select a.Question;

				    foreach (var q in questions.Where(x => x.Count() > 0))
				    {
				        var randomIndexes = GetRandomIndexes(q.Count());
				        foreach (var i in randomIndexes)
				        {
				            q.ElementAt(i).AlreadyAnswered = false;
				        }
				    }

                    TotalQuestions = GetPendingQuestions();
				}
				else
				{
					ToogleAnsweredFlag(false);
				}
				OnPropertyChanged("Platforms");

                if(mode == 1)
                {
                    //var window = new AskQuestions();
                    //window.ShowDialog();
                }
			}
			finally
			{
				IsRunning = false;
			}
		}

	    private IEnumerable<int> GetRandomIndexes(int itemsCount)
	    {
            var rand = new Random(100);
	        var list = new List<int>();
	        if (itemsCount <= QuestionsCount)
	        {
                for(var i=0;i<itemsCount;i++)
                    list.Add(i);
	        }
	        else
	        {
	            if (Math.Floor(Convert.ToDecimal(itemsCount)/Convert.ToDecimal(QuestionsCount)) >= 2M)
	            {
	                while (list.Count < Math.Min(QuestionsCount, itemsCount))
	                {
	                    var randIndex = rand.Next(0, itemsCount - 1);
	                    if (!list.Contains(randIndex))
	                        list.Add(randIndex);
	                }
	            }
	            else
	            {
	                var dismissList = new List<int>();
	                for (var i = 0; i < itemsCount - QuestionsCount; i++)
	                {
                        var randIndex = rand.Next(0, itemsCount - 1);
                        if (!dismissList.Contains(randIndex))
                            dismissList.Add(randIndex);
	                }

                    for (var i = 0; i < itemsCount; i++)
                        if(!dismissList.Contains(i))
                            list.Add(i);
	            }
	        }

	        return list.ToArray();
	    }

		private void ToogleAnsweredFlag(bool answered)
		{
			foreach (var q2 in from p in Platforms
			    from ka in p.KnowledgeArea
			    from a in ka.Area
			    from q in a.Question
			select q)
			{
				q2.AlreadyAnswered = answered;
			}
		}

        private int GetPendingQuestions()
        {
            var pendingQuestions = 0;
            foreach (var q2 in from p in Platforms
                               from ka in p.KnowledgeArea
                               from a in ka.Area
                               from q in a.Question
                               select q)
            {
                pendingQuestions += q2.AlreadyAnswered ? 0 : 1;
            }

            return pendingQuestions;
        }

        private Question GetNextQuestion()
        {
            foreach(var p in Platforms)
            {
                foreach(var ka in p.KnowledgeArea)
                {
                    foreach(var a in ka.Area)
                    {
                        foreach(var q in a.Question)
                        {
                            if (!q.AlreadyAnswered)
                            {
                                if (p != SelectedPlatform)
                                    SelectedPlatform = p;

                                if (ka != SelectedKnowledgeArea)
                                    SelectedKnowledgeArea = ka;

                                if (a != SelectedArea)
                                    SelectedArea = a;

                                return q;
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}
