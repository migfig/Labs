#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using static Common.Extensions;
#endregion usings

namespace RelatedRecords.Data.ViewModels
{
    public partial class MainViewModel
    {
        public IEnumerable<string> ExpandCommands()
        {
            var cmdsArray = GetCommands().Split(Environment.NewLine.ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries);

            var list = new List<string>();
            foreach (var cmd in cmdsArray)
            {
                list.AddRange(Expando(cmd));
            }
            return list; //.OrderBy(x => x);
        }

        private IEnumerable<string> Expando(string command)
        {
            var method = _helpCommandMethods.First(m =>
                m.GetCustomAttributes(typeof(HelpCommandAttribute), false)
                .Cast<HelpCommandAttribute>()
                .First()
                .Commands.Contains(command));

            try
            {
                return (IEnumerable<string>)method.Invoke(this, new object[] { command });
            }
            catch (Exception e)
            {
                ErrorLog.Error(e, "When running method {method} with {command}", method, command);
                return new List<string>();
            }
        }

        [HelpCommand("help",
            "home",
            "back")]
        [HelpDescriptionCommand("Navigates back(to parent/previous table)",
            "Displays this help",
            "Navigate to root(default dataset table)")]
        public IEnumerable<string> ExpandItem(string command)
        {
            return command.Split(new char[] { '`' }, StringSplitOptions.RemoveEmptyEntries);
        }

        [HelpCommand("child [{Table:current} | {index}]")]
        [HelpDescriptionCommand("Drill down into children using name or index position")]
        public IEnumerable<string> ExpandChild(string command)
        {
            var list = new List<string>();
            if (null != CurrentTable && CurrentTable.Children.Any())
            {
                for (var i = 0; i < CurrentTable.Children.Count; i++)
                {
                    list.Add("child " + CurrentTable.Children[i].Root.ConfigTable.name);
                    list.Add("child " + i.ToString());
                }
            }

            return list;
        }

        [HelpCommand("clone [catalog {Dataset:current}] [as {Dataset:max}]")]
        [HelpDescriptionCommand("Clones CatalogName as Default new catalog name")]
        public IEnumerable<string> ExpandClone(string command)
        {
            var list = new List<string>();
            list.Add("clone");
            foreach (var ds in SelectedConfiguration.Dataset)
            {
                list.Add("clone catalog " + ds.name);
                list.Add(string.Format("clone catalog {0} as {0}{1}",
                    ds.name,
                    SelectedConfiguration.Dataset.Count + 1));
                list.Add(string.Format("clone as {0}{1}",
                    ds.name,
                    SelectedConfiguration.Dataset.Count + 1));
            }

            return list;
        }

        [HelpCommand("columns [max]")]
        [HelpDescriptionCommand("Displays current table top N columns")]
        public IEnumerable<string> ExpandColumns(string command)
        {
            var list = new List<string>();
            if (null != CurrentTable)
            {
                list.Add("columns");
                list.Add("columns " + CurrentTable.Root.ConfigTable.Column.Count.ToString());
            }

            return list;
        }

        [HelpCommand("export [{Table}] as (html | sql | json | xml)")]
        [HelpDescriptionCommand("Exports current or specified table as html, sql, json or xml")]
        public IEnumerable<string> ExpandExport(string command)
        {
            var list = new List<string>();
            if (null != CurrentTable)
            {
                list.Add("export as html");
                list.Add("export as sql");
                list.Add("export as json");
                list.Add("export as xml");
                list.Add("export " + CurrentTable.Root.ConfigTable.name + " as html");
                list.Add("export " + CurrentTable.Root.ConfigTable.name + " as sql");
                list.Add("export " + CurrentTable.Root.ConfigTable.name + " as json");
                list.Add("export " + CurrentTable.Root.ConfigTable.name + " as xml");
            }
            else
            {
                foreach (var t in SelectedDataset.Table)
                {
                    list.Add("export " + t.name + " as html");
                    list.Add("export " + t.name + " as sql");
                    list.Add("export " + t.name + " as json");
                    list.Add("export " + t.name + " as xml");
                }
            }

            return list;
        }

        [HelpCommand("import catalog {Dataset:max} [server ServerName] [user UserName password Password]")]
        [HelpDescriptionCommand("Imports an existing Database catalog providing server name, user and password values")]
        public IEnumerable<string> ExpandImport(string command)
        {
            var list = new List<string>();
            foreach (var ds in SelectedConfiguration.Dataset)
            {
                list.Add("import catalog " + ds.name);
                list.Add("import catalog " + ds.name + " server ");
                list.Add("import catalog " + ds.name + " user  password ");
                list.Add("import catalog " + ds.name + " server  user  password ");
            }

            return list;
        }

        [HelpCommand("load [catalog {Dataset}]")]
        [HelpDescriptionCommand("Loads current or specified catalog name and sets as default")]
        public IEnumerable<string> ExpandLoad(string command)
        {
            var list = new List<string>();
            list.Add("load");
            foreach (var ds in SelectedConfiguration.Dataset)
            {
                list.Add("load catalog " + ds.name);
            }

            return list;
        }

        [HelpCommand("refresh [catalog {Dataset}]")]
        [HelpDescriptionCommand("Refreshes current or specified catalog schema definition")]
        public IEnumerable<string> ExpandRefresh(string command)
        {
            var list = new List<string>();
            list.Add("refresh");
            foreach (var ds in SelectedConfiguration.Dataset)
            {
                list.Add("refresh catalog " + ds.name);
            }

            return list;
        }

        private IEnumerable<string> buildRelations(CTable table)
        {
            var list = new List<string>();
            foreach (var t in SelectedDataset.Table.Where(x => x.name != table.name))
            {
                var commonCols = from sc in table.Column
                                 let tc = t.Column.Where(x => x.name == sc.name).FirstOrDefault()
                                 where null != tc && sc.DbType == tc.DbType
                                 select new
                                 {
                                     SourceColumn = sc.name,
                                     TargetColumn = tc.name
                                 };

                var primaryForeignCols = from sc in table.Column
                                         let tc = t.Column.Where(x => x.isPrimaryKey && sc.isForeignKey)
                                            .FirstOrDefault()
                                         where null != tc && sc.DbType == tc.DbType
                                         select new
                                         {
                                             SourceColumn = sc.name,
                                             TargetColumn = tc.name
                                         };

                foreach (var c in commonCols)
                {
                    list.Add(string.Format("relate to {0} on {1} = {2}",
                        t.name,
                        c.SourceColumn,
                        c.TargetColumn));
                    list.Add(string.Format("relate {0} to {1} on {2} = {3}",
                        table.name,
                        t.name,
                        c.SourceColumn,
                        c.TargetColumn));
                }
                foreach (var c in primaryForeignCols)
                {
                    var rel = string.Format("relate to {0} on {1} = {2}",
                        t.name,
                        c.SourceColumn,
                        c.TargetColumn);

                    var relFull = string.Format("relate {0} to {1} on {2} = {3}",
                        table.name,
                        t.name,
                        c.SourceColumn,
                        c.TargetColumn);

                    if (!list.Contains(rel))
                        list.Add(rel);

                    if (!list.Contains(relFull))
                        list.Add(relFull);
                }
            }
            return list;
        }

        [HelpCommand("relate [{Table:source}] to {Table:target} on {Column:source} = {Column:target}")]
        [HelpDescriptionCommand("Relates current or specified table to another table using parent/child relationship on its columns")]
        public IEnumerable<string> ExpandRelate(string command)
        {
            var list = new List<string>();
            if (null != CurrentTable)
            {
                list.AddRange(buildRelations(CurrentTable.Root.ConfigTable));
            }
            else
            {
                foreach (var s in SelectedDataset.Table)
                {
                    list.AddRange(buildRelations(s));
                }
            }
            return list;
        }

        [HelpCommand("remove [catalog {Dataset}]")]
        [HelpDescriptionCommand("Removes current or specified catalog name")]
        public IEnumerable<string> ExpandRemove(string command)
        {
            var list = new List<string>();
            list.Add("remove");
            foreach (var ds in SelectedConfiguration.Dataset)
            {
                list.Add("remove catalog " + ds.name);
            }

            return list;
        }

        private IEnumerable<string> GetColumnOperators(CColumn column)
        {
            var list = new List<string>();

            string opString = string.Empty;
            switch (GetMappingType(column.DbType.ToString()).ToString())
            {
                case "System.DateTime":
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                case "System.Double":
                    opString = "=,<>,>,>=,<,<=,Is,Is Not";
                    break;
                case "System.Boolean":
                    opString = "=,<>,Is,Is Not";
                    break;
                default:
                    opString = "=,<>,Like,Is,Is Not";
                    break;
            }

            foreach (string op in opString.Split(new char[] { ',' }))
                if (!list.Contains(op))
                    list.Add(op);

            return list;
        }

        private Type GetMappingType(string colType)
        {
            string typeName = colType.ToLower().Split(new char[] { '(' })[0];
            var types = new Dictionary<Type, List<string>>{
                {typeof(System.Int32), new List<string> {"int", "smallint", "long", "tinyint"}}
                ,{typeof(System.Byte[]), new List<string> {"image", "blob", "binary", "sql_variant", "varbinary"}}
                ,{typeof(System.DateTime), new List<string> {"datetime", "datetime2", "date", "timestamp", "datetimeoffset", "smalldatetime", "time"}}
                ,{typeof(System.Double), new List<string> {"money", "real", "number", "bigint", "decimal", "float", "numeric"}}
                ,{typeof(System.Boolean), new List<string> {"bit"}}
            };

            var theType = from key in types.Keys
                          where types[key].Contains(colType)
                          select key;

            if (null != theType && null != theType.FirstOrDefault())
                return theType.FirstOrDefault();

            return typeof(System.String);
        }


        [HelpCommand("table {Table} [default] [where {Column} {Operator} (Value | MinValue and MaxValue)]")]
        [HelpDescriptionCommand("Queries a table and optionally sets as default dataset table.Operator can be any standard T-Sql operator and Value is any standard T-Sql value")]
        public IEnumerable<string> ExpandTable(string command)
        {
            var list = new List<string>();
            foreach (var t in SelectedDataset.Table)
            {
                list.Add(string.Format("table {0}", t.name));
                list.Add(string.Format("table {0} default", t.name));
                foreach (var c in t.Column)
                {
                    var operators = GetColumnOperators(c);
                    foreach (var o in operators)
                    {
                        list.Add(string.Format("table {0} where {1} {2} {3}",
                            t.name,
                            c.name,
                            o,
                            Extensions.GetDefaultValue(c, o)));
                    }

                    #region between

                    switch (c.DbType)
                    {
                        case eDbType.bigint:
                        case eDbType.date:
                        case eDbType.datetime:
                        case eDbType.datetime2:
                        case eDbType.datetimeoffset:
                        case eDbType.@decimal:
                        case eDbType.@float:
                        case eDbType.@int:
                        case eDbType.@long:
                        case eDbType.money:
                        case eDbType.numeric:
                        case eDbType.real:
                        case eDbType.smalldatetime:
                        case eDbType.smallint:
                        case eDbType.smallmoney:
                        case eDbType.time:
                        case eDbType.timestamp:
                        case eDbType.tinyint:
                            list.Add(string.Format("table {0} where {1} BETWEEN {2} and {3}",
                                t.name,
                                c.name,
                                Extensions.GetDefaultValue(c),
                                Extensions.GetDefaultValue(c)));
                            list.Add(string.Format("table {0} where {1} not BETWEEN {2} and {3}",
                                t.name,
                                c.name,
                                Extensions.GetDefaultValue(c),
                                Extensions.GetDefaultValue(c)));
                            break;
                    }

                    #endregion between
                }
            }

            return list;
        }

        [HelpCommand("tables [max]")]
        [HelpDescriptionCommand("Display top n dataset tables (its names and columns count)")]
        public IEnumerable<string> ExpandTables(string command)
        {
            var list = new List<string>();
            list.Add("tables");
            list.Add("tables " + SelectedDataset.Table.Count.ToString());

            return list;
        }

        [HelpCommand("top max")]
        [HelpDescriptionCommand("Displays top n records of current table")]
        public IEnumerable<string> ExpandTopN(string command)
        {
            var list = new List<string>();
            list.Add("top " + Extensions.MaxRowCount.ToString());

            return list;
        }

        [HelpCommand("unrelate [{Table:source}] to {Table:target}")]
        [HelpDescriptionCommand("Unrelates current or specified table to child table")]
        public IEnumerable<string> ExpandUnrelate(string command)
        {
            var list = new List<string>();
            if (null != CurrentTable && CurrentTable.Children.Any())
            {
                var rels = from r in SelectedDataset.Relationship
                           where r.fromTable == CurrentTable.Root.ConfigTable.name
                           select r;
                foreach (var r in rels)
                {
                    list.Add(string.Format("unrelate {0} to {1}",
                        r.fromTable,
                        r.toTable));
                }
            }
            else
            {
                foreach (var r in SelectedDataset.Relationship)
                {
                    list.Add(string.Format("unrelate {0} to {1}",
                        r.fromTable,
                        r.toTable));
                }

            }
            return list;
        }

        [HelpCommand("query {Column} [row {Index}]")]
        [HelpDescriptionCommand("Creates a query string for the provided column and optionally row number and copies content into clipboard")]
        public IEnumerable<string> ExpandQuery(string command)
        {
            var list = new List<string>();
            if(null != CurrentTable)
            {
                foreach(var c in CurrentTable.Root.ConfigTable.Column)
                {
                    list.Add("query " + c.name);
                    list.Add(string.Format("query {0} row {1}", 
                        c.name, 
                        CurrentTable.Root.Table.Rows.Count-1));
                }
            }
            return list;
        }

        [HelpCommand("run {Query} [with <paramName> = <value> [,...]]")]
        [HelpDescriptionCommand("Executes a query with or without parameters")]
        public IEnumerable<string> ExpandRunQuery(string command)
        {
            var list = new List<string>();
            if (SelectedDataset.Query.Any())
            {
                foreach (var q in SelectedDataset.Query)
                {
                    if (q.Parameter.Any())
                    {
                        list.Add(q.Parameter.Aggregate("run " + q.name, 
                            (seed, i) => seed + string.Format("{0} = {1}", i.name, i.defaultValue)));
                    }
                    else
                    {
                        list.Add("run " + q.name);
                    }
                }
            }
            return list;
        }

        [HelpCommand("transform [[{SqlObject}] [template {Template}]")]
        [HelpDescriptionCommand("Transforms a Sql Object into a code template using default or given template name")]
        public IEnumerable<string> ExpandTransform(string command)
        {
            var list = new List<string>();
            list.Add("transform");
            list.Add("transform template [TemplateName]");
            foreach (var o in SelectedDataset.Table)
            {
                list.Add("transform " + o.name);
                list.Add(string.Format("transform {0} template {1}",
                    o.name,
                    "[TemplateName]"));
            }
            return list;
        }

        private string GetCommands()
        {
            var atts = from m in _helpCommandMethods
                       let att = m.GetCustomAttributes(typeof(HelpCommandAttribute), false)
                            .Cast<HelpCommandAttribute>()
                            .FirstOrDefault()
                       where null != att
                       select att.Commands;

            var cmds = string.Empty;
            foreach (var c in atts)
            {
                cmds += string.Join(Environment.NewLine, c) + Environment.NewLine;
            }

            return cmds;
        }

        private string GetDescriptions()
        {
            var atts = from m in _helpDescCommandMethods
                       let att = m.GetCustomAttributes(typeof(HelpDescriptionCommandAttribute), false)
                            .Cast<HelpDescriptionCommandAttribute>()
                            .FirstOrDefault()
                       where null != att
                       select att.Descriptions;

            var descs = string.Empty;
            foreach (var c in atts)
            {
                descs += string.Join(Environment.NewLine, c) + Environment.NewLine;
            }

            return descs;
        }
    }
}
