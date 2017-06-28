using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeCycle.DSL.Monitor
{
    public class MonitorRepository : IMonitorRepository
    {
        public Task<Customer> CreateCustomer(string customerKey, string name)
        {

        } 

        public Task<Customer> GetCustomer(int id)
        {

        }

        public Task<IEnumerable<Customer>> GetCustomers()
        {

        }

        public Task<bool> UpdateCustomer(int id, bool isDisabled)
        {

        }

        public Task<bool> DeleteCustomer(int id)
        {

        }

        public Task<ServerGroup> CreateServerGroup(int customerId, string name)
        {

        }

        public Task<ServerGroup> GetServerGroup(int id)
        {

        }

        public Task<IEnumerable<ServerGroup>> GetServerGroups(int customerId)
        {

        }

        public Task<bool> DeleteServerGroup(int id)
        {

        }

        public Task<Server> CreateServer(int serverGroupId, string name)
        {

        }

        public Task<Server> GetServer(int id)
        {

        }

        public Task<IEnumerable<Server>> GetServers(int serverGroupid)
        {

        }

        public Task<bool> DeleteServer(int id)
        {

        }

        public Task<Resource> CreateResource(int customerId, string name)
        {

        }

        public Task<Resource> GetResource(int id)
        {

        }

        public Task<IEnumerable<Resource>> GetResources(int serverId)
        {

        }

        public Task<bool> AssignResource(int serverId, int id)
        {

        }

        public Task<bool> UnAssignResource(int serverId, int id)
        {

        }

        public Task<bool> DeleteResource(int id)
        {

        }
    }
}
