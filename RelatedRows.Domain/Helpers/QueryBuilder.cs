using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RelatedRows.Domain
{
    public static class QueryBuilder
    {
        public static string GetQuery(this CTable table, long top, long skip = 0, CTable parent = null, DataRow row = null)
        {
            var query = $"SELECT *, COUNT(*) OVER() AS [CountOver$] FROM (";
            var subquery = $"SELECT * FROM {table.catalog}.{table.schemaName}.{table.name}";
            var offsetFetch = $") AS Q ORDER BY 1 OFFSET {skip} ROWS FETCH NEXT {top} ROWS ONLY";

            if (parent != null 
                && parent.Relationship.Any(r => r.toTable.Equals(table.name)) 
                && parent.DataTable != null && parent.DataTable.Rows.Count > 0)
            {
                row = row ?? parent.DataTable.Rows[0];

                subquery += parent.Relationship
                    .Where(r => r.toTable.Equals(table.name))
                    .SelectMany(r => r.ColumnRelationship)
                    .Aggregate(" WHERE", 
                        (seed, cr) => seed + $" AND {cr.toColumn} = {row.Value(cr.fromColumn.UnQuoteName())}");

                return subquery.Replace("WHERE AND ", "WHERE ");
            }

            return query + subquery + offsetFetch;
        }

        public static string GetQueryTooltip(this CTable table, string column, DataRow row)
        {
            var query = $"SELECT * FROM {table.catalog}.{table.schemaName}.{table.name} WHERE {column.QuoteName()} = {row.Value(column.UnQuoteName())}";

            query += Environment.NewLine
                    + $"UPDATE {table.catalog}.{table.schemaName}.{table.name} SET "
                    + table.Column
                        .Where(c => !c.name.Equals(column.QuoteName()))
                        .Aggregate("",
                            (seed, c) => seed + $", {c.name} = {row.Value(c.name.UnQuoteName())}")
                    + $" WHERE {column.QuoteName()} = {row.Value(column.UnQuoteName())}";

            query += Environment.NewLine
                + $"DELETE FROM {table.catalog}.{table.schemaName}.{table.name} WHERE {column.QuoteName()} = {row.Value(column.UnQuoteName())}";

            var hasIdentity = table.Column.Any(x => x.isIdentity);
            query += Environment.NewLine;

            if (hasIdentity)
                query += string.Format("SET IDENTITY_INSERT {0}.{1}.{2} ON{3}", table.catalog, table.schemaName, table.name, Environment.NewLine);
            query += $"INSERT INTO {table.catalog}.{table.schemaName}.{table.name} ("
                    + table.Column
                        .Aggregate("",
                            (seed, c) => seed + $", {c.name}")
                    + ") VALUES ("
                    + table.Column
                        .Aggregate("",
                            (seed, c) => seed + $", {row.Value(c.name.UnQuoteName())}")
                    + ")";
            if (hasIdentity)
            {
                query += Environment.NewLine;
                query += string.Format("SET IDENTITY_INSERT {0}.{1}.{2} OFF{3}", table.catalog, table.schemaName, table.name, Environment.NewLine);
            }

            return query.Replace("SET ,", "SET ").Replace("(, ", "(");
        }

        public static string UnQuoteName(this string value)
        {
            return value.Replace("[","").Replace("]","");
        }

        public static string QuoteName(this string value)
        {
            return !value.StartsWith("[") ? $"[{value}]" : value;
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

        public static bool AreEqual(this DataRow row, DataRow other)
        {
            if (null == other || null == row) return false;

            for (var i = 0; i < row.ItemArray.Length; i++)
            {
                if (!row[i].Equals(other[i]))
                    return false;
            }

            return true;
        }

        public static long RowsCount(this DataTable table)
        {
            var rows = table != null && !string.IsNullOrEmpty(table.Namespace) ? table.Namespace : "0";
            Logger.Log.Verbose("{@rows} rows found for table {@tableName}", rows, table.TableName);

            return long.Parse(rows);
        }

        public static DataTable ToDataTable(this CTable table)
        {
            var dataTable = new DataTable(table.name);
            dataTable.Columns.AddRange(
                table.Column.Select(c => new DataColumn(c.name.UnQuoteName(), GetType(c.DbType))).ToArray());
            return dataTable;
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

        public static string SqlInsert(this CTable table, bool includeChildren = true, IList<string> tables = null)
        {
            tables = tables ?? new List<string>();
            if (tables.Contains(table.name)) return string.Empty;

            var value = new StringBuilder();
            if (null != table
                && table.DataTable != null
                && table.DataTable.Rows.Count > 0)
            {
                var hasIdentity = table.Column.Any(x => x.isIdentity);
                if (hasIdentity)
                    value.AppendFormat("SET IDENTITY_INSERT {0}.{1}.{2} ON{3}", table.catalog, table.schemaName, table.name, Environment.NewLine);
                value.SqlInsert(table.DataTable);
                if (hasIdentity)
                    value.AppendFormat("SET IDENTITY_INSERT {0}.{1}.{2} OFF{3}", table.catalog, table.schemaName, table.name, Environment.NewLine);
                value.Append(Environment.NewLine);

                tables.Add(table.name);

                if (includeChildren)
                {
                    foreach (var child in table.Children)
                        value.Append(child.SqlInsert(includeChildren, tables));
                }
            }

            return value.ToString();
        }

        public static StringBuilder SqlInsert(this StringBuilder value, DataTable table)
        {
            if (null != table && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                    value.SqlInsert(row);
            }

            return value;
        }           

        public static StringBuilder SqlInsert(this StringBuilder value, DataRow row)
        {
            try
            {
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return value;
        }

        public static string CsvExport(this DataTable table)
        {
            var value = new StringBuilder();
            if (null != table
                && table.Rows.Count > 0)
            {
                value.AppendFormat("{0}{1}",
                    table.Columns.Cast<DataColumn>().Aggregate("",
                        (seed, c) => seed + $",{c.ColumnName}").Substring(1), 
                    Environment.NewLine);

                foreach (DataRow row in table.Rows)
                    value.AppendFormat("{0}{1}", table.Columns.Cast<DataColumn>().Aggregate("",
                        (seed, c) => seed + $",{row.Value(c.ColumnName, "\"")}").Substring(1), 
                        Environment.NewLine);
            }

            return value.ToString();
        }

        public static string Value(this DataRow row, string columnName, string quoteChar = "'")
        {
            var value = "NULL";
            if (!row.Table.Columns.Contains(columnName) || row[columnName] is DBNull) return value;

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
                    return string.Format("{0}{1}{0}", quoteChar, row[columnName].ToString().Replace("'","''"));
            }

            return value;
        }

        private static string PwdRegex = @"([\s]*(?<passwordkey>password|Password|PASSWORD)=(?<passwordvalue>[a-zA-Z\s\.0-9%\\\^\`~!@#\$&\*\(\)_+\-=\[\{\]\}\]|:',\<\>/\?]*))";

        public static string Inflated(this string value)
        {
            var pwdRegEx = new Regex(PwdRegex, RegexOptions.IgnoreCase);

            var match = pwdRegEx.Match(value);
            if (null != match && match.Success)
            {
                var passwordKey = match.Groups["passwordkey"].Value;
                var passwordValue = match.Groups["passwordvalue"].Value;
                if (!string.IsNullOrEmpty(passwordKey)
                    && !string.IsNullOrEmpty(passwordValue))
                {
                    try
                    {
                        value = value.Replace(
                            string.Format("{0}={1}", passwordKey, passwordValue),
                            string.Format("Password={0}", Flat.Inflate(passwordValue, "Whatever*(itConvains~!@#|=-+_()*&&^%$#@!~")));
                    }
                    catch (Exception) {; }
                }
            }

            return value;
        }

        public static string Deflated(this string value)
        {
            var pwdRegEx = new Regex(PwdRegex, RegexOptions.IgnoreCase);

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
                            string.Format("Password={0}", Flat.Deflate(passwordValue, "Whatever*(itConvains~!@#|=-+_()*&&^%$#@!~")));
                    }
                    catch (Exception) {; }
                }
            }

            return value;
        }

        public static string SecureString(this string value)
        {
            return string.Join(";",value.Split(';').Where(s => !s.ToLower().Contains("password")));
        }
    }
}
