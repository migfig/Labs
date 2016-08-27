using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Common;

namespace Log.Common.Services.Common
{
    public static class ObjectHelper
    {       
        //Needs further coding (left for now)
        public static T DeepClone<T>(this T src)
        {
            var tgt = Activator.CreateInstance<T>();
            var srcType = src.GetType();
            var tgtType = tgt.GetType();
            foreach(var sp in srcType.GetRuntimeProperties())
            {
                var tp = tgtType.GetRuntimeProperty(sp.Name);
                var sti = sp.PropertyType.GetTypeInfo();
                if (sti.IsPrimitive || sti.IsAssignableFrom(typeof(string).GetTypeInfo())
                    || sti.IsAssignableFrom(typeof(DateTime).GetTypeInfo()))
                {
                    tp.SetValue(tgt, sp.GetValue(src));
                }
                else if(sti.IsArray)
                {
                    tp.SetValue(tgt, Activator.CreateInstance(sp.PropertyType));
                }
            }

            return tgt;
        }
    }
}
