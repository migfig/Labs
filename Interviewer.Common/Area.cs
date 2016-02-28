using System.Collections.ObjectModel;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Interviewer.Common
{
	public class Area : BaseModel
	{
		private ObservableCollection<Question> questionField;

		private int knowledgeAreaIdField;

		[XmlElement("Question", Form = XmlSchemaForm.Unqualified, IsNullable = true)]
		public ObservableCollection<Question> Question
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
		public int KnowledgeAreaId
        {
			get
			{
				return this.knowledgeAreaIdField;
			}
			set
			{
				this.knowledgeAreaIdField = value;
			}
		}

		public Area()
		{
			this.questionField = new ObservableCollection<Question>();
		}
	}
}
