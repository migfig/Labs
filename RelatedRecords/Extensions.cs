using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Dapper;

namespace RelatedRecords
{
    public static class Extensions
    {
        public static string ToSelectString(this CTable table, bool asStar = false)
        {
            var query = new StringBuilder();
            query.Append("SELECT ");
            if(asStar)
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
                && null != andOrs && andOrs.Length >= operators.Length-1 )
            {
                query.Append("WHERE ");
                var idx = 0;
                pars.ToList().ForEach(p =>
                {
                    query.AppendFormat("{0}{1} {2} {3}", 
                        idx == 0 ? string.Empty : " " + andOrs[idx-1] + " ", 
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
            if (pars.Length > 0 && null != operators && operators.Length == pars.Length
                && null != andOrs && andOrs.Length >= operators.Length - 1)
            {
                query.Append(table.ToSelectString(asStar));

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

        public static DatatableEx Query(this CTable table,
            CDataset dataset,
            string connectionString,
            string[] operators, string[] andOrs,
            bool asStar = false, params IDbDataParameter[] pars)
        {
            var result = new DatatableEx(new DataTable(table.name));
            var query = table.ToSelectWhereString(operators, andOrs, asStar, pars);

            using (var connection = new SqlConnection(connectionString))
            {
                var reader = connection.ExecuteReader(query);
                result.Root.Load(reader);

                var queries = table.RelatedTablesSelect(dataset, result.Root.Rows[0]);
                queries.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries)
                    .ToList()
                    .ForEach(q =>
                    {
                        var rdr = connection.ExecuteReader(q);
                        var tbl = new DataTable("");
                        tbl.Load(rdr);
                        result.Children.Add(tbl);
                    });
            }

            return result;
        }

        public static string RelatedTablesSelect(this CTable table, 
            CDataset dataset, DataRow row)
        {
            var query = new StringBuilder();
            var rels = from r in dataset.Relationship
                       where r.fromTable.ToLower() == table.name.ToLower()
                       select new {
                           Relation = r,
                           Table = dataset.Table
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
                    row[c.name] = GetDefaultValue(c, i+1, table.name);
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
                    if(value is int)
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
                if(type == typeof(string))
                {
                    return string.Format("{0} {1}", tableName, sequenceNum);
                }

                return new object();
            }
        }

        private static Type GetType(eDbType type)
        {
            switch(type)
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
            if(value is string || value is Guid)
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

            independent.ToList().ForEach(t =>
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

        public static string[] ToArray(this string value, params string[] parts)
        {
            return parts;
        }
    }
}
