using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reflector
{
    public class StrBuilder
    {
        private readonly StringBuilder _builder;

        public StrBuilder()
            :this(string.Empty)
        {
        }

        public StrBuilder(string start, params StrBuilder[] builder)
        {
            _builder = new StringBuilder(start);
            if (null != builder && builder.Any())
            {
                foreach (var b in builder)
                {
                    _builder.Append(b.ToString());
                }
            }
        }

        public StrBuilder(string start, IEnumerable<StrBuilder> builder)
        {
            _builder = new StringBuilder(start);
            if (null != builder && builder.Any())
            {
                foreach (var b in builder)
                {
                    _builder.Append(b.ToString());
                }
            }
        }

        public override string ToString()
        {
            return _builder.ToString();
        }
    }
}
