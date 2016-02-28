using Interviewer.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfInterviewer
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

        public async void LoadConfiguration()
        {
            LoadConfiguration(await GetXmlText());
        }

        private void LoadConfiguration(string text)
        {
            if (!_isLoaded)
            {
                var ser = new DataContractJsonSerializer(typeof(configuration));
                var stream = new MemoryStream(UTF8Encoding.UTF8.GetBytes(text));
                SelectedConfiguration = (configuration)ser.ReadObject(stream);
                
                RunQuestionsCommand.Execute(1);
                
                _isLoaded = true;
            }
        }

        private static async Task<string> GetXmlText()
        {                       
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                var response = await client.GetAsync("http://localhost:52485/api/configuration");
                if (response.IsSuccessStatusCode)
                {
                    var text = await response.Content.ReadAsStringAsync();

                    return text;
                }
            }        

            return string.Empty;
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
                SelectedProfile = SelectedConfiguration.Profile.FirstOrDefault();
				return SelectedConfiguration.Profile.AsEnumerable();
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
