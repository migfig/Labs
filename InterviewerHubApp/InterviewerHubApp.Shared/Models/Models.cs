using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;

namespace Interviewer.Common
{    
    public class configuration: BaseModel
    {
        public ObservableCollection<Platform> Platform { get; set; }
        public ObservableCollection<Profile> Profile { get; set; }
        
        public override bool IsValid()
        {
            return Platform.Any();
        }
    }

    public class Platform : BaseModel
    {
        public ObservableCollection<KnowledgeArea> KnowledgeArea { get; set; }
        public ObservableCollection<Profile> Profile { get; set; }
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
        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) && Name.Trim().ToLower() != "undefined";
        }
    }

    public class KnowledgeArea : BaseModel
    {
        public ObservableCollection<Area> Area { get; set; }
        public int PlatformId { get; set; }
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
        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) && Name.Trim().ToLower() != "undefined"
                && PlatformId > 0;
        }
    }

    public class Area : BaseModel
    {
        public ObservableCollection<Question> Question { get; set; }
        public int KnowledgeAreaId { get; set; }
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
        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) && Name.Trim().ToLower() != "undefined"
                && KnowledgeAreaId > 0;
        }
    }

    public class Question : BaseModel
    {
        public int AreaId { get; set; }
        public int Weight { get; set; }
        public int Level { get; set; }
        private string _value;
        public string Value
        {
            get { return _value; }
            set
            {
                IsDirty = !string.IsNullOrEmpty(_value) && _value != value;
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool AlreadyAnswered { get; set; }
        public int rating { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Value) && Value.Trim().ToLower() != "undefined"
                && AreaId > 0;
        }
    }

    public class Profile : BaseModel
    {
        public ObservableCollection<Requirement> Requirement { get; set; }
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
        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) && Name.Trim().ToLower() != "undefined"
                && Id > 0;
        }
    }

    public class Requirement : BaseModel
    {
        public int ProfileId { get; set; }
        public int PlatformId { get; set; }
        public int KnowledgeAreaId { get; set; }
        public int AreaId { get; set; }
        public bool IsRequired { get; set; }
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
        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) && Name.Trim().ToLower() != "undefined"
                && Id > 0
                && PlatformId > 0
                && ProfileId > 0
                && KnowledgeAreaId > 0
                && AreaId > 0;
        }
    }
}
