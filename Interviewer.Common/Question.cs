using System.Xml.Serialization;

namespace Interviewer.Common
{
	public class Question : BaseModel
	{
        private int areaIdField;
		private int weightField = 0;
		private int levelField = 1;
		private string valueField;

		private bool alreadyAnsweredField;
		private int ratingField;

        [XmlAttribute]
        public int AreaId
        {
            get
            {
                return this.areaIdField;
            }
            set
            {
                this.areaIdField = value;
            }
        }

        [XmlAttribute]
		public int Weight
		{
			get
			{
				return this.weightField;
			}
			set
			{
				this.weightField = value;
			}
		}

		[XmlAttribute]
		public int Level
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

		[XmlAttribute]
		public string Value
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

		//[XmlIgnore]
		//public ICommand QuestionUpCommand
		//{
		//	get
		//	{
		//		return MainViewModel.ViewModel.QuestionUpCommand;
		//	}
		//}

		//[XmlIgnore]
		//public ICommand QuestionDownCommand
		//{
		//	get
		//	{
		//		return MainViewModel.ViewModel.QuestionDownCommand;
		//	}
		//}

		//[XmlIgnore]
		//public ICommand QuestionUndefCommand
		//{
		//	get
		//	{
		//		return MainViewModel.ViewModel.QuestionUndefCommand;
		//	}
		//}
	}
}
