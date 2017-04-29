using Common.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Documents;

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
        public ObservableCollection<StringValue> Attachments { get; set; }
        public ObservableCollection<StringValue> Queries { get; set; }
        public ObservableCollection<StringValue> Scripts { get; set; }
        public ObservableCollection<StringValue> AcceptanceCriteria { get; set; }
        public ObservableCollection<StringValue> DeveloperCriteria { get; set; }
        public ObservableCollection<Issue> Issues { get; set; }
        public ObservableCollection<SubTask> SubTasks { get; set; }

        [JsonIgnore]
        public FlowDocument Document
        {
            get
            {
                return BuildDocument();
            }
        }

        private FlowDocument BuildDocument()
        {
            var doc = new FlowDocument();
            doc.LineStackingStrategy = System.Windows.LineStackingStrategy.BlockLineHeight;
            doc.LineHeight = 12;
            doc.Blocks.Add(new Paragraph(new Bold(new Run("Summary"))));
            doc.Blocks.Add(new Paragraph(new Run(" "+Title)));
            doc.Blocks.Add(new Paragraph(new Bold(new Run("Description"))));
            doc.Blocks.Add(new Paragraph(new Run(" " + Description)));

            doc.Blocks.Add(new Paragraph(new Bold(new Run("Url"))));
            var url = new Hyperlink(new Run(Url.ToString()));
            doc.Blocks.Add(new Paragraph(url));
            doc.Blocks.Add(new Paragraph(new Bold(new Run("Parent Story"))));
            var parentUrl = new Hyperlink(new Run(ParentStoryUrl.ToString()));
            parentUrl.NavigateUri = ParentStoryUrl;
            doc.Blocks.Add(new Paragraph(parentUrl));

            doc.Blocks.Add(new Paragraph(new Italic(new Run("Status: " + Status.ToString()))));

            doc.Blocks.Add(new Paragraph(new Italic(new Run("Date Started: " + DateStarted.Value.ToString("yyyy-MM-dd hh:mm:ss")))));
            if(DateEnded.HasValue && DateEnded.Value != DateTime.MinValue)
                doc.Blocks.Add(new Paragraph(new Italic(new Run("Date Ended: " + DateEnded.Value.ToString("yyyy-MM-dd hh:mm:ss")))));

            if (Attachments.Any())
            {
                doc.Blocks.Add(new Paragraph(new Bold(new Run("Attachments"))));
                foreach (var item in Attachments)
                {
                    var itemUri = new Uri(item.Value);
                    var itemUrl = new Hyperlink(new Run(item.Value));
                    doc.Blocks.Add(new Paragraph(itemUrl));
                }
            }

            if (AcceptanceCriteria.Any())
            {
                doc.Blocks.Add(new Paragraph(new Bold(new Run("Acceptance Criteria"))));
                foreach (var item in AcceptanceCriteria)
                {
                    doc.Blocks.Add(new Paragraph(new Run(" + " + item.Value)));
                }
            }

            if (DeveloperCriteria.Any())
            {
                doc.Blocks.Add(new Paragraph(new Bold(new Run("Development Criteria"))));
                foreach (var item in DeveloperCriteria)
                {
                    doc.Blocks.Add(new Paragraph(new Run(" + " + item.Value)));
                }
            }

            var i = 0;
            var j = 0;
            if (KeyIdentifiers.Any())
            {
                doc.Blocks.Add(new Paragraph(new Bold(new Run("Key Identifiers"))));
                foreach (var item in KeyIdentifiers)
                {
                    doc.Blocks.Add(new Paragraph(new Run(" " + (++i).ToString() + ". " + item.Description)));

                    if (item.Questions.Any())
                    {
                        j = 0;
                        doc.Blocks.Add(new Paragraph(new Italic(new Run(" Questions"))));
                        foreach(var q in item.Questions)
                        {
                            doc.Blocks.Add(new Paragraph(new Run("   " + (++j).ToString() + ".Q. " + q.Ask)));
                            doc.Blocks.Add(new Paragraph(new Run("     A. " + q.Answer)));
                        }
                    }
                }
            }

            if (Issues.Any())
            {
                i = 0;
                doc.Blocks.Add(new Paragraph(new Bold(new Run("Issues"))));
                foreach (var item in Issues)
                {
                    doc.Blocks.Add(new Paragraph(new Run(" " + (++i).ToString() + ". " + item.Description)));
                    doc.Blocks.Add(new Paragraph(new Italic(new Run("  Is Open: " + item.IsOpen.ToString()))));
                }
            }

            if (Queries.Any())
            {
                doc.Blocks.Add(new Paragraph(new Bold(new Run("Queries"))));
                foreach (var item in Queries)
                {
                    var itemUri = new Uri(item.Value);
                    var itemUrl = new Hyperlink(new Run(item.Value));
                    doc.Blocks.Add(new Paragraph(itemUrl));
                }
            }

            if (Scripts.Any())
            {
                doc.Blocks.Add(new Paragraph(new Bold(new Run("Scripts"))));
                foreach (var item in Scripts)
                {
                    var itemUri = new Uri(item.Value);
                    var itemUrl = new Hyperlink(new Run(item.Value));
                    doc.Blocks.Add(new Paragraph(itemUrl));
                }
            }

            if (TestCases.Any())
            {
                i = 0;
                doc.Blocks.Add(new Paragraph(new Bold(new Run("Test Cases"))));
                foreach (var item in TestCases)
                {
                    doc.Blocks.Add(new Paragraph(new Run(" " + (++i).ToString() + ". " + item.Description)));
                    doc.Blocks.Add(new Paragraph(new Italic(new Run("  Status: " + item.Status.ToString()))));

                    if (item.Steps.Any())
                    {
                        j = 0;
                        doc.Blocks.Add(new Paragraph(new Italic(new Run(" Steps"))));
                        foreach (var q in item.Steps)
                        {
                            doc.Blocks.Add(new Paragraph(new Run("   " + (++j).ToString() + ". " + q.Value)));
                        }
                    }
                }
            }

            if (Checkups.Any())
            {
                i = 0;
                doc.Blocks.Add(new Paragraph(new Bold(new Run("Checkups"))));
                foreach (var item in Checkups)
                {
                    doc.Blocks.Add(new Paragraph(new Run(" " + (++i).ToString() + ". " + item.Description)));
                    doc.Blocks.Add(new Paragraph(new Italic(new Run("  Applied: " + item.Applied.ToString()))));
                }
            }

            if (SubTasks.Any())
            {
                i = 0;
                doc.Blocks.Add(new Paragraph(new Bold(new Run("SubTasks"))));
                foreach (var item in SubTasks)
                {
                    doc.Blocks.Add(new Paragraph(new Run(" " + (++i).ToString() + ". " + item.Title)));
                    doc.Blocks.Add(new Paragraph(new Italic(new Run("  Status: " + item.Status))));
                }
            }

            return doc;
        }

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
                && Attachments.Any() ? Attachments.All(s => Uri.IsWellFormedUriString(s.Value, UriKind.RelativeOrAbsolute)) : true
                && Queries != null
                && Queries.Any() ? Queries.All(s => Uri.IsWellFormedUriString(s.Value, UriKind.RelativeOrAbsolute)) : true
                && Scripts != null
                && Scripts.Any() ? Scripts.All(s => Uri.IsWellFormedUriString(s.Value, UriKind.RelativeOrAbsolute)) : true
                && SubTasks.Any() ? SubTasks.All(s => s.IsValid()) : true;
        }

        public static Story New()
        {
            var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "story.json");
            if(File.Exists(fileName))
            {
                return JsonConvert.DeserializeObject<Story>(File.ReadAllText(fileName));
            }

            var story = new Story
            {
                Name = string.Empty,
                Title = string.Empty,
                Description = string.Empty,
                Url = new Uri("http://localhost:3033/api/stories"),
                ParentStoryUrl = NewUri(),
                DateStarted = DateTime.MinValue,
                DateEnded = DateTime.MinValue,
                Status = StoryStatus.NotStarted,
                KeyIdentifiers = new ObservableCollection<Keyidentifier> { Keyidentifier.New() },
                TestCases = new ObservableCollection<Testcase> { Testcase.New() },
                Checkups = new ObservableCollection<Checkup> { Checkup.New() },
                Attachments = new ObservableCollection<StringValue> { NewAttachment() },
                Queries = new ObservableCollection<StringValue> { NewQuery() },
                Scripts = new ObservableCollection<StringValue> { NewScript() },
                AcceptanceCriteria = new ObservableCollection<StringValue> { NewAcceptanceCriteria() },
                DeveloperCriteria = new ObservableCollection<StringValue> { NewDeveloperCriteria() },
                Issues = new ObservableCollection<Issue> { Issue.New() },
                SubTasks = new ObservableCollection<SubTask> { SubTask.New() }
            };
            story.TestCases.First().KeyIdentifierIds.First().Value = story.KeyIdentifiers.First().Id.ToString();

            File.WriteAllText(fileName, JsonConvert.SerializeObject(story, Formatting.Indented));

            return story;
        }

        public static Uri NewUri()
        {
            return new Uri("http://localhost");
        }

        public static StringValue NewAttachment()
        {
            return new StringValue(NewUri().ToString(), "Attachment");
        }

        public static StringValue NewQuery()
        {
            return new StringValue(@"file://c:\", "Query");
        }

        public static StringValue NewScript()
        {
            return new StringValue(@"file://c:\", "Script");
        }

        public static StringValue NewAcceptanceCriteria()
        {
            return new StringValue(string.Empty, "AcceptanceCriteria");
        }

        public static StringValue NewDeveloperCriteria()
        {
            return new StringValue(string.Empty, "DeveloperCriteria");
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

        public static Keyidentifier New()
        {
            return new Keyidentifier
            {
                Id = Guid.NewGuid(),
                Description = string.Empty,
                Category = IdentifierCategory.Deliverable,
                Questions = new ObservableCollection<Question> { Question.New() }
            };
        }
    }

    public class Question: IValidable
    {
        public string Ask { get; set; }
        public string Answer { get; set; }
        public ObservableCollection<StringValue> Urls { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Ask)
                && Urls != null
                && Urls.Any() ? Urls.All(s => Uri.IsWellFormedUriString(s.Value, UriKind.RelativeOrAbsolute)) : true;
        }

        public static Question New()
        {
            return new Question
            {
                Ask = string.Empty,
                Answer = string.Empty,
                Urls = new ObservableCollection<StringValue> { NewUrl() }
            };
        }

        public static StringValue NewUrl()
        {
            return new StringValue("http://localhost", "Url");
        }
    }

    public class Issue : IValidable
    {
        public string Description { get; set; }
        public bool IsOpen { get; set; }
        public DateTime? DateClosed { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Description);
        }

        public static Issue New()
        {
            return new Issue
            {
                Description = string.Empty,
                DateClosed = DateTime.MinValue
            };
        }
    }

    public class Testcase: IValidable
    {
        public string Description { get; set; }
        public ObservableCollection<StringValue> Steps { get; set; }
        public bool Applied { get; set; }
        public DateTime? DateApplied { get; set; }
        public TestcaseStatus Status { get; set; }
        public ObservableCollection<StringValue> KeyIdentifierIds { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Description)
                && Steps != null
                && Steps.Any() ? Steps.All(s => !string.IsNullOrEmpty(s.ToString())) : true
                && KeyIdentifierIds != null
                && KeyIdentifierIds.Any() ? KeyIdentifierIds.All(s => Guid.Parse(s.Value) != Guid.Empty) : true;
        }

        public static Testcase New()
        {
            return new Testcase
            {
                Description = string.Empty,
                Steps = new ObservableCollection<StringValue> { new StringValue(string.Empty, "Step") },
                DateApplied = DateTime.MinValue,
                Status = TestcaseStatus.Pending,
                KeyIdentifierIds = new ObservableCollection<StringValue> { new StringValue(Guid.Empty.ToString(), "KeyIdentifierId") }
            };
        }

        public static StringValue NewStep()
        {
            return new StringValue(string.Empty, "Step");
        }

        public static StringValue NewKeyIdentifierId(Guid id)
        {
            return new StringValue(id.ToString(), "KeyIdentifierId");
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

        public static Checkup New()
        {
            return new Checkup
            {
                Description = string.Empty,
                DateApplied = DateTime.MinValue
            };
        }
    }

    public class SubTask : IValidable
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public Uri Url { get; set; }
        public string Status { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Title)
                && Uri.IsWellFormedUriString(Url.ToString(), UriKind.RelativeOrAbsolute);
        }

        public static SubTask New()
        {
            return new SubTask
            {
                Name = string.Empty,
                Title = string.Empty,
                Url = new Uri("http://localhost"),
                Status = string.Empty
            };
        }
    }

    public class StringValue 
    {
        public StringValue(string value, string type)
        {
            Value = value;
            Type = type;
        }
        public string Value { get; set; }
        public string Type { get; set; }
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

    public class Flatter
    {
        public string Value { get; set; }

        public static Flatter Get(string key, string value)
        {
            var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "flatter.json");
            if (File.Exists(fileName))
            {
                var flat = JsonConvert.DeserializeObject<Flatter>(File.ReadAllText(fileName));
                flat.Value = Convert.ToBase64String(Encoding.UTF8.GetBytes(Flat.Deflate(flat.Value, typeof(Flatter).FullName + "|" + typeof(Story).FullName)));
                return flat;
            }

            File.WriteAllText(fileName, JsonConvert.SerializeObject(
                new Flatter
                {
                    Value = Flat.Inflate(key + ":" + value, typeof(Flatter).FullName + "|" + typeof(Story).FullName)
                }));

            return new Flatter { Value = Convert.ToBase64String(Encoding.UTF8.GetBytes(key + ":" + value)) };
        }

        public static Flatter Get()
        {
            var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "flatter.json");
            if (File.Exists(fileName))
            {
                var flat = JsonConvert.DeserializeObject<Flatter>(File.ReadAllText(fileName));
                flat.Value = Convert.ToBase64String(Encoding.UTF8.GetBytes(Flat.Deflate(flat.Value, typeof(Flatter).FullName + "|" + typeof(Story).FullName)));
                return flat;
            }

            return new Flatter { Value = string.Empty };
        }
    }
}
