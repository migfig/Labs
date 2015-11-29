#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using RelatedRecords.Parser;
using com.calitha.goldparser;
using static Common.Extensions;
#endregion usings

namespace RelatedRecords.Data.ViewModels
{
    #region attributes

    public class CommandAttribute : Attribute
    {
        private SymbolConstants[] _symbols;
        public SymbolConstants[] Symbols
        {
            get { return _symbols; }
        }
        public CommandAttribute(params SymbolConstants[] symbols)
        {
            _symbols = symbols;
        }

        public override string ToString()
        {
            var value = string.Empty;
            _symbols.ToList().ForEach(s => value += "_" + ((int)s).ToString());

            return value;
        }
    }

    public class HelpCommandAttribute: Attribute
    {
        private string[] _commands;
        public string[] Commands
        {
            get { return _commands; }
        }
        public HelpCommandAttribute(params string[] commands)
        {
            _commands = commands;
        }
    }

    public class HelpDescriptionCommandAttribute : Attribute
    {
        private string[] _descriptions;
        public string[] Descriptions
        {
            get { return _descriptions; }
        }
        public HelpDescriptionCommandAttribute(params string[] descriptions)
        {
            _descriptions = descriptions;
        }
    }

    #endregion attributes

    public partial class MainViewModel
    {
        #region command handlers

        public void HandleCommand(ParseResults results)
        {
            var method = _commandMethods.FirstOrDefault(m =>
                m.GetCustomAttributes(typeof(CommandAttribute), false)
                    .Cast<CommandAttribute>()
                    .First().ToString() == results.ToString());

            if (null != method)
            {
                try
                {
                    method.Invoke(this, new object[] { results.Tokens });
                }
                catch (Exception e)
                {
                    ErrorLog.Error(e, "When running method {method} with {Tokens}", method, results.Tokens);
                }
            }
        }

        [Command(SymbolConstants.SYMBOL_BACK)]
        public void Back(IEnumerable<TerminalToken> tokens)
        {
            DoBack();
        }

        [Command(SymbolConstants.SYMBOL_CLONE
        , SymbolConstants.SYMBOL_AS
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void CloneAsId(IEnumerable<TerminalToken> tokens)
        {
            var catalog = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoCloneAsId(catalog);
        }

        [Command(SymbolConstants.SYMBOL_CLONE
        , SymbolConstants.SYMBOL_CATALOG
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_AS
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void CloneCatalogIdAsId(IEnumerable<TerminalToken> tokens)
        {
            var srcCatalog = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var tgtCatalog = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            DoCloneCatalogIdAsId(srcCatalog, tgtCatalog);
        }

        [Command(SymbolConstants.SYMBOL_CLONE
        , SymbolConstants.SYMBOL_CATALOG
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void CloneCatalogId(IEnumerable<TerminalToken> tokens)
        {
            var catalog = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoCloneCatalogId(catalog);
        }

        [Command(SymbolConstants.SYMBOL_CLONE)]
        public void Clone(IEnumerable<TerminalToken> tokens)
        {
            DoClone();
        }

        [Command(SymbolConstants.SYMBOL_COLUMNS
        , SymbolConstants.SYMBOL_INTEGER)]
        public void ColumnsInt(IEnumerable<TerminalToken> tokens)
        {
            var topN = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoColumnsInt(topN);
        }

        [Command(SymbolConstants.SYMBOL_COLUMNS)]
        public void Columns(IEnumerable<TerminalToken> tokens)
        {
            DoColumns();
        }

        [Command(SymbolConstants.SYMBOL_EXPORT
        , SymbolConstants.SYMBOL_AS
        , SymbolConstants.SYMBOL_HTML)]
        public void ExportAsHtml(IEnumerable<TerminalToken> tokens)
        {
            DoExportAsHtml();
        }

        [Command(SymbolConstants.SYMBOL_EXPORT
        , SymbolConstants.SYMBOL_AS
        , SymbolConstants.SYMBOL_JSON)]
        public void ExportAsJson(IEnumerable<TerminalToken> tokens)
        {
            DoExportAsJson();
        }

        [Command(SymbolConstants.SYMBOL_EXPORT
        , SymbolConstants.SYMBOL_AS
        , SymbolConstants.SYMBOL_SQL)]
        public void ExportAsSql(IEnumerable<TerminalToken> tokens)
        {
            DoExportAsSql();
        }

        [Command(SymbolConstants.SYMBOL_EXPORT
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_AS
        , SymbolConstants.SYMBOL_HTML)]
        public void ExportIdAsHtml(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoExportIdAsHtml(table);
        }

        [Command(SymbolConstants.SYMBOL_EXPORT
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_AS
        , SymbolConstants.SYMBOL_JSON)]
        public void ExportIdAsJson(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoExportIdAsJson(table);
        }

        [Command(SymbolConstants.SYMBOL_EXPORT
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_AS
        , SymbolConstants.SYMBOL_SQL)]
        public void ExportIdAsSql(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoExportIdAsSql(table);
        }

        [Command(SymbolConstants.SYMBOL_EXPORT
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_AS
        , SymbolConstants.SYMBOL_XML)]
        public void ExportIdAsXml(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoExportIdAsXml(table);
        }

        [Command(SymbolConstants.SYMBOL_EXPORT
        , SymbolConstants.SYMBOL_AS
        , SymbolConstants.SYMBOL_XML)]
        public void ExportAsXml(IEnumerable<TerminalToken> tokens)
        {
            DoExportAsXml();
        }

        [Command(SymbolConstants.SYMBOL_IMPORT
        , SymbolConstants.SYMBOL_CATALOG
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_SERVER
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_USER
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_PASSWORD
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void ImportCatalogIdSvrIdUserIdPwdId(IEnumerable<TerminalToken> tokens)
        {
            var catalog = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var server = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var user = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var password = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 3).Text;
            DoImportCatalogIdSvrIdUserIdPwdId(catalog, server, user, password);
        }

        [Command(SymbolConstants.SYMBOL_IMPORT
        , SymbolConstants.SYMBOL_CATALOG
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_USER
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_PASSWORD
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void ImportCatalogIdUserIdPwdId(IEnumerable<TerminalToken> tokens)
        {
            var catalog = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var user = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var password = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            DoImportCatalogIdUserIdPwdId(catalog, user, password);
        }

        [Command(SymbolConstants.SYMBOL_IMPORT
        , SymbolConstants.SYMBOL_CATALOG
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void ImportCatalogId(IEnumerable<TerminalToken> tokens)
        {
            var catalog = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoImportCatalogId(catalog);
        }

        [Command(SymbolConstants.SYMBOL_LOAD
        , SymbolConstants.SYMBOL_CATALOG
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void LoadCatalogId(IEnumerable<TerminalToken> tokens)
        {
            var catalog = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoLoadCatalogId(catalog);
        }

        [Command(SymbolConstants.SYMBOL_LOAD)]
        public void Load(IEnumerable<TerminalToken> tokens)
        {
            DoLoad();
        }

        [Command(SymbolConstants.SYMBOL_RELATE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_TO
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_ON
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void RelateIdToIdOnIdEqId(IEnumerable<TerminalToken> tokens)
        {
            var srcTbl = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var tgtTbl = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var srcCol = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var tgtCol = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 3).Text;
            DoRelateIdToIdOnIdEqId(srcTbl, tgtTbl, srcCol, tgtCol);
        }

        [Command(SymbolConstants.SYMBOL_RELATE
        , SymbolConstants.SYMBOL_TO
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_ON
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void RelateToIdOnIdEqId(IEnumerable<TerminalToken> tokens)
        {
            var tgtTbl = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var srcCol = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var tgtCol = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            DoRelateToIdOnIdEqId(tgtTbl, srcCol, tgtCol);
        }

        [Command(SymbolConstants.SYMBOL_REMOVE
        , SymbolConstants.SYMBOL_CATALOG
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void RemoveCatalogId(IEnumerable<TerminalToken> tokens)
        {
            var catalog = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoRemoveCatalogId(catalog);
        }

        [Command(SymbolConstants.SYMBOL_REMOVE)]
        public void Remove(IEnumerable<TerminalToken> tokens)
        {
            DoRemove();
        }

        [Command(SymbolConstants.SYMBOL_REFRESH)]
        public void Refresh(IEnumerable<TerminalToken> tokens)
        {
            DoRefresh();
        }

        [Command(SymbolConstants.SYMBOL_REFRESH
        , SymbolConstants.SYMBOL_CATALOG
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void RefreshCatalogId(IEnumerable<TerminalToken> tokens)
        {
            var catalog = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoRefreshCatalogId(catalog);
        }

        [Command(SymbolConstants.SYMBOL_ROOT)]
        public void Root(IEnumerable<TerminalToken> tokens)
        {
            DoRoot();
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_DEFAULT
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_STRINGLITERAL)]
        public void TableIdDefaultWhereIdEqStrLit(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 0).Text;
            DoTableIdDefaultWhereIdOperatorStrLit(table, column, value);
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_STRINGLITERAL)]
        public void TableIdWhereIdEqStrLit(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 0).Text;
            DoTableIdWhereIdOperatorValue(table, column, value, typeof(string));
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_LIKE
        , SymbolConstants.SYMBOL_STRINGLITERAL)]
        public void TableIdWhereIdLikeStrLit(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 0).Text;
            DoTableIdWhereIdOperatorValue(table, column, value, typeof(string), "like");
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_DEFAULT
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_LIKE
        , SymbolConstants.SYMBOL_STRINGLITERAL)]
        public void TableIdDefaultWhereIdLikeStrLit(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 0).Text;
            DoTableIdDefaultWhereIdOperatorStrLit(table, column, value, "like");
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_DEFAULT)]
        public void TableIdDefault(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoTableIdDefault(table);
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_BETWEEN
        , SymbolConstants.SYMBOL_INTEGER
        , SymbolConstants.SYMBOL_AND
        , SymbolConstants.SYMBOL_INTEGER)]
        public void TableIdWhereIdBetweenIntAndInt(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var minValue = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            var maxValue = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 1).Text;
            DoTableIdWhereIdBetweenValueAndValue(table, column, minValue, maxValue);
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_BETWEEN
        , SymbolConstants.SYMBOL_DECIMAL
        , SymbolConstants.SYMBOL_AND
        , SymbolConstants.SYMBOL_DECIMAL)]
        public void TableIdWhereIdBetweenDecAndDec(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var minValue = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            var maxValue = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 1).Text;
            DoTableIdWhereIdBetweenValueAndValue(table, column, minValue, maxValue, typeof(double));
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_GTEQ
        , SymbolConstants.SYMBOL_MINUS
        , SymbolConstants.SYMBOL_INTEGER)]
        public void TableIdWhereIdGtEqMinusInt(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoTableIdWhereIdOperatorValue(table, column, "-" + value);
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_GTEQ
        , SymbolConstants.SYMBOL_MINUS
        , SymbolConstants.SYMBOL_DECIMAL)]
        public void TableIdWhereIdGtEqMinusDec(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            DoTableIdWhereIdOperatorValue(table, column, "-" + value, typeof(double), ">=");
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_GTEQ
        , SymbolConstants.SYMBOL_INTEGER)]
        public void TableIdWhereIdGtEqInt(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoTableIdWhereIdOperatorValue(table, column, value, typeof(long), ">=");
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_GTEQ
        , SymbolConstants.SYMBOL_DECIMAL)]
        public void TableIdWhereIdGtEqDec(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            DoTableIdWhereIdOperatorValue(table, column, value, typeof(double), ">=");
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_GT
        , SymbolConstants.SYMBOL_MINUS
        , SymbolConstants.SYMBOL_INTEGER)]
        public void TableIdWhereIdGtMinusInt(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoTableIdWhereIdOperatorValue(table, column, "-" + value, typeof(long), ">");
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_GT
        , SymbolConstants.SYMBOL_MINUS
        , SymbolConstants.SYMBOL_DECIMAL)]
        public void TableIdWhereIdGtMinusDec(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            DoTableIdWhereIdOperatorValue(table, column, "-" + value, typeof(double), ">");
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_GT
        , SymbolConstants.SYMBOL_INTEGER)]
        public void TableIdWhereIdGtInt(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoTableIdWhereIdOperatorValue(table, column, value, typeof(long), ">");
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_GT
        , SymbolConstants.SYMBOL_DECIMAL)]
        public void TableIdWhereIdGtDec(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            DoTableIdWhereIdOperatorValue(table, column, value, typeof(double), ">");
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_LTEQ
        , SymbolConstants.SYMBOL_MINUS
        , SymbolConstants.SYMBOL_INTEGER)]
        public void TableIdWhereIdLtEqMinusInt(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoTableIdWhereIdOperatorValue(table, column, "-" + value, typeof(long), "<=");
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_LTEQ
        , SymbolConstants.SYMBOL_MINUS
        , SymbolConstants.SYMBOL_DECIMAL)]
        public void TableIdWhereIdLtEqMinusDec(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            DoTableIdWhereIdOperatorValue(table, column, "-" + value, typeof(double), "<=");
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_LTEQ
        , SymbolConstants.SYMBOL_INTEGER)]
        public void TableIdWhereIdLtEqInt(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoTableIdWhereIdOperatorValue(table, column, value, typeof(long), "<=");
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_LTEQ
        , SymbolConstants.SYMBOL_DECIMAL)]
        public void TableIdWhereIdLtEqDec(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            DoTableIdWhereIdOperatorValue(table, column, value, typeof(double), "<=");
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_LTGT
        , SymbolConstants.SYMBOL_INTEGER)]
        public void TableIdWhereIdLtGtInt(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoTableIdWhereIdOperatorValue(table, column, value, typeof(long), "<>");
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_LTGT
        , SymbolConstants.SYMBOL_DECIMAL)]
        public void TableIdWhereIdLtGtDec(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            DoTableIdWhereIdOperatorValue(table, column, value, typeof(double), "<>");
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_LTGT
        , SymbolConstants.SYMBOL_MINUS
        , SymbolConstants.SYMBOL_INTEGER)]
        public void TableIdWhereIdLtGtMinusInt(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoTableIdWhereIdOperatorValue(table, column, "-" + value, typeof(long), "<>");
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_LTGT
        , SymbolConstants.SYMBOL_MINUS
        , SymbolConstants.SYMBOL_DECIMAL)]
        public void TableIdWhereIdLtGtMinusDec(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            DoTableIdWhereIdOperatorValue(table, column, "-" + value, typeof(double), "<>");
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_MINUS
        , SymbolConstants.SYMBOL_INTEGER)]
        public void TableIdWhereIdEqMinusInt(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoTableIdWhereIdOperatorValue(table, column, "-" + value, typeof(long));
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_MINUS
        , SymbolConstants.SYMBOL_DECIMAL)]
        public void TableIdWhereIdEqMinusDec(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            DoTableIdWhereIdOperatorValue(table, column, "-" + value, typeof(double));
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER)]
        public void TableIdWhereIdEqInt(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoTableIdWhereIdOperatorValue(table, column, value, typeof(long));
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_DECIMAL)]
        public void TableIdWhereIdEqDec(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            DoTableIdWhereIdOperatorValue(table, column, value, typeof(double));
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_IS
        , SymbolConstants.SYMBOL_NOT
        , SymbolConstants.SYMBOL_NULL)]
        public void TableIdWhereIdIsNotNull(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_NULL, 0).Text;
            DoTableIdWhereIdOperatorValue(table, column, value, typeof(string), "is not");
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_IS
        , SymbolConstants.SYMBOL_NULL)]
        public void TableIdWhereIdIsNull(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var column = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_NULL, 0).Text;
            DoTableIdWhereIdOperatorValue(table, column, value, typeof(string), "is");
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void TableId(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoTableId(table);
        }

        [Command(SymbolConstants.SYMBOL_TABLES
        , SymbolConstants.SYMBOL_INTEGER)]
        public void TablesInt(IEnumerable<TerminalToken> tokens)
        {
            var topN = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoTablesInt(topN);
        }

        [Command(SymbolConstants.SYMBOL_TABLES)]
        public void Tables(IEnumerable<TerminalToken> tokens)
        {
            DoTables();
        }

        [Command(SymbolConstants.SYMBOL_TOP
        , SymbolConstants.SYMBOL_INTEGER)]
        public void TopInt(IEnumerable<TerminalToken> tokens)
        {
            var topN = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoTopInt(topN);
        }

        [Command(SymbolConstants.SYMBOL_UNRELATE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_TO
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void UnrelateIdToId(IEnumerable<TerminalToken> tokens)
        {
            var srcTbl = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var tgtTbl = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            DoUnrelateIdToId(srcTbl, tgtTbl);
        }

        [Command(SymbolConstants.SYMBOL_UNRELATE
        , SymbolConstants.SYMBOL_TO
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void UnrelateToId(IEnumerable<TerminalToken> tokens)
        {
            var tgtTbl = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoUnrelateToId(tgtTbl);
        }

        [Command(SymbolConstants.SYMBOL_CHILD)]
        public void Child(IEnumerable<TerminalToken> tokens)
        {
            DoChild();
        }

        [Command(SymbolConstants.SYMBOL_CHILD
        , SymbolConstants.SYMBOL_INTEGER)]
        public void ChildInt(IEnumerable<TerminalToken> tokens)
        {
            var index = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoChildInt(index);
        }

        [Command(SymbolConstants.SYMBOL_CHILD
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void ChildId(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoChildId(table);
        }

        [Command(SymbolConstants.SYMBOL_HELP)]
        public void Help(IEnumerable<TerminalToken> tokens)
        {
            DoHelp();
        }

        #endregion command handlers

        #region utility methods 

        private DatatableEx findDataTable(string name)
        {
            if (null == CurrentTable) return null;

            return CurrentTable
                .Children
                .FirstOrDefault(x => x.Root.ConfigTable.name.ToLower() == name.ToLower());
        }

        private DatatableEx findDataTableByIndex(int index)
        {
            if (null == CurrentTable 
                || index < 0 
                || index > CurrentTable.Children.Count-1) return null;

            return CurrentTable
                .Children
                .ElementAt(index);
        }

        private CTable findTable(string name)
        {
            return SelectedDataset
                .Table
                .FirstOrDefault(x => x.name.ToLower() == name.ToLower());
        }

        private CDataset findDataset(string name)
        {
            return SelectedConfiguration
                .Dataset
                .FirstOrDefault(x => x.name.ToLower() == name.ToLower());
        }

        private object ParseValue(string value, Type type)
        {
            var valType = type ?? typeof(long);
            switch (valType.ToString())
            {
                case "System.Int64":
                    return Int64.Parse(value);
                case "System.Double":
                    return Double.Parse(value);
                default:
                    return value;
            }
        }

        private bool TableIsCurrent(string tableName)
        {
            return (null != CurrentTable
                && CurrentTable.Root.ConfigTable.name.ToLower() == tableName.ToLower());
        }

        private void ThrowError(string format, params string[] args)
        {
            //throw new Exception(string.Format(format, args));
            ErrorLog.Error(format, args);
        }

        #endregion utility methods

        #region utility classes

        internal class State
        {
            private string _datasetName;
            private string _tableName;
            private List<DatatableEx> _navigation;

            private readonly MainViewModel _model;
            public State(MainViewModel model)
            {
                _model = model;
            }

            public bool SaveState()
            {
                if (null != _model.SelectedDataset)
                {
                    _datasetName = _model.SelectedDataset.name;
                    _tableName = null != _model.CurrentTable
                        ? _model.CurrentTable.Root.ConfigTable.name
                        : _model.SelectedDataset.defaultTable;
                    _navigation = _model.TableNavigation.ToList();
                    return true;
                }

                return false;
            }

            public async void RestoreState()
            {
                var ds = _model
                    .SelectedConfiguration
                    .Dataset
                    .FirstOrDefault(x => x.name == _datasetName);
                if (null == ds)
                {
                    ds = _model.SelectedConfiguration.Dataset.First();
                }
                _model.SelectedDataset = ds;

                foreach (var t in _navigation)
                {
                    _model.TableNavigation.Push(
                        await t.Root.ConfigTable.Query("".ToArray(), "".ToArray(), true));
                }

                _model.CurrentTable = _navigation.FirstOrDefault(x => x.Root.ConfigTable.name == _tableName);
            }
        }

        #endregion utility classes
    }
}
