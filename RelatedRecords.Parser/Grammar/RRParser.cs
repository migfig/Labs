using System;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using com.calitha.commons;
using com.calitha.goldparser;
using System.Collections.Generic;

namespace RelatedRecords.Parser
{
    public static class Extensions
    {
        public static SymbolConstants SymbolEnum(this Symbol symbol)
        {
            return (SymbolConstants)Enum.ToObject(typeof(SymbolConstants), symbol.Id);
        }

        public static SymbolTerminal SymbolTerminal(this SymbolConstants symbol, string name = "")
        {
            return new SymbolTerminal((int)symbol, !string.IsNullOrEmpty(name) ? name : symbol.ToString());
        }

        public static TerminalToken TerminalToken(this IEnumerable<TerminalToken> tokens, SymbolConstants symbol)
        {
            return tokens
                    .First(t => t.Symbol.SymbolEnum() == symbol);
        }

        public static TerminalToken TerminalToken(this IEnumerable<TerminalToken> tokens, SymbolConstants symbol, int index)
        {
            return tokens
                    .Where(t => t.Symbol.SymbolEnum() == symbol)
                    .ElementAt(index);
        }
    }

    [Serializable()]
    public class SymbolException : System.Exception
    {
        public SymbolException(string message) : base(message)
        {
        }

        public SymbolException(string message,
            Exception inner) : base(message, inner)
        {
        }

        protected SymbolException(SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

    }

    [Serializable()]
    public class RuleException : System.Exception
    {

        public RuleException(string message) : base(message)
        {
        }

        public RuleException(string message,
                             Exception inner) : base(message, inner)
        {
        }

        protected RuleException(SerializationInfo info,
                                StreamingContext context) : base(info, context)
        {
        }

    }

    public class ParseResults
    {
        public bool isAccepted { get; set; }
        public string Error { get; set; }

        public List<TerminalToken> Tokens { get; private set; }

        public ParseResults()
        {
            Tokens = new List<TerminalToken>();
        }

        public override string ToString()
        {
            var value = string.Empty;
            Tokens.ForEach(s => value += "_" + s.Symbol.Id.ToString());

            return value;
        }
    }

    public enum SymbolConstants : int
    {
        SYMBOL_EOF = 0, // (EOF)
        SYMBOL_ERROR = 1, // (Error)
        SYMBOL_WHITESPACE = 2, // Whitespace
        SYMBOL_MINUS = 3, // '-'
        SYMBOL_LPAREN = 4, // '('
        SYMBOL_RPAREN = 5, // ')'
        SYMBOL_COMMA = 6, // ','
        SYMBOL_LT = 7, // '<'
        SYMBOL_LTEQ = 8, // '<='
        SYMBOL_LTGT = 9, // '<>'
        SYMBOL_EQ = 10, // '='
        SYMBOL_GT = 11, // '>'
        SYMBOL_GTEQ = 12, // '>='
        SYMBOL_AND = 13, // and
        SYMBOL_AS = 14, // as
        SYMBOL_BACK = 15, // back
        SYMBOL_BETWEEN = 16, // between
        SYMBOL_CATALOG = 17, // catalog
        SYMBOL_CATALOGS = 18, // catalogs
        SYMBOL_CHILD = 19, // child
        SYMBOL_CLONE = 20, // clone
        SYMBOL_COLUMNS = 21, // columns
        SYMBOL_DECIMAL = 22, // Decimal
        SYMBOL_DEFAULT = 23, // default
        SYMBOL_EXPORT = 24, // export
        SYMBOL_HELP = 25, // help
        SYMBOL_HOME = 26, // home
        SYMBOL_HTML = 27, // html
        SYMBOL_IDENTIFIER = 28, // Identifier
        SYMBOL_IMPORT = 29, // import
        SYMBOL_INTEGER = 30, // Integer
        SYMBOL_IS = 31, // is
        SYMBOL_JSON = 32, // json
        SYMBOL_LIKE = 33, // like
        SYMBOL_LOAD = 34, // load
        SYMBOL_NEWLINE = 35, // NewLine
        SYMBOL_NOCHILD = 36, // nochild
        SYMBOL_NOT = 37, // not
        SYMBOL_NULL = 38, // null
        SYMBOL_ON = 39, // on
        SYMBOL_PASSWORD = 40, // password
        SYMBOL_QUERY = 41, // query
        SYMBOL_REFRESH = 42, // refresh
        SYMBOL_RELATE = 43, // relate
        SYMBOL_REMOVE = 44, // remove
        SYMBOL_ROW = 45, // row
        SYMBOL_RUN = 46, // run
        SYMBOL_SERVER = 47, // server
        SYMBOL_SQL = 48, // sql
        SYMBOL_STRINGLITERAL = 49, // StringLiteral
        SYMBOL_TABLE = 50, // table
        SYMBOL_TABLES = 51, // tables
        SYMBOL_TEMPLATE = 52, // template
        SYMBOL_TO = 53, // to
        SYMBOL_TOP = 54, // top
        SYMBOL_TRANSFORM = 55, // transform
        SYMBOL_UNRELATE = 56, // unrelate
        SYMBOL_USER = 57, // user
        SYMBOL_WHERE = 58, // where
        SYMBOL_WITH = 59, // with
        SYMBOL_XML = 60, // xml
        SYMBOL_ASEXP = 61, // <AsExp>
        SYMBOL_BACKEXP = 62, // <BackExp>
        SYMBOL_CATALOGEXP = 63, // <CatalogExp>
        SYMBOL_CATALOGSEXP = 64, // <CatalogsExp>
        SYMBOL_CHILDEXP = 65, // <ChildExp>
        SYMBOL_CLONEEXP = 66, // <CloneExp>
        SYMBOL_COLUMNSEXP = 67, // <ColumnsExp>
        SYMBOL_COMMANDEXP = 68, // <CommandExp>
        SYMBOL_EXPORTEXP = 69, // <ExportExp>
        SYMBOL_EXPRESSION = 70, // <Expression>
        SYMBOL_HELPEXP = 71, // <HelpExp>
        SYMBOL_IMPORTEXP = 72, // <ImportExp>
        SYMBOL_LOADEXP = 73, // <LoadExp>
        SYMBOL_NEGATEEXP = 74, // <Negate Exp>
        SYMBOL_NL = 75, // <nl>
        SYMBOL_NLOPT = 76, // <nl Opt>
        SYMBOL_PARAMEXPRESSION = 77, // <ParamExpression>
        SYMBOL_PARAMEXPRESSION2 = 78, // <ParamExpression2>
        SYMBOL_PARAMEXPRESSION3 = 79, // <ParamExpression3>
        SYMBOL_PARAMEXPRESSION4 = 80, // <ParamExpression4>
        SYMBOL_PARAMEXPRESSION5 = 81, // <ParamExpression5>
        SYMBOL_PARAMVALUE = 82, // <ParamValue>
        SYMBOL_PASSWORDEXP = 83, // <PasswordExp>
        SYMBOL_PVALUE = 84, // <PValue>
        SYMBOL_QUERYEXP = 85, // <QueryExp>
        SYMBOL_REFRESHEXP = 86, // <RefreshExp>
        SYMBOL_RELATEEXP = 87, // <RelateExp>
        SYMBOL_REMOVEEXP = 88, // <RemoveExp>
        SYMBOL_ROOTEXP = 89, // <RootExp>
        SYMBOL_RUNQUERYEXP = 90, // <RunQueryExp>
        SYMBOL_SERVEREXP = 91, // <ServerExp>
        SYMBOL_START = 92, // <Start>
        SYMBOL_TABLEEXP = 93, // <TableExp>
        SYMBOL_TABLESEXP = 94, // <TablesExp>
        SYMBOL_TOPNEXP = 95, // <TopnExp>
        SYMBOL_TRANSFORMEXP = 96, // <TransformExp>
        SYMBOL_UNRELATEEXP = 97, // <UnrelateExp>
        SYMBOL_USEREXP = 98, // <UserExp>
        SYMBOL_VALUE = 99  // <Value>
    };

    public enum RuleConstants : int
    {
        RULE_NL_NEWLINE = 0, // <nl> ::= NewLine <nl>
        RULE_NL_NEWLINE2 = 1, // <nl> ::= NewLine
        RULE_NLOPT_NEWLINE = 2, // <nl Opt> ::= NewLine <nl Opt>
        RULE_NLOPT = 3, // <nl Opt> ::= 
        RULE_START = 4, // <Start> ::= <nl Opt> <CommandExp> <nl Opt>
        RULE_COMMANDEXP = 5, // <CommandExp> ::= <ImportExp>
        RULE_COMMANDEXP2 = 6, // <CommandExp> ::= <CloneExp>
        RULE_COMMANDEXP3 = 7, // <CommandExp> ::= <RemoveExp>
        RULE_COMMANDEXP4 = 8, // <CommandExp> ::= <RefreshExp>
        RULE_COMMANDEXP5 = 9, // <CommandExp> ::= <LoadExp>
        RULE_COMMANDEXP6 = 10, // <CommandExp> ::= <TableExp>
        RULE_COMMANDEXP7 = 11, // <CommandExp> ::= <RelateExp>
        RULE_COMMANDEXP8 = 12, // <CommandExp> ::= <UnrelateExp>
        RULE_COMMANDEXP9 = 13, // <CommandExp> ::= <ExportExp>
        RULE_COMMANDEXP10 = 14, // <CommandExp> ::= <BackExp>
        RULE_COMMANDEXP11 = 15, // <CommandExp> ::= <RootExp>
        RULE_COMMANDEXP12 = 16, // <CommandExp> ::= <TablesExp>
        RULE_COMMANDEXP13 = 17, // <CommandExp> ::= <CatalogsExp>
        RULE_COMMANDEXP14 = 18, // <CommandExp> ::= <ColumnsExp>
        RULE_COMMANDEXP15 = 19, // <CommandExp> ::= <TopnExp>
        RULE_COMMANDEXP16 = 20, // <CommandExp> ::= <ChildExp>
        RULE_COMMANDEXP17 = 21, // <CommandExp> ::= <HelpExp>
        RULE_COMMANDEXP18 = 22, // <CommandExp> ::= <QueryExp>
        RULE_COMMANDEXP19 = 23, // <CommandExp> ::= <RunQueryExp>
        RULE_COMMANDEXP20 = 24, // <CommandExp> ::= <TransformExp>
        RULE_IMPORTEXP_IMPORT = 25, // <ImportExp> ::= import <CatalogExp>
        RULE_IMPORTEXP_IMPORT2 = 26, // <ImportExp> ::= import <CatalogExp> <UserExp> <PasswordExp>
        RULE_IMPORTEXP_IMPORT3 = 27, // <ImportExp> ::= import <CatalogExp> <ServerExp> <UserExp> <PasswordExp>
        RULE_CLONEEXP_CLONE = 28, // <CloneExp> ::= clone
        RULE_CLONEEXP_CLONE2 = 29, // <CloneExp> ::= clone <CatalogExp>
        RULE_CLONEEXP_CLONE3 = 30, // <CloneExp> ::= clone <AsExp>
        RULE_CLONEEXP_CLONE4 = 31, // <CloneExp> ::= clone <CatalogExp> <AsExp>
        RULE_REMOVEEXP_REMOVE = 32, // <RemoveExp> ::= remove
        RULE_REMOVEEXP_REMOVE2 = 33, // <RemoveExp> ::= remove <CatalogExp>
        RULE_REFRESHEXP_REFRESH = 34, // <RefreshExp> ::= refresh
        RULE_REFRESHEXP_REFRESH2 = 35, // <RefreshExp> ::= refresh <CatalogExp>
        RULE_LOADEXP_LOAD = 36, // <LoadExp> ::= load
        RULE_LOADEXP_LOAD2 = 37, // <LoadExp> ::= load <CatalogExp>
        RULE_LOADEXP_LOAD_DEFAULT = 38, // <LoadExp> ::= load <CatalogExp> default
        RULE_TABLEEXP_TABLE = 39, // <TableExp> ::= table
        RULE_TABLEEXP_TABLE_STRINGLITERAL = 40, // <TableExp> ::= table Identifier
        RULE_TABLEEXP_TABLE_STRINGLITERAL_DEFAULT = 41, // <TableExp> ::= table Identifier default
        RULE_TABLEEXP_TABLE_STRINGLITERAL_DEFAULT_WHERE = 42, // <TableExp> ::= table Identifier default where <Expression>
        RULE_TABLEEXP_TABLE_STRINGLITERAL_WHERE = 43, // <TableExp> ::= table Identifier where <Expression>
        RULE_RELATEEXP_RELATE_TO_STRINGLITERAL_ON_STRINGLITERAL_EQ_STRINGLITERAL = 44, // <RelateExp> ::= relate to Identifier on Identifier '=' Identifier
        RULE_RELATEEXP_RELATE_STRINGLITERAL_TO_STRINGLITERAL_ON_STRINGLITERAL_EQ_STRINGLITERAL = 45, // <RelateExp> ::= relate Identifier to Identifier on Identifier '=' Identifier
        RULE_UNRELATEEXP_UNRELATE_TO_STRINGLITERAL = 46, // <UnrelateExp> ::= unrelate to Identifier
        RULE_UNRELATEEXP_UNRELATE_STRINGLITERAL_TO_STRINGLITERAL = 47, // <UnrelateExp> ::= unrelate Identifier to Identifier
        RULE_EXPORTEXP_EXPORT_AS_SQL = 48, // <ExportExp> ::= export as sql
        RULE_EXPORTEXP_EXPORT_AS_HTML = 49, // <ExportExp> ::= export as html
        RULE_EXPORTEXP_EXPORT_AS_JSON = 50, // <ExportExp> ::= export as json
        RULE_EXPORTEXP_EXPORT_AS_XML = 51, // <ExportExp> ::= export as xml
        RULE_EXPORTEXP_EXPORT_STRINGLITERAL_AS_SQL = 52, // <ExportExp> ::= export Identifier as sql
        RULE_EXPORTEXP_EXPORT_STRINGLITERAL_AS_HTML = 53, // <ExportExp> ::= export Identifier as html
        RULE_EXPORTEXP_EXPORT_STRINGLITERAL_AS_JSON = 54, // <ExportExp> ::= export Identifier as json
        RULE_EXPORTEXP_EXPORT_STRINGLITERAL_AS_XML = 55, // <ExportExp> ::= export Identifier as xml
        RULE_EXPORTEXP_EXPORT_AS_SQL_NOCHILD = 56, // <ExportExp> ::= export as sql nochild
        RULE_EXPORTEXP_EXPORT_AS_HTML_NOCHILD = 57, // <ExportExp> ::= export as html nochild
        RULE_EXPORTEXP_EXPORT_AS_JSON_NOCHILD = 58, // <ExportExp> ::= export as json nochild
        RULE_EXPORTEXP_EXPORT_AS_XML_NOCHILD = 59, // <ExportExp> ::= export as xml nochild
        RULE_EXPORTEXP_EXPORT_STRINGLITERAL_AS_SQL_NOCHILD = 60, // <ExportExp> ::= export Identifier as sql nochild
        RULE_EXPORTEXP_EXPORT_STRINGLITERAL_AS_HTML_NOCHILD = 61, // <ExportExp> ::= export Identifier as html nochild
        RULE_EXPORTEXP_EXPORT_STRINGLITERAL_AS_JSON_NOCHILD = 62, // <ExportExp> ::= export Identifier as json nochild
        RULE_EXPORTEXP_EXPORT_STRINGLITERAL_AS_XML_NOCHILD = 63, // <ExportExp> ::= export Identifier as xml nochild
        RULE_BACKEXP_BACK = 64, // <BackExp> ::= back
        RULE_ROOTEXP_HOME = 65, // <RootExp> ::= home
        RULE_TABLESEXP_TABLES = 66, // <TablesExp> ::= tables
        RULE_TABLESEXP_TABLES_INTEGER = 67, // <TablesExp> ::= tables Integer
        RULE_CATALOGSEXP_CATALOGS = 68, // <CatalogsExp> ::= catalogs
        RULE_CATALOGSEXP_CATALOGS_INTEGER = 69, // <CatalogsExp> ::= catalogs Integer
        RULE_COLUMNSEXP_COLUMNS = 70, // <ColumnsExp> ::= columns
        RULE_COLUMNSEXP_COLUMNS_INTEGER = 71, // <ColumnsExp> ::= columns Integer
        RULE_TOPNEXP_TOP_INTEGER = 72, // <TopnExp> ::= top Integer
        RULE_CHILDEXP_CHILD = 73, // <ChildExp> ::= child
        RULE_CHILDEXP_CHILD_INTEGER = 74, // <ChildExp> ::= child Integer
        RULE_CHILDEXP_CHILD_STRINGLITERAL = 75, // <ChildExp> ::= child Identifier
        RULE_CATALOGEXP_CATALOG_STRINGLITERAL = 76, // <CatalogExp> ::= catalog Identifier
        RULE_USEREXP_USER_STRINGLITERAL = 77, // <UserExp> ::= user Identifier
        RULE_PASSWORDEXP_PASSWORD_STRINGLITERAL = 78, // <PasswordExp> ::= password Identifier
        RULE_SERVEREXP_SERVER_STRINGLITERAL = 79, // <ServerExp> ::= server Identifier
        RULE_ASEXP_AS_STRINGLITERAL = 80, // <AsExp> ::= as Identifier
        RULE_HELPEXP_HELP = 81, // <HelpExp> ::= help
        RULE_QUERYEXP_QUERY_STRINGLITERAL = 82, // <QueryExp> ::= query Identifier
        RULE_QUERYEXP_QUERY_STRINGLITERAL_ROW_INTEGER = 83, // <QueryExp> ::= query Identifier row Integer
        RULE_TRANSFORMEXP_TRANSFORM = 84, // <TransformExp> ::= transform
        RULE_TRANSFORMEXP_TRANSFORM_TEMPLATE_STRINGLITERAL = 85, // <TransformExp> ::= transform template Identifier
        RULE_TRANSFORMEXP_TRANSFORM_STRINGLITERAL = 86, // <TransformExp> ::= transform Identifier
        RULE_TRANSFORMEXP_TRANSFORM_STRINGLITERAL_TEMPLATE_STRINGLITERAL = 87, // <TransformExp> ::= transform Identifier template Identifier
        RULE_RUNQUERYEXP_RUN_STRINGLITERAL = 88, // <RunQueryExp> ::= run Identifier
        RULE_RUNQUERYEXP_RUN_STRINGLITERAL_WITH = 89, // <RunQueryExp> ::= run Identifier with <ParamExpression>
        RULE_RUNQUERYEXP_RUN_STRINGLITERAL_WITH_COMMA = 90, // <RunQueryExp> ::= run Identifier with <ParamExpression> ',' <ParamExpression2>
        RULE_RUNQUERYEXP_RUN_STRINGLITERAL_WITH_COMMA_COMMA = 91, // <RunQueryExp> ::= run Identifier with <ParamExpression> ',' <ParamExpression2> ',' <ParamExpression3>
        RULE_RUNQUERYEXP_RUN_STRINGLITERAL_WITH_COMMA_COMMA_COMMA = 92, // <RunQueryExp> ::= run Identifier with <ParamExpression> ',' <ParamExpression2> ',' <ParamExpression3> ',' <ParamExpression4>
        RULE_RUNQUERYEXP_RUN_STRINGLITERAL_WITH_COMMA_COMMA_COMMA_COMMA = 93, // <RunQueryExp> ::= run Identifier with <ParamExpression> ',' <ParamExpression2> ',' <ParamExpression3> ',' <ParamExpression4> ',' <ParamExpression5>
        RULE_PARAMEXPRESSION_EQ = 94, // <ParamExpression> ::= <ParamExpression> '=' <ParamValue>
        RULE_PARAMEXPRESSION = 95, // <ParamExpression> ::= <ParamValue>
        RULE_PARAMEXPRESSION2_EQ = 96, // <ParamExpression2> ::= <ParamExpression2> '=' <ParamValue>
        RULE_PARAMEXPRESSION2 = 97, // <ParamExpression2> ::= <ParamValue>
        RULE_PARAMEXPRESSION3_EQ = 98, // <ParamExpression3> ::= <ParamExpression3> '=' <ParamValue>
        RULE_PARAMEXPRESSION3 = 99, // <ParamExpression3> ::= <ParamValue>
        RULE_PARAMEXPRESSION4_EQ = 100, // <ParamExpression4> ::= <ParamExpression4> '=' <ParamValue>
        RULE_PARAMEXPRESSION4 = 101, // <ParamExpression4> ::= <ParamValue>
        RULE_PARAMEXPRESSION5_EQ = 102, // <ParamExpression5> ::= <ParamExpression5> '=' <ParamValue>
        RULE_PARAMEXPRESSION5 = 103, // <ParamExpression5> ::= <ParamValue>
        RULE_PARAMVALUE_MINUS = 104, // <ParamValue> ::= '-' <PValue>
        RULE_PARAMVALUE = 105, // <ParamValue> ::= <PValue>
        RULE_PVALUE_IDENTIFIER = 106, // <PValue> ::= Identifier
        RULE_PVALUE_NULL = 107, // <PValue> ::= null
        RULE_PVALUE_INTEGER = 108, // <PValue> ::= Integer
        RULE_PVALUE_DECIMAL = 109, // <PValue> ::= Decimal
        RULE_PVALUE_STRINGLITERAL = 110, // <PValue> ::= StringLiteral
        RULE_EXPRESSION_GT = 111, // <Expression> ::= <Expression> '>' <Negate Exp>
        RULE_EXPRESSION_LT = 112, // <Expression> ::= <Expression> '<' <Negate Exp>
        RULE_EXPRESSION_LTEQ = 113, // <Expression> ::= <Expression> '<=' <Negate Exp>
        RULE_EXPRESSION_GTEQ = 114, // <Expression> ::= <Expression> '>=' <Negate Exp>
        RULE_EXPRESSION_EQ = 115, // <Expression> ::= <Expression> '=' <Negate Exp>
        RULE_EXPRESSION_LTGT = 116, // <Expression> ::= <Expression> '<>' <Negate Exp>
        RULE_EXPRESSION_IS = 117, // <Expression> ::= <Expression> is <Negate Exp>
        RULE_EXPRESSION_NOT_BETWEEN_AND = 118, // <Expression> ::= <Expression> not between <Negate Exp> and <Negate Exp>
        RULE_EXPRESSION_BETWEEN_AND = 119, // <Expression> ::= <Expression> between <Negate Exp> and <Negate Exp>
        RULE_EXPRESSION_LIKE_STRINGLITERAL = 120, // <Expression> ::= <Expression> like StringLiteral
        RULE_EXPRESSION = 121, // <Expression> ::= <Negate Exp>
        RULE_NEGATEEXP_MINUS = 122, // <Negate Exp> ::= '-' <Value>
        RULE_NEGATEEXP_NOT = 123, // <Negate Exp> ::= not <Value>
        RULE_NEGATEEXP = 124, // <Negate Exp> ::= <Value>
        RULE_VALUE_IDENTIFIER = 125, // <Value> ::= Identifier
        RULE_VALUE_LPAREN_RPAREN = 126, // <Value> ::= '(' <Expression> ')'
        RULE_VALUE_NULL = 127, // <Value> ::= null
        RULE_VALUE_INTEGER = 128, // <Value> ::= Integer
        RULE_VALUE_DECIMAL = 129, // <Value> ::= Decimal
        RULE_VALUE_STRINGLITERAL = 130  // <Value> ::= StringLiteral
    };

    public class RRParser
    {
        private LALRParser parser;
        private readonly ParseResults _results;

        public RRParser(string filename)
        {
            _results = new ParseResults();
            FileStream stream = new FileStream(filename,
                                               FileMode.Open, 
                                               FileAccess.Read, 
                                               FileShare.Read);
            Init(stream);
            stream.Close();
        }

        public RRParser(string baseName, string resourceName)
        {
            byte[] buffer = ResourceUtil.GetByteArrayResource(
                System.Reflection.Assembly.GetExecutingAssembly(),
                baseName,
                resourceName);
            MemoryStream stream = new MemoryStream(buffer);
            Init(stream);
            stream.Close();
        }

        public RRParser(Stream stream)
        {
            Init(stream);
        }

        private void Init(Stream stream)
        {
            CGTReader reader = new CGTReader(stream);
            parser = reader.CreateNewParser();
            parser.TrimReductions = false;
            parser.StoreTokens = LALRParser.StoreTokensMode.NoUserObject;

            parser.OnReduce += new LALRParser.ReduceHandler(ReduceEvent);
            parser.OnTokenRead += new LALRParser.TokenReadHandler(TokenReadEvent);
            parser.OnAccept += new LALRParser.AcceptHandler(AcceptEvent);
            parser.OnTokenError += new LALRParser.TokenErrorHandler(TokenErrorEvent);
            parser.OnParseError += new LALRParser.ParseErrorHandler(ParseErrorEvent);
        }

        public ParseResults Parse(string source)
        {
            _results.isAccepted = false;
            _results.Error = string.Empty;          
            _results.Tokens.Clear();

            parser.Parse(source);

            if(_results.isAccepted)
            {
                //remove EOF token
                _results.Tokens
                    .Remove(_results.Tokens.Last());
            }

            return _results;
        }

        private void TokenReadEvent(LALRParser parser, TokenReadEventArgs args)
        {
            try
            {
                args.Token.UserObject = CreateObject(args.Token);
                _results.Tokens.Add(args.Token);
            }
            catch (Exception e)
            {
                _results.Error += e.Message;
                args.Continue = false;
            }
        }

        private void ReduceEvent(LALRParser parser, ReduceEventArgs args)
        {
            try
            {
                args.Token.UserObject = CreateObject(args.Token);
            }
            catch (Exception e)
            {
                _results.Error += e.Message;
                args.Continue = false;
            }
        }

        private void AcceptEvent(LALRParser parser, AcceptEventArgs args)
        {
            _results.isAccepted = true;
        }

        private void TokenErrorEvent(LALRParser parser, TokenErrorEventArgs args)
        {
            _results.Error += "Token error with input: '" + args.Token.ToString() + "'";
        }

        private void ParseErrorEvent(LALRParser parser, ParseErrorEventArgs args)
        {
            _results.Error = "Parse error caused by token: '" + args.UnexpectedToken.ToString() + "'";
        }

        private Object CreateObject(TerminalToken token)
        {
            switch (token.Symbol.Id)
            {
                case (int)SymbolConstants.SYMBOL_EOF:
                    //(EOF)
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_ERROR:
                    //(Error)
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_WHITESPACE:
                    //Whitespace
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_MINUS:
                    //'-'
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_LPAREN:
                    //'('
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_RPAREN:
                    //')'
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_COMMA:
                    //','
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_LT:
                    //'<'
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_LTEQ:
                    //'<='
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_LTGT:
                    //'<>'
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_EQ:
                    //'='
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_GT:
                    //'>'
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_GTEQ:
                    //'>='
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_AND:
                    //and
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_AS:
                    //as
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_BACK:
                    //back
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_BETWEEN:
                    //between
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_CATALOG:
                    //catalog
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_CATALOGS:
                    //catalogs
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_CHILD:
                    //child
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_CLONE:
                    //clone
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_COLUMNS:
                    //columns
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_DECIMAL:
                    //Decimal
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_DEFAULT:
                    //default
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_EXPORT:
                    //export
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_HELP:
                    //help
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_HOME:
                    //home
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_HTML:
                    //html
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_IDENTIFIER:
                    //Identifier
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_IMPORT:
                    //import
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_INTEGER:
                    //Integer
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_IS:
                    //is
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_JSON:
                    //json
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_LIKE:
                    //like
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_LOAD:
                    //load
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_NEWLINE:
                    //NewLine
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_NOCHILD:
                    //nochild
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_NOT:
                    //not
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_NULL:
                    //null
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_ON:
                    //on
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_PASSWORD:
                    //password
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_QUERY:
                    //query
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_REFRESH:
                    //refresh
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_RELATE:
                    //relate
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_REMOVE:
                    //remove
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_ROW:
                    //row
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_RUN:
                    //run
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_SERVER:
                    //server
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_SQL:
                    //sql
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_STRINGLITERAL:
                    //StringLiteral
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_TABLE:
                    //table
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_TABLES:
                    //tables
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_TEMPLATE:
                    //template
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_TO:
                    //to
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_TOP:
                    //top
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_TRANSFORM:
                    //transform
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_UNRELATE:
                    //unrelate
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_USER:
                    //user
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_WHERE:
                    //where
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_WITH:
                    //with
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_XML:
                    //xml
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_ASEXP:
                    //<AsExp>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_BACKEXP:
                    //<BackExp>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_CATALOGEXP:
                    //<CatalogExp>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_CATALOGSEXP:
                    //<CatalogsExp>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_CHILDEXP:
                    //<ChildExp>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_CLONEEXP:
                    //<CloneExp>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_COLUMNSEXP:
                    //<ColumnsExp>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_COMMANDEXP:
                    //<CommandExp>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_EXPORTEXP:
                    //<ExportExp>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_EXPRESSION:
                    //<Expression>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_HELPEXP:
                    //<HelpExp>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_IMPORTEXP:
                    //<ImportExp>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_LOADEXP:
                    //<LoadExp>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_NEGATEEXP:
                    //<Negate Exp>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_NL:
                    //<nl>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_NLOPT:
                    //<nl Opt>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_PARAMEXPRESSION:
                    //<ParamExpression>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_PARAMEXPRESSION2:
                    //<ParamExpression2>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_PARAMEXPRESSION3:
                    //<ParamExpression3>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_PARAMEXPRESSION4:
                    //<ParamExpression4>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_PARAMEXPRESSION5:
                    //<ParamExpression5>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_PARAMVALUE:
                    //<ParamValue>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_PASSWORDEXP:
                    //<PasswordExp>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_PVALUE:
                    //<PValue>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_QUERYEXP:
                    //<QueryExp>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_REFRESHEXP:
                    //<RefreshExp>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_RELATEEXP:
                    //<RelateExp>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_REMOVEEXP:
                    //<RemoveExp>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_ROOTEXP:
                    //<RootExp>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_RUNQUERYEXP:
                    //<RunQueryExp>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_SERVEREXP:
                    //<ServerExp>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_START:
                    //<Start>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_TABLEEXP:
                    //<TableExp>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_TABLESEXP:
                    //<TablesExp>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_TOPNEXP:
                    //<TopnExp>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_TRANSFORMEXP:
                    //<TransformExp>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_UNRELATEEXP:
                    //<UnrelateExp>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_USEREXP:
                    //<UserExp>
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_VALUE:
                    //<Value>
                    //todo: Create a new object that corresponds to the symbol
                    return null;
            }
            //throw new SymbolException("Unknown symbol");
            return null;
        }

        public static Object CreateObject(NonterminalToken token)
        {
            switch (token.Rule.Id)
            {
                case (int)RuleConstants.RULE_NL_NEWLINE:
                    //<nl> ::= NewLine <nl>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_NL_NEWLINE2:
                    //<nl> ::= NewLine
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_NLOPT_NEWLINE:
                    //<nl Opt> ::= NewLine <nl Opt>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_NLOPT:
                    //<nl Opt> ::= 
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_START:
                    //<Start> ::= <nl Opt> <CommandExp> <nl Opt>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_COMMANDEXP:
                    //<CommandExp> ::= <ImportExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_COMMANDEXP2:
                    //<CommandExp> ::= <CloneExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_COMMANDEXP3:
                    //<CommandExp> ::= <RemoveExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_COMMANDEXP4:
                    //<CommandExp> ::= <RefreshExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_COMMANDEXP5:
                    //<CommandExp> ::= <LoadExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_COMMANDEXP6:
                    //<CommandExp> ::= <TableExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_COMMANDEXP7:
                    //<CommandExp> ::= <RelateExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_COMMANDEXP8:
                    //<CommandExp> ::= <UnrelateExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_COMMANDEXP9:
                    //<CommandExp> ::= <ExportExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_COMMANDEXP10:
                    //<CommandExp> ::= <BackExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_COMMANDEXP11:
                    //<CommandExp> ::= <RootExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_COMMANDEXP12:
                    //<CommandExp> ::= <TablesExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_COMMANDEXP13:
                    //<CommandExp> ::= <CatalogsExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_COMMANDEXP14:
                    //<CommandExp> ::= <ColumnsExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_COMMANDEXP15:
                    //<CommandExp> ::= <TopnExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_COMMANDEXP16:
                    //<CommandExp> ::= <ChildExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_COMMANDEXP17:
                    //<CommandExp> ::= <HelpExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_COMMANDEXP18:
                    //<CommandExp> ::= <QueryExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_COMMANDEXP19:
                    //<CommandExp> ::= <RunQueryExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_COMMANDEXP20:
                    //<CommandExp> ::= <TransformExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_IMPORTEXP_IMPORT:
                    //<ImportExp> ::= import <CatalogExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_IMPORTEXP_IMPORT2:
                    //<ImportExp> ::= import <CatalogExp> <UserExp> <PasswordExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_IMPORTEXP_IMPORT3:
                    //<ImportExp> ::= import <CatalogExp> <ServerExp> <UserExp> <PasswordExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_CLONEEXP_CLONE:
                    //<CloneExp> ::= clone
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_CLONEEXP_CLONE2:
                    //<CloneExp> ::= clone <CatalogExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_CLONEEXP_CLONE3:
                    //<CloneExp> ::= clone <AsExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_CLONEEXP_CLONE4:
                    //<CloneExp> ::= clone <CatalogExp> <AsExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_REMOVEEXP_REMOVE:
                    //<RemoveExp> ::= remove
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_REMOVEEXP_REMOVE2:
                    //<RemoveExp> ::= remove <CatalogExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_REFRESHEXP_REFRESH:
                    //<RefreshExp> ::= refresh
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_REFRESHEXP_REFRESH2:
                    //<RefreshExp> ::= refresh <CatalogExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_LOADEXP_LOAD:
                    //<LoadExp> ::= load
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_LOADEXP_LOAD2:
                    //<LoadExp> ::= load <CatalogExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_LOADEXP_LOAD_DEFAULT:
                    //<LoadExp> ::= load <CatalogExp> default
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_TABLEEXP_TABLE:
                    //<TableExp> ::= table
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_TABLEEXP_TABLE_STRINGLITERAL:
                    //<TableExp> ::= table Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_TABLEEXP_TABLE_STRINGLITERAL_DEFAULT:
                    //<TableExp> ::= table Identifier default
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_TABLEEXP_TABLE_STRINGLITERAL_DEFAULT_WHERE:
                    //<TableExp> ::= table Identifier default where <Expression>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_TABLEEXP_TABLE_STRINGLITERAL_WHERE:
                    //<TableExp> ::= table Identifier where <Expression>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_RELATEEXP_RELATE_TO_STRINGLITERAL_ON_STRINGLITERAL_EQ_STRINGLITERAL:
                    //<RelateExp> ::= relate to Identifier on Identifier '=' Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_RELATEEXP_RELATE_STRINGLITERAL_TO_STRINGLITERAL_ON_STRINGLITERAL_EQ_STRINGLITERAL:
                    //<RelateExp> ::= relate Identifier to Identifier on Identifier '=' Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_UNRELATEEXP_UNRELATE_TO_STRINGLITERAL:
                    //<UnrelateExp> ::= unrelate to Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_UNRELATEEXP_UNRELATE_STRINGLITERAL_TO_STRINGLITERAL:
                    //<UnrelateExp> ::= unrelate Identifier to Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_AS_SQL:
                    //<ExportExp> ::= export as sql
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_AS_HTML:
                    //<ExportExp> ::= export as html
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_AS_JSON:
                    //<ExportExp> ::= export as json
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_AS_XML:
                    //<ExportExp> ::= export as xml
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_STRINGLITERAL_AS_SQL:
                    //<ExportExp> ::= export Identifier as sql
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_STRINGLITERAL_AS_HTML:
                    //<ExportExp> ::= export Identifier as html
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_STRINGLITERAL_AS_JSON:
                    //<ExportExp> ::= export Identifier as json
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_STRINGLITERAL_AS_XML:
                    //<ExportExp> ::= export Identifier as xml
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_AS_SQL_NOCHILD:
                    //<ExportExp> ::= export as sql nochild
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_AS_HTML_NOCHILD:
                    //<ExportExp> ::= export as html nochild
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_AS_JSON_NOCHILD:
                    //<ExportExp> ::= export as json nochild
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_AS_XML_NOCHILD:
                    //<ExportExp> ::= export as xml nochild
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_STRINGLITERAL_AS_SQL_NOCHILD:
                    //<ExportExp> ::= export Identifier as sql nochild
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_STRINGLITERAL_AS_HTML_NOCHILD:
                    //<ExportExp> ::= export Identifier as html nochild
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_STRINGLITERAL_AS_JSON_NOCHILD:
                    //<ExportExp> ::= export Identifier as json nochild
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_STRINGLITERAL_AS_XML_NOCHILD:
                    //<ExportExp> ::= export Identifier as xml nochild
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_BACKEXP_BACK:
                    //<BackExp> ::= back
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_ROOTEXP_HOME:
                    //<RootExp> ::= home
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_TABLESEXP_TABLES:
                    //<TablesExp> ::= tables
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_TABLESEXP_TABLES_INTEGER:
                    //<TablesExp> ::= tables Integer
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_CATALOGSEXP_CATALOGS:
                    //<CatalogsExp> ::= catalogs
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_CATALOGSEXP_CATALOGS_INTEGER:
                    //<CatalogsExp> ::= catalogs Integer
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_COLUMNSEXP_COLUMNS:
                    //<ColumnsExp> ::= columns
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_COLUMNSEXP_COLUMNS_INTEGER:
                    //<ColumnsExp> ::= columns Integer
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_TOPNEXP_TOP_INTEGER:
                    //<TopnExp> ::= top Integer
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_CHILDEXP_CHILD:
                    //<ChildExp> ::= child
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_CHILDEXP_CHILD_INTEGER:
                    //<ChildExp> ::= child Integer
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_CHILDEXP_CHILD_STRINGLITERAL:
                    //<ChildExp> ::= child Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_CATALOGEXP_CATALOG_STRINGLITERAL:
                    //<CatalogExp> ::= catalog Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_USEREXP_USER_STRINGLITERAL:
                    //<UserExp> ::= user Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_PASSWORDEXP_PASSWORD_STRINGLITERAL:
                    //<PasswordExp> ::= password Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_SERVEREXP_SERVER_STRINGLITERAL:
                    //<ServerExp> ::= server Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_ASEXP_AS_STRINGLITERAL:
                    //<AsExp> ::= as Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_HELPEXP_HELP:
                    //<HelpExp> ::= help
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_QUERYEXP_QUERY_STRINGLITERAL:
                    //<QueryExp> ::= query Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_QUERYEXP_QUERY_STRINGLITERAL_ROW_INTEGER:
                    //<QueryExp> ::= query Identifier row Integer
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_TRANSFORMEXP_TRANSFORM:
                    //<TransformExp> ::= transform
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_TRANSFORMEXP_TRANSFORM_TEMPLATE_STRINGLITERAL:
                    //<TransformExp> ::= transform template Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_TRANSFORMEXP_TRANSFORM_STRINGLITERAL:
                    //<TransformExp> ::= transform Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_TRANSFORMEXP_TRANSFORM_STRINGLITERAL_TEMPLATE_STRINGLITERAL:
                    //<TransformExp> ::= transform Identifier template Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_RUNQUERYEXP_RUN_STRINGLITERAL:
                    //<RunQueryExp> ::= run Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_RUNQUERYEXP_RUN_STRINGLITERAL_WITH:
                    //<RunQueryExp> ::= run Identifier with <ParamExpression>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_RUNQUERYEXP_RUN_STRINGLITERAL_WITH_COMMA:
                    //<RunQueryExp> ::= run Identifier with <ParamExpression> ',' <ParamExpression2>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_RUNQUERYEXP_RUN_STRINGLITERAL_WITH_COMMA_COMMA:
                    //<RunQueryExp> ::= run Identifier with <ParamExpression> ',' <ParamExpression2> ',' <ParamExpression3>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_RUNQUERYEXP_RUN_STRINGLITERAL_WITH_COMMA_COMMA_COMMA:
                    //<RunQueryExp> ::= run Identifier with <ParamExpression> ',' <ParamExpression2> ',' <ParamExpression3> ',' <ParamExpression4>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_RUNQUERYEXP_RUN_STRINGLITERAL_WITH_COMMA_COMMA_COMMA_COMMA:
                    //<RunQueryExp> ::= run Identifier with <ParamExpression> ',' <ParamExpression2> ',' <ParamExpression3> ',' <ParamExpression4> ',' <ParamExpression5>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_PARAMEXPRESSION_EQ:
                    //<ParamExpression> ::= <ParamExpression> '=' <ParamValue>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_PARAMEXPRESSION:
                    //<ParamExpression> ::= <ParamValue>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_PARAMEXPRESSION2_EQ:
                    //<ParamExpression2> ::= <ParamExpression2> '=' <ParamValue>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_PARAMEXPRESSION2:
                    //<ParamExpression2> ::= <ParamValue>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_PARAMEXPRESSION3_EQ:
                    //<ParamExpression3> ::= <ParamExpression3> '=' <ParamValue>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_PARAMEXPRESSION3:
                    //<ParamExpression3> ::= <ParamValue>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_PARAMEXPRESSION4_EQ:
                    //<ParamExpression4> ::= <ParamExpression4> '=' <ParamValue>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_PARAMEXPRESSION4:
                    //<ParamExpression4> ::= <ParamValue>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_PARAMEXPRESSION5_EQ:
                    //<ParamExpression5> ::= <ParamExpression5> '=' <ParamValue>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_PARAMEXPRESSION5:
                    //<ParamExpression5> ::= <ParamValue>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_PARAMVALUE_MINUS:
                    //<ParamValue> ::= '-' <PValue>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_PARAMVALUE:
                    //<ParamValue> ::= <PValue>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_PVALUE_IDENTIFIER:
                    //<PValue> ::= Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_PVALUE_NULL:
                    //<PValue> ::= null
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_PVALUE_INTEGER:
                    //<PValue> ::= Integer
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_PVALUE_DECIMAL:
                    //<PValue> ::= Decimal
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_PVALUE_STRINGLITERAL:
                    //<PValue> ::= StringLiteral
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPRESSION_GT:
                    //<Expression> ::= <Expression> '>' <Negate Exp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPRESSION_LT:
                    //<Expression> ::= <Expression> '<' <Negate Exp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPRESSION_LTEQ:
                    //<Expression> ::= <Expression> '<=' <Negate Exp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPRESSION_GTEQ:
                    //<Expression> ::= <Expression> '>=' <Negate Exp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPRESSION_EQ:
                    //<Expression> ::= <Expression> '=' <Negate Exp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPRESSION_LTGT:
                    //<Expression> ::= <Expression> '<>' <Negate Exp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPRESSION_IS:
                    //<Expression> ::= <Expression> is <Negate Exp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPRESSION_NOT_BETWEEN_AND:
                    //<Expression> ::= <Expression> not between <Negate Exp> and <Negate Exp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPRESSION_BETWEEN_AND:
                    //<Expression> ::= <Expression> between <Negate Exp> and <Negate Exp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPRESSION_LIKE_STRINGLITERAL:
                    //<Expression> ::= <Expression> like StringLiteral
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPRESSION:
                    //<Expression> ::= <Negate Exp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_NEGATEEXP_MINUS:
                    //<Negate Exp> ::= '-' <Value>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_NEGATEEXP_NOT:
                    //<Negate Exp> ::= not <Value>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_NEGATEEXP:
                    //<Negate Exp> ::= <Value>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_VALUE_IDENTIFIER:
                    //<Value> ::= Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_VALUE_LPAREN_RPAREN:
                    //<Value> ::= '(' <Expression> ')'
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_VALUE_NULL:
                    //<Value> ::= null
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_VALUE_INTEGER:
                    //<Value> ::= Integer
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_VALUE_DECIMAL:
                    //<Value> ::= Decimal
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_VALUE_STRINGLITERAL:
                    //<Value> ::= StringLiteral
                    //todo: Create a new object using the stored user objects.
                    return null;
            }
            //throw new RuleException("Unknown rule");
            return null;
        }
    }
}
