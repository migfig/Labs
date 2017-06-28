using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeCycle.DSL.Monitor.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var customer = await new Customer("ABC-1234", "ACME").Save();
            var group = new ServerGroup("Hadoop Servers");
            var server = new Server("Kafka Server");
            var cpu = new Resource("CPU");
            var memory = new Resource("Memory");

            server.Resources.Add(cpu);
            server.Resources.Add(memory);
            group.Servers.Add(server);
            customer.ServerGroups.Add(group);
            //await customer.Save();
            await customer.Delete();
        }
    }
}
