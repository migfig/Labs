using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using System.Linq;
using Windows.UI.Xaml;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Interviewer.Data
{
    public class KeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public KeyValue(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }

    public abstract class BaseModel : INotifyPropertyChanged
	{
        public int Id { get; set; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                IsDirty = !string.IsNullOrEmpty(_name) && _name != value;
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                IsDirty = !string.IsNullOrEmpty(_description) && _description != value;
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

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

        private ObservableCollection<KeyValue> _properties;
        public ObservableCollection<KeyValue> Properties
        {
            get
            {
                if(null == _properties)
                {
                    _properties = new ObservableCollection<KeyValue>();
                    _properties.Add(new KeyValue("Name", this.Name));
                    _properties.Add(new KeyValue("Description", this.Description));
                }
                return _properties;
            }
        }        

        #region property changed handler

        public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = this.PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}
        
        #endregion
    }
}
