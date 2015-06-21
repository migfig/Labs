using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelatedRecords
{
    public class DatatableEx
    {
        public DataTable Root { get; private set; }
        public ObservableCollection<DataTable> Children { get; private set; }

        public DatatableEx(DataTable root, ObservableCollection<DataTable> children)
        {
            Root = root;
            Children = children;
        }

        public DatatableEx(DataTable root, params DataTable[] children)
        {
            Root = root;
            Children = new ObservableCollection<DataTable>(children);
        }
    }
}
