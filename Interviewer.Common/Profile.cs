using System.Collections.ObjectModel;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Interviewer.Common
{
    public class Profile : BaseModel
	{
		private ObservableCollection<Requirement> requirementField;

		private ObservableCollection<Optional> optionalField;

		private string nameField;

		[XmlElement("requirement", Form = XmlSchemaForm.Unqualified)]
		public ObservableCollection<Requirement> requirement
		{
			get
			{
				return this.requirementField;
			}
			set
			{
				this.requirementField = value;
			}
		}

		[XmlElement("optional", Form = XmlSchemaForm.Unqualified)]
		public ObservableCollection<Optional> optional
		{
			get
			{
				return this.optionalField;
			}
			set
			{
				this.optionalField = value;
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

		public Profile()
		{
			this.optionalField = new ObservableCollection<Optional>();
			this.requirementField = new ObservableCollection<Requirement>();
		}
	}
}
