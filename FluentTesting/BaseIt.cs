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
    public abstract class BaseIt<T> where T : class
    {
        /// <summary>
        /// Fluent parent instance
        /// </summary>
        protected T Parent { get; private set; }

        protected BaseIt(T parent)
        {
            Parent = parent;
        }
    }
}
