using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace VStudio.Extensions.Path2Improve.ViewModels
{
    public interface IValidable
    {
        bool IsValid();
    }

    public class Story: IValidable
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Uri Url { get; set; }
        public Uri ParentStoryUrl { get; set; }
        public DateTime? DateStarted { get; set; }
        public DateTime? DateEnded { get; set; }
        public StoryStatus Status { get; set; }
        public ObservableCollection<Keyidentifier> KeyIdentifiers { get; set; }
        public ObservableCollection<Testcase> TestCases { get; set; }
        public ObservableCollection<Checkup> Checkups { get; set; }
        public ObservableCollection<Uri> Attachments { get; set; }
        public ObservableCollection<Uri> Queries { get; set; }
        public ObservableCollection<Uri> Scripts { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Name)
                && !string.IsNullOrEmpty(Title)
                && !string.IsNullOrEmpty(Description)
                && Uri.IsWellFormedUriString(Url.ToString(), UriKind.RelativeOrAbsolute)
                && Uri.IsWellFormedUriString(ParentStoryUrl.ToString(), UriKind.RelativeOrAbsolute)
                && KeyIdentifiers != null
                && KeyIdentifiers.Any() ? KeyIdentifiers.All(s => s.IsValid()) : true
                && TestCases != null
                && TestCases.Any() ? TestCases.All(s => s.IsValid()) : true
                && Checkups != null
                && Checkups.Any() ? Checkups.All(s => s.IsValid()) : true
                && Attachments != null
                && Attachments.Any() ? Attachments.All(s => Uri.IsWellFormedUriString(s.ToString(), UriKind.RelativeOrAbsolute)) : true
                && Queries != null
                && Queries.Any() ? Queries.All(s => Uri.IsWellFormedUriString(s.ToString(), UriKind.RelativeOrAbsolute)) : true
                && Scripts != null
                && Scripts.Any() ? Scripts.All(s => Uri.IsWellFormedUriString(s.ToString(), UriKind.RelativeOrAbsolute)) : true;
        }
    }

    public class Keyidentifier: IValidable
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public IdentifierCategory Category { get; set; }
        public ObservableCollection<Question> Questions { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Description)
                && Id != Guid.Empty
                && Questions != null
                && Questions.Any() ? Questions.All(s => s.IsValid()) : true;
        }
    }

    public class Question: IValidable
    {
        public string Ask { get; set; }
        public string Answer { get; set; }
        public ObservableCollection<Uri> Urls { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Ask)
                && Urls != null
                && Urls.Any() ? Urls.All(s => Uri.IsWellFormedUriString(s.ToString(), UriKind.RelativeOrAbsolute)) : true;
        }
    }

    public class Testcase: IValidable
    {
        public string Description { get; set; }
        public ObservableCollection<string> Steps { get; set; }
        public bool Applied { get; set; }
        public DateTime? DateApplied { get; set; }
        public TestcaseStatus Status { get; set; }
        public ObservableCollection<Guid> KeyIdentifierIds { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Description)
                && Steps != null
                && Steps.Any() ? Steps.All(s => !string.IsNullOrEmpty(s)) : true
                && KeyIdentifierIds != null
                && KeyIdentifierIds.Any() ? KeyIdentifierIds.All(s => s != Guid.Empty) : true;
        }
    }

    public class Checkup: IValidable
    {
        public string Description { get; set; }
        public bool Applied { get; set; }
        public DateTime? DateApplied { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Description);
        }
    }

    public enum StoryStatus
    {
        NotStarted,
        InProgress,
        Completed,
        Postponed,
        Cancelled
    }

    public enum IdentifierCategory
    {
        Deliverable,
        EnvironmentSetup,
        Other
    }

    public enum TestcaseStatus
    {
        Pending,
        Failed,
        Success
    }   
}
