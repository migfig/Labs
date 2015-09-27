using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Xml.Serialization;
using WpfInterviewer.Views;

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
				return true;
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
                new AddQuestions().ShowDialog();
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

		private ICommand _runQuestionsCommand;

		private ICommand _questionUpCommand;

		private ICommand _questionDownCommand;

		private ICommand _questionUndefCommand;

		private int _passedCount = 0;

		private int _failedCount = 0;

		private int _undefinedCount = 0;

		private string _interviewedPerson;

		public static MainViewModel ViewModel
		{
			get
			{
			    if (null == _viewModel)
			    {
			        _viewModel = new MainViewModel();
			        _viewModel.LoadConfiguration(
                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                        ConfigurationManager.AppSettings["DefaultConfiguration"])
                    );
			    }
                return _viewModel;
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
				var profiles = from i in SelectedConfiguration.Items
				    let p = i as Profile
				    where p != null
				    select p;
				
                SelectedProfile = profiles.FirstOrDefault();
				return profiles;
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
				var platforms = from i in SelectedConfiguration.Items
				    let p = i as Platform
				    where p != null
				    select p;
				
                SelectedPlatform = platforms.FirstOrDefault();
				return platforms;
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

				SelectedKnowledgeArea = value.knowledgeArea.FirstOrDefault();
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

				SelectedArea = value.area.FirstOrDefault();
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
                
                SelectedQuestion = value.question.FirstOrDefault();
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

		public ICommand RunQuestionsCommand
		{
			get
			{
				return _runQuestionsCommand ?? new MainViewModel.RunTestCommand();
			}
		}

		public ICommand QuestionUpCommand
		{
			get
			{
				return _questionUpCommand ?? new MainViewModel.RunQuestionCommand(1);
			}
		}

		public ICommand QuestionDownCommand
		{
			get
			{
                return _questionDownCommand ?? new MainViewModel.RunQuestionCommand(0);
			}
		}

		public ICommand QuestionUndefCommand
		{
			get
			{
                return _questionUndefCommand ?? new MainViewModel.RunQuestionCommand(-1);
			}
		}

        private ICommand _showAddQuestionsCommand;
        public ICommand ShowAddQuestionsCommand
        {
            get
            {
                return _showAddQuestionsCommand ?? new MainViewModel.RunShowAddQuestionsCommand();
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

		private void LoadConfiguration(string fileName)
		{
			if (!_isLoaded)
			{
				if (File.Exists(fileName))
				{
					var ser = new XmlSerializer(typeof(configuration));
					using (var stream = new StreamReader(fileName))
					{
						SelectedConfiguration = (configuration)ser.Deserialize(stream);
					}
					_isLoaded = true;
				}
			}
		}

		private void QuestionAnswered(int rating, Question question)
		{
			question.AlreadyAnswered = true;
			question.rating = rating;
			SelectedQuestion = question;
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
		}

		private void RunQuestions(int mode)
		{
			IsRunning = true;
			try
			{
				if (mode == 1)
				{
					ToogleAnsweredFlag(true);

				    var questions = from p in Platforms
				        from ka in p.knowledgeArea
				        from a in ka.area
				        select a.question;

				    foreach (var q in questions.Where(x => x.Count > 0))
				    {
				        var randomIndexes = GetRandomIndexes(q.Count);
				        foreach (var i in randomIndexes)
				        {
				            q.ElementAt(i).AlreadyAnswered = false;
				        }
				    }
				}
				else
				{
					ToogleAnsweredFlag(false);
				}
				OnPropertyChanged("Platforms");
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
			    from ka in p.knowledgeArea
			    from a in ka.area
			    from q in a.question
			select q)
			{
				q2.AlreadyAnswered = answered;
			}
		}
	}
}
