using System.Collections.ObjectModel;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Interviewer.Common
{
	public class KnowledgeArea : BaseModel
	{
		private ObservableCollection<Area> areaField;

		private string nameField;

		[XmlElement("area", Form = XmlSchemaForm.Unqualified)]
		public ObservableCollection<Area> area
		{
			get
			{
				return this.areaField;
			}
			set
			{
				this.areaField = value;
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

        public KnowledgeArea()
		{
			this.areaField = new ObservableCollection<Area>();
		}
	}
}
