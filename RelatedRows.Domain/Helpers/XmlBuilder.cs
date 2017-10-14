using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace RelatedRows.Domain
{
    public static class XmlBuilder
    {
        public static XElement BuildElements(CDatasource dataSource, string schema)
        {
            var doc = XDocument.Load(new StringReader(schema));
            //doc.Save("schema.xml");
            var dataSourceName = dataSource.serverName
                .Split('.').First().Split('\\').First() + "-" + dataSource.databaseName;

            return new XElement("Configuration",
                        new XAttribute("defaultDatasource", dataSourceName),
                        new XAttribute("defaultDataset", dataSourceName),
                    new XElement("Datasource",
                        new XAttribute("name", dataSourceName),
                        new XAttribute("provider", dataSource.providerName),
                        new XElement("ConnectionString", dataSource.ConnectionString)),
                    new XElement("Dataset",
                        new XAttribute("name", dataSourceName),
                        new XAttribute("dataSourceName", dataSourceName),
                        new XAttribute("defaultTable", doc.Root.Elements("Table").First().Attribute("name").Value),

                        from t in doc.Root.Elements("Table")
                        select
                            new XElement("Table", 
                                    new XAttribute("catalog", t.Attribute("catalog").Value),
                                    new XAttribute("schemaName", t.Attribute("schemaName").Value),
                                    new XAttribute("name", t.Attribute("name").Value),
                                    new XAttribute("rows", t.Attribute("rows").Value),
                                from c in t.Elements("Column")
                                select
                                    new XElement("Column",
                                        from a in c.Attributes()
                                        select
                                            new XAttribute(a.Name.LocalName, a.Value)
                                        )
                                ),

                        from t in doc.Root.Elements("Table")
                        from c in t.Elements("Column")
                        from r in c.Elements("Relationship")
                        let isForeignKey = c.Attribute("isForeignKey").Value == "1"
                        let fromTable = t.Attribute("name").Value
                        let fromColumn = c.Attribute("name").Value
                        let toTable = r.Attribute("toTable").Value
                        where isForeignKey
                        select
                            new XElement("Relationship",
                                new XAttribute("name", string.Format("{0}->{1}", fromTable, toTable)),
                                new XAttribute("fromTable", fromTable),
                                new XAttribute("toTable", toTable),
                                new XElement("ColumnRelationship",
                                    new XAttribute("fromColumn", fromColumn),
                                    new XAttribute("toColumn", r.Attribute("toColumn").Value)))
                        )
                    );
        }

        public static XElement BuildQueryElements(CDatasource dataSource, string schema)
        {
            var doc = XDocument.Load(new StringReader(schema));
            var dataSourceName = dataSource.serverName
                .Split('.').First().Split('\\').First() + "-" + dataSource.databaseName;

            return new XElement("Configuration",
                        new XAttribute("defaultDatasource", dataSourceName),
                        new XAttribute("defaultDataset", dataSourceName),
                    new XElement("Datasource",
                        new XAttribute("name", dataSourceName),
                        new XAttribute("provider", dataSource.providerName),
                        new XElement("ConnectionString", dataSource.ConnectionString)),
                    new XElement("Dataset",
                        new XAttribute("name", dataSourceName),
                        new XAttribute("dataSourceName", dataSourceName),
                        new XAttribute("defaultTable", doc.Root.Elements("Query").First().Attribute("name").Value),

                from q in doc.Root.Elements("Query")
                select
                    new XElement("Query",
                            new XAttribute("catalog", q.Attribute("catalog").Value),
                            new XAttribute("schemaName", q.Attribute("schemaName").Value),
                            new XAttribute("name", q.Attribute("name").Value),
                            new XAttribute("type", q.Attribute("type").Value),
                            new XAttribute("isStoreProcedure", q.Attribute("type").Value.Contains("P")),

                        from p in q.Elements("Parameter")
                        select
                            new XElement("Parameter",
                                from a in p.Attributes()
                                select
                                    new XAttribute(a.Name.LocalName, a.Value)
                                ),

                        from t in q.Elements("Text")
                        select
                            new XCData(t.Value)
                        )));
        }
    }
}
