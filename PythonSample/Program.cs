using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Write("Run operation 2 ^ 3 = ");
            Console.WriteLine(PythonInter.Run(@"
def run(number, times):
    return number ** times;
", 2, 3));

            //Console.WriteLine()
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
        }
    }
}
