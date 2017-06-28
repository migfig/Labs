using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeCycle.DSL.Monitor
{
    public class Customer: LifeCycleItem<Customer>
    {
        public int Id { get; set; }
        public string CustomerKey { get; }
        public string Name { get; }
        public bool IsDisabled { get; set; }

        public IList<ServerGroup> ServerGroups { get; set; }

        public Customer(string customerKey, string name)
        {
            CustomerKey = customerKey;
            Name = name;
        }

        public override async Task<Customer> Save()
        {
            var customer = await base.Exec(
                async () => await new MonitorRepository().CreateCustomer(CustomerKey, Name));
            //foreach (var sg in ServerGroups)
            //    await sg.Save();

            return customer;
        }

        public override async Task<bool> Delete()
        {
            return await base.Exec(
                async () => await new MonitorRepository().DeleteCustomer(Id));
        }
    }

    public class ServerGroup: LifeCycleItem<ServerGroup>
    {
        public int Id { get; set; }
        public string Name { get; }
        public int CustomerId { get; set; }

        public IList<Server> Servers { get; set; }

        public ServerGroup(string name)
        {
            Name = name;
        }
    }

    public class Server
    {
        public int Id { get; set; }
        public string Name { get; }
        public int ServerGroupId { get; set; }

        public IList<Resource> Resources { get; set; }

        public Server(string name)
        {
            Name = name;
        }
    }

    public class Resource
    {
        public int Id { get; set; }
        public string Name { get; }

        public Resource(string name)
        {
            Name = name;
        }
    }
}
