using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WpfInterviewer
{
	[GeneratedCode("Xsd2Code", "3.4.0.32990"), DesignerCategory("code"), XmlType(AnonymousType = true)]
	[Serializable]
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
