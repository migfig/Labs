using Interviewer.Common;
using System.Threading.Tasks;

namespace Interviewer.Data.Repositories
{
    public interface IRepository
    {
        Task<configuration> GetConfiguration();
        Task<configuration> GetConfiguration(string platform, string knowledgeArea, string area, string question);
    }
}
