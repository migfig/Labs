using System.Collections.ObjectModel;
using System.Data;

namespace RelatedRecords
{
    public class DatatableEx
    {
        public TableContainer Root { get; private set; }
        public ObservableCollection<DatatableEx> Children { get; private set; }

        public DatatableEx(TableContainer root, ObservableCollection<DatatableEx> children)
        {
            Root = root;
            Children = children;
        }

        public DatatableEx(TableContainer root, params DatatableEx[] children)
        {
            Root = root;
            Children = new ObservableCollection<DatatableEx>(children);
        }

        public override string ToString()
        {
            return null != Root && null != Root.Table ?
                Root.Table.TableName : string.Empty;
        }
    }

    public class TableContainer
    {
        public DataTable Table { get; set; }
        public CTable ConfigTable { get; set; }

        public TableContainer(DataTable table, CTable configTable)
        {
            Table = table;
            ConfigTable = configTable;
        }
    }
}
