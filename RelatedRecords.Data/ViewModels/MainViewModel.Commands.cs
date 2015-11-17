﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RelatedRecords.Parser;
using com.calitha.goldparser;

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

    #endregion attributes

    public partial class MainViewModel
    {
        public void HandleCommand(ParseResults results)
        {
            var method = _commandMethods.First(m =>
                m.GetCustomAttributes(typeof(CommandAttribute), false)
                    .Cast<CommandAttribute>()
                    .First().ToString() == results.ToString());

            method.Invoke(this, new object[] { results.Tokens });
        }

        #region command handlers

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
            DoTableIdDefaultWhereIdEqStrLit(table, column, value);
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
            DoTableIdWhereIdBetweenIntAndInt(table, column, minValue, maxValue);
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
            DoTableIdWhereIdBetweenDecAndDec(table, column, minValue, maxValue);
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
            DoTableIdWhereIdGtEqMinusInt(table, column, value);
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
            DoTableIdWhereIdGtEqMinusDec(table, column, value);
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
            DoTableIdWhereIdGtEqInt(table, column, value);
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
            DoTableIdWhereIdGtEqDec(table, column, value);
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
            DoTableIdWhereIdGtMinusInt(table, column, value);
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
            DoTableIdWhereIdGtMinusDec(table, column, value);
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
            DoTableIdWhereIdGtInt(table, column, value);
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
            DoTableIdWhereIdGtDec(table, column, value);
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
            DoTableIdWhereIdLtEqMinusInt(table, column, value);
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
            DoTableIdWhereIdLtEqMinusDec(table, column, value);
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
            DoTableIdWhereIdLtEqInt(table, column, value);
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
            DoTableIdWhereIdLtEqDec(table, column, value);
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
            DoTableIdWhereIdLtGtInt(table, column, value);
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
            DoTableIdWhereIdLtGtDec(table, column, value);
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
            DoTableIdWhereIdLtGtMinusInt(table, column, value);
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
            DoTableIdWhereIdLtGtMinusDec(table, column, value);
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
            DoTableIdWhereIdEqMinusInt(table, column, value);
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
            DoTableIdWhereIdEqMinusDec(table, column, value);
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
            DoTableIdWhereIdEqInt(table, column, value);
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
            DoTableIdWhereIdEqDec(table, column, value);
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
            DoTableIdWhereIdIsNotNull(table, column, value);
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
            DoTableIdWhereIdIsNull(table, column, value);
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

        #endregion command handlers

        #region helper methods

        private void DoBack()
        {
            if (_tableNavigation.Any())
            {
                CurrentTable = _tableNavigation.Pop();
            }
        }

        private void DoCloneAsId(string catalog)
        {
        }

        private void DoCloneCatalogIdAsId(string srcCatalog, string tgtCatalog)
        {
        }

        private void DoCloneCatalogId(string catalog)
        {
        }

        private void DoClone()
        {
        }

        private void DoColumnsInt(string topN)
        {
        }

        private void DoColumns()
        {
        }

        private void DoExportAsHtml()
        {
        }

        private void DoExportAsJson()
        {
        }

        private void DoExportAsSql()
        {
        }

        private void DoExportIdAsHtml(string table)
        {
        }

        private void DoExportIdAsJson(string table)
        {
        }

        private void DoExportIdAsSql(string table)
        {
        }

        private void DoExportIdAsXml(string table)
        {
        }

        private void DoExportAsXml()
        {
        }

        private void DoImportCatalogIdSvrIdUserIdPwdId(string catalog, string server, string user, string password)
        {
        }

        private void DoImportCatalogIdUserIdPwdId(string catalog, string user, string password)
        {
        }

        private void DoImportCatalogId(string catalog)
        {
        }

        private void DoLoadCatalogId(string catalog)
        {
            var ds = SelectedConfiguration
                .Dataset
                .FirstOrDefault(x =>
                    x.name.ToLower() == catalog.ToLower());
            if (null != ds)
            {
                SelectedDataset = ds;
            }
        }

        private async void DoLoad()
        {
            CurrentTable = await SelectedDataset
                .Table
                .First(x => x.name == SelectedDataset.defaultTable)
                .Query("".ToArray(), "".ToArray(), true);

            _tableNavigation.Push(CurrentTable);
        }

        private void DoRelateIdToIdOnIdEqId(string srcTblName, string tgtTblName, string srcColName, string tgtColName)
        {
            var srcTable = SelectedDataset
                .Table
                .FirstOrDefault(x =>
                    x.name.ToLower() == srcTblName.ToLower());

            if (null == srcTable) return;

            var tgtTable = SelectedDataset
            .Table
            .FirstOrDefault(x =>
                x.name.ToLower() == tgtTblName.ToLower());

            if (null == tgtTable) return;

            var srcCol = srcTable
                .Column
                .FirstOrDefault(x => x.name.ToLower() == srcColName.ToLower());

            if (null == srcCol) return;

            var tgtCol = tgtTable
                .Column
                .FirstOrDefault(x => x.name.ToLower() == tgtColName.ToLower());

            if (null == tgtCol || srcCol.DbType != tgtCol.DbType) return;

            var relationship = new CRelationship
            {
                name = string.Format("{0}->{1}", srcTable.name, tgtTable.name),
                fromTable = srcTable.name,
                toTable = tgtTable.name,
                fromColumn = srcCol.name,
                toColumn = tgtCol.name
            };

            if (SelectedDataset.Relationship.Any(x => x.name == relationship.name)) return;

            var currentDatasetName = SelectedDataset.name;
            var currentTableName = null != CurrentTable ? CurrentTable.Root.ConfigTable.name : string.Empty;

            SelectedDataset.Relationship.Add(relationship);
            SaveConfiguration();
            LoadConfiguration();
        }

        private void DoRelateToIdOnIdEqId(string tgtTblName, string srcColName, string tgtColName)
        {
            var currentTableName = null != CurrentTable
                ? CurrentTable.Root.ConfigTable.name
                : SelectedDataset.defaultTable;

            DoRelateIdToIdOnIdEqId(currentTableName, tgtTblName, srcColName, tgtColName);
        }

        private void DoRemoveCatalogId(string datasetName)
        {
            if (SelectedConfiguration.Dataset.Count > 1)
            {
                var currentDatasetName = SelectedDataset.name;
                var dataSet = SelectedConfiguration
                    .Dataset
                    .FirstOrDefault(x =>
                    x.name.ToLower() == datasetName.ToLower());
                if (null == dataSet) return;

                SelectedConfiguration.Dataset.Remove(dataSet);
                if (SelectedConfiguration.defaultDataset == currentDatasetName)
                {
                    SelectedConfiguration.defaultDataset = SelectedConfiguration.Dataset.First().name;
                    SelectedConfiguration.defaultDatasource = SelectedConfiguration.Dataset.First().dataSourceName;
                }

                SaveConfiguration();
                LoadConfiguration();
            }
        }

        private void DoRemove()
        {
            DoRemoveCatalogId(SelectedDataset.name);
        }

        private void DoRefresh()
        {
        }

        private void DoRefreshCatalogId(string catalog)
        {
        }

        private void DoRoot()
        {
            _tableNavigation.Clear();
        }

        private void DoTableIdDefaultWhereIdEqStrLit(string tableName, string column, string value)
        {
            var table = SelectedDataset
                .Table
                .FirstOrDefault(x =>
                    x.name.ToLower() == tableName.ToLower());
            if (null == table || SelectedDataset.defaultTable == table.name) return;

            SelectedDataset.defaultTable = table.name;
            SaveConfiguration();
            LoadConfiguration();
        }

        private void DoTableIdDefault(string tableName)
        {
            var table = SelectedDataset
                .Table
                .FirstOrDefault(x =>
                    x.name.ToLower() == tableName.ToLower());
            if (null == table || SelectedDataset.defaultTable == table.name) return;

            SelectedDataset.defaultTable = table.name;
            SaveConfiguration();
        }

        private void DoTableIdWhereIdBetweenIntAndInt(string tableName, string columnName, string minValue, string maxValue)
        {
        }

        private void DoTableIdWhereIdBetweenDecAndDec(string tableName, string columnName, string minValue, string maxValue)
        {
        }

        private void DoTableIdWhereIdGtEqMinusInt(string tableName, string columnName, string value)
        {
        }

        private void DoTableIdWhereIdGtEqMinusDec(string tableName, string columnName, string value)
        {
        }

        private void DoTableIdWhereIdGtEqInt(string tableName, string columnName, string value)
        {
        }

        private void DoTableIdWhereIdGtEqDec(string tableName, string columnName, string value)
        {
        }

        private void DoTableIdWhereIdGtMinusInt(string tableName, string columnName, string value)
        {
        }

        private void DoTableIdWhereIdGtMinusDec(string tableName, string columnName, string value)
        {
        }

        private void DoTableIdWhereIdGtInt(string tableName, string columnName, string value)
        {
        }

        private void DoTableIdWhereIdGtDec(string tableName, string columnName, string value)
        {
        }

        private void DoTableIdWhereIdLtEqMinusInt(string tableName, string columnName, string value)
        {
        }

        private void DoTableIdWhereIdLtEqMinusDec(string tableName, string columnName, string value)
        {
        }

        private void DoTableIdWhereIdLtEqInt(string tableName, string columnName, string value)
        {
        }

        private void DoTableIdWhereIdLtEqDec(string tableName, string columnName, string value)
        {
        }

        private void DoTableIdWhereIdLtGtInt(string tableName, string columnName, string value)
        {
        }

        private void DoTableIdWhereIdLtGtDec(string tableName, string columnName, string value)
        {
        }

        private void DoTableIdWhereIdLtGtMinusInt(string tableName, string columnName, string value)
        {
        }

        private void DoTableIdWhereIdLtGtMinusDec(string tableName, string columnName, string value)
        {
        }

        private void DoTableIdWhereIdEqMinusInt(string tableName, string columnName, string value)
        {
        }

        private void DoTableIdWhereIdEqMinusDec(string tableName, string columnName, string value)
        {
        }

        private void DoTableIdWhereIdEqInt(string tableName, string columnName, string value)
        {
        }

        private void DoTableIdWhereIdEqDec(string tableName, string columnName, string value)
        {
        }

        private void DoTableIdWhereIdIsNotNull(string tableName, string columnName, string value)
        {
        }

        private void DoTableIdWhereIdIsNull(string tableName, string columnName, string value)
        {
        }

        private async void DoTableId(string tableName)
        {
            var table = findTable(tableName);
            if (null == table) return;

            var needsPush = (null != CurrentTable && table.name == CurrentTable.Root.ConfigTable.name);
            CurrentTable = await table.Query("".ToArray(), "".ToArray(), true);

            if (needsPush)
            {
                _tableNavigation.Push(CurrentTable);
            }
        }

        private void DoTablesInt(string topN)
        {
            _tableNavigation.Push(SelectedDataset
                .ToDataTable(int.Parse(topN))
                .ToDatatableEx(SelectedDataset.ToTable()));
        }

        private void DoTables()
        {
            _tableNavigation.Push(SelectedDataset
                .ToDataTable()
                .ToDatatableEx(SelectedDataset.ToTable()));
        }

        private void DoTopInt(string topN)
        {
        }

        private void DoUnrelateIdToId(string srcTblName, string tgtTblName)
        {
            var srcTable = SelectedDataset
                .Table
                .FirstOrDefault(x =>
                    x.name.ToLower() == srcTblName.ToLower());

            if (null == srcTable) return;

            var tgtTable = SelectedDataset
            .Table
            .FirstOrDefault(x =>
                x.name.ToLower() == tgtTblName.ToLower());

            if (null == tgtTable || srcTable.name == tgtTable.name) return;

            var relationship = SelectedDataset
                .Relationship.FirstOrDefault(x =>
                    x.name == CRelationship.GetName(srcTable.name, tgtTable.name));
            if (null == relationship) return;

            SelectedDataset.Relationship.Remove(relationship);
            SaveConfiguration();
            LoadConfiguration();
        }

        private void DoUnrelateToId(string tgtTblName)
        {
            var currentTableName = null != CurrentTable
                ? CurrentTable.Root.ConfigTable.name
                : SelectedDataset.defaultTable;
            DoUnrelateIdToId(currentTableName, tgtTblName);
        }

        private void DoChild()
        {
        }

        private void DoChildInt(string index)
        {
        }

        private void DoChildId(string tableName)
        {
        }

        #endregion helper methods

        #region utility methods 

        private CTable findTable(string name)
        {
            return SelectedDataset
                .Table
                .FirstOrDefault(x => x.name.ToLower() == name.ToLower());
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

            public void SaveState()
            {
                _datasetName = _model.SelectedDataset.name;
                _tableName = null != _model.CurrentTable
                    ? _model.CurrentTable.Root.ConfigTable.name
                    : _model.SelectedDataset.defaultTable;
                _navigation = _model.TableNavigation.ToList();
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

        #endregion
    }
}
