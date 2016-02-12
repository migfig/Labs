using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelatedRecords
{
    public class DataSourceProvider
    {
        private static DataSourceProvider _dataSourceProvider = new DataSourceProvider();
        public static DataSourceProvider Data
        {
            get { return _dataSourceProvider; }
        }

        private IDataSource _dataSource;
        public IDataSource Source
        {
            get { return _dataSource; }
        }

        public DataSourceProvider()
            : this(new DataSetDataSource())
             //this(new SqlDataSource())
        {
        }

        public DataSourceProvider(IDataSource dataSource)
        {
            _dataSource = dataSource;
        }
    }
}
