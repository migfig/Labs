using System.Collections.ObjectModel;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Interviewer.Common
{
    public class configuration : BaseModel
	{
		private ObservableCollection<Platform> platformField;
        private ObservableCollection<Profile> profileField;

        [XmlElement("Platform")]
		public ObservableCollection<Platform> Platform
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

        [XmlElement("Profile")]
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

        public configuration()
		{
			this.platformField = new ObservableCollection<Platform>();
            this.profileField = new ObservableCollection<Profile>();
		}
	}
}
