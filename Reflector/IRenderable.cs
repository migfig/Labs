using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reflector
{
    public interface IRenderable
    {
        string SourcePath { get; }
        bool IncludeSystemObjects { get; }
        string Render(Type type, Type[] onlyTypes, string[] onlyMethods);
        object GetAttributeValues(object attribute);
    }
}
