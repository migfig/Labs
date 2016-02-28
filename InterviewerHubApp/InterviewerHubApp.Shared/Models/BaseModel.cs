using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Interviewer.Common
{
	public class BaseModel : INotifyPropertyChanged
	{        
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
