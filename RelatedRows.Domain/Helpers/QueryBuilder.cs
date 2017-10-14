using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RelatedRows.Domain
{
    public abstract class QueryBuilderBase
    {
        public abstract string GetQuery(CTable table, long top, long skip = 0, CTable parent = null, DataRow row = null);
        public abstract string GetQueryTooltip(CTable table, string column, DataRow row);
        public abstract string SqlInsert(CTable table, bool includeChildren = true, IList<string> tables = null);

        public virtual void SqlInsert(StringBuilder value, DataTable table)
        {
            if (null != table && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                    this.SqlInsert(value, row);
            }
        }

        public virtual void SqlInsert(StringBuilder value, DataRow row)
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
        }
    }

    public static class QueryBuilderExt
    {
        public static string UnQuoteName(this string value, string openQuoteChar = "[", string closeQuoteChar = "]")
        {
            return value
                .Replace(openQuoteChar, "").Replace(closeQuoteChar, "")
                .Replace("\"", "");
        }

        public static string QuoteName(this string value, string openQuoteChar = "[", string closeQuoteChar = "]")
        {
            return !value.StartsWith(openQuoteChar) 
                ? $"{openQuoteChar}{value}{closeQuoteChar}" 
                : !value.StartsWith("\"") ? $"\"{value}\"" : value;
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

        public static string GetQueryTooltip(this CTable table, string column, DataRow row)
        {
            var query = $"SELECT * FROM {table.catalog}.{table.schemaName}.{table.name} WHERE {column.UnQuoteName().QuoteName("\"","\"")} = {row.Value(column.UnQuoteName())}";

            query += Environment.NewLine
                    + $"UPDATE {table.catalog}.{table.schemaName}.{table.name} SET "
                    + table.Column
                        .Where(c => !c.name.Equals(column.QuoteName()))
                        .Aggregate("",
                            (seed, c) => seed + $", {c.name} = {row.Value(c.name.UnQuoteName())}")
                    + $" WHERE {column.UnQuoteName().QuoteName("\"", "\"")} = {row.Value(column.UnQuoteName())}";

            query += Environment.NewLine
                + $"DELETE FROM {table.catalog}.{table.schemaName}.{table.name} WHERE {column.UnQuoteName().QuoteName("\"", "\"")} = {row.Value(column.UnQuoteName())}";

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
                    return string.Format("{0}{1}{0}", quoteChar, row[columnName].ToString().Replace("'", "''"));
            }

            return value;
        }

        public static string QuoteParam(this CColumn col, string quoteChar = "'")
        {
            if(col.DbType.IsString())
                    return $"'@{col.name.UnQuoteName()}'";

            return $"@{col.name.UnQuoteName()}";
        }

        public static string DefaultValue(this CParameter parameter)
        {
            var value = "NULL";

            switch (parameter.type)
            {
                case eDbType.@bool:
                case eDbType.bit:
                    return "0";
                case eDbType.binary:
                case eDbType.varbinary:
                case eDbType.sql_variant:
                case eDbType.image:
                    return "0x0";
                case eDbType.@int:
                case eDbType.@long:
                case eDbType.@decimal:
                case eDbType.real:
                case eDbType.money:
                case eDbType.smallmoney:
                case eDbType.tinyint:
                case eDbType.@float:
                case eDbType.bigint:
                case eDbType.numeric:
                case eDbType.smallint:
                    return default(Int32).ToString();
                case eDbType.@string:
                case eDbType.varchar:
                case eDbType.nvarchar:
                case eDbType.@char:
                case eDbType.text:
                case eDbType.xml:
                case eDbType.ntext:
                case eDbType.nchar:
                    return string.Empty;
                case eDbType.date:
                case eDbType.datetime:
                case eDbType.datetime2:
                case eDbType.datetimeoffset:
                    return DateTime.UtcNow.ToString("yyyy-mm-dd");
                case eDbType.guid:
                case eDbType.uniqueidentifier:
                    return Guid.Empty.ToString();
            }

            return value;
        }

        public static string DefaultValue(this CColumn col, string defaultValue = "")
        {
            if (!string.IsNullOrEmpty(defaultValue)) return defaultValue;

            var value = "NULL";

            switch (col.DbType)
            {
                case eDbType.@bool:
                case eDbType.bit:
                    return "0";
                case eDbType.binary:
                case eDbType.varbinary:
                case eDbType.sql_variant:
                case eDbType.image:
                    return "0x0";
                case eDbType.@int:
                case eDbType.@long:
                case eDbType.@decimal:
                case eDbType.real:
                case eDbType.money:
                case eDbType.smallmoney:
                case eDbType.tinyint:
                case eDbType.@float:
                case eDbType.bigint:
                case eDbType.numeric:
                case eDbType.smallint:
                    return default(Int32).ToString();
                case eDbType.@string:
                case eDbType.varchar:
                case eDbType.nvarchar:
                case eDbType.@char:
                case eDbType.text:
                case eDbType.xml:
                case eDbType.ntext:
                case eDbType.nchar:
                    return string.Empty;
                case eDbType.date:
                case eDbType.datetime:
                case eDbType.datetime2:
                case eDbType.datetimeoffset:
                case eDbType.time:
                case eDbType.timestamp:
                    return DateTime.UtcNow.ToString("yyyy-mm-dd");
                case eDbType.guid:
                case eDbType.uniqueidentifier:
                    return Guid.Empty.ToString();
            }

            return value;
        }        

        public static object GetDefaultValue(this CColumn column, string @operator, string defaultValue = "")
        {
            if (@operator.ToLower().StartsWith("is"))
                return "NULL";
            else if (@operator.ToLower().StartsWith("like"))
                return "'%" + column.DefaultValue(defaultValue) + "%'";
            else
                return column.DbType.IsString() 
                    ? column.DefaultValue(defaultValue).ToString().QuoteName("'","'") 
                    : column.DefaultValue(defaultValue);
        }       

        public static bool IsString(this eDbType type)
        {
            switch (type)
            {
                case eDbType.@string:
                case eDbType.varchar:
                case eDbType.nvarchar:
                case eDbType.@char:
                case eDbType.text:
                case eDbType.xml:
                case eDbType.ntext:
                case eDbType.nchar:
                case eDbType.date:
                case eDbType.datetime:
                case eDbType.datetime2:
                case eDbType.datetimeoffset:
                case eDbType.time:
                case eDbType.timestamp:
                case eDbType.guid:
                case eDbType.uniqueidentifier:
                    return true;
            }
            return false;
        }

        public static string RestoreText(this string value, CParameter parameter)
        {
            //WHERE  [ArtistId] = @ArtistId AND [Name] = '@Name'
            value =
                value
                    .Replace($" AND {parameter.name.Replace("@", string.Empty).QuoteName()} = ", string.Empty)
                    .Replace($"{parameter.name.Replace("@", string.Empty).QuoteName()} = ", string.Empty)
                    .Replace((parameter.type.IsString() ? $"'{parameter.name}'" : parameter.name), string.Empty);

            return value //TODO: cleanup needs a better approach!
                .Replace("WHERE AND ", "WHERE ")
                .Replace("WHERE  AND ", "WHERE ")
                .Replace("WHERE   AND ", "WHERE ")
                .Replace("WHERE  ;", ";")
                .Replace("WHERE ;", ";")
                .Trim();
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
            return string.Join(";", value.Split(';').Where(s => !s.ToLower().Contains("password")));
        }

        public static string ExportPath
        {
            get
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "export");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }

        public static void PersistAndRunText(this string objName, string content, string ext = "sql")
        {
            if (string.IsNullOrEmpty(content)) return;

            string txtFile = Path.Combine(ExportPath, $"{objName.UnQuoteName()}-{DateTime.Now.ToString("yyyyMMMdd-hhmmss")}.{ext}");
            using (var stream = new StreamWriter(txtFile))
            {
                stream.Write(content);
            }

            Process.Start(txtFile);
        }

        public static string Capitalize(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            value = value.Trim().UnQuoteName();

            return value.Length > 1
                ? $"{value[0].ToString().ToUpper()}{value.Substring(1)}"
                : value.ToUpper();
        }

        public static string Lowerize(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            value = value.Trim().UnQuoteName();

            return value.Length > 1
                ? $"{value[0].ToString().ToLower()}{value.Substring(1)}"
                : value.ToLower();
        }

        public static string Camelize(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            value = value.Trim().UnQuoteName();

            var replaced = Regex.Replace(value, @"[\[\]" + "\"" + "]?(?<Char>[a-z]{1})", "${Char}");
            var split = replaced.Split('_','-');
            value = String.Join("", split.Select(s => s.Capitalize()));

            return value.Capitalize();
        }

        public static string CamelizeLower(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            value = value.Trim().UnQuoteName();

            var replaced = Regex.Replace(value, @"[\[\]" + "\"" + "]?(?<Char>[a-z]{1})", "${Char}");
            var split = replaced.Split('_', '-');
            value = String.Join("", split.Select(s => s.Lowerize()));

            return value.Lowerize();
        }

        public static bool ContainsAnyOfList(this string value, IEnumerable<string> list)
        {
            foreach (var item in list)
                if (value.Contains(item))
                    return true;

            return false;
        }
    }    
}
