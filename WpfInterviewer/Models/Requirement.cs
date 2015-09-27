using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Xml.Serialization;

namespace WpfInterviewer
{
	[GeneratedCode("Xsd2Code", "3.4.0.32990"), DesignerCategory("code"), XmlType(AnonymousType = true)]
	[Serializable]
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
