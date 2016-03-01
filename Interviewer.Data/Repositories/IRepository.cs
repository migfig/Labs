using Interviewer.Common;
using System.Threading.Tasks;

namespace Interviewer.Data.Repositories
{
    public interface IRepository
    {
        Task<configuration> GetConfiguration();
        Task<configuration> GetConfiguration(string platform, string knowledgeArea, string area, string question, string profile);
        Task<int> AddItem<T>(T item);
        Task<int> UpdateItem<T>(T item);
        Task<int> DeleteItem<T>(T item);
    }
}
