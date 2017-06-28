using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace RelatedRows.Domain
{
    public interface ITabularSource: IListSource
    {
        string Name { get; }
        ObservableCollection<ITabularColumn> Columns { get; }
        ObservableCollection<ITabularRow> Rows { get; }
        ITabularRow this[int index] { get; }
        ITabularSource LoadFromReader(IDataReader reader);
    }

    public class TabularSource : ITabularSource
    {
        public string Name { get; private set; }
        public ObservableCollection<ITabularColumn> Columns { get; private set; }
        public ObservableCollection<ITabularRow> Rows { get; private set; }

        public bool ContainsListCollection => false;

        public ITabularRow this[int index]
        {
            get { return Rows.FirstOrDefault(r => r.Index.Equals(index)); }
        }

        public TabularSource(CTable source)
        {
            Name = source.name;
            Columns = new ObservableCollection<ITabularColumn>(
                source.Column.Select(c => new TabularColumn(c.name, c.DbType, null)));
            Rows = new ObservableCollection<ITabularRow>();
        }

        public ITabularSource LoadFromReader(IDataReader reader)
        {
            var index = 0;
            if(reader.FieldCount > 0)
                while(reader.Read())
                {
                    Rows.Add(
                        new TabularRow(++index, Columns.Select(c => 
                            new TabularColumn(c.Name, c.DbType, reader.GetValue(reader.GetOrdinal(c.UnQuotedName())))).ToList()));
                }

            return this;
        }

        public IList GetList()
        {
            return Rows.ToList();
        }
    }

    public interface ITabularRow
    {
        int Index { get; }
        ObservableCollection<ITabularColumn> Data { get; }
        ObservableCollection<object> Values { get; }
        ITabularColumn this[string name] { get; }
    }

    public class TabularRow : ITabularRow
    {
        public int Index { get; private set; }
        public ObservableCollection<ITabularColumn> Data { get; private set; }
        public ObservableCollection<object> Values {
            get { return new ObservableCollection<object>(Data.Select(d => d.Value)); }
        }

        public ITabularColumn this[string name]
        {
            get { return Data.FirstOrDefault(c => c.Name.ToLower().Equals(name.ToLower())); }
        }

        public TabularRow(int index, IEnumerable<ITabularColumn> data)
        {
            Index = index;
            Data = new ObservableCollection<ITabularColumn>(data);
        }
    }

    public interface ITabularColumn
    {
        string Name { get; }
        eDbType DbType { get; }
        object Value { get; }
        void Set(object value);
        string UnQuotedName();
    }

    public class TabularColumn : ITabularColumn
    {
        public string Name { get; private set; }
        public eDbType DbType { get; private set; }
        public object Value { get; private set; }

        public TabularColumn(string name, eDbType dbType, object value)
        {
            Name = name;
            DbType = dbType;
            Value = value;
        }

        public void Set(object value)
        {
            Value = value;
        }

        public string UnQuotedName()
        {
            return Name.Replace("[", "").Replace("]", "");
        }
    }
}
