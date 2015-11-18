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
        SYMBOL_LT = 6, // '<'
        SYMBOL_LTEQ = 7, // '<='
        SYMBOL_LTGT = 8, // '<>'
        SYMBOL_EQ = 9, // '='
        SYMBOL_GT = 10, // '>'
        SYMBOL_GTEQ = 11, // '>='
        SYMBOL_AND = 12, // and
        SYMBOL_AS = 13, // as
        SYMBOL_BACK = 14, // back
        SYMBOL_BETWEEN = 15, // between
        SYMBOL_CATALOG = 16, // catalog
        SYMBOL_CHILD = 17, // child
        SYMBOL_CLONE = 18, // clone
        SYMBOL_COLUMNS = 19, // columns
        SYMBOL_DECIMAL = 20, // Decimal
        SYMBOL_DEFAULT = 21, // default
        SYMBOL_EXPORT = 22, // export
        SYMBOL_HELP = 23, // help
        SYMBOL_HTML = 24, // html
        SYMBOL_IDENTIFIER = 25, // Identifier
        SYMBOL_IMPORT = 26, // import
        SYMBOL_INTEGER = 27, // Integer
        SYMBOL_IS = 28, // is
        SYMBOL_JSON = 29, // json
        SYMBOL_LIKE = 30, // like
        SYMBOL_LOAD = 31, // load
        SYMBOL_NEWLINE = 32, // NewLine
        SYMBOL_NOT = 33, // not
        SYMBOL_NULL = 34, // null
        SYMBOL_ON = 35, // on
        SYMBOL_PASSWORD = 36, // password
        SYMBOL_REFRESH = 37, // refresh
        SYMBOL_RELATE = 38, // relate
        SYMBOL_REMOVE = 39, // remove
        SYMBOL_ROOT = 40, // root
        SYMBOL_SERVER = 41, // server
        SYMBOL_SQL = 42, // sql
        SYMBOL_STRINGLITERAL = 43, // StringLiteral
        SYMBOL_TABLE = 44, // table
        SYMBOL_TABLES = 45, // tables
        SYMBOL_TO = 46, // to
        SYMBOL_TOP = 47, // top
        SYMBOL_UNRELATE = 48, // unrelate
        SYMBOL_USER = 49, // user
        SYMBOL_WHERE = 50, // where
        SYMBOL_XML = 51, // xml
        SYMBOL_ASEXP = 52, // <AsExp>
        SYMBOL_BACKEXP = 53, // <BackExp>
        SYMBOL_CATALOGEXP = 54, // <CatalogExp>
        SYMBOL_CHILDEXP = 55, // <ChildExp>
        SYMBOL_CLONEEXP = 56, // <CloneExp>
        SYMBOL_COLUMNSEXP = 57, // <ColumnsExp>
        SYMBOL_COMMANDEXP = 58, // <CommandExp>
        SYMBOL_EXPORTEXP = 59, // <ExportExp>
        SYMBOL_EXPRESSION = 60, // <Expression>
        SYMBOL_HELPEXP = 61, // <HelpExp>
        SYMBOL_IMPORTEXP = 62, // <ImportExp>
        SYMBOL_LOADEXP = 63, // <LoadExp>
        SYMBOL_NEGATEEXP = 64, // <Negate Exp>
        SYMBOL_NL = 65, // <nl>
        SYMBOL_NLOPT = 66, // <nl Opt>
        SYMBOL_PASSWORDEXP = 67, // <PasswordExp>
        SYMBOL_REFRESHEXP = 68, // <RefreshExp>
        SYMBOL_RELATEEXP = 69, // <RelateExp>
        SYMBOL_REMOVEEXP = 70, // <RemoveExp>
        SYMBOL_ROOTEXP = 71, // <RootExp>
        SYMBOL_SERVEREXP = 72, // <ServerExp>
        SYMBOL_START = 73, // <Start>
        SYMBOL_TABLEEXP = 74, // <TableExp>
        SYMBOL_TABLESEXP = 75, // <TablesExp>
        SYMBOL_TOPNEXP = 76, // <TopnExp>
        SYMBOL_UNRELATEEXP = 77, // <UnrelateExp>
        SYMBOL_USEREXP = 78, // <UserExp>
        SYMBOL_VALUE = 79  // <Value>
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
        RULE_COMMANDEXP13 = 17, // <CommandExp> ::= <ColumnsExp>
        RULE_COMMANDEXP14 = 18, // <CommandExp> ::= <TopnExp>
        RULE_COMMANDEXP15 = 19, // <CommandExp> ::= <ChildExp>
        RULE_COMMANDEXP16 = 20, // <CommandExp> ::= <HelpExp>
        RULE_IMPORTEXP_IMPORT = 21, // <ImportExp> ::= import <CatalogExp>
        RULE_IMPORTEXP_IMPORT2 = 22, // <ImportExp> ::= import <CatalogExp> <UserExp> <PasswordExp>
        RULE_IMPORTEXP_IMPORT3 = 23, // <ImportExp> ::= import <CatalogExp> <ServerExp> <UserExp> <PasswordExp>
        RULE_CLONEEXP_CLONE = 24, // <CloneExp> ::= clone
        RULE_CLONEEXP_CLONE2 = 25, // <CloneExp> ::= clone <CatalogExp>
        RULE_CLONEEXP_CLONE3 = 26, // <CloneExp> ::= clone <AsExp>
        RULE_CLONEEXP_CLONE4 = 27, // <CloneExp> ::= clone <CatalogExp> <AsExp>
        RULE_REMOVEEXP_REMOVE = 28, // <RemoveExp> ::= remove
        RULE_REMOVEEXP_REMOVE2 = 29, // <RemoveExp> ::= remove <CatalogExp>
        RULE_REFRESHEXP_REFRESH = 30, // <RefreshExp> ::= refresh
        RULE_REFRESHEXP_REFRESH2 = 31, // <RefreshExp> ::= refresh <CatalogExp>
        RULE_LOADEXP_LOAD = 32, // <LoadExp> ::= load
        RULE_LOADEXP_LOAD2 = 33, // <LoadExp> ::= load <CatalogExp>
        RULE_TABLEEXP_TABLE = 34, // <TableExp> ::= table
        RULE_TABLEEXP_TABLE_IDENTIFIER = 35, // <TableExp> ::= table Identifier
        RULE_TABLEEXP_TABLE_IDENTIFIER_DEFAULT = 36, // <TableExp> ::= table Identifier default
        RULE_TABLEEXP_TABLE_IDENTIFIER_DEFAULT_WHERE = 37, // <TableExp> ::= table Identifier default where <Expression>
        RULE_TABLEEXP_TABLE_IDENTIFIER_WHERE = 38, // <TableExp> ::= table Identifier where <Expression>
        RULE_RELATEEXP_RELATE_TO_IDENTIFIER_ON_IDENTIFIER_EQ_IDENTIFIER = 39, // <RelateExp> ::= relate to Identifier on Identifier '=' Identifier
        RULE_RELATEEXP_RELATE_IDENTIFIER_TO_IDENTIFIER_ON_IDENTIFIER_EQ_IDENTIFIER = 40, // <RelateExp> ::= relate Identifier to Identifier on Identifier '=' Identifier
        RULE_UNRELATEEXP_UNRELATE_TO_IDENTIFIER = 41, // <UnrelateExp> ::= unrelate to Identifier
        RULE_UNRELATEEXP_UNRELATE_IDENTIFIER_TO_IDENTIFIER = 42, // <UnrelateExp> ::= unrelate Identifier to Identifier
        RULE_EXPORTEXP_EXPORT_AS_SQL = 43, // <ExportExp> ::= export as sql
        RULE_EXPORTEXP_EXPORT_AS_HTML = 44, // <ExportExp> ::= export as html
        RULE_EXPORTEXP_EXPORT_AS_JSON = 45, // <ExportExp> ::= export as json
        RULE_EXPORTEXP_EXPORT_AS_XML = 46, // <ExportExp> ::= export as xml
        RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_SQL = 47, // <ExportExp> ::= export Identifier as sql
        RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_HTML = 48, // <ExportExp> ::= export Identifier as html
        RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_JSON = 49, // <ExportExp> ::= export Identifier as json
        RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_XML = 50, // <ExportExp> ::= export Identifier as xml
        RULE_BACKEXP_BACK = 51, // <BackExp> ::= back
        RULE_ROOTEXP_ROOT = 52, // <RootExp> ::= root
        RULE_TABLESEXP_TABLES = 53, // <TablesExp> ::= tables
        RULE_TABLESEXP_TABLES_INTEGER = 54, // <TablesExp> ::= tables Integer
        RULE_COLUMNSEXP_COLUMNS = 55, // <ColumnsExp> ::= columns
        RULE_COLUMNSEXP_COLUMNS_INTEGER = 56, // <ColumnsExp> ::= columns Integer
        RULE_TOPNEXP_TOP_INTEGER = 57, // <TopnExp> ::= top Integer
        RULE_CHILDEXP_CHILD = 58, // <ChildExp> ::= child
        RULE_CHILDEXP_CHILD_INTEGER = 59, // <ChildExp> ::= child Integer
        RULE_CHILDEXP_CHILD_IDENTIFIER = 60, // <ChildExp> ::= child Identifier
        RULE_CATALOGEXP_CATALOG_IDENTIFIER = 61, // <CatalogExp> ::= catalog Identifier
        RULE_USEREXP_USER_IDENTIFIER = 62, // <UserExp> ::= user Identifier
        RULE_PASSWORDEXP_PASSWORD_IDENTIFIER = 63, // <PasswordExp> ::= password Identifier
        RULE_SERVEREXP_SERVER_IDENTIFIER = 64, // <ServerExp> ::= server Identifier
        RULE_ASEXP_AS_IDENTIFIER = 65, // <AsExp> ::= as Identifier
        RULE_HELPEXP_HELP = 66, // <HelpExp> ::= help
        RULE_EXPRESSION_GT = 67, // <Expression> ::= <Expression> '>' <Negate Exp>
        RULE_EXPRESSION_LT = 68, // <Expression> ::= <Expression> '<' <Negate Exp>
        RULE_EXPRESSION_LTEQ = 69, // <Expression> ::= <Expression> '<=' <Negate Exp>
        RULE_EXPRESSION_GTEQ = 70, // <Expression> ::= <Expression> '>=' <Negate Exp>
        RULE_EXPRESSION_EQ = 71, // <Expression> ::= <Expression> '=' <Negate Exp>
        RULE_EXPRESSION_LTGT = 72, // <Expression> ::= <Expression> '<>' <Negate Exp>
        RULE_EXPRESSION_IS = 73, // <Expression> ::= <Expression> is <Negate Exp>
        RULE_EXPRESSION_BETWEEN_AND = 74, // <Expression> ::= <Expression> between <Negate Exp> and <Negate Exp>
        RULE_EXPRESSION_LIKE_STRINGLITERAL = 75, // <Expression> ::= <Expression> like StringLiteral
        RULE_EXPRESSION = 76, // <Expression> ::= <Negate Exp>
        RULE_NEGATEEXP_MINUS = 77, // <Negate Exp> ::= '-' <Value>
        RULE_NEGATEEXP_NOT = 78, // <Negate Exp> ::= not <Value>
        RULE_NEGATEEXP = 79, // <Negate Exp> ::= <Value>
        RULE_VALUE_IDENTIFIER = 80, // <Value> ::= Identifier
        RULE_VALUE_LPAREN_RPAREN = 81, // <Value> ::= '(' <Expression> ')'
        RULE_VALUE_NULL = 82, // <Value> ::= null
        RULE_VALUE_INTEGER = 83, // <Value> ::= Integer
        RULE_VALUE_DECIMAL = 84, // <Value> ::= Decimal
        RULE_VALUE_STRINGLITERAL = 85  // <Value> ::= StringLiteral
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

                case (int)SymbolConstants.SYMBOL_ROOT:
                    //root
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

                case (int)SymbolConstants.SYMBOL_TO:
                    //to
                    //todo: Create a new object that corresponds to the symbol
                    return null;

                case (int)SymbolConstants.SYMBOL_TOP:
                    //top
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

                case (int)SymbolConstants.SYMBOL_PASSWORDEXP:
                    //<PasswordExp>
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
            throw new SymbolException("Unknown symbol");
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
                    //<CommandExp> ::= <ColumnsExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_COMMANDEXP14:
                    //<CommandExp> ::= <TopnExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_COMMANDEXP15:
                    //<CommandExp> ::= <ChildExp>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_COMMANDEXP16:
                    //<CommandExp> ::= <HelpExp>
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

                case (int)RuleConstants.RULE_TABLEEXP_TABLE:
                    //<TableExp> ::= table
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_TABLEEXP_TABLE_IDENTIFIER:
                    //<TableExp> ::= table Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_TABLEEXP_TABLE_IDENTIFIER_DEFAULT:
                    //<TableExp> ::= table Identifier default
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_TABLEEXP_TABLE_IDENTIFIER_DEFAULT_WHERE:
                    //<TableExp> ::= table Identifier default where <Expression>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_TABLEEXP_TABLE_IDENTIFIER_WHERE:
                    //<TableExp> ::= table Identifier where <Expression>
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_RELATEEXP_RELATE_TO_IDENTIFIER_ON_IDENTIFIER_EQ_IDENTIFIER:
                    //<RelateExp> ::= relate to Identifier on Identifier '=' Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_RELATEEXP_RELATE_IDENTIFIER_TO_IDENTIFIER_ON_IDENTIFIER_EQ_IDENTIFIER:
                    //<RelateExp> ::= relate Identifier to Identifier on Identifier '=' Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_UNRELATEEXP_UNRELATE_TO_IDENTIFIER:
                    //<UnrelateExp> ::= unrelate to Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_UNRELATEEXP_UNRELATE_IDENTIFIER_TO_IDENTIFIER:
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

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_SQL:
                    //<ExportExp> ::= export Identifier as sql
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_HTML:
                    //<ExportExp> ::= export Identifier as html
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_JSON:
                    //<ExportExp> ::= export Identifier as json
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_XML:
                    //<ExportExp> ::= export Identifier as xml
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_BACKEXP_BACK:
                    //<BackExp> ::= back
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_ROOTEXP_ROOT:
                    //<RootExp> ::= root
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

                case (int)RuleConstants.RULE_CHILDEXP_CHILD_IDENTIFIER:
                    //<ChildExp> ::= child Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_CATALOGEXP_CATALOG_IDENTIFIER:
                    //<CatalogExp> ::= catalog Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_USEREXP_USER_IDENTIFIER:
                    //<UserExp> ::= user Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_PASSWORDEXP_PASSWORD_IDENTIFIER:
                    //<PasswordExp> ::= password Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_SERVEREXP_SERVER_IDENTIFIER:
                    //<ServerExp> ::= server Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_ASEXP_AS_IDENTIFIER:
                    //<AsExp> ::= as Identifier
                    //todo: Create a new object using the stored user objects.
                    return null;

                case (int)RuleConstants.RULE_HELPEXP_HELP:
                    //<HelpExp> ::= help
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
            throw new RuleException("Unknown rule");
        }
    }
}
