using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace Common.Controllers
{
    [RoutePrefix("api/xslt")]
    public class XsltController: ApiController
    {
        [Route("transform"), HttpPost]
        public async Task<string> TransformXml([FromBody] XElement xml, string styleSheet)
        {
            var result = string.Empty;
            try
            {
                var xslt = new XslCompiledTransform(true);
                xslt.Load(Path.Combine(ConfigurationManager.AppSettings["xsltPath"], styleSheet));

                var builder = new StringBuilder();
                using (var stream = new StringWriter(builder))
                {
                    xslt.Transform(xml.CreateReader(), null, stream);
                }

                result = builder.ToString();
            } catch(Exception) {; }

            return await Task.FromResult(result);
        }
    }
}
