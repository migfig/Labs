using Common.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Controls;

namespace RelatedRecords.Wpf.ViewModels
{
    public partial class MainViewModel
    {
        public ObservableCollection<string> SelectedDataTableColumns
        {
            get
            {
                ObservableCollection<string> list = new ObservableCollection<string>();

                if (null != WorkingTable && null != WorkingTable.Root.Table)
                    foreach (DataColumn dc in WorkingTable.Root.Table.Columns)
                        list.Add(dc.ColumnName);

                return list;
            }
        }

        public DatatableEx WorkingTable
        {
            get
            {
                return SelectedViewType == eViewType.Datasets
                       ? SelectedDataTable
                       : SelectedRootTable;
            }
            set
            {
                if (SelectedViewType == eViewType.Datasets)
                    SelectedDataTable = value;
                else if (SelectedViewType == eViewType.Tables)
                    SelectedRootTable = value;
            }
        }

        public ObservableCollection<string> SelectedColumnOperators
        {
            get
            {
                ObservableCollection<string> list = new ObservableCollection<string>();
                
                if (null != WorkingTable && !string.IsNullOrEmpty(this._selectedColumn))
                    foreach (DataColumn dc in WorkingTable.Root.Table.Columns)
                    {
                        if (dc.ColumnName.Equals(this._selectedColumn))
                        {
                            string opString = string.Empty;
                            switch (getMappingType(getColumnType(dc.ColumnName)).ToString()) //dc.DataType.ToString()
                            {
                                case "System.DateTime":
                                case "System.Int32":
                                case "System.Double":
                                    opString = "=,<>,>,>=,<,<=,NULL,NOT NULL";
                                    break;
                                case "System.Boolean":
                                    opString = "=,<>,NULL,NOT NULL";
                                    break;
                                default:
                                    opString = "=,<>,Starts With,Ends With,NULL,NOT NULL";
                                    break;
                            }
                            foreach (string op in opString.Split(new char[] { ',' }))
                                if (!list.Contains(op))
                                    list.Add(op);

                            if (list.Count > 0 && string.IsNullOrEmpty(this._selectedOperator))
                                this.SelectedOperator = list[0];
                            break;
                        }
                    }

                return list;
            }
        }

        public Type getMappingType(string colType)
        {
            string typeName = colType.ToLower().Split(new char[] { '(' })[0];
            Dictionary<Type, List<string>> types = new Dictionary<Type, List<string>>{
                {typeof(System.Int32), new List<string> {"int", "smallint"}}
                ,{typeof(System.Byte[]), new List<string> {"image", "blob", "binary"}}
                ,{typeof(System.DateTime), new List<string> {"datetime", "datetime2", "date", "timestamp", "datetimeoffset"}}
                ,{typeof(System.Double), new List<string> {"money", "real", "number", "bigint", "decimal", "float", "uniqueidentifier"}}
                ,{typeof(System.Boolean), new List<string> {"bit"}}
            };

            var theType = from key in types.Keys
                          where types[key].Contains(colType)
                          select key;
            if (null != theType && null != theType.FirstOrDefault())
                return theType.FirstOrDefault();

            return typeof(System.String);
        }

        public string getColumnType(string columnName)
        {
            if (null != WorkingTable && SelectedDataset.Table.Count > 0)
            {
                var column = from col in
                                 (from table in SelectedDataset.Table
                                  where table.name.Equals(WorkingTable.Root.ConfigTable.name)
                                  select table).FirstOrDefault().Column
                             where col.name.Equals(columnName)
                             select col;
                return column.FirstOrDefault().DbType.ToString();
            }

            return "System.String";
        }

        public bool UseSchemaConstraints
        {
            get { return Properties.Settings.Default.UseSchemaConstraints; }
            set
            {
                if (Properties.Settings.Default.UseSchemaConstraints == value) return;
                Properties.Settings.Default.UseSchemaConstraints = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        public int MaxRowCount
        {
            get { return Properties.Settings.Default.DefaultMaxRowCount; }
            set
            {
                if (Properties.Settings.Default.DefaultMaxRowCount == value) return;
                Properties.Settings.Default.DefaultMaxRowCount = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged();
                Extensions.MaxRowCount = value;
            }
        }

        public List<int> MaxRows
        {
            get
            {
                var list = new List<int>();
                foreach (var item in Properties.Settings.Default.MaxRows)
                    list.Add(int.Parse(item));

                return list;
            }
        }

        public DataGridSelectionMode SelectionMode
        {
            get { return (DataGridSelectionMode)Enum.Parse(typeof(DataGridSelectionMode), Properties.Settings.Default.DefaultSelectionMode); }
            set
            {
                if (value.ToString() == Properties.Settings.Default.DefaultSelectionMode) return;
                Properties.Settings.Default.DefaultSelectionMode = value.ToString();
                Properties.Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        public List<DataGridSelectionMode> SelectionModes
        {
            get
            {
                var list = new List<DataGridSelectionMode>();
                foreach (var item in Properties.Settings.Default.SelectionModes)
                    list.Add((DataGridSelectionMode)Enum.Parse(typeof(DataGridSelectionMode), item));

                return list;
            }
        }

        public DataGridClipboardCopyMode CopyMode
        {
            get { return (DataGridClipboardCopyMode)Enum.Parse(typeof(DataGridClipboardCopyMode), Properties.Settings.Default.DefaultCopyMode); }
            set
            {
                if (value.ToString() == Properties.Settings.Default.DefaultCopyMode) return;
                Properties.Settings.Default.DefaultCopyMode = value.ToString();
                Properties.Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        public List<DataGridClipboardCopyMode> CopyModes
        {
            get
            {
                var list = new List<DataGridClipboardCopyMode>();
                foreach (var item in Properties.Settings.Default.CopyModes)
                    list.Add((DataGridClipboardCopyMode)Enum.Parse(typeof(DataGridClipboardCopyMode), item));

                return list;
            }
        }

        private string _searchCriteria = string.Empty;
        public string SearchCriteria
        {
            get { return _searchCriteria; }
            set
            {
                if (_searchCriteria == value) return;
                _searchCriteria = value;
                OnPropertyChanged("SearchCriteria");
                this.FilterCriteria = value;
                OnPropertyChanged("ClipboardText");
                SearchCommand.AsRelay().RaiseCanExecuteChanged();
                CopyCommand.AsRelay().RaiseCanExecuteChanged();
            }
        }

        private string _filterCriteria = string.Empty;
        public string FilterCriteria
        {
            get { return _filterCriteria; }
            set
            {
                if (_filterCriteria == value) return;
                _filterCriteria = value;
                OnPropertyChanged("FilterCriteria");
                FilterCommand.AsRelay().RaiseCanExecuteChanged();
            }
        }

        private string _selectedColumn = string.Empty;
        public string SelectedColumn
        {
            get { return _selectedColumn; }
            set
            {
                if (_selectedColumn == value) return;
                _selectedColumn = value;
                OnPropertyChanged();
                OnPropertyChanged("SelectedColumnOperators");
                OnPropertyChanged("SelectedOperator");
                OnPropertyChanged("ClipboardText");
                FilterCommand.AsRelay().RaiseCanExecuteChanged();
            }
        }

        private string _selectedOperator = "=";
        public string SelectedOperator
        {
            get { return _selectedOperator; }
            set
            {
                if (_selectedOperator == value) return;
                _selectedOperator = value;
                OnPropertyChanged();
                OnPropertyChanged("ClipboardText");
                FilterCommand.AsRelay().RaiseCanExecuteChanged();
            }
        }

        public string SelectedCriteria
        {
            get
            {
                if (!string.IsNullOrEmpty(this._selectedColumn)
                    && !string.IsNullOrEmpty(this._selectedOperator)
                    && !string.IsNullOrEmpty(this._filterCriteria))
                    return string.Format("{0} {1} '{2}'",
                        this._selectedColumn, this._selectedOperator, this._filterCriteria);

                return string.Empty;
            }

            set
            {
                if (!string.IsNullOrEmpty(value) && !value.Equals("<empty>"))
                {
                    string[] values = value.Split(' ');
                    this.SelectedColumn = values[0];
                    this.SelectedOperator = values[1];
                    this.FilterCriteria = values[2].Replace("'", string.Empty);
                    this.SearchCriteria = this.FilterCriteria;
                    _searchCommand.Execute(null);
                }
            }
        }

        public string SelectedSearchCriteria
        {
            get
            {
                if (!string.IsNullOrEmpty(this._selectedColumn)
                    && !string.IsNullOrEmpty(this._selectedOperator)
                    && !string.IsNullOrEmpty(this._searchCriteria))
                    return getSelectedSearchCriteriaFromProvider();

                return string.Empty;
            }
        }

        private string getSelectedSearchCriteriaFromProvider()
        {
            switch (this._selectedOperator)
            {
                case "Starts With":
                    return string.Format("{0} {1} {2}"
                        , this._selectedColumn
                        , "LIKE"
                        , string.Format("'{0}%'", this._searchCriteria));
                case "Ends With":
                    return string.Format("{0} {1} {2}"
                        , this._selectedColumn
                        , "LIKE"
                        , string.Format("'%{0}'", this._searchCriteria));
                case "NULL":
                    return string.Format("{0} {1}"
                        , this._selectedColumn
                        , "IS NULL");
                case "NOT NULL":
                    return string.Format("{0} {1}"
                        , this._selectedColumn
                        , "IS NOT NULL");
                default:
                    if (null != SelectedRootDataRowView
                        && null != SelectedRootDataRowView.Row
                        && null != SelectedColumn)
                    {
                        return string.Format("{0} {1} {2}"
                            , this._selectedColumn
                            , this._selectedOperator
                            , SelectedRootDataRowView.Row.Value(SelectedColumn));
                    }
                    else
                    {
                        return string.Format("{0} {1} {2}"
                            , this._selectedColumn
                            , this._selectedOperator
                            , string.Format("'{0}'", this._searchCriteria));
                    }
            }
        }
    }
}
