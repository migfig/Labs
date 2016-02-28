using System.Xml.Serialization;

namespace Interviewer.Common
{    
    public class Requirement : BaseModel
	{
        private int profileIdField;
        private int platformIdField;
		private int knowledgeAreaIdField;
		private int areaIdField;
        private bool isRequiredField;

        [XmlAttribute]
        public int ProfileId
        {
            get
            {
                return this.profileIdField;
            }
            set
            {
                this.profileIdField = value;
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

		[XmlAttribute]
		public int KnowledgeAreaId
		{
			get
			{
				return this.knowledgeAreaIdField;
			}
			set
			{
				this.knowledgeAreaIdField = value;
			}
		}

		[XmlAttribute]
		public int AreaId
		{
			get
			{
				return this.areaIdField;
			}
			set
			{
				this.areaIdField = value;
			}
		}

        [XmlAttribute]
        public bool IsRequired
        {
            get
            {
                return this.isRequiredField;
            }
            set
            {
                this.isRequiredField = value;
            }
        }
    }
}
