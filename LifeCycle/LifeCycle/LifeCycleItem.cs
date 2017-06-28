using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeCycle
{
    public abstract class LifeCycleItem<T> where T: class
    {
        public virtual Task<T> Save()
        {
            return null;
        }

        public virtual Task<bool> Delete()
        {
            return null;
        }

        protected async Task<T> Exec(Func<Task<T>> func)
        {
            if (null == func) return null;
            return await func();
        }

        protected async Task<bool> Exec(Func<Task<bool>> func)
        {
            if (null == func) return false;
            return await func();
        }
    }
}
