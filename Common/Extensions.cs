using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class Extensions
    {
        public static string Quote(this string value, string append = "")
        {
            return string.Format("\"{0}\"{1}", value, append);
        }
    }
}
