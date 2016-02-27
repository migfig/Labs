using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Interviewer.Common
{
	public class BaseModel : INotifyPropertyChanged
	{
        private string idField;

        public BaseModel()
        {
            this.idField = System.Guid.NewGuid().ToString();
        }

        [XmlIgnore]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = this.PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
