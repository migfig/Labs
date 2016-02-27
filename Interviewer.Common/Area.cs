using System.Collections.ObjectModel;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Interviewer.Common
{
	public class Area : BaseModel
	{
		private ObservableCollection<Question> questionField;

		private string nameField;

		[XmlElement("question", Form = XmlSchemaForm.Unqualified, IsNullable = true)]
		public ObservableCollection<Question> question
		{
			get
			{
				return this.questionField;
			}
			set
			{
				this.questionField = value;
			}
		}

		[XmlAttribute]
		public string name
		{
			get
			{
				return this.nameField;
			}
			set
			{
				this.nameField = value;
			}
		}

		public Area()
		{
			this.questionField = new ObservableCollection<Question>();
		}
	}
}
