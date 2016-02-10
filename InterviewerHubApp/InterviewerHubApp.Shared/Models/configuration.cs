using System.Collections.ObjectModel;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WpfInterviewer
{
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
