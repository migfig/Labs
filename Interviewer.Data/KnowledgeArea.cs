using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interviewer.Data
{
    public class KnowledgeArea: BaseClass
    {
        public int PlatformId { get; set; }
        public virtual IEnumerable<Area> Areas { get; set; }
    }
}
