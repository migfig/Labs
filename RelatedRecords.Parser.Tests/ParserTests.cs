﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using static Common.Extensions;
using System.Collections.Generic;
using com.calitha.goldparser;

namespace RelatedRecords.Parser.Tests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void RelatedRecords_Parser_Tests()
        {
            #region text commands

            var commands = @"
back
clone as SomeOtherName
clone catalog MyCatalog as SomeOther12_catName
clone catalog SampleCat
clone
columns 2
columns
export as html
export as json
export as sql
export Html_Table as html
export _Tablename as json
export _Table_Name_12 as sql
export ThisTable as xml
export as xml
import catalog SampleZ server localhostz user devz password pwdz
import catalog SampleY user devy password pwdy
import catalog SampleX
load catalog CatalogName
load
relate ThisTable to OtherTable12 on Column1 = column_2
relate to OtherTable12 on Column1 = column_2
remove catalog CatalogName
remove
refresh
refresh catalog My_Catalog
root
table Test21 default where col1 = '1.34'
table Test21 default
table Test21 where col1 between 1 and 10
table Test21 where col1 between 1.34 and 245.234
table Test21 where col1 >= -1
table Test21 where col1 >= -121.340
table Test21 where col1 >= 1
table Test21 where col1 >= 121.340
table Test21 where col1 > -1
table Test21 where col1 > -121.340
table Test21 where col1 > 1
table Test21 where col1 > 121.340
table Test21 where col1 <= -1
table Test21 where col1 <= -121.340
table Test21 where col1 <= 1
table Test21 where col1 <= 121.340
table Test21 where col1 <> 1
table Test21 where col1 <> 121.340
table Test21 where col1 <> -1
table Test21 where col1 <> -121.340
table Test21 where col1 = -1
table Test21 where col1 = -121.340
table Test21 where col1 = 1
table Test21 where col1 = 121.340
table Test21 where col_some12 is not null
table Test21 where col12xo is null
table Test21
tables 10
tables
top 100
unrelate This_Table12 to OtherTable12
unrelate to OtherTable12
child
child 2
child MyTable
";
            #endregion text commands

            var parser = new RRParser(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "relatedrecords.cgt"));

            using (var stream = File.CreateText(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "methods.cs")))
            {
                var methods = new StringBuilder();
                var hlpMethods = new StringBuilder();

                foreach (var cmd in commands.Replace("'", "\"").Split(Environment.NewLine.ToCharArray(),
                    StringSplitOptions.RemoveEmptyEntries))
                {
                    var results = parser.Parse(cmd);
                    Assert.IsTrue(results.isAccepted);

                    DumpCommandMethods(results.Tokens, methods);
                    DumpHelperMethods(results.Tokens, hlpMethods);
                }

                stream.Write(methods.ToString());
                stream.Write(hlpMethods.ToString());
            }
        }

        private void DumpCommandMethods(IEnumerable<TerminalToken> tokens, StringBuilder methods)
        {
            methods.Append("[Command(");
            foreach (var t in tokens)
            {
                methods.AppendFormat("{0}SymbolConstants.{1}{2}",
                    t == tokens.First() ? string.Empty : ",",
                    t.Symbol.SymbolEnum(),
                    t == tokens.Last() ? string.Empty : Environment.NewLine);
            }
            methods.AppendFormat(")]{0}", Environment.NewLine);

            methods.Append("public void ");
            foreach (var t in tokens)
            {
                methods.Append(FixId(CapitalizeWords(t.Symbol.Name)));
            }
            methods.AppendFormat("(IEnumerable<TerminalToken> tokens){0}",
                Environment.NewLine);

            methods.AppendFormat("{{{0}", Environment.NewLine);

            var filtered = getFilteredTokens(tokens);

            var idList = new Dictionary<int, int>();
            foreach (var t in filtered)
            {
                methods.AppendFormat("\tvar {0}{2} = tokens.TerminalToken(SymbolConstants.{1}, {2}).Text;{3}",
                    FixId(CapitalizeWords(t.Symbol.Name)),
                    t.Symbol.SymbolEnum(),
                    idList.ContainsKey(t.Symbol.Id) ? idList[t.Symbol.Id] : 0,
                    Environment.NewLine);

                if (!idList.ContainsKey(t.Symbol.Id))
                    idList.Add(t.Symbol.Id, 1);
                else
                    idList[t.Symbol.Id]++;
            }

            methods.Append("\tDo");
            foreach (var t in tokens)
            {
                methods.Append(FixId(CapitalizeWords(t.Symbol.Name)));
            }
            methods.Append("(");

            idList.Clear();
            foreach (var t in filtered)
            {
                methods.AppendFormat("{0}{1}{2}",
                    t == filtered.First() ? string.Empty : ",",
                    FixId(CapitalizeWords(t.Symbol.Name)),
                    idList.ContainsKey(t.Symbol.Id) ? idList[t.Symbol.Id] : 0);

                if (!idList.ContainsKey(t.Symbol.Id))
                    idList.Add(t.Symbol.Id, 1);
                else
                    idList[t.Symbol.Id]++;
            }
            methods.AppendFormat(");{0}", Environment.NewLine);

            methods.AppendFormat("}}{0}{0}", Environment.NewLine);
        }

        private void DumpHelperMethods(IEnumerable<TerminalToken> tokens, StringBuilder methods)
        {
            methods.Append("private void Do");
            foreach (var t in tokens)
            {
                methods.Append(FixId(CapitalizeWords(t.Symbol.Name)));
            }
            methods.Append("(");

            var idList = new Dictionary<int, int>();
            var filtered = getFilteredTokens(tokens);
            foreach (var t in filtered)
            {
                methods.AppendFormat("{0}{1}{2}",
                    t == filtered.First() ? "string " : ", string ",
                    FixId(CapitalizeWords(t.Symbol.Name)),
                    idList.ContainsKey(t.Symbol.Id) ? idList[t.Symbol.Id] : 0);

                if (!idList.ContainsKey(t.Symbol.Id))
                    idList.Add(t.Symbol.Id, 1);
                else
                    idList[t.Symbol.Id]++;
            }
            methods.AppendFormat("){0}", Environment.NewLine);
            methods.AppendFormat("{{{0}", Environment.NewLine);
            methods.AppendFormat("}}{0}{0}", Environment.NewLine);
        }

        private IEnumerable<TerminalToken> getFilteredTokens(IEnumerable<TerminalToken> tokens)
        {
            return tokens.Where(x =>
                x.Symbol.SymbolEnum() == SymbolConstants.SYMBOL_DECIMAL
                || x.Symbol.SymbolEnum() == SymbolConstants.SYMBOL_IDENTIFIER
                || x.Symbol.SymbolEnum() == SymbolConstants.SYMBOL_INTEGER
                || x.Symbol.SymbolEnum() == SymbolConstants.SYMBOL_NULL
                || x.Symbol.SymbolEnum() == SymbolConstants.SYMBOL_PASSWORD
                || x.Symbol.SymbolEnum() == SymbolConstants.SYMBOL_SERVER
                || x.Symbol.SymbolEnum() == SymbolConstants.SYMBOL_STRINGLITERAL
                || x.Symbol.SymbolEnum() == SymbolConstants.SYMBOL_USER
                || x.Symbol.SymbolEnum() == SymbolConstants.SYMBOL_VALUE
                );
        }

        private string FixId(string value)
        {
            return value.Replace("=", "Eq")
                    .Replace(">=", "GtEq")
                    .Replace(">", "Gt")
                    .Replace("<=", "LtEq")
                    .Replace("<", "Lt")
                    .Replace("<>", "NotEq")
                    .Replace("-", "Minus")
                    .Replace("String Literal", "StrLit")
                    .Replace("Identifier", "Id")
                    .Replace("Integer", "Int")
                    .Replace("Decimal", "Dec")
                    .Replace("Password", "Pwd")
                    .Replace("Server", "Svr")
                    ;
        }
    }
}
