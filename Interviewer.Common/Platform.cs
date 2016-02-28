using System.Collections.ObjectModel;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Interviewer.Common
{    
	public class Platform : BaseModel
	{
		private ObservableCollection<KnowledgeArea> knowledgeAreaField;
        private ObservableCollection<Profile> profileField;

		[XmlElement("KnowledgeArea", Form = XmlSchemaForm.Unqualified, Order = 0)]
		public ObservableCollection<KnowledgeArea> KnowledgeArea
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

        [XmlElement("Profile", Form = XmlSchemaForm.Unqualified, Order = 1)]
        public ObservableCollection<Profile> Profile
        {
            get
            {
                return this.profileField;
            }
            set
            {
                this.profileField = value;
            }
        }

        public Platform()
		{
			this.knowledgeAreaField = new ObservableCollection<KnowledgeArea>();
            this.profileField = new ObservableCollection<Profile>();
		}
	}
}
