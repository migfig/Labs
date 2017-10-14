using RelatedRows.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;

namespace RelatedRows.Providers.PostgreSql
{
    public class PostgreSqlQueryBuilder : QueryBuilderBase
    {
        public override string GetQuery(CTable table, long top, long skip = 0, CTable parent = null, DataRow row = null)
        {
            var query = $"WITH TR AS (SELECT COUNT(*) AS Total FROM {table.catalog}.{table.schemaName}.{table.name}) ";
            var subquery = $"SELECT * FROM {table.catalog}.{table.schemaName}.{table.name}";
            var offsetFetch = $" ORDER BY 1 LIMIT {top} OFFSET {skip}";

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

                subquery = subquery.Replace("WHERE AND ", "WHERE ");

                if (!table.requiresPagination) return subquery;
            }

            if (!string.IsNullOrEmpty(table.ColumnExpression))
                subquery += $" WHERE {table.ColumnExpression}";

            return query + subquery.Replace("* FROM", "*, (SELECT Total FROM TR) AS \"CountOver$\" FROM") + offsetFetch;
        }

        public override string GetQueryTooltip(CTable table, string column, DataRow row)
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

        public override string SqlInsert(CTable table, bool includeChildren = true, IList<string> tables = null)
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
                SqlInsert(value, table.DataTable);
                if (hasIdentity)
                    value.AppendFormat("SET IDENTITY_INSERT {0}.{1}.{2} OFF{3}", table.catalog, table.schemaName, table.name, Environment.NewLine);
                value.Append(Environment.NewLine);

                tables.Add(table.name);

                if (includeChildren)
                {
                    foreach (var child in table.Children)
                        value.Append(SqlInsert(child, includeChildren, tables));
                }
            }

            return value.ToString();
        }        
    }
}
