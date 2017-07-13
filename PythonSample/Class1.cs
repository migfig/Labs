using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LumenWorks.Framework.IO.Csv;

namespace PythonSample
{
    public class Class1
    {
        public void LoadCsv()
        {
            using (var csv = new CachedCsvReader(new StreamReader("data.csv"), false))
            {
                csv.Columns.Add(new Column { Name = "PriceDate", Type = typeof(DateTime) });
                csv.Columns.Add(new Column { Name = "OpenPrice", Type = typeof(decimal) });
                csv.Columns.Add(new Column { Name = "HighPrice", Type = typeof(decimal) });
                csv.Columns.Add(new Column { Name = "LowPrice", Type = typeof(decimal) });
                csv.Columns.Add(new Column { Name = "ClosePrice", Type = typeof(decimal) });
                csv.Columns.Add(new Column { Name = "Volume", Type = typeof(int) });

                // Field headers will now be picked from the Columns collection
                //csv.Dump();
                //csv[]
            }
        }
    }
}
