using System;
using System.Linq;

namespace Visor.VStudio
{
    public class ViewCodeArgs : EventArgs
    {
        public string NameSpace { get; private set; }
        public string ClassName { get; private set; }
        public int LineNumber { get; private set; }
        public string ProgId { get; private set; }
        public string ProjectName { get; private set; }

        public ViewCodeArgs(string progId, string fullName, int lineNumber)
            :this(progId, fullName, string.Empty, lineNumber)
        {
        }

        public ViewCodeArgs(string progId, string fullName, int lineNumber, string projectName)
            :this(progId, fullName, projectName, lineNumber)
        {
        }

        protected ViewCodeArgs(string progId, string fullName, string projectName, int lineNumber)
        {
            ProgId = progId;
            if (!string.IsNullOrWhiteSpace(fullName))
            {
                var parts = fullName.Replace(".cs", string.Empty).Split('\\');
                NameSpace = string.Join("\\", parts.Take(parts.Length - 1));
                ClassName = parts.Last() + ".cs";
            }
            LineNumber = lineNumber;
            ProjectName = projectName;
        }
    }
}
