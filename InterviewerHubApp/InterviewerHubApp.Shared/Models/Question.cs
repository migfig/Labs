using System.Windows.Input;
using System.Xml.Serialization;

namespace WpfInterviewer
{
	public class Question : BaseModel
	{
		private int valueField = 0;

		private int levelField = 1;

		private string valueField1;

		private bool alreadyAnsweredField;

		private int ratingField;

		[XmlAttribute]
		public int value
		{
			get
			{
				return this.valueField;
			}
			set
			{
				this.valueField = value;
			}
		}

		[XmlAttribute]
		public int level
		{
			get
			{
				return this.levelField;
			}
			set
			{
				this.levelField = value;
			}
		}

		[XmlText]
		public string Value
		{
			get
			{
				return this.valueField1;
			}
			set
			{
				this.valueField1 = value;
			}
		}

		[XmlIgnore]
		public bool AlreadyAnswered
		{
			get
			{
				return this.alreadyAnsweredField;
			}
			set
			{
				this.alreadyAnsweredField = value;
				this.OnPropertyChanged("AlreadyAnswered");
			}
		}

		[XmlIgnore]
		public int rating
		{
			get
			{
				return this.ratingField;
			}
			set
			{
				this.ratingField = value;
			}
		}

		[XmlIgnore]
		public ICommand QuestionUpCommand
		{
			get
			{
				return MainViewModel.ViewModel.QuestionUpCommand;
			}
		}

		[XmlIgnore]
		public ICommand QuestionDownCommand
		{
			get
			{
				return MainViewModel.ViewModel.QuestionDownCommand;
			}
		}

		[XmlIgnore]
		public ICommand QuestionUndefCommand
		{
			get
			{
				return MainViewModel.ViewModel.QuestionUndefCommand;
			}
		}
	}
}
