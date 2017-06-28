using System.Collections.Generic;
using System.Threading.Tasks;

namespace LifeCycle.DSL.Monitor
{
    public interface IMonitorRepository: IRepositoryBase
    {
        Task<bool> AssignResource(int serverId, int id);
        Task<Customer> CreateCustomer(string customerKey, string name);
        Task<Resource> CreateResource(int customerId, string name);
        Task<Server> CreateServer(int serverGroupId, string name);
        Task<ServerGroup> CreateServerGroup(int customerId, string name);
        Task<bool> DeleteCustomer(int id);
        Task<bool> DeleteResource(int id);
        Task<bool> DeleteServer(int id);
        Task<bool> DeleteServerGroup(int id);
        Task<Customer> GetCustomer(int id);
        Task<IEnumerable<Customer>> GetCustomers();
        Task<Resource> GetResource(int id);
        Task<IEnumerable<Resource>> GetResources(int serverId);
        Task<Server> GetServer(int id);
        Task<ServerGroup> GetServerGroup(int id);
        Task<IEnumerable<ServerGroup>> GetServerGroups(int customerId);
        Task<IEnumerable<Server>> GetServers(int serverGroupid);
        Task<bool> UnAssignResource(int serverId, int id);
        Task<bool> UpdateCustomer(int id, bool isDisabled);
    }
}