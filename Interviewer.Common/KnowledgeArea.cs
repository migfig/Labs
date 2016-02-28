using System.Collections.ObjectModel;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Interviewer.Common
{
	public class KnowledgeArea : BaseModel
	{
		private ObservableCollection<Area> areaField;

		private int platformIdField;

		[XmlElement("Area", Form = XmlSchemaForm.Unqualified)]
		public ObservableCollection<Area> Area
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
		public int PlatformId
        {
			get
			{
				return this.platformIdField;
			}
			set
			{
				this.platformIdField = value;
			}
		}

        public KnowledgeArea()
		{
			this.areaField = new ObservableCollection<Area>();
		}
	}
}
