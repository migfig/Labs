using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interviewer.Data
{
    public class Platform: BaseClass
    {
        public IEnumerable<KnowledgeArea> KnowledgeAreas { get; set; }
    }
}
