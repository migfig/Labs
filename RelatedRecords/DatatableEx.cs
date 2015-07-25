using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelatedRecords
{
    public class DatatableEx
    {
        public TableContainer Root { get; private set; }
        public ObservableCollection<TableContainer> Children { get; private set; }

        public DatatableEx(TableContainer root, ObservableCollection<TableContainer> children)
        {
            Root = root;
            Children = children;
        }

        public DatatableEx(TableContainer root, params TableContainer[] children)
        {
            Root = root;
            Children = new ObservableCollection<TableContainer>(children);
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
