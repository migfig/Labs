using System;
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
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoCloneAsId(Id0);
        }

        [Command(SymbolConstants.SYMBOL_CLONE
        , SymbolConstants.SYMBOL_CATALOG
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_AS
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void CloneCatalogIdAsId(IEnumerable<TerminalToken> tokens)
        {
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            DoCloneCatalogIdAsId(Id0, Id1);
        }

        [Command(SymbolConstants.SYMBOL_CLONE
        , SymbolConstants.SYMBOL_CATALOG
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void CloneCatalogId(IEnumerable<TerminalToken> tokens)
        {
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoCloneCatalogId(Id0);
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
            var Int0 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoColumnsInt(Int0);
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
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoExportIdAsHtml(Id0);
        }

        [Command(SymbolConstants.SYMBOL_EXPORT
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_AS
        , SymbolConstants.SYMBOL_JSON)]
        public void ExportIdAsJson(IEnumerable<TerminalToken> tokens)
        {
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoExportIdAsJson(Id0);
        }

        [Command(SymbolConstants.SYMBOL_EXPORT
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_AS
        , SymbolConstants.SYMBOL_SQL)]
        public void ExportIdAsSql(IEnumerable<TerminalToken> tokens)
        {
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoExportIdAsSql(Id0);
        }

        [Command(SymbolConstants.SYMBOL_EXPORT
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_AS
        , SymbolConstants.SYMBOL_XML)]
        public void ExportIdAsXml(IEnumerable<TerminalToken> tokens)
        {
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoExportIdAsXml(Id0);
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
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Svr0 = tokens.TerminalToken(SymbolConstants.SYMBOL_SERVER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var User0 = tokens.TerminalToken(SymbolConstants.SYMBOL_USER, 0).Text;
            var Id2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var Pwd0 = tokens.TerminalToken(SymbolConstants.SYMBOL_PASSWORD, 0).Text;
            var Id3 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 3).Text;
            DoImportCatalogIdSvrIdUserIdPwdId(Id0, Svr0, Id1, User0, Id2, Pwd0, Id3);
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
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var User0 = tokens.TerminalToken(SymbolConstants.SYMBOL_USER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var Pwd0 = tokens.TerminalToken(SymbolConstants.SYMBOL_PASSWORD, 0).Text;
            var Id2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            DoImportCatalogIdUserIdPwdId(Id0, User0, Id1, Pwd0, Id2);
        }

        [Command(SymbolConstants.SYMBOL_IMPORT
        , SymbolConstants.SYMBOL_CATALOG
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void ImportCatalogId(IEnumerable<TerminalToken> tokens)
        {
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoImportCatalogId(Id0);
        }

        [Command(SymbolConstants.SYMBOL_LOAD
        , SymbolConstants.SYMBOL_CATALOG
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void LoadCatalogId(IEnumerable<TerminalToken> tokens)
        {
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoLoadCatalogId(Id0);
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
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var Id2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            var Id3 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 3).Text;
            DoRelateIdToIdOnIdEqId(Id0, Id1, Id2, Id3);
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
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var Id2 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 2).Text;
            DoRelateToIdOnIdEqId(Id0, Id1, Id2);
        }

        [Command(SymbolConstants.SYMBOL_REMOVE
        , SymbolConstants.SYMBOL_CATALOG
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void RemoveCatalogId(IEnumerable<TerminalToken> tokens)
        {
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoRemoveCatalogId(Id0);
        }

        [Command(SymbolConstants.SYMBOL_REMOVE)]
        public void Remove(IEnumerable<TerminalToken> tokens)
        {
            DoRemove();
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
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var StrLit0 = tokens.TerminalToken(SymbolConstants.SYMBOL_STRINGLITERAL, 0).Text;
            DoTableIdDefaultWhereIdEqStrLit(Id0, Id1, StrLit0);
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_DEFAULT)]
        public void TableIdDefault(IEnumerable<TerminalToken> tokens)
        {
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoTableIdDefault(Id0);
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
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var Int0 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            var Int1 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 1).Text;
            DoTableIdWhereIdBetweenIntAndInt(Id0, Id1, Int0, Int1);
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
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var Dec0 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            var Dec1 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 1).Text;
            DoTableIdWhereIdBetweenDecAndDec(Id0, Id1, Dec0, Dec1);
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
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var Int0 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoTableIdWhereIdGtEqMinusInt(Id0, Id1, Int0);
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
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var Dec0 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            DoTableIdWhereIdGtEqMinusDec(Id0, Id1, Dec0);
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_GTEQ
        , SymbolConstants.SYMBOL_INTEGER)]
        public void TableIdWhereIdGtEqInt(IEnumerable<TerminalToken> tokens)
        {
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var Int0 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoTableIdWhereIdGtEqInt(Id0, Id1, Int0);
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_GTEQ
        , SymbolConstants.SYMBOL_DECIMAL)]
        public void TableIdWhereIdGtEqDec(IEnumerable<TerminalToken> tokens)
        {
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var Dec0 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            DoTableIdWhereIdGtEqDec(Id0, Id1, Dec0);
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
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var Int0 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoTableIdWhereIdGtMinusInt(Id0, Id1, Int0);
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
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var Dec0 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            DoTableIdWhereIdGtMinusDec(Id0, Id1, Dec0);
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_GT
        , SymbolConstants.SYMBOL_INTEGER)]
        public void TableIdWhereIdGtInt(IEnumerable<TerminalToken> tokens)
        {
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var Int0 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoTableIdWhereIdGtInt(Id0, Id1, Int0);
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_GT
        , SymbolConstants.SYMBOL_DECIMAL)]
        public void TableIdWhereIdGtDec(IEnumerable<TerminalToken> tokens)
        {
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var Dec0 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            DoTableIdWhereIdGtDec(Id0, Id1, Dec0);
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
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var Int0 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoTableIdWhereIdLtEqMinusInt(Id0, Id1, Int0);
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
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var Dec0 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            DoTableIdWhereIdLtEqMinusDec(Id0, Id1, Dec0);
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_LTEQ
        , SymbolConstants.SYMBOL_INTEGER)]
        public void TableIdWhereIdLtEqInt(IEnumerable<TerminalToken> tokens)
        {
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var Int0 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoTableIdWhereIdLtEqInt(Id0, Id1, Int0);
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_LTEQ
        , SymbolConstants.SYMBOL_DECIMAL)]
        public void TableIdWhereIdLtEqDec(IEnumerable<TerminalToken> tokens)
        {
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var Dec0 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            DoTableIdWhereIdLtEqDec(Id0, Id1, Dec0);
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_LTGT
        , SymbolConstants.SYMBOL_INTEGER)]
        public void TableIdWhereIdLtGtInt(IEnumerable<TerminalToken> tokens)
        {
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var Int0 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoTableIdWhereIdLtGtInt(Id0, Id1, Int0);
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_LTGT
        , SymbolConstants.SYMBOL_DECIMAL)]
        public void TableIdWhereIdLtGtDec(IEnumerable<TerminalToken> tokens)
        {
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var Dec0 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            DoTableIdWhereIdLtGtDec(Id0, Id1, Dec0);
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
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var Int0 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoTableIdWhereIdLtGtMinusInt(Id0, Id1, Int0);
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
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var Dec0 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            DoTableIdWhereIdLtGtMinusDec(Id0, Id1, Dec0);
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
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var Int0 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoTableIdWhereIdEqMinusInt(Id0, Id1, Int0);
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
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var Dec0 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            DoTableIdWhereIdEqMinusDec(Id0, Id1, Dec0);
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_INTEGER)]
        public void TableIdWhereIdEqInt(IEnumerable<TerminalToken> tokens)
        {
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var Int0 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoTableIdWhereIdEqInt(Id0, Id1, Int0);
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_EQ
        , SymbolConstants.SYMBOL_DECIMAL)]
        public void TableIdWhereIdEqDec(IEnumerable<TerminalToken> tokens)
        {
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var Dec0 = tokens.TerminalToken(SymbolConstants.SYMBOL_DECIMAL, 0).Text;
            DoTableIdWhereIdEqDec(Id0, Id1, Dec0);
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
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var Null0 = tokens.TerminalToken(SymbolConstants.SYMBOL_NULL, 0).Text;
            DoTableIdWhereIdIsNotNull(Id0, Id1, Null0);
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_WHERE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_IS
        , SymbolConstants.SYMBOL_NULL)]
        public void TableIdWhereIdIsNull(IEnumerable<TerminalToken> tokens)
        {
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            var Null0 = tokens.TerminalToken(SymbolConstants.SYMBOL_NULL, 0).Text;
            DoTableIdWhereIdIsNull(Id0, Id1, Null0);
        }

        [Command(SymbolConstants.SYMBOL_TABLE
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void TableId(IEnumerable<TerminalToken> tokens)
        {
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoTableId(Id0);
        }

        [Command(SymbolConstants.SYMBOL_TABLES
        , SymbolConstants.SYMBOL_INTEGER)]
        public void TablesInt(IEnumerable<TerminalToken> tokens)
        {
            var Int0 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoTablesInt(Int0);
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
            var Int0 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoTopInt(Int0);
        }

        [Command(SymbolConstants.SYMBOL_UNRELATE
        , SymbolConstants.SYMBOL_IDENTIFIER
        , SymbolConstants.SYMBOL_TO
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void UnrelateIdToId(IEnumerable<TerminalToken> tokens)
        {
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            var Id1 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 1).Text;
            DoUnrelateIdToId(Id0, Id1);
        }

        [Command(SymbolConstants.SYMBOL_UNRELATE
        , SymbolConstants.SYMBOL_TO
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void UnrelateToId(IEnumerable<TerminalToken> tokens)
        {
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoUnrelateToId(Id0);
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
            var Int0 = tokens.TerminalToken(SymbolConstants.SYMBOL_INTEGER, 0).Text;
            DoChildInt(Int0);
        }

        [Command(SymbolConstants.SYMBOL_CHILD
        , SymbolConstants.SYMBOL_IDENTIFIER)]
        public void ChildId(IEnumerable<TerminalToken> tokens)
        {
            var Id0 = tokens.TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER, 0).Text;
            DoChildId(Id0);
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

        private void DoCloneAsId(string Id0)
        {
        }

        private void DoCloneCatalogIdAsId(string Id0, string Id1)
        {
        }

        private void DoCloneCatalogId(string Id0)
        {
        }

        private void DoClone()
        {
        }

        private void DoColumnsInt(string Int0)
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

        private void DoExportIdAsHtml(string Id0)
        {
        }

        private void DoExportIdAsJson(string Id0)
        {
        }

        private void DoExportIdAsSql(string Id0)
        {
        }

        private void DoExportIdAsXml(string Id0)
        {
        }

        private void DoExportAsXml()
        {
        }

        private void DoImportCatalogIdSvrIdUserIdPwdId(string Id0, string Svr0, string Id1, string User0, string Id2, string Pwd0, string Id3)
        {
        }

        private void DoImportCatalogIdUserIdPwdId(string Id0, string User0, string Id1, string Pwd0, string Id2)
        {
        }

        private void DoImportCatalogId(string Id0)
        {
        }

        private void DoLoadCatalogId(string Id0)
        {
            var ds = SelectedConfiguration
                .Dataset
                .FirstOrDefault(x =>
                    x.name.ToLower() == Id0.ToLower());
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

        private void DoRelateIdToIdOnIdEqId(string Id0, string Id1, string Id2, string Id3)
        {
            var srcTable = SelectedDataset
                .Table
                .FirstOrDefault(x =>
                    x.name.ToLower() == Id0.ToLower());

            if (null == srcTable) return;

            var tgtTable = SelectedDataset
            .Table
            .FirstOrDefault(x =>
                x.name.ToLower() == Id1.ToLower());

            if (null == tgtTable) return;

            var srcCol = srcTable
                .Column
                .FirstOrDefault(x => x.name.ToLower() == Id2.ToLower());

            if (null == srcCol) return;

            var tgtCol = tgtTable
                .Column
                .FirstOrDefault(x => x.name.ToLower() == Id3.ToLower());

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

            if (SelectedDataset.name != currentDatasetName)
            {
                SelectedDataset = SelectedConfiguration.Dataset.First(x => x.name == currentDatasetName);
            }

            if (!string.IsNullOrWhiteSpace(currentTableName))
            {
                Command = "table " + currentTableName;
                ExecuteCommand();
            }
        }

        private void DoRelateToIdOnIdEqId(string Id0, string Id1, string Id2)
        {
            //var currentTableName = null != CurrentTable
            //    ? CurrentTable.Root.ConfigTable.name
            //    : SelectedDataset.defaultTable;

            //var list = tokens.ToList();
            //list.Insert(1,
            //    new TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER.SymbolTerminal(currentTableName),
            //        currentTableName,
            //        new Location(0, 0, 0)));
            //RelateIdToIdOnIdEqId(list);
        }

        private void DoRemoveCatalogId(string Id0)
        {
            if (SelectedConfiguration.Dataset.Count > 1)
            {
                var currentDatasetName = SelectedDataset.name;
                var dataSet = SelectedConfiguration
                    .Dataset
                    .FirstOrDefault(x =>
                    x.name.ToLower() == Id0.ToLower());
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
            //var list = tokens.ToList();
            //list.Add(
            //    new TerminalToken(SymbolConstants.SYMBOL_CATALOG.SymbolTerminal("catalog"),
            //        "catalog",
            //        new Location(0, 0, 0)));
            //list.Add(
            //    new TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER.SymbolTerminal(SelectedDataset.name),
            //        SelectedDataset.name,
            //        new Location(0, 0, 0)));

            //RemoveCatalogId(list);
        }

        private void DoRoot()
        {
            _tableNavigation.Clear();
        }

        private void DoTableIdDefaultWhereIdEqStrLit(string Id0, string Id1, string StrLit0)
        {
            var table = SelectedDataset
                .Table
                .FirstOrDefault(x =>
                    x.name.ToLower() == Id0.ToLower());
            if (null == table || SelectedDataset.defaultTable == table.name) return;

            SelectedDataset.defaultTable = table.name;
            SaveConfiguration();
            LoadConfiguration();
        }

        private void DoTableIdDefault(string Id0)
        {
            var table = SelectedDataset
                .Table
                .FirstOrDefault(x =>
                    x.name.ToLower() == Id0.ToLower());
            if (null == table || SelectedDataset.defaultTable == table.name) return;

            SelectedDataset.defaultTable = table.name;
            SaveConfiguration();
        }

        private void DoTableIdWhereIdBetweenIntAndInt(string Id0, string Id1, string Int0, string Int1)
        {
        }

        private void DoTableIdWhereIdBetweenDecAndDec(string Id0, string Id1, string Dec0, string Dec1)
        {
        }

        private void DoTableIdWhereIdGtEqMinusInt(string Id0, string Id1, string Int0)
        {
        }

        private void DoTableIdWhereIdGtEqMinusDec(string Id0, string Id1, string Dec0)
        {
        }

        private void DoTableIdWhereIdGtEqInt(string Id0, string Id1, string Int0)
        {
        }

        private void DoTableIdWhereIdGtEqDec(string Id0, string Id1, string Dec0)
        {
        }

        private void DoTableIdWhereIdGtMinusInt(string Id0, string Id1, string Int0)
        {
        }

        private void DoTableIdWhereIdGtMinusDec(string Id0, string Id1, string Dec0)
        {
        }

        private void DoTableIdWhereIdGtInt(string Id0, string Id1, string Int0)
        {
        }

        private void DoTableIdWhereIdGtDec(string Id0, string Id1, string Dec0)
        {
        }

        private void DoTableIdWhereIdLtEqMinusInt(string Id0, string Id1, string Int0)
        {
        }

        private void DoTableIdWhereIdLtEqMinusDec(string Id0, string Id1, string Dec0)
        {
        }

        private void DoTableIdWhereIdLtEqInt(string Id0, string Id1, string Int0)
        {
        }

        private void DoTableIdWhereIdLtEqDec(string Id0, string Id1, string Dec0)
        {
        }

        private void DoTableIdWhereIdLtGtInt(string Id0, string Id1, string Int0)
        {
        }

        private void DoTableIdWhereIdLtGtDec(string Id0, string Id1, string Dec0)
        {
        }

        private void DoTableIdWhereIdLtGtMinusInt(string Id0, string Id1, string Int0)
        {
        }

        private void DoTableIdWhereIdLtGtMinusDec(string Id0, string Id1, string Dec0)
        {
        }

        private void DoTableIdWhereIdEqMinusInt(string Id0, string Id1, string Int0)
        {
        }

        private void DoTableIdWhereIdEqMinusDec(string Id0, string Id1, string Dec0)
        {
        }

        private void DoTableIdWhereIdEqInt(string Id0, string Id1, string Int0)
        {
        }

        private void DoTableIdWhereIdEqDec(string Id0, string Id1, string Dec0)
        {
        }

        private void DoTableIdWhereIdIsNotNull(string Id0, string Id1, string Null0)
        {
        }

        private void DoTableIdWhereIdIsNull(string Id0, string Id1, string Null0)
        {
        }

        private async void DoTableId(string Id0)
        {
            var table = findTable(Id0);
            if (null == table) return;

            var needsPush = (null != CurrentTable && table.name == CurrentTable.Root.ConfigTable.name);
            CurrentTable = await table.Query("".ToArray(), "".ToArray(), true);

            if (needsPush)
            {
                _tableNavigation.Push(CurrentTable);
            }
        }

        private void DoTablesInt(string Int0)
        {
            _tableNavigation.Push(SelectedDataset
                .ToDataTable(int.Parse(Int0))
                .ToDatatableEx(SelectedDataset.ToTable()));
        }

        private void DoTables()
        {
            _tableNavigation.Push(SelectedDataset
                .ToDataTable()
                .ToDatatableEx(SelectedDataset.ToTable()));
        }

        private void DoTopInt(string Int0)
        {
        }

        private void DoUnrelateIdToId(string Id0, string Id1)
        {
            var srcTable = SelectedDataset
                .Table
                .FirstOrDefault(x =>
                    x.name.ToLower() == Id0.ToLower());

            if (null == srcTable) return;

            var tgtTable = SelectedDataset
            .Table
            .FirstOrDefault(x =>
                x.name.ToLower() == Id1.ToLower());

            if (null == tgtTable || srcTable.name == tgtTable.name) return;

            var relationship = SelectedDataset
                .Relationship.FirstOrDefault(x =>
                    x.name == CRelationship.GetName(srcTable.name, tgtTable.name));
            if (null == relationship) return;

            SelectedDataset.Relationship.Remove(relationship);
            SaveConfiguration();
            LoadConfiguration();
        }

        private void DoUnrelateToId(string Id0)
        {
            //var currentTableName = null != CurrentTable
            //    ? CurrentTable.Root.ConfigTable.name
            //    : SelectedDataset.defaultTable;

            //var list = tokens.ToList();
            //list.Insert(1,
            //    new TerminalToken(SymbolConstants.SYMBOL_IDENTIFIER.SymbolTerminal(currentTableName),
            //        currentTableName,
            //        new Location(0, 0, 0)));
            //UnrelateIdToId(list);
        }

        private void DoChild()
        {
        }

        private void DoChildInt(string Int0)
        {
        }

        private void DoChildId(string Id0)
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
    }
}
