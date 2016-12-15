#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using RelatedRecords.Parser;
using com.calitha.goldparser;
using static Common.Extensions;
using System.ComponentModel;
using System.Windows.Threading;
using System.Threading.Tasks;
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
                    Dispatcher.CurrentDispatcher.InvokeAsync(() =>
                    {
                        IsBusy = true;
                    }, DispatcherPriority.Send);

                    Dispatcher.CurrentDispatcher.InvokeAsync(() =>
                    {
                        method.Invoke(this, new object[] { results.Tokens });
                        IsBusy = false;
                        Command = string.Empty;
                    }, DispatcherPriority.ApplicationIdle);
                    //Dispatcher.CurrentDispatcher.InvokeAsync(() =>
                    //{
                    //    IsBusy = true;
                    //    var task = new Task(() =>
                    //    {
                    //        method.Invoke(this, new object[] { results.Tokens });
                    //    });
                    //    task.Start();
                    //    Task.WaitAll(task);
                    //    IsBusy = false;
                    //    Command = string.Empty;
                    //}, DispatcherPriority.ApplicationIdle);
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
        , SymbolConstants.SYMBOL_HTML
        ,SymbolConstants.SYMBOL_NOCHILD)]
        public void ExportAsHtmlNoChild(IEnumerable<TerminalToken> tokens)
        {
            DoExportAsHtml(false);
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
        , SymbolConstants.SYMBOL_JSON
        , SymbolConstants.SYMBOL_NOCHILD)]
        public void ExportAsJsonNoChild(IEnumerable<TerminalToken> tokens)
        {
            DoExportAsJson(false);
        }

        [Command(SymbolConstants.SYMBOL_EXPORT
        , SymbolConstants.SYMBOL_AS
        , SymbolConstants.SYMBOL_SQL)]
        public void ExportAsSql(IEnumerable<TerminalToken> tokens)
        {
            DoExportAsSql();
        }

        [Command(SymbolConstants.SYMBOL_EXPORT
        , SymbolConstants.SYMBOL_AS
        , SymbolConstants.SYMBOL_SQL
        , SymbolConstants.SYMBOL_NOCHILD)]
        public void ExportAsSqlNochild(IEnumerable<TerminalToken> tokens)
        {
            DoExportAsSql(false);
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
        , SymbolConstants.SYMBOL_HTML
        , SymbolConstants.SYMBOL_NOCHILD)]
        public void ExportIdAsHtmlNoChild(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoExportIdAsHtml(table, false);
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
        , SymbolConstants.SYMBOL_JSON
        , SymbolConstants.SYMBOL_NOCHILD)]
        public void ExportIdAsJsonNoChild(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoExportIdAsJson(table, false);
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
        , SymbolConstants.SYMBOL_SQL
        , SymbolConstants.SYMBOL_NOCHILD)]
        public void ExportIdAsSqlNochild(IEnumerable<TerminalToken> tokens)
        {
            var table = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoExportIdAsSql(table, false);
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
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_AS
        , SymbolConstants.SYMBOL_XML
        , SymbolConstants.SYMBOL_NOCHILD)]
        public void ExportIdAsXmlNoChild(IEnumerable<TerminalToken> tokens)
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

        [Command(SymbolConstants.SYMBOL_EXPORT
        , SymbolConstants.SYMBOL_AS
        , SymbolConstants.SYMBOL_XML
        , SymbolConstants.SYMBOL_NOCHILD)]
        public void ExportAsXmlNoChild(IEnumerable<TerminalToken> tokens)
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
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_DEFAULT)]
        public void LoadCatalogIdDefault(IEnumerable<TerminalToken> tokens)
        {
            var catalog = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoLoadCatalogId(catalog, true);
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

        [Command(SymbolConstants.SYMBOL_HOME)]
        public void Root(IEnumerable<TerminalToken> tokens)
        {
            DoRoot();
        }

        #region table commands

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
        , SymbolConstants.SYMBOL_NOT
        , SymbolConstants.SYMBOL_BETWEEN
        , SymbolConstants.SYMBOL_INTEGER
        , SymbolConstants.SYMBOL_AND
        , SymbolConstants.SYMBOL_INTEGER)]
        public void TableIdWhereIdNotBetweenIntAndInt(IEnumerable<TerminalToken> tokens)
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
        , SymbolConstants.SYMBOL_NOT
        , SymbolConstants.SYMBOL_BETWEEN
        , SymbolConstants.SYMBOL_DECIMAL
        , SymbolConstants.SYMBOL_AND
        , SymbolConstants.SYMBOL_DECIMAL)]
        public void TableIdWhereIdNotBetweenDecAndDec(IEnumerable<TerminalToken> tokens)
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

        #endregion table commands

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

        [Command(SymbolConstants.SYMBOL_CATALOGS)]
        public void Catalogs(IEnumerable<TerminalToken> tokens)
        {
            DoCatalogsInt();
        }

        [Command(SymbolConstants.SYMBOL_CATALOGS
        , SymbolConstants.SYMBOL_INTEGER)]
        public void CatalogsInt(IEnumerable<TerminalToken> tokens)
        {
            var topN = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoCatalogsInt(int.Parse(topN));
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

        [Command(SymbolConstants.SYMBOL_QUERY
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void QueryId(IEnumerable<TerminalToken> tokens)
        {
            var columnName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoQueryIdRowInt(columnName);
        }

        [Command(SymbolConstants.SYMBOL_QUERY
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_ROW
        , SymbolConstants.SYMBOL_INTEGER)]
        public void QueryIdRowInt(IEnumerable<TerminalToken> tokens)
        {
            var columnName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var row = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoQueryIdRowInt(columnName, int.Parse(row));
        }

        [Command(SymbolConstants.SYMBOL_TRANSFORM)]
        public void Transform(IEnumerable<TerminalToken> tokens)
        {
            DoTransformIdTemplateId();
        }

        [Command(SymbolConstants.SYMBOL_TRANSFORM
        , SymbolConstants.SYMBOL_TEMPLATE
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void TransformTemplateId(IEnumerable<TerminalToken> tokens)
        {
            var template = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            DoTransformIdTemplateId(string.Empty, template);
        }

        [Command(SymbolConstants.SYMBOL_TRANSFORM
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void TransformId(IEnumerable<TerminalToken> tokens)
        {
            var sqlObject = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoTransformIdTemplateId(sqlObject);
        }

        [Command(SymbolConstants.SYMBOL_TRANSFORM
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_TEMPLATE
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void TransformIdTemplateId(IEnumerable<TerminalToken> tokens)
        {
            var sqlObject = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var template = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            DoTransformIdTemplateId(sqlObject, template);
        }

        #region run commands

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void RunId(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoRunIdWithParams(queryName);
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER)]
        public void RunIdWithIdEqInt(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoRunIdWithParams(queryName, new QueryParam(paramName, value));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_DECIMAL)]
        public void RunIdWithIdEqIntCommaIdEqDec(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            DoRunIdWithParams(queryName, 
                new QueryParam(paramName, value), 
                new QueryParam(paramName2, value2, SymbolConstants.SYMBOL_DECIMAL));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_DECIMAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_STRINGLITERAL)]
        public void RunIdWithIdEqIntCommaIdEqDecCommaIdEqStrLit(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            var paranName3 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 3).Text;
            var value3 = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 0).Text;
            DoRunIdWithParams(queryName, 
                new QueryParam(paramName, value), 
                new QueryParam(paramName2, value2, SymbolConstants.SYMBOL_DECIMAL),
                new QueryParam(paranName3, value3, SymbolConstants.SYMBOL_STRINGLITERAL));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_DECIMAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_STRINGLITERAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_NULL)]
        public void RunIdWithIdEqIntCommaIdEqDecCommaIdEqStrLitCommaIdEqNull(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            var paramName3 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 3).Text;
            var value3 = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 0).Text;
            var paramName4 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 4).Text;
            var value4 = tokens.TerminalToken(SymbolConstants.SYMBOL_NULL, 0).Text;
            DoRunIdWithParams(queryName, 
                new QueryParam(paramName, value), 
                new QueryParam(paramName2, value2, SymbolConstants.SYMBOL_DECIMAL),
                new QueryParam(paramName3, value3, SymbolConstants.SYMBOL_STRINGLITERAL),
                new QueryParam(paramName4, value4, SymbolConstants.SYMBOL_NULL));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER)]
        public void RunIdWithIdEqIntCommaIdEqInt(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 1).Text;
            DoRunIdWithParams(queryName, 
                new QueryParam(paramName, value), 
                new QueryParam(paramName2, value2));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER)]
        public void RunIdWithIdEqIntCommaIdEqIntCommaIdEqInt(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 1).Text;
            var paramName3 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 3).Text;
            var value3 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 2).Text;
            DoRunIdWithParams(queryName, 
                new QueryParam(paramName, value), 
                new QueryParam(paramName2, value2),
                new QueryParam(paramName3, value3));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER)]
        public void RunIdWithIdEqIntCommaIdEqIntCommaIdEqIntCommaIdEqInt(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 1).Text;
            var paramName3 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 3).Text;
            var value3 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 2).Text;
            var paramName4 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 4).Text;
            var value4 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 3).Text;
            DoRunIdWithParams(queryName, 
                new QueryParam(paramName, value),
                new QueryParam(paramName2, value2),
                new QueryParam(paramName3, value3),
                new QueryParam(paramName4, value4));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER)]
        public void RunIdWithIdEqIntCommaIdEqIntCommaIdEqIntCommaIdEqIntCommaIdEqInt(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 1).Text;
            var paramName3 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 3).Text;
            var value3 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 2).Text;
            var paramName4 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 4).Text;
            var value4 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 3).Text;
            var paramName5 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 5).Text;
            var value5 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 4).Text;
            DoRunIdWithParams(queryName,
                new QueryParam(paramName, value),
                new QueryParam(paramName2, value2),
                new QueryParam(paramName3, value3),
                new QueryParam(paramName4, value4),
                new QueryParam(paramName5, value5));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_DECIMAL)]
        public void RunIdWithIdEqDec(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            DoRunIdWithParams(queryName, new QueryParam(paramName, value, SymbolConstants.SYMBOL_DECIMAL));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_DECIMAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER)]
        public void RunIdWithIdEqDecCommaIdEqInt(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoRunIdWithParams(queryName,
                new QueryParam(paramName, value, SymbolConstants.SYMBOL_DECIMAL),
                new QueryParam(paramName2, value2));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_DECIMAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_STRINGLITERAL)]
        public void RunIdWithIdEqDecCommaIdEqIntCommaIdEqStrLit(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            var paramName3 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 3).Text;
            var value3 = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 0).Text;
            DoRunIdWithParams(queryName,
                new QueryParam(paramName, value, SymbolConstants.SYMBOL_DECIMAL),
                new QueryParam(paramName2, value2),
                new QueryParam(paramName3, value3, SymbolConstants.SYMBOL_STRINGLITERAL));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_DECIMAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_STRINGLITERAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_NULL)]
        public void RunIdWithIdEqDecCommaIdEqIntCommaIdEqStrLitCommaIdEqNull(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            var paramName3 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 3).Text;
            var value3 = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 0).Text;
            var paramName4 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 4).Text;
            var value4 = tokens.TerminalToken(SymbolConstants.SYMBOL_NULL, 0).Text;
            DoRunIdWithParams(queryName,
                new QueryParam(paramName, value, SymbolConstants.SYMBOL_DECIMAL),
                new QueryParam(paramName2, value2, SymbolConstants.SYMBOL_INTEGER),
                new QueryParam(paramName3, value3, SymbolConstants.SYMBOL_STRINGLITERAL),
                new QueryParam(paramName4, value4, SymbolConstants.SYMBOL_NULL));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_DECIMAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_DECIMAL)]
        public void RunIdWithIdEqDecCommaIdEqDec(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 1).Text;
            DoRunIdWithParams(queryName,
                new QueryParam(paramName, value, SymbolConstants.SYMBOL_DECIMAL),
                new QueryParam(paramName2, value2, SymbolConstants.SYMBOL_DECIMAL));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_DECIMAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_DECIMAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_DECIMAL)]
        public void RunIdWithIdEqDecCommaIdEqDecCommaIdEqDec(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 1).Text;
            var paramName3 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 3).Text;
            var value3 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 2).Text;
            DoRunIdWithParams(queryName,
                new QueryParam(paramName, value, SymbolConstants.SYMBOL_DECIMAL),
                new QueryParam(paramName2, value2, SymbolConstants.SYMBOL_DECIMAL),
                new QueryParam(paramName3, value3, SymbolConstants.SYMBOL_DECIMAL));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_DECIMAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_DECIMAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_DECIMAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_DECIMAL)]
        public void RunIdWithIdEqDecCommaIdEqDecCommaIdEqDecCommaIdEqDec(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 1).Text;
            var paramName3 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 3).Text;
            var value3 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 2).Text;
            var paramName4 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 4).Text;
            var value4 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 3).Text;
            DoRunIdWithParams(queryName,
                new QueryParam(paramName, value, SymbolConstants.SYMBOL_DECIMAL),
                new QueryParam(paramName2, value2, SymbolConstants.SYMBOL_DECIMAL),
                new QueryParam(paramName3, value3, SymbolConstants.SYMBOL_DECIMAL),
                new QueryParam(paramName4, value4, SymbolConstants.SYMBOL_DECIMAL));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_DECIMAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_DECIMAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_DECIMAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_DECIMAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_DECIMAL)]
        public void RunIdWithIdEqDecCommaIdEqDecCommaIdEqDecCommaIdEqDecCommaIdEqDec(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 1).Text;
            var paramName3 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 3).Text;
            var value3 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 2).Text;
            var paramName4 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 4).Text;
            var value4 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 3).Text;
            var paramName5 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 5).Text;
            var value5 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 4).Text;
            DoRunIdWithParams(queryName,
                new QueryParam(paramName, value, SymbolConstants.SYMBOL_DECIMAL),
                new QueryParam(paramName2, value2, SymbolConstants.SYMBOL_DECIMAL),
                new QueryParam(paramName3, value3, SymbolConstants.SYMBOL_DECIMAL),
                new QueryParam(paramName4, value4, SymbolConstants.SYMBOL_DECIMAL),
                new QueryParam(paramName5, value5, SymbolConstants.SYMBOL_DECIMAL));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_STRINGLITERAL)]
        public void RunIdWithIdEqStrLit(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 0).Text;
            DoRunIdWithParams(queryName, new QueryParam(paramName, value, SymbolConstants.SYMBOL_STRINGLITERAL));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_STRINGLITERAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER)]
        public void RunIdWithIdEqStrLitCommaIdEqInt(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoRunIdWithParams(queryName,
                new QueryParam(paramName, value, SymbolConstants.SYMBOL_STRINGLITERAL),
                new QueryParam(paramName2, value2));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_STRINGLITERAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_DECIMAL)]
        public void RunIdWithIdEqStrLitCommaIdEqIntCommaIdEqDec(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            var paramName3 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 3).Text;
            var value3 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            DoRunIdWithParams(queryName,
                new QueryParam(paramName, value, SymbolConstants.SYMBOL_STRINGLITERAL),
                new QueryParam(paramName2, value2),
                new QueryParam(paramName3, value3, SymbolConstants.SYMBOL_DECIMAL));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_STRINGLITERAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_DECIMAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_NULL)]
        public void RunIdWithIdEqStrLitCommaIdEqIntCommaIdEqDecCommaIdEqNull(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            var paramName3 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 3).Text;
            var value3 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            var paramName4 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 4).Text;
            var value4 = tokens.TerminalToken(SymbolConstants.SYMBOL_NULL, 0).Text;
            DoRunIdWithParams(queryName,
                new QueryParam(paramName, value, SymbolConstants.SYMBOL_STRINGLITERAL),
                new QueryParam(paramName2, value2),
                new QueryParam(paramName3, value3, SymbolConstants.SYMBOL_DECIMAL),
                new QueryParam(paramName4, value4, SymbolConstants.SYMBOL_NULL));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_STRINGLITERAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_STRINGLITERAL)]
        public void RunIdWithIdEqStrLitCommaIdEqStrLit(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 1).Text;
            DoRunIdWithParams(queryName,
                new QueryParam(paramName, value, SymbolConstants.SYMBOL_STRINGLITERAL),
                new QueryParam(paramName2, value2, SymbolConstants.SYMBOL_STRINGLITERAL));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_STRINGLITERAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_STRINGLITERAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_STRINGLITERAL)]
        public void RunIdWithIdEqStrLitCommaIdEqStrLitCommaIdEqStrLit(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 1).Text;
            var paramName3 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 3).Text;
            var value3 = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 2).Text;
            DoRunIdWithParams(queryName,
                new QueryParam(paramName, value, SymbolConstants.SYMBOL_STRINGLITERAL),
                new QueryParam(paramName3, value2, SymbolConstants.SYMBOL_STRINGLITERAL),
                new QueryParam(paramName3, value3, SymbolConstants.SYMBOL_STRINGLITERAL));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_STRINGLITERAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_STRINGLITERAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_STRINGLITERAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_STRINGLITERAL)]
        public void RunIdWithIdEqStrLitCommaIdEqStrLitCommaIdEqStrLitCommaIdEqStrLit(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 1).Text;
            var paramName3 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 3).Text;
            var value3 = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 2).Text;
            var paramName4 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 4).Text;
            var value4 = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 3).Text;
            DoRunIdWithParams(queryName,
                new QueryParam(paramName, value, SymbolConstants.SYMBOL_STRINGLITERAL),
                new QueryParam(paramName2, value2, SymbolConstants.SYMBOL_STRINGLITERAL),
                new QueryParam(paramName3, value3, SymbolConstants.SYMBOL_STRINGLITERAL),
                new QueryParam(paramName4, value4, SymbolConstants.SYMBOL_STRINGLITERAL));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_STRINGLITERAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_STRINGLITERAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_STRINGLITERAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_STRINGLITERAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_STRINGLITERAL)]
        public void RunIdWithIdEqStrLitCommaIdEqStrLitCommaIdEqStrLitCommaIdEqStrLitCommaIdEqStrLit(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 1).Text;
            var paramName3 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 3).Text;
            var value3 = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 2).Text;
            var paramName4 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 4).Text;
            var value4 = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 3).Text;
            var paramName5 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 5).Text;
            var value5 = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 4).Text;
            DoRunIdWithParams(queryName,
                new QueryParam(paramName, value, SymbolConstants.SYMBOL_STRINGLITERAL),
                new QueryParam(paramName2, value2, SymbolConstants.SYMBOL_STRINGLITERAL),
                new QueryParam(paramName3, value3, SymbolConstants.SYMBOL_STRINGLITERAL),
                new QueryParam(paramName4, value4, SymbolConstants.SYMBOL_STRINGLITERAL),
                new QueryParam(paramName5, value5, SymbolConstants.SYMBOL_STRINGLITERAL));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_NULL)]
        public void RunIdWithIdEqNull(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_NULL, 0).Text;
            DoRunIdWithParams(queryName, new QueryParam(paramName, value, SymbolConstants.SYMBOL_NULL));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_NULL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER)]
        public void RunIdWithIdEqNullCommaIdEqInt(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_NULL, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoRunIdWithParams(queryName,
                new QueryParam(paramName, value, SymbolConstants.SYMBOL_NULL),
                new QueryParam(paramName2, value2));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_NULL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_DECIMAL)]
        public void RunIdWithIdEqNullCommaIdEqIntCommaIdEqDec(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_NULL, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            var paramName3 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 3).Text;
            var value3 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            DoRunIdWithParams(queryName,
                new QueryParam(paramName, value, SymbolConstants.SYMBOL_NULL),
                new QueryParam(paramName2, value2),
                new QueryParam(paramName3, value3, SymbolConstants.SYMBOL_DECIMAL));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_NULL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_DECIMAL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_STRINGLITERAL)]
        public void RunIdWithIdEqNullCommaIdEqIntCommaIdEqDecCommaIdEqStrLit(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_NULL, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            var paramName3 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 3).Text;
            var value3 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            var paramName4 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 4).Text;
            var value4 = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 0).Text;
            DoRunIdWithParams(queryName,
                new QueryParam(paramName, value, SymbolConstants.SYMBOL_NULL),
                new QueryParam(paramName2, value2),
                new QueryParam(paramName3, value3, SymbolConstants.SYMBOL_DECIMAL),
                new QueryParam(paramName4, value4, SymbolConstants.SYMBOL_STRINGLITERAL));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_NULL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_NULL)]
        public void RunIdWithIdEqNullCommaIdEqNull(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_NULL, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_NULL, 1).Text;
            DoRunIdWithParams(queryName,
                new QueryParam(paramName, value, SymbolConstants.SYMBOL_NULL),
                new QueryParam(paramName2, value2, SymbolConstants.SYMBOL_NULL));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_NULL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_NULL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_NULL)]
        public void RunIdWithIdEqNullCommaIdEqNullCommaIdEqNull(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_NULL, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_NULL, 1).Text;
            var paramName3 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 3).Text;
            var value3 = tokens.TerminalToken(SymbolConstants.SYMBOL_NULL, 2).Text;
            DoRunIdWithParams(queryName,
                new QueryParam(paramName, value, SymbolConstants.SYMBOL_NULL),
                new QueryParam(paramName2, value2, SymbolConstants.SYMBOL_NULL),
                new QueryParam(paramName3, value3, SymbolConstants.SYMBOL_NULL));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_NULL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_NULL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_NULL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_NULL)]
        public void RunIdWithIdEqNullCommaIdEqNullCommaIdEqNullCommaIdEqNull(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_NULL, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_NULL, 1).Text;
            var paramName3 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 3).Text;
            var value3 = tokens.TerminalToken(SymbolConstants.SYMBOL_NULL, 2).Text;
            var paramName4 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 4).Text;
            var value4 = tokens.TerminalToken(SymbolConstants.SYMBOL_NULL, 3).Text;
            DoRunIdWithParams(queryName,
                new QueryParam(paramName, value, SymbolConstants.SYMBOL_NULL),
                new QueryParam(paramName2, value2, SymbolConstants.SYMBOL_NULL),
                new QueryParam(paramName3, value3, SymbolConstants.SYMBOL_NULL),
                new QueryParam(paramName4, value4, SymbolConstants.SYMBOL_NULL));
        }

        [Command(SymbolConstants.SYMBOL_RUN
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WITH
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_NULL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_NULL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_NULL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_NULL
        , SymbolConstants.SYMBOL_COMMA
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_NULL)]
        public void RunIdWithIdEqNullCommaIdEqNullCommaIdEqNullCommaIdEqNullCommaIdEqNull(IEnumerable<TerminalToken> tokens)
        {
            var queryName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var paramName = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var value = tokens.TerminalToken(SymbolConstants.SYMBOL_NULL, 0).Text;
            var paramName2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var value2 = tokens.TerminalToken(SymbolConstants.SYMBOL_NULL, 1).Text;
            var paramName3 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 3).Text;
            var value3 = tokens.TerminalToken(SymbolConstants.SYMBOL_NULL, 2).Text;
            var paramName4 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 4).Text;
            var value4 = tokens.TerminalToken(SymbolConstants.SYMBOL_NULL, 3).Text;
            var paramName5 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 5).Text;
            var value5 = tokens.TerminalToken(SymbolConstants.SYMBOL_NULL, 4).Text;
            DoRunIdWithParams(queryName,
                new QueryParam(paramName, value, SymbolConstants.SYMBOL_NULL),
                new QueryParam(paramName2, value2, SymbolConstants.SYMBOL_NULL),
                new QueryParam(paramName3, value3, SymbolConstants.SYMBOL_NULL),
                new QueryParam(paramName4, value4, SymbolConstants.SYMBOL_NULL),
                new QueryParam(paramName5, value5, SymbolConstants.SYMBOL_NULL));
        }

        #endregion run commands

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

        private bool IsDefaultTable(string tableName)
        {
            return tableName.ToLower() == SelectedDataset.defaultTable.ToLower();
        }

        private void PushCurrentTable(DatatableEx table)
        {
            if(null != CurrentTable)
                _tableNavigation.Push(CurrentTable);
            _goBack.RaiseCanExecuteChanged();
           
            CurrentTable = table;
        }

        private void ClearState()
        {
            CurrentTable = null;
            _tableNavigation.Clear();
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

        internal class QueryParam
        {
            private readonly SymbolConstants _symbol;
            public string Name { get; private set; }
            public object Value { get; private set; }

            public QueryParam(string name, string value, SymbolConstants symbol = SymbolConstants.SYMBOL_INTEGER)
            {
                Name = name;
                _symbol = symbol;
                Value = value;
            }
        }

        internal class Worker
        {
            private readonly BackgroundWorker _worker;
            private readonly MainViewModel _model;

            public Worker(MainViewModel model)
            {
                _model = model;
                _worker = new BackgroundWorker();
            }

            public delegate void SetBusyDelegate();
            public void SetBusy()
            {
                _model.IsBusy = true;
            }

            public void Run(Func<object> workBlock, Action<object> completedBlock = null)
            {
                try
                {
                    Dispatcher.CurrentDispatcher
                        .BeginInvoke(DispatcherPriority.Send, new SetBusyDelegate(SetBusy));

                    _worker.DoWork += (s, o) =>
                    {
                        if (null != workBlock)
                        {
                            o.Result = workBlock.Invoke();
                        }
                    };
                    _worker.RunWorkerCompleted += (s, o) =>
                    {
                        if (null != completedBlock)
                        {
                            completedBlock.Invoke(o.Result);
                        }
                        else if(o.Result is DatatableEx)
                        {
                            _model.PushCurrentTable(o.Result as DatatableEx);
                        }
                        _model.IsBusy = false;
                    };
                    _worker.RunWorkerAsync(this);
                }
                catch (Exception e)
                {
                    ErrorLog.Error(e, "When running method");
                }
            }

        }

        #endregion utility classes
    }
}
