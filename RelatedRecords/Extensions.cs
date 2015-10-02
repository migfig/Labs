using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Collections.Generic;

namespace RelatedRecords
{
    public static class Extensions
    {
        #region static selected items

        private static CConfiguration _selectedConfiguration;
        public static CConfiguration SelectedConfiguration
        {
            get { return _selectedConfiguration; }
            set
            {
                _selectedConfiguration = value;
                SelectedDataset = _selectedConfiguration
                    .Dataset
                    .First(d => d.name == _selectedConfiguration.defaultDataset);
            }
        }

        private static CDataset _selectedDataset;
        public static CDataset SelectedDataset
        {
            get { return _selectedDataset; }
            set
            {
                if (value == null) return;

                _selectedDataset = value;
                SelectedDatasource = SelectedConfiguration
                    .Datasource.First(x => x.name == _selectedDataset.dataSourceName);

                foreach (var table in _selectedDataset.Table)
                {
                    if (table.Children.Count == 0)
                        table.AddChildren();
                }
            }
        }

        private static CDatasource _selectedDatasource;
        public static CDatasource SelectedDatasource
        {
            get { return _selectedDatasource; }
            set
            {
                _selectedDatasource = value;
            }
        }

        private static int _maxRowCount = 100;
        public static int MaxRowCount
        {
            get { return _maxRowCount; }
            set
            {
                _maxRowCount = value;
            }
        }

        #endregion static selected items

        public static void AddChildren(this CTable table)
        {
            table.Children = new ObservableCollection<CTable>(
                from r in SelectedDataset.Relationship
                where r.fromTable == table.name
                select SelectedDataset.Table.First(x => x.name == r.toTable)
                );
        }

        public static string ToSelectString(this CTable table, bool asStar = false)
        {
            var query = new StringBuilder();
            query.AppendFormat("SELECT TOP {0} ", MaxRowCount);
            if (asStar)
            {
                query.Append("*");
            }
            else
            {
                var isFirst = true;
                table.Column.ToList().ForEach(c => {
                    query.Append(c.ToSelectString(isFirst));
                    isFirst = false;
                });
            }
            query.AppendFormat(" FROM {0}", table.name);

            return query.ToString();
        }

        public static string ToSchemaString(this CTable table)
        {
            var query = new StringBuilder();
            query.AppendFormat("create Table [dbo].[{0}] (", table.name);
            var isFirst = true;
            table.Column.ToList().ForEach(c =>
            {
                query.Append(c.ToSchemaString(isFirst));
                isFirst = false;
            });
            query.Append(");");
            return query.ToString();
        }

        public static string ToSchemaDropString(this CTable table)
        {
            var query = new StringBuilder();
            query.AppendFormat("Drop Table [dbo].[{0}];", table.name);
            return query.ToString();
        }

        public static string ToWhereString(this CTable table, string[] operators, string[] andOrs,
            params IDbDataParameter[] pars)
        {
            var query = new StringBuilder();
            if (pars.Length > 0 && null != operators && operators.Length == pars.Length
                && null != andOrs && andOrs.Length >= operators.Length - 1)
            {
                query.Append("WHERE ");
                var idx = 0;
                pars.ToList().ForEach(p =>
                {
                    query.AppendFormat("{0}{1} {2} {3}",
                        idx == 0 ? string.Empty : " " + andOrs[idx - 1] + " ",
                        p.ParameterName,
                        operators[idx++],
                        QuoteValue(p.Value));
                });
            }
            return query.ToString().Trim();
        }

        public static string ToSelectWhereString(this CTable table, string[] operators, string[] andOrs,
            bool asStar = false, params IDbDataParameter[] pars)
        {
            var query = new StringBuilder();
            query.Append(table.ToSelectString(asStar));

            if (pars.Length > 0 && null != operators && operators.Length == pars.Length
                && null != andOrs && andOrs.Length >= operators.Length - 1)
            {
                query.Append(" WHERE ");
                var idx = 0;
                pars.ToList().ForEach(p =>
                {
                    query.AppendFormat("{0}{1} {2} {3}",
                        idx == 0 ? string.Empty : " " + andOrs[idx - 1] + " ",
                        p.ParameterName,
                        operators[idx++],
                        QuoteValue(p.Value));
                });
            }
            return query.ToString().Trim();
        }

        public static IDbDataParameter[] ToParams(this CQuery query)
        {
            var pars = new List<IDbDataParameter>();
            query.Parameter.ToList().ForEach(p => 
                pars.Add(new SqlParameter(p.name, GetParameterValue(p))));

            return pars.ToArray();
        }

        private static object GetParameterValue(CParameter p)
        {
            switch(p.type)
            {
                case eDbType.bigint:
                case eDbType.@long:
                    return Convert.ToInt64(p.defaultValue);
                case eDbType.binary:
                case eDbType.image:
                case eDbType.varbinary:
                    var i = 0;
                    var bytes = new byte[p.defaultValue.Length];
                    foreach (var c in p.defaultValue)
                        bytes[i++] = Convert.ToByte(c);
                    return bytes; 
                case eDbType.bit:
                case eDbType.@bool:
                    return Convert.ToBoolean(p.defaultValue);
                case eDbType.@char:
                case eDbType.nchar:
                    return Convert.ToChar(p.defaultValue);
                case eDbType.date:
                case eDbType.datetime:
                case eDbType.datetime2:
                case eDbType.datetimeoffset:
                case eDbType.smalldatetime:
                case eDbType.time:
                case eDbType.timestamp:
                    return Convert.ToDateTime(p.defaultValue);
                case eDbType.@decimal:
                case eDbType.@float:
                case eDbType.money:
                case eDbType.numeric:
                case eDbType.real:
                case eDbType.smallmoney:
                    return Convert.ToDecimal(p.defaultValue);                    
                case eDbType.geography:
                case eDbType.geometry:
                case eDbType.hierarchyid:
                    return p.defaultValue;
                case eDbType.@int:
                    return Convert.ToInt32(p.defaultValue);
                case eDbType.guid:
                case eDbType.uniqueidentifier:
                case eDbType.ntext:
                case eDbType.nvarchar:
                case eDbType.@string:
                case eDbType.text:
                case eDbType.varchar:
                case eDbType.sql_variant:
                case eDbType.xml:
                    return Convert.ToString(p.defaultValue);
                case eDbType.smallint:
                case eDbType.tinyint:
                    return Convert.ToInt16(p.defaultValue);
            }

            return p.defaultValue;
        }

        public static string QuoteValue(this IDbDataParameter parameter)
        {
            if (parameter.DbType == DbType.AnsiString
                || parameter.DbType == DbType.AnsiString
                || parameter.DbType == DbType.AnsiStringFixedLength
                || parameter.DbType == DbType.Date
                || parameter.DbType == DbType.DateTime
                || parameter.DbType == DbType.DateTime2
                || parameter.DbType == DbType.DateTimeOffset
                || parameter.DbType == DbType.Guid
                || parameter.DbType == DbType.String
                || parameter.DbType == DbType.StringFixedLength
                || parameter.DbType == DbType.Time)
                return "'" + parameter.Value.ToString() + "'";

            else if (parameter.DbType == DbType.Boolean)
                return bool.Parse(parameter.Value.ToString()) == true ? "1" : "0"; 

            return parameter.Value.ToString();
        }

        public static string ToSelectWhereString(this CQuery query, params IDbDataParameter[] pars)
        {
            if (query.isStoreProcedure) return query.name;

            var q = query.Text;

            if (pars.Length > 0)
            {
                pars.ToList().ForEach(p =>
                {
                    q = q.Replace("{{" + p.ParameterName + "}}", p.QuoteValue());
                });
            }

            return q;
        }

        private static string ParseTableName(string query)
        {
            return Regex.Match(query,
                @"SELECT[\[\sa-zA-Z0-9,\*\]]*FROM[\s]*(?<tablename>(\[\w*\s\w*\])|([\[]?\w*[\]]?))",
                RegexOptions.IgnoreCase)
                    .Groups["tablename"].Value;
        }

        public static async Task<DatatableEx> Query(this CQuery query, params IDbDataParameter[] pars)
        {
            var result = new DatatableEx(new TableContainer(new DataTable(query.name), new CTable { name = query.name }));
            var q = query.ToSelectWhereString(pars);

            Common.Extensions.TraceLog.Information("Running query {query} for query {name}", q, query.name);

            try
            {
                using (var connection = new SqlConnection(SelectedDatasource.ConnectionString))
                {
                    if (query.isStoreProcedure)
                    {                        
                        var reader = await connection.ExecuteReaderAsync(q, pars, commandType: CommandType.StoredProcedure);
                        result.Root.Table.Load(reader);
                        reader.Close();
                    }
                    else
                    {
                        var reader = await connection.ExecuteReaderAsync(q);
                        result.Root.Table.Load(reader);
                        reader.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Common.Extensions.ErrorLog.Error(e, "@ Query(CQuery) query: {q}, connectionString: {ConnectionString}", q, SelectedDatasource.ConnectionString.Trimmed());
            }

            return result;
        }

        public static async Task<DatatableEx> Query(this CTable table,
            string[] operators, string[] andOrs,
            bool asStar = false, params IDbDataParameter[] pars)
        {
            var result = new DatatableEx(new TableContainer(new DataTable(table.name), table));
            var query = table.ToSelectWhereString(operators, andOrs, asStar, pars);

            Common.Extensions.TraceLog.Information("Running query {query} for parent table {name}", query, table.name);

            try
            {
                using (var connection = new SqlConnection(SelectedDatasource.ConnectionString))
                {
                    var reader = await connection.ExecuteReaderAsync(query);
                    result.Root.Table.Load(reader);
                    reader.Close();
                }

                result.QueryChildren(result.Root.Table.Rows[0]);
            }
            catch (Exception e)
            {
                Common.Extensions.ErrorLog.Error(e, "@ Query(CTable) query: {query}, connectionString: {ConnectionString}", query, SelectedDatasource.ConnectionString.Trimmed());
            }

            return result;
        }

        public static async void QueryChildren(this DatatableEx parent, DataRow row)
        {
            var table = SelectedDataset.Table.First(t => t.name == parent.Root.Table.TableName);
            var queries = table.RelatedTablesSelect(row);
            if (!string.IsNullOrEmpty(queries))
            {
                foreach(var q in queries.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var skipLoadingData = false;
                    var tableName = ParseTableName(q);
                    var childTable = SelectedDataset.Table.First(x => x.name == tableName);
                    var relationship = table.GetRelationShip(childTable);
                    var child = parent.Children.FirstOrDefault(x => x.Root.Table.TableName == tableName);
                    var appendChild = child == null;

                    if (null != relationship)
                    {
                        if(null != child)
                        {
                            var filter = string.Format("{0}={1}", relationship.toColumn, row.Value(relationship.fromColumn));
                            var rows = child.Root.Table.Select(filter);
                            child.Root.Table.DefaultView.RowFilter = filter;
                            skipLoadingData = rows != null && rows.Any();
                        }
                    }

                    if (!skipLoadingData)
                    {
                        Common.Extensions.TraceLog.Information("Running query {q} for child table {tableName}",
                            q, tableName);

                        var tbl = await GetDataTable(q);
                        if (null != tbl)
                        {
                            if (child == null)
                            {
                                child = new DatatableEx(new TableContainer(tbl, childTable));
                            }
                            else
                            {
                                Common.Extensions.TraceLog.Information("Appending data for children table {name}",
                                    child.Root.ConfigTable.name);

                                tbl.CopyRows(child.Root.Table);
                            }
                        }
                    }

                    if (null != child)
                    {
                        if (child.Root.ConfigTable.Children.Count > 0
                            && child.Root.Table.Rows.Count > 0)
                        {
                            child.QueryChildren(child.Root.Table.Rows[0]);
                        }

                        if (appendChild && !parent.Children.Any(x => x.Root.Table.TableName == child.Root.Table.TableName))
                            parent.Children.Add(child);
                    }
                }
            }
        }

        private static async Task<DataTable> GetDataTable(string query)
        {
            try
            {
                using (var connection = new SqlConnection(SelectedDatasource.ConnectionString))
                {
                    var rdr = await connection.ExecuteReaderAsync(query);
                    var table = new DataTable(ParseTableName(query));
                    table.Load(rdr);
                    rdr.Close();

                    return table;
                }
            }
            catch (Exception e)
            {
                Common.Extensions.ErrorLog.Error(e, "@ GetDataTable() query: {query}, connectionString: {ConnectionString}", query, SelectedDatasource.ConnectionString.Trimmed());
            }

            return null;
        }

        private static void CopyRows(this DataTable source, DataTable target)
        {
            foreach (DataRow r in source.Rows)
            {
                var newRow = target.NewRow();
                foreach (DataColumn c in source.Columns)
                {
                    newRow[c.ColumnName] = r[c.ColumnName];
                }
                target.Rows.Add(newRow);
            }
        }

        public static string RelatedTablesSelect(this CTable table,
            DataRow row)
        {
            var query = new StringBuilder();
            var rels = from r in SelectedDataset.Relationship
                       where r.fromTable.ToLower() == table.name.ToLower()
                       select new {
                           Relation = r,
                           Table = SelectedDataset.Table
                            .First(t => t.name.ToLower() == r.toTable.ToLower())
                       };
            rels.ToList().ForEach(r =>
            {
                query.AppendFormat("{0} {1};",
                    r.Table.ToSelectString(true),
                    r.Table.ToWhereString(new string[] { "=" },
                        new string[] { "And" },
                        new SqlParameter {
                            ParameterName = r.Relation.toColumn,
                            Value = row[r.Relation.fromColumn]
                        }));
            });

            return query.ToString();
        }

        public static CRelationship GetRelationShip(this CTable table, CTable toTable)
        {
            return (from r in SelectedDataset.Relationship
                    where r.fromTable.ToLower() == table.name.ToLower()
                         && r.toTable.ToLower() == toTable.name.ToLower()
                    select r).FirstOrDefault();
        }

        public static DataTable ToDataTable(this CTable table, int testRows = 1)
        {
            var dtable = new DataTable(table.name);
            table.Column.ToList().ForEach(c => {
                dtable.Columns.Add(c.name, GetType(c.DbType));
            });

            for (var i = 0; i < testRows; i++)
            {
                var row = dtable.NewRow();
                table.Column.ToList().ForEach(c =>
                {
                    row[c.name] = GetDefaultValue(c, i + 1, table.name);
                });
                dtable.Rows.Add(row);
            }

            return dtable;
        }

        public static string ToInsertString(this CTable table, int testRows = 1)
        {
            var query = new StringBuilder();
            var dt = table.ToDataTable(testRows);
            for (var i = 0; i < testRows; i++)
            {
                query.AppendFormat("Insert Into [dbo].[{0}] (", table.name);
                var isFirst = true;
                table.Column.ToList().ForEach(c =>
                {
                    query.Append(c.ToSelectString(isFirst));
                    isFirst = false;
                });
                query.Append(") Values (");
                isFirst = true;
                table.Column.ToList().ForEach(c =>
                {
                    query.AppendFormat("{0}{1}",
                        isFirst ? string.Empty : ",",
                        QuoteValue(dt.Rows[i][c.name]));
                    isFirst = false;
                });
                query.Append(");");
            }

            return query.ToString();
        }

        private static object GetDefaultValue(CColumn column, int sequenceNum = 0, string tableName = "Value")
        {
            var type = GetType(column.DbType);
            try
            {
                var value = Activator.CreateInstance(type);
                if (sequenceNum > 0)
                {
                    if (value is int)
                    {
                        value = sequenceNum;
                    }
                    else if (value is long)
                    {
                        value = (long)sequenceNum;
                    }
                    else if (value is Guid)
                    {
                        value = Guid.Parse(sequenceNum.ToString("000") + value.ToString().Substring(3));
                    }
                }
                return value;
            }
            catch (Exception)
            {
                if (type == typeof(string))
                {
                    return string.Format("{0} {1}", tableName, sequenceNum);
                }

                return new object();
            }
        }

        private static Type GetType(eDbType type)
        {
            switch (type)
            {
                case eDbType.guid:
                    return typeof(Guid);
                case eDbType.@int:
                    return typeof(int);
                case eDbType.@long:
                    return typeof(long);
                case eDbType.@bool:
                    return typeof(bool);
                case eDbType.datetime:
                    return typeof(DateTime);
                case eDbType.@float:
                    return typeof(float);
                case eDbType.binary:
                    return typeof(byte[]);
                    //more types need to be added
            }

            return typeof(string);
        }

        private static string QuoteValue(object value)
        {
            if (value is string || value is Guid)
            {
                return "'" + value.ToString() + "'";
            }

            return value.ToString();
        }

        public static string ToSelectString(this CColumn column, bool isFirst = false)
        {
            var query = new StringBuilder();
            query.AppendFormat("{0}[{1}] ", isFirst ? string.Empty : ",", column.name);

            return query.ToString().Trim();
        }

        public static string ToSchemaString(this CColumn column, bool isFirst = false)
        {
            var query = new StringBuilder();
            query.AppendFormat("{0}[{1}] {2} {3} {4}", isFirst ? string.Empty : ",",
                column.name,
                GetTypeString(column.DbType).ToUpper(),
                column.isNullable ? "NULL" : "NOT NULL",
                column.isPrimaryKey ? "PRIMARY KEY" : string.Empty);

            return query.ToString().Trim();
        }

        public static string ToSchemaString(this CDataset dataset)
        {
            var query = new StringBuilder();
            var independent = from t in dataset.Table
                              where !(from r in dataset.Relationship
                                      select r.fromTable).Distinct().Contains(t.name)
                              select t;
            var dependent = from t in dataset.Table
                            where !(from r in dataset.Relationship
                                    select r.toTable).Distinct().Contains(t.name)
                            select t;
            var interdependent = from t in dataset.Table
                                 where !independent.Contains(t) && !dependent.Contains(t)
                                 select t;

            dependent.ToList().ForEach(t =>
            {
                //query.Append(t.ToSchemaDropString());
                query.Append(t.ToSchemaString());
            });
            independent.ToList().ForEach(t =>
            {
                //query.Append(t.ToSchemaDropString());
                query.Append(t.ToSchemaString());
            });
            interdependent.ToList().ForEach(t =>
            {
                //query.Append(t.ToSchemaDropString());
                query.Append(t.ToSchemaString());
            });

            dataset.Relationship.ToList().ForEach(r =>
            {
                query.Append(r.ToConstraint());
            });

            return query.ToString();
        }

        public static string ToSchemaDropString(this CDataset dataset)
        {
            var query = new StringBuilder();
            dataset.Table.ToList().ForEach(t =>
            {
                query.Append(t.ToSchemaDropString());
            });

            return query.ToString();
        }

        public static string ToInsertString(this CDataset dataset, int testRows = 1)
        {
            var query = new StringBuilder();
            var independent = from t in dataset.Table
                              where !(from r in dataset.Relationship
                                      select r.fromTable).Distinct().Contains(t.name)
                              select t;
            var dependent = from t in dataset.Table
                            where !(from r in dataset.Relationship
                                    select r.toTable).Distinct().Contains(t.name)
                            select t;
            var interdependent = from t in dataset.Table
                                 where !independent.Contains(t) && !dependent.Contains(t)
                                 orderby t.name
                                 select t;

            independent.ToList().ForEach(t =>
            {
                query.Append(t.ToInsertString(testRows));
            });
            interdependent.ToList().ForEach(t =>
            {
                query.Append(t.ToInsertString(testRows));
            });
            dependent.ToList().ForEach(t =>
            {
                query.Append(t.ToInsertString(testRows));
            });

            return query.ToString();
        }

        private static string GetTypeString(eDbType type)
        {
            switch (type)
            {
                case eDbType.guid:
                    return "UniqueIdentifier";
                case eDbType.@int:
                case eDbType.datetime:
                    return type.ToString();
                case eDbType.@long:
                    return "BigInt";
                case eDbType.@bool:
                    return "Bit";
                case eDbType.@float:
                    return "Decimal";
                case eDbType.binary:
                    return "Binary(MAX)";
                    //more types need to be added
            }

            return "Varchar(250)";
        }

        public static string ToWhereString(this CColumn column, object value, bool isFirst = false,
            string operatorValue = "=", string andOr = "And")
        {
            var query = new StringBuilder();
            if (value != null)
            {
                query.AppendFormat("{0}{1} {2} {3} ", isFirst ? string.Empty : " " + andOr + " ",
                    column.name,
                    operatorValue,
                    QuoteValue(value));
            }

            return query.ToString().Trim();
        }

        public static string ToWhereString(this CColumn column, IDbDataParameter value, bool isFirst = false,
            string operatorValue = "=", string andOr = "And")
        {
            var query = new StringBuilder();
            if (value != null)
            {
                query.AppendFormat("{0}{1} {2} {3} ", isFirst ? string.Empty : " " + andOr + " ",
                    column.name,
                    operatorValue,
                    QuoteValue(value.Value));
            }

            return query.ToString().Trim();
        }

        public static string ToConstraint(this CRelationship relationship)
        {
            var query = new StringBuilder();
            query.AppendFormat("Alter Table [dbo].[{0}] Add Constraint {1} Foreign Key ({2}) References [dbo].[{3}]({4});",
                relationship.fromTable,
                string.Format("FK_{0}_{1}",
                    relationship.toTable.Replace(" ", string.Empty),
                    relationship.toColumn.Replace(" ", string.Empty)),
                relationship.fromColumn,
                relationship.toTable,
                relationship.toColumn);

            return query.ToString();
        }

        public static bool AreEqual(this DataRowView row, DataRowView other)
        {
            if (null == other || null == row) return false;

            for (var i = 0; i < row.Row.ItemArray.Length; i++)
            {
                if (!row[i].Equals(other[i]))
                    return false;
            }

            return true;
        }

        public static string[] ToArray(this string value, params string[] parts)
        {
            var concat = value + "," + string.Join(",", parts);
            return concat.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static StringBuilder SqlInsert(this StringBuilder value, DatatableEx table)
        {
            if (null != table
                && table.Root.Table != null
                && table.Root.Table.Rows.Count > 0)
            {
                value.SqlInsert(table.Root.Table);
                foreach (var child in table.Children)
                    value.SqlInsert(child);
            }

            return value;
        }

        public static StringBuilder SqlInsert(this StringBuilder value, DataTable table)
        {
            if (null != table && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    value.SqlInsert(row);
                }

                value.Append(Environment.NewLine);
            }

            return value;
        }

        public static StringBuilder SqlInsert(this StringBuilder value, DataRow row)
        {
            try {
                value.AppendFormat("Insert Into {0} (", row.Table.TableName);
                var columns = row.Table.Columns.Cast<DataColumn>();
                var values = new StringBuilder("Values (");

                foreach (DataColumn col in columns)
                {
                    value.AppendFormat("[{0}]{1}", col.ColumnName, col != columns.Last() ? "," : string.Empty);
                    values.AppendFormat("{0}{1}", row.Value(col.ColumnName), col != columns.Last() ? "," : string.Empty);
                }
                values.Append(")");
                value.Append(") ");
                value.Append(values.ToString());
                value.Append(";");
                value.Append(Environment.NewLine);
            }
            catch(Exception e)
            {
                Common.Extensions.ErrorLog.Error(e, "@ SqlInsert(row)");
            }

            return value;
        }

        public static string Value(this DataRow row, string columnName)
        {
            var value = "NULL";
            switch (row.Table.Columns[columnName].DataType.ToString())
            {
                case "System.Boolean":
                    return row[columnName].ToString() == "True" ? "1" : "0";
                case "System.Byte[]":
                    var bytes = (byte[])row[columnName];
                    value = string.Empty;
                    foreach (var b in bytes)
                        value += b;
                    return value;
                case "System.Int32":
                case "System.Int64":
                case "System.Decimal":
                case "System.Double":
                case "System.Single":
                case "System.Int16":
                case "System.Byte":
                    return row[columnName].ToString();
                case "System.String":
                case "System.DateTime":
                case "System.TimeSpan":
                case "System.DateTimeOffset":
                case "System.Guid":
                case "System.Object":
                    return string.Format("'{0}'", row[columnName]);
            }

            return value;
        }

        public static string Inflated(this string value)
        {
            var pwdRegEx = new Regex(ConfigurationManager.AppSettings["passwordRegEx"]
                .Replace("&lt;", "<")
                .Replace("&gt;", ">")
                .Replace("&amp;", "&")
                .Replace("&quot;", "\"")
                , RegexOptions.IgnoreCase);

            var match = pwdRegEx.Match(value);
            if (null != match && match.Success)
            {
                var passwordKey = match.Groups["passwordkey"].Value;
                var passwordValue = match.Groups["passwordvalue"].Value;
                if (!string.IsNullOrEmpty(passwordKey) 
                    && !string.IsNullOrEmpty(passwordValue))
                {
                    try {
                        value = value.Replace(
                            string.Format("{0}={1}", passwordKey, passwordValue),
                            string.Format("Password={0}", Common.Helpers.Flat.Inflate(passwordValue, "Whatever*(itConvains~!@#|=-+_()*&&^%$#@!~")));
                    } catch(Exception) {; }
                }
            }

            return value;
        }

        public static string Deflated(this string value)
        {
            var pwdRegEx = new Regex(ConfigurationManager.AppSettings["passwordRegEx"]
                .Replace("&lt;", "<")
                .Replace("&gt;", ">")
                .Replace("&amp;", "&")
                .Replace("&quot;", "\"")
                , RegexOptions.IgnoreCase);

            var match = pwdRegEx.Match(value);
            if (null != match && match.Success)
            {
                var passwordKey = match.Groups["passwordkey"].Value;
                var passwordValue = match.Groups["passwordvalue"].Value;
                if (!string.IsNullOrEmpty(passwordKey) && !string.IsNullOrEmpty(passwordValue))
                {
                    try {
                        value = value.Replace(
                            string.Format("{0}={1}", passwordKey, passwordValue),
                            string.Format("Password={0}", Common.Helpers.Flat.Deflate(passwordValue, "Whatever*(itConvains~!@#|=-+_()*&&^%$#@!~")));
                    } catch(Exception) {; }
                }
            }

            return value;
        }

        public static string Trimmed(this string value)
        {
            var pwdRegEx = new Regex(ConfigurationManager.AppSettings["passwordRegEx"]
                .Replace("&lt;", "<")
                .Replace("&gt;", ">")
                .Replace("&amp;", "&")
                .Replace("&quot;", "\"")
                , RegexOptions.IgnoreCase);

            var match = pwdRegEx.Match(value);
            if (null != match && match.Success)
            {
                var passwordKey = match.Groups["passwordkey"].Value;
                var passwordValue = match.Groups["passwordvalue"].Value;
                if (!string.IsNullOrEmpty(passwordKey) && !string.IsNullOrEmpty(passwordValue))
                {
                    try
                    {
                        value = value.Replace(
                            string.Format("{0}={1}", passwordKey, passwordValue),
                            string.Empty);
                    }
                    catch (Exception) {; }
                }
            }

            return value;
        }
    }
}
