using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WpfInterviewer
{
	[GeneratedCode("Xsd2Code", "3.4.0.32990"), DesignerCategory("code"), XmlRoot(Namespace = "", IsNullable = false), XmlType(AnonymousType = true)]
	[Serializable]
	public class configuration : BaseModel
	{
		private ObservableCollection<object> itemsField;

		[XmlElement("platform", typeof(Platform), Form = XmlSchemaForm.Unqualified), XmlElement("profile", typeof(Profile), Form = XmlSchemaForm.Unqualified)]
		public ObservableCollection<object> Items
		{
			get
			{
				return this.itemsField;
			}
			set
			{
				this.itemsField = value;
			}
		}

		public configuration()
		{
			this.itemsField = new ObservableCollection<object>();
		}
	}
}
