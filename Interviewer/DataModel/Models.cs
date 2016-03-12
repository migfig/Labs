using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using System.Runtime.Serialization.Json;
using System.IO;

namespace Interviewer.Data
{    
    public sealed class InterviewerDataSource
    {
        private readonly static object _lock = new object();

        private static InterviewerDataSource _dataSource = new InterviewerDataSource();
        private configuration _configuration = new configuration();
        public configuration Configuration
        {
            get { return _configuration; }
        }

        public static async Task<configuration> GetConfiguration()
        {
            await _dataSource.GetConfigurationData();
            return _dataSource.Configuration;
        }

        public static async Task<Platform> GetPlatform(int id)
        {
            await _dataSource.GetConfigurationData();
            return _dataSource.Configuration.Platform.FirstOrDefault(x => x.Id == id);
        } 

        public static async Task<Profile> GetProfile(int id)
        {
            await _dataSource.GetConfigurationData();
            return _dataSource.Configuration.Profile.FirstOrDefault(x => x.Id == id);
        }

        public static async Task<KnowledgeArea> GetKnowledgeArea(int id)
        {
            await _dataSource.GetConfigurationData();
            return (from p in _dataSource.Configuration.Platform
                   from ka in p.KnowledgeArea
                   where ka.Id == id
                   select ka).FirstOrDefault();
        }

        public static async Task<Area> GetArea(int id)
        {
            await _dataSource.GetConfigurationData();
            return (from p in _dataSource.Configuration.Platform
                    from ka in p.KnowledgeArea
                    from a in ka.Area
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public static async Task<Question> GetQuestion(int id)
        {
            await _dataSource.GetConfigurationData();
            return (from p in _dataSource.Configuration.Platform
                    from ka in p.KnowledgeArea
                    from a in ka.Area
                    from q in a.Question
                    where q.Id == id
                    select q).FirstOrDefault();
        }

        private async Task GetConfigurationData()
        {
            lock(_lock)
            {
                if (this.Configuration != null && this.Configuration.Platform.Any())
                    return;
            }

            Uri dataUri = new Uri("ms-appx:///DataModel/interviewer.json");

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            string jsonText = await FileIO.ReadTextAsync(file);

            var ser = new DataContractJsonSerializer(typeof(configuration));
            var stream = new MemoryStream(UTF8Encoding.UTF8.GetBytes(jsonText));
            lock(_lock)
            {
                _configuration = (configuration)ser.ReadObject(stream);
            }
        }
    }

    public class configuration: BaseModel
    {
        public ObservableCollection<Platform> Platform { get; set; }
        public ObservableCollection<Profile> Profile { get; set; }

        public configuration()
        {
            Platform = new ObservableCollection<Data.Platform>();
            Profile = new ObservableCollection<Data.Profile>();
        }
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
        public Platform()
        {
            KnowledgeArea = new ObservableCollection<Data.KnowledgeArea>();
            Profile = new ObservableCollection<Data.Profile>();
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
        public KnowledgeArea()
        {
            Area = new ObservableCollection<Data.Area>();
        }
        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) && Name.Trim().ToLower() != "undefined"
                && PlatformId > 0;
        }
        public override string ToString()
        {
            return this.Name;
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
        public Area()
        {
            Question = new ObservableCollection<Data.Question>();
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
        public Profile()
        {
            Requirement = new ObservableCollection<Data.Requirement>();
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
