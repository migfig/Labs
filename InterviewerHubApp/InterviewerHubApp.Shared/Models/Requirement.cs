using System.Xml.Serialization;

namespace WpfInterviewer
{
	public class Requirement : BaseModel
	{
		private string platformField;

		private string knowledgeAreaField;

		private string areaField;

		[XmlAttribute]
		public string platform
		{
			get
			{
				return this.platformField;
			}
			set
			{
				this.platformField = value;
			}
		}

		[XmlAttribute]
		public string knowledgeArea
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
		public string area
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
	}
}
