using System.Collections.ObjectModel;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Interviewer.Common
{    
    public class Profile : BaseModel
	{
		private ObservableCollection<Requirement> requirementField;

		[XmlElement("Requirement", Form = XmlSchemaForm.Unqualified)]
		public ObservableCollection<Requirement> Requirement
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

		public Profile()
		{
			this.requirementField = new ObservableCollection<Requirement>();
		}
	}
}
