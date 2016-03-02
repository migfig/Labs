using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using System.Linq;
using Windows.UI.Xaml;
using System;

namespace Interviewer.Common
{
    public abstract class BaseModel : INotifyPropertyChanged
	{
        private string _imagePath;  
        public string ImagePath
        {
            get
            {
                if(string.IsNullOrWhiteSpace(_imagePath))
                {
                    _imagePath = "Assets/" + GetType().FullName.Split('.').Last().ToLower() + ".png";
                }
                return _imagePath;
            }
            set
            {
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        private bool _isDirty = false;
        public bool IsDirty
        {
            get { return _isDirty; }
            set {
                _isDirty = value;
                OnPropertyChanged();
                OnPropertyChanged("DirtyVisibility");
            }
        }

        public Visibility DirtyVisibility
        {
            get { return _isDirty ? Visibility.Visible : Visibility.Collapsed; }
        }

        public abstract bool IsValid();        

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
