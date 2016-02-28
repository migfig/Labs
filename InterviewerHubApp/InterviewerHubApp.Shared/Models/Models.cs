using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Interviewer.Common
{    
    public class configuration: BaseModel
    {
        public ObservableCollection<Platform> Platform { get; set; }
        public ObservableCollection<Profile> Profile { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Platform : BaseModel
    {
        public ObservableCollection<KnowledgeArea> KnowledgeArea { get; set; }
        public ObservableCollection<Profile> Profile { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class KnowledgeArea : BaseModel
    {
        public ObservableCollection<Area> Area { get; set; }
        public int PlatformId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Area : BaseModel
    {
        public ObservableCollection<Question> Question { get; set; }
        public int KnowledgeAreaId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Question : BaseModel
    {
        public int AreaId { get; set; }
        public int Weight { get; set; }
        public int Level { get; set; }
        public string Value { get; set; }
        public bool AlreadyAnswered { get; set; }
        public int rating { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Profile : BaseModel
    {
        public ObservableCollection<Requirement> Requirement { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Requirement : BaseModel
    {
        public int ProfileId { get; set; }
        public int PlatformId { get; set; }
        public int KnowledgeAreaId { get; set; }
        public int AreaId { get; set; }
        public bool IsRequired { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
