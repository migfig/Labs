using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interviewer.Data
{
    public class Question: BaseClass
    {
        public int Weight { get; set; }
        public int Level { get; set; }
        public string Value { get; set; }
    }
}
