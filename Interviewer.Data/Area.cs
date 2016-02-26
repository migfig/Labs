using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interviewer.Data
{
    public class Area: BaseClass
    {
        public virtual IEnumerable<Question> Questions { get; set; }
    }
}
