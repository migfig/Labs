using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonSample
{
    public static class PythonInter
    {
        public static object Run(string code, int number, int times)
        {
            var engine = Python.CreateEngine();
            dynamic scope = engine.CreateScope();
            //engine.ImportModule("numpy");
            engine.Execute(code, scope);

            return scope.run(number, times);
        }
    }
}
