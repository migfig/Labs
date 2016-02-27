using System.Collections.ObjectModel;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Interviewer.Common
{
	public class Platform : BaseModel
	{
		private ObservableCollection<KnowledgeArea> knowledgeAreaField;

		private string nameField;

		[XmlElement("knowledgeArea", Form = XmlSchemaForm.Unqualified)]
		public ObservableCollection<KnowledgeArea> knowledgeArea
		{
			get
			{
				return this.knowledgeAreaField;
			}
			set
			{
				this.knowledgeAreaField = value;
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

		public Platform()
		{
			this.knowledgeAreaField = new ObservableCollection<KnowledgeArea>();
		}
	}
}
