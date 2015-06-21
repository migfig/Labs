using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentTesting
{
    /// <summary>
    /// Base class for all fluent items
    /// </summary>
    public abstract class BaseIt
    {
        /// <summary>
        /// Fluent parent instance
        /// </summary>
        protected It Parent { get; private set; }

        protected BaseIt(It parent)
        {
            Parent = parent;
        }
    }
}
