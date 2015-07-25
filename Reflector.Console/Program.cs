using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reflector.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parser();
            System.Console.WriteLine(parser.Render(typeof(Parser), string.Empty));
        }
    }
}
