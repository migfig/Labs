﻿
using System;
using System.IO;
using System.Runtime.Serialization;
using com.calitha.goldparser.lalr;
using com.calitha.commons;

namespace com.calitha.goldparser
{

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

    enum SymbolConstants : int
    {
        SYMBOL_EOF              =  0, // (EOF)
        SYMBOL_ERROR            =  1, // (Error)
        SYMBOL_WHITESPACE       =  2, // Whitespace
        SYMBOL_MINUS            =  3, // '-'
        SYMBOL_LPAREN           =  4, // '('
        SYMBOL_RPAREN           =  5, // ')'
        SYMBOL_COMMA            =  6, // ','
        SYMBOL_LT               =  7, // '<'
        SYMBOL_LTEQ             =  8, // '<='
        SYMBOL_LTGT             =  9, // '<>'
        SYMBOL_EQ               = 10, // '='
        SYMBOL_GT               = 11, // '>'
        SYMBOL_GTEQ             = 12, // '>='
        SYMBOL_AND              = 13, // and
        SYMBOL_AS               = 14, // as
        SYMBOL_BACK             = 15, // back
        SYMBOL_BETWEEN          = 16, // between
        SYMBOL_CATALOG          = 17, // catalog
        SYMBOL_CHILD            = 18, // child
        SYMBOL_CLONE            = 19, // clone
        SYMBOL_COLUMNS          = 20, // columns
        SYMBOL_DECIMAL          = 21, // Decimal
        SYMBOL_DEFAULT          = 22, // default
        SYMBOL_EXPORT           = 23, // export
        SYMBOL_HELP             = 24, // help
        SYMBOL_HOME             = 25, // home
        SYMBOL_HTML             = 26, // html
        SYMBOL_IDENTIFIER       = 27, // Identifier
        SYMBOL_IMPORT           = 28, // import
        SYMBOL_INTEGER          = 29, // Integer
        SYMBOL_IS               = 30, // is
        SYMBOL_JSON             = 31, // json
        SYMBOL_LIKE             = 32, // like
        SYMBOL_LOAD             = 33, // load
        SYMBOL_NEWLINE          = 34, // NewLine
        SYMBOL_NOCHILD          = 35, // nochild
        SYMBOL_NOT              = 36, // not
        SYMBOL_NULL             = 37, // null
        SYMBOL_ON               = 38, // on
        SYMBOL_PASSWORD         = 39, // password
        SYMBOL_QUERY            = 40, // query
        SYMBOL_REFRESH          = 41, // refresh
        SYMBOL_RELATE           = 42, // relate
        SYMBOL_REMOVE           = 43, // remove
        SYMBOL_ROW              = 44, // row
        SYMBOL_RUN              = 45, // run
        SYMBOL_SERVER           = 46, // server
        SYMBOL_SQL              = 47, // sql
        SYMBOL_STRINGLITERAL    = 48, // StringLiteral
        SYMBOL_TABLE            = 49, // table
        SYMBOL_TABLES           = 50, // tables
        SYMBOL_TEMPLATE         = 51, // template
        SYMBOL_TO               = 52, // to
        SYMBOL_TOP              = 53, // top
        SYMBOL_TRANSFORM        = 54, // transform
        SYMBOL_UNRELATE         = 55, // unrelate
        SYMBOL_USER             = 56, // user
        SYMBOL_WHERE            = 57, // where
        SYMBOL_WITH             = 58, // with
        SYMBOL_XML              = 59, // xml
        SYMBOL_ASEXP            = 60, // <AsExp>
        SYMBOL_BACKEXP          = 61, // <BackExp>
        SYMBOL_CATALOGEXP       = 62, // <CatalogExp>
        SYMBOL_CHILDEXP         = 63, // <ChildExp>
        SYMBOL_CLONEEXP         = 64, // <CloneExp>
        SYMBOL_COLUMNSEXP       = 65, // <ColumnsExp>
        SYMBOL_COMMANDEXP       = 66, // <CommandExp>
        SYMBOL_EXPORTEXP        = 67, // <ExportExp>
        SYMBOL_EXPRESSION       = 68, // <Expression>
        SYMBOL_HELPEXP          = 69, // <HelpExp>
        SYMBOL_IMPORTEXP        = 70, // <ImportExp>
        SYMBOL_LOADEXP          = 71, // <LoadExp>
        SYMBOL_NEGATEEXP        = 72, // <Negate Exp>
        SYMBOL_NL               = 73, // <nl>
        SYMBOL_NLOPT            = 74, // <nl Opt>
        SYMBOL_PARAMEXPRESSION  = 75, // <ParamExpression>
        SYMBOL_PARAMEXPRESSION2 = 76, // <ParamExpression2>
        SYMBOL_PARAMEXPRESSION3 = 77, // <ParamExpression3>
        SYMBOL_PARAMEXPRESSION4 = 78, // <ParamExpression4>
        SYMBOL_PARAMEXPRESSION5 = 79, // <ParamExpression5>
        SYMBOL_PARAMVALUE       = 80, // <ParamValue>
        SYMBOL_PASSWORDEXP      = 81, // <PasswordExp>
        SYMBOL_PVALUE           = 82, // <PValue>
        SYMBOL_QUERYEXP         = 83, // <QueryExp>
        SYMBOL_REFRESHEXP       = 84, // <RefreshExp>
        SYMBOL_RELATEEXP        = 85, // <RelateExp>
        SYMBOL_REMOVEEXP        = 86, // <RemoveExp>
        SYMBOL_ROOTEXP          = 87, // <RootExp>
        SYMBOL_RUNQUERYEXP      = 88, // <RunQueryExp>
        SYMBOL_SERVEREXP        = 89, // <ServerExp>
        SYMBOL_START            = 90, // <Start>
        SYMBOL_TABLEEXP         = 91, // <TableExp>
        SYMBOL_TABLESEXP        = 92, // <TablesExp>
        SYMBOL_TOPNEXP          = 93, // <TopnExp>
        SYMBOL_TRANSFORMEXP     = 94, // <TransformExp>
        SYMBOL_UNRELATEEXP      = 95, // <UnrelateExp>
        SYMBOL_USEREXP          = 96, // <UserExp>
        SYMBOL_VALUE            = 97  // <Value>
    };

    enum RuleConstants : int
    {
        RULE_NL_NEWLINE                                                            =  0, // <nl> ::= NewLine <nl>
        RULE_NL_NEWLINE2                                                           =  1, // <nl> ::= NewLine
        RULE_NLOPT_NEWLINE                                                         =  2, // <nl Opt> ::= NewLine <nl Opt>
        RULE_NLOPT                                                                 =  3, // <nl Opt> ::= 
        RULE_START                                                                 =  4, // <Start> ::= <nl Opt> <CommandExp> <nl Opt>
        RULE_COMMANDEXP                                                            =  5, // <CommandExp> ::= <ImportExp>
        RULE_COMMANDEXP2                                                           =  6, // <CommandExp> ::= <CloneExp>
        RULE_COMMANDEXP3                                                           =  7, // <CommandExp> ::= <RemoveExp>
        RULE_COMMANDEXP4                                                           =  8, // <CommandExp> ::= <RefreshExp>
        RULE_COMMANDEXP5                                                           =  9, // <CommandExp> ::= <LoadExp>
        RULE_COMMANDEXP6                                                           = 10, // <CommandExp> ::= <TableExp>
        RULE_COMMANDEXP7                                                           = 11, // <CommandExp> ::= <RelateExp>
        RULE_COMMANDEXP8                                                           = 12, // <CommandExp> ::= <UnrelateExp>
        RULE_COMMANDEXP9                                                           = 13, // <CommandExp> ::= <ExportExp>
        RULE_COMMANDEXP10                                                          = 14, // <CommandExp> ::= <BackExp>
        RULE_COMMANDEXP11                                                          = 15, // <CommandExp> ::= <RootExp>
        RULE_COMMANDEXP12                                                          = 16, // <CommandExp> ::= <TablesExp>
        RULE_COMMANDEXP13                                                          = 17, // <CommandExp> ::= <ColumnsExp>
        RULE_COMMANDEXP14                                                          = 18, // <CommandExp> ::= <TopnExp>
        RULE_COMMANDEXP15                                                          = 19, // <CommandExp> ::= <ChildExp>
        RULE_COMMANDEXP16                                                          = 20, // <CommandExp> ::= <HelpExp>
        RULE_COMMANDEXP17                                                          = 21, // <CommandExp> ::= <QueryExp>
        RULE_COMMANDEXP18                                                          = 22, // <CommandExp> ::= <RunQueryExp>
        RULE_COMMANDEXP19                                                          = 23, // <CommandExp> ::= <TransformExp>
        RULE_IMPORTEXP_IMPORT                                                      = 24, // <ImportExp> ::= import <CatalogExp>
        RULE_IMPORTEXP_IMPORT2                                                     = 25, // <ImportExp> ::= import <CatalogExp> <UserExp> <PasswordExp>
        RULE_IMPORTEXP_IMPORT3                                                     = 26, // <ImportExp> ::= import <CatalogExp> <ServerExp> <UserExp> <PasswordExp>
        RULE_CLONEEXP_CLONE                                                        = 27, // <CloneExp> ::= clone
        RULE_CLONEEXP_CLONE2                                                       = 28, // <CloneExp> ::= clone <CatalogExp>
        RULE_CLONEEXP_CLONE3                                                       = 29, // <CloneExp> ::= clone <AsExp>
        RULE_CLONEEXP_CLONE4                                                       = 30, // <CloneExp> ::= clone <CatalogExp> <AsExp>
        RULE_REMOVEEXP_REMOVE                                                      = 31, // <RemoveExp> ::= remove
        RULE_REMOVEEXP_REMOVE2                                                     = 32, // <RemoveExp> ::= remove <CatalogExp>
        RULE_REFRESHEXP_REFRESH                                                    = 33, // <RefreshExp> ::= refresh
        RULE_REFRESHEXP_REFRESH2                                                   = 34, // <RefreshExp> ::= refresh <CatalogExp>
        RULE_LOADEXP_LOAD                                                          = 35, // <LoadExp> ::= load
        RULE_LOADEXP_LOAD2                                                         = 36, // <LoadExp> ::= load <CatalogExp>
        RULE_TABLEEXP_TABLE                                                        = 37, // <TableExp> ::= table
        RULE_TABLEEXP_TABLE_IDENTIFIER                                             = 38, // <TableExp> ::= table Identifier
        RULE_TABLEEXP_TABLE_IDENTIFIER_DEFAULT                                     = 39, // <TableExp> ::= table Identifier default
        RULE_TABLEEXP_TABLE_IDENTIFIER_DEFAULT_WHERE                               = 40, // <TableExp> ::= table Identifier default where <Expression>
        RULE_TABLEEXP_TABLE_IDENTIFIER_WHERE                                       = 41, // <TableExp> ::= table Identifier where <Expression>
        RULE_RELATEEXP_RELATE_TO_IDENTIFIER_ON_IDENTIFIER_EQ_IDENTIFIER            = 42, // <RelateExp> ::= relate to Identifier on Identifier '=' Identifier
        RULE_RELATEEXP_RELATE_IDENTIFIER_TO_IDENTIFIER_ON_IDENTIFIER_EQ_IDENTIFIER = 43, // <RelateExp> ::= relate Identifier to Identifier on Identifier '=' Identifier
        RULE_UNRELATEEXP_UNRELATE_TO_IDENTIFIER                                    = 44, // <UnrelateExp> ::= unrelate to Identifier
        RULE_UNRELATEEXP_UNRELATE_IDENTIFIER_TO_IDENTIFIER                         = 45, // <UnrelateExp> ::= unrelate Identifier to Identifier
        RULE_EXPORTEXP_EXPORT_AS_SQL                                               = 46, // <ExportExp> ::= export as sql
        RULE_EXPORTEXP_EXPORT_AS_HTML                                              = 47, // <ExportExp> ::= export as html
        RULE_EXPORTEXP_EXPORT_AS_JSON                                              = 48, // <ExportExp> ::= export as json
        RULE_EXPORTEXP_EXPORT_AS_XML                                               = 49, // <ExportExp> ::= export as xml
        RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_SQL                                    = 50, // <ExportExp> ::= export Identifier as sql
        RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_HTML                                   = 51, // <ExportExp> ::= export Identifier as html
        RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_JSON                                   = 52, // <ExportExp> ::= export Identifier as json
        RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_XML                                    = 53, // <ExportExp> ::= export Identifier as xml
        RULE_EXPORTEXP_EXPORT_AS_SQL_NOCHILD                                       = 54, // <ExportExp> ::= export as sql nochild
        RULE_EXPORTEXP_EXPORT_AS_HTML_NOCHILD                                      = 55, // <ExportExp> ::= export as html nochild
        RULE_EXPORTEXP_EXPORT_AS_JSON_NOCHILD                                      = 56, // <ExportExp> ::= export as json nochild
        RULE_EXPORTEXP_EXPORT_AS_XML_NOCHILD                                       = 57, // <ExportExp> ::= export as xml nochild
        RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_SQL_NOCHILD                            = 58, // <ExportExp> ::= export Identifier as sql nochild
        RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_HTML_NOCHILD                           = 59, // <ExportExp> ::= export Identifier as html nochild
        RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_JSON_NOCHILD                           = 60, // <ExportExp> ::= export Identifier as json nochild
        RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_XML_NOCHILD                            = 61, // <ExportExp> ::= export Identifier as xml nochild
        RULE_BACKEXP_BACK                                                          = 62, // <BackExp> ::= back
        RULE_ROOTEXP_HOME                                                          = 63, // <RootExp> ::= home
        RULE_TABLESEXP_TABLES                                                      = 64, // <TablesExp> ::= tables
        RULE_TABLESEXP_TABLES_INTEGER                                              = 65, // <TablesExp> ::= tables Integer
        RULE_COLUMNSEXP_COLUMNS                                                    = 66, // <ColumnsExp> ::= columns
        RULE_COLUMNSEXP_COLUMNS_INTEGER                                            = 67, // <ColumnsExp> ::= columns Integer
        RULE_TOPNEXP_TOP_INTEGER                                                   = 68, // <TopnExp> ::= top Integer
        RULE_CHILDEXP_CHILD                                                        = 69, // <ChildExp> ::= child
        RULE_CHILDEXP_CHILD_INTEGER                                                = 70, // <ChildExp> ::= child Integer
        RULE_CHILDEXP_CHILD_IDENTIFIER                                             = 71, // <ChildExp> ::= child Identifier
        RULE_CATALOGEXP_CATALOG_IDENTIFIER                                         = 72, // <CatalogExp> ::= catalog Identifier
        RULE_USEREXP_USER_IDENTIFIER                                               = 73, // <UserExp> ::= user Identifier
        RULE_PASSWORDEXP_PASSWORD_IDENTIFIER                                       = 74, // <PasswordExp> ::= password Identifier
        RULE_SERVEREXP_SERVER_IDENTIFIER                                           = 75, // <ServerExp> ::= server Identifier
        RULE_ASEXP_AS_IDENTIFIER                                                   = 76, // <AsExp> ::= as Identifier
        RULE_HELPEXP_HELP                                                          = 77, // <HelpExp> ::= help
        RULE_QUERYEXP_QUERY_IDENTIFIER                                             = 78, // <QueryExp> ::= query Identifier
        RULE_QUERYEXP_QUERY_IDENTIFIER_ROW_INTEGER                                 = 79, // <QueryExp> ::= query Identifier row Integer
        RULE_TRANSFORMEXP_TRANSFORM                                                = 80, // <TransformExp> ::= transform
        RULE_TRANSFORMEXP_TRANSFORM_IDENTIFIER                                     = 81, // <TransformExp> ::= transform Identifier
        RULE_TRANSFORMEXP_TRANSFORM_IDENTIFIER_TEMPLATE_IDENTIFIER                 = 82, // <TransformExp> ::= transform Identifier template Identifier
        RULE_RUNQUERYEXP_RUN_IDENTIFIER                                            = 83, // <RunQueryExp> ::= run Identifier
        RULE_RUNQUERYEXP_RUN_IDENTIFIER_WITH                                       = 84, // <RunQueryExp> ::= run Identifier with <ParamExpression>
        RULE_RUNQUERYEXP_RUN_IDENTIFIER_WITH_COMMA                                 = 85, // <RunQueryExp> ::= run Identifier with <ParamExpression> ',' <ParamExpression2>
        RULE_RUNQUERYEXP_RUN_IDENTIFIER_WITH_COMMA_COMMA                           = 86, // <RunQueryExp> ::= run Identifier with <ParamExpression> ',' <ParamExpression2> ',' <ParamExpression3>
        RULE_RUNQUERYEXP_RUN_IDENTIFIER_WITH_COMMA_COMMA_COMMA                     = 87, // <RunQueryExp> ::= run Identifier with <ParamExpression> ',' <ParamExpression2> ',' <ParamExpression3> ',' <ParamExpression4>
        RULE_RUNQUERYEXP_RUN_IDENTIFIER_WITH_COMMA_COMMA_COMMA_COMMA               = 88, // <RunQueryExp> ::= run Identifier with <ParamExpression> ',' <ParamExpression2> ',' <ParamExpression3> ',' <ParamExpression4> ',' <ParamExpression5>
        RULE_PARAMEXPRESSION_EQ                                                    = 89, // <ParamExpression> ::= <ParamExpression> '=' <ParamValue>
        RULE_PARAMEXPRESSION                                                       = 90, // <ParamExpression> ::= <ParamValue>
        RULE_PARAMEXPRESSION2_EQ                                                   = 91, // <ParamExpression2> ::= <ParamExpression2> '=' <ParamValue>
        RULE_PARAMEXPRESSION2                                                      = 92, // <ParamExpression2> ::= <ParamValue>
        RULE_PARAMEXPRESSION3_EQ                                                   = 93, // <ParamExpression3> ::= <ParamExpression3> '=' <ParamValue>
        RULE_PARAMEXPRESSION3                                                      = 94, // <ParamExpression3> ::= <ParamValue>
        RULE_PARAMEXPRESSION4_EQ                                                   = 95, // <ParamExpression4> ::= <ParamExpression4> '=' <ParamValue>
        RULE_PARAMEXPRESSION4                                                      = 96, // <ParamExpression4> ::= <ParamValue>
        RULE_PARAMEXPRESSION5_EQ                                                   = 97, // <ParamExpression5> ::= <ParamExpression5> '=' <ParamValue>
        RULE_PARAMEXPRESSION5                                                      = 98, // <ParamExpression5> ::= <ParamValue>
        RULE_PARAMVALUE_MINUS                                                      = 99, // <ParamValue> ::= '-' <PValue>
        RULE_PARAMVALUE                                                            = 100, // <ParamValue> ::= <PValue>
        RULE_PVALUE_IDENTIFIER                                                     = 101, // <PValue> ::= Identifier
        RULE_PVALUE_NULL                                                           = 102, // <PValue> ::= null
        RULE_PVALUE_INTEGER                                                        = 103, // <PValue> ::= Integer
        RULE_PVALUE_DECIMAL                                                        = 104, // <PValue> ::= Decimal
        RULE_PVALUE_STRINGLITERAL                                                  = 105, // <PValue> ::= StringLiteral
        RULE_EXPRESSION_GT                                                         = 106, // <Expression> ::= <Expression> '>' <Negate Exp>
        RULE_EXPRESSION_LT                                                         = 107, // <Expression> ::= <Expression> '<' <Negate Exp>
        RULE_EXPRESSION_LTEQ                                                       = 108, // <Expression> ::= <Expression> '<=' <Negate Exp>
        RULE_EXPRESSION_GTEQ                                                       = 109, // <Expression> ::= <Expression> '>=' <Negate Exp>
        RULE_EXPRESSION_EQ                                                         = 110, // <Expression> ::= <Expression> '=' <Negate Exp>
        RULE_EXPRESSION_LTGT                                                       = 111, // <Expression> ::= <Expression> '<>' <Negate Exp>
        RULE_EXPRESSION_IS                                                         = 112, // <Expression> ::= <Expression> is <Negate Exp>
        RULE_EXPRESSION_NOT_BETWEEN_AND                                            = 113, // <Expression> ::= <Expression> not between <Negate Exp> and <Negate Exp>
        RULE_EXPRESSION_BETWEEN_AND                                                = 114, // <Expression> ::= <Expression> between <Negate Exp> and <Negate Exp>
        RULE_EXPRESSION_LIKE_STRINGLITERAL                                         = 115, // <Expression> ::= <Expression> like StringLiteral
        RULE_EXPRESSION                                                            = 116, // <Expression> ::= <Negate Exp>
        RULE_NEGATEEXP_MINUS                                                       = 117, // <Negate Exp> ::= '-' <Value>
        RULE_NEGATEEXP_NOT                                                         = 118, // <Negate Exp> ::= not <Value>
        RULE_NEGATEEXP                                                             = 119, // <Negate Exp> ::= <Value>
        RULE_VALUE_IDENTIFIER                                                      = 120, // <Value> ::= Identifier
        RULE_VALUE_LPAREN_RPAREN                                                   = 121, // <Value> ::= '(' <Expression> ')'
        RULE_VALUE_NULL                                                            = 122, // <Value> ::= null
        RULE_VALUE_INTEGER                                                         = 123, // <Value> ::= Integer
        RULE_VALUE_DECIMAL                                                         = 124, // <Value> ::= Decimal
        RULE_VALUE_STRINGLITERAL                                                   = 125  // <Value> ::= StringLiteral
    };

    public class MyParser
    {
        private LALRParser parser;

        public MyParser(string filename)
        {
            FileStream stream = new FileStream(filename,
                                               FileMode.Open, 
                                               FileAccess.Read, 
                                               FileShare.Read);
            Init(stream);
            stream.Close();
        }

        public MyParser(string baseName, string resourceName)
        {
            byte[] buffer = ResourceUtil.GetByteArrayResource(
                System.Reflection.Assembly.GetExecutingAssembly(),
                baseName,
                resourceName);
            MemoryStream stream = new MemoryStream(buffer);
            Init(stream);
            stream.Close();
        }

        public MyParser(Stream stream)
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

        public void Parse(string source)
        {
            parser.Parse(source);

        }

        private void TokenReadEvent(LALRParser parser, TokenReadEventArgs args)
        {
            try
            {
                args.Token.UserObject = CreateObject(args.Token);
            }
            catch (Exception e)
            {
                args.Continue = false;
                //todo: Report message to UI?
            }
        }

        private Object CreateObject(TerminalToken token)
        {
            switch (token.Symbol.Id)
            {
                case (int)SymbolConstants.SYMBOL_EOF :
                //(EOF)
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ERROR :
                //(Error)
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_WHITESPACE :
                //Whitespace
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_MINUS :
                //'-'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LPAREN :
                //'('
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_RPAREN :
                //')'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_COMMA :
                //','
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LT :
                //'<'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LTEQ :
                //'<='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LTGT :
                //'<>'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EQ :
                //'='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_GT :
                //'>'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_GTEQ :
                //'>='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_AND :
                //and
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_AS :
                //as
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_BACK :
                //back
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_BETWEEN :
                //between
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CATALOG :
                //catalog
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CHILD :
                //child
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CLONE :
                //clone
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_COLUMNS :
                //columns
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DECIMAL :
                //Decimal
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DEFAULT :
                //default
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EXPORT :
                //export
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_HELP :
                //help
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_HOME :
                //home
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_HTML :
                //html
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IDENTIFIER :
                //Identifier
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IMPORT :
                //import
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_INTEGER :
                //Integer
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IS :
                //is
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_JSON :
                //json
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LIKE :
                //like
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LOAD :
                //load
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NEWLINE :
                //NewLine
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NOCHILD :
                //nochild
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NOT :
                //not
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NULL :
                //null
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ON :
                //on
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PASSWORD :
                //password
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_QUERY :
                //query
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_REFRESH :
                //refresh
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_RELATE :
                //relate
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_REMOVE :
                //remove
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ROW :
                //row
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_RUN :
                //run
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_SERVER :
                //server
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_SQL :
                //sql
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STRINGLITERAL :
                //StringLiteral
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TABLE :
                //table
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TABLES :
                //tables
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TEMPLATE :
                //template
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TO :
                //to
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TOP :
                //top
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TRANSFORM :
                //transform
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_UNRELATE :
                //unrelate
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_USER :
                //user
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_WHERE :
                //where
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_WITH :
                //with
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_XML :
                //xml
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ASEXP :
                //<AsExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_BACKEXP :
                //<BackExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CATALOGEXP :
                //<CatalogExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CHILDEXP :
                //<ChildExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CLONEEXP :
                //<CloneExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_COLUMNSEXP :
                //<ColumnsExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_COMMANDEXP :
                //<CommandExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EXPORTEXP :
                //<ExportExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EXPRESSION :
                //<Expression>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_HELPEXP :
                //<HelpExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IMPORTEXP :
                //<ImportExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LOADEXP :
                //<LoadExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NEGATEEXP :
                //<Negate Exp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NL :
                //<nl>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NLOPT :
                //<nl Opt>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PARAMEXPRESSION :
                //<ParamExpression>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PARAMEXPRESSION2 :
                //<ParamExpression2>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PARAMEXPRESSION3 :
                //<ParamExpression3>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PARAMEXPRESSION4 :
                //<ParamExpression4>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PARAMEXPRESSION5 :
                //<ParamExpression5>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PARAMVALUE :
                //<ParamValue>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PASSWORDEXP :
                //<PasswordExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PVALUE :
                //<PValue>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_QUERYEXP :
                //<QueryExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_REFRESHEXP :
                //<RefreshExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_RELATEEXP :
                //<RelateExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_REMOVEEXP :
                //<RemoveExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ROOTEXP :
                //<RootExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_RUNQUERYEXP :
                //<RunQueryExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_SERVEREXP :
                //<ServerExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_START :
                //<Start>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TABLEEXP :
                //<TableExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TABLESEXP :
                //<TablesExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TOPNEXP :
                //<TopnExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TRANSFORMEXP :
                //<TransformExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_UNRELATEEXP :
                //<UnrelateExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_USEREXP :
                //<UserExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_VALUE :
                //<Value>
                //todo: Create a new object that corresponds to the symbol
                return null;

            }
            throw new SymbolException("Unknown symbol");
        }

        private void ReduceEvent(LALRParser parser, ReduceEventArgs args)
        {
            try
            {
                args.Token.UserObject = CreateObject(args.Token);
            }
            catch (Exception e)
            {
                args.Continue = false;
                //todo: Report message to UI?
            }
        }

        public static Object CreateObject(NonterminalToken token)
        {
            switch (token.Rule.Id)
            {
                case (int)RuleConstants.RULE_NL_NEWLINE :
                //<nl> ::= NewLine <nl>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_NL_NEWLINE2 :
                //<nl> ::= NewLine
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_NLOPT_NEWLINE :
                //<nl Opt> ::= NewLine <nl Opt>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_NLOPT :
                //<nl Opt> ::= 
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_START :
                //<Start> ::= <nl Opt> <CommandExp> <nl Opt>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP :
                //<CommandExp> ::= <ImportExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP2 :
                //<CommandExp> ::= <CloneExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP3 :
                //<CommandExp> ::= <RemoveExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP4 :
                //<CommandExp> ::= <RefreshExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP5 :
                //<CommandExp> ::= <LoadExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP6 :
                //<CommandExp> ::= <TableExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP7 :
                //<CommandExp> ::= <RelateExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP8 :
                //<CommandExp> ::= <UnrelateExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP9 :
                //<CommandExp> ::= <ExportExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP10 :
                //<CommandExp> ::= <BackExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP11 :
                //<CommandExp> ::= <RootExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP12 :
                //<CommandExp> ::= <TablesExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP13 :
                //<CommandExp> ::= <ColumnsExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP14 :
                //<CommandExp> ::= <TopnExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP15 :
                //<CommandExp> ::= <ChildExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP16 :
                //<CommandExp> ::= <HelpExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP17 :
                //<CommandExp> ::= <QueryExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP18 :
                //<CommandExp> ::= <RunQueryExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP19 :
                //<CommandExp> ::= <TransformExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_IMPORTEXP_IMPORT :
                //<ImportExp> ::= import <CatalogExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_IMPORTEXP_IMPORT2 :
                //<ImportExp> ::= import <CatalogExp> <UserExp> <PasswordExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_IMPORTEXP_IMPORT3 :
                //<ImportExp> ::= import <CatalogExp> <ServerExp> <UserExp> <PasswordExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_CLONEEXP_CLONE :
                //<CloneExp> ::= clone
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_CLONEEXP_CLONE2 :
                //<CloneExp> ::= clone <CatalogExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_CLONEEXP_CLONE3 :
                //<CloneExp> ::= clone <AsExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_CLONEEXP_CLONE4 :
                //<CloneExp> ::= clone <CatalogExp> <AsExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_REMOVEEXP_REMOVE :
                //<RemoveExp> ::= remove
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_REMOVEEXP_REMOVE2 :
                //<RemoveExp> ::= remove <CatalogExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_REFRESHEXP_REFRESH :
                //<RefreshExp> ::= refresh
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_REFRESHEXP_REFRESH2 :
                //<RefreshExp> ::= refresh <CatalogExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_LOADEXP_LOAD :
                //<LoadExp> ::= load
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_LOADEXP_LOAD2 :
                //<LoadExp> ::= load <CatalogExp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_TABLEEXP_TABLE :
                //<TableExp> ::= table
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_TABLEEXP_TABLE_IDENTIFIER :
                //<TableExp> ::= table Identifier
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_TABLEEXP_TABLE_IDENTIFIER_DEFAULT :
                //<TableExp> ::= table Identifier default
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_TABLEEXP_TABLE_IDENTIFIER_DEFAULT_WHERE :
                //<TableExp> ::= table Identifier default where <Expression>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_TABLEEXP_TABLE_IDENTIFIER_WHERE :
                //<TableExp> ::= table Identifier where <Expression>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_RELATEEXP_RELATE_TO_IDENTIFIER_ON_IDENTIFIER_EQ_IDENTIFIER :
                //<RelateExp> ::= relate to Identifier on Identifier '=' Identifier
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_RELATEEXP_RELATE_IDENTIFIER_TO_IDENTIFIER_ON_IDENTIFIER_EQ_IDENTIFIER :
                //<RelateExp> ::= relate Identifier to Identifier on Identifier '=' Identifier
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_UNRELATEEXP_UNRELATE_TO_IDENTIFIER :
                //<UnrelateExp> ::= unrelate to Identifier
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_UNRELATEEXP_UNRELATE_IDENTIFIER_TO_IDENTIFIER :
                //<UnrelateExp> ::= unrelate Identifier to Identifier
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_AS_SQL :
                //<ExportExp> ::= export as sql
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_AS_HTML :
                //<ExportExp> ::= export as html
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_AS_JSON :
                //<ExportExp> ::= export as json
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_AS_XML :
                //<ExportExp> ::= export as xml
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_SQL :
                //<ExportExp> ::= export Identifier as sql
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_HTML :
                //<ExportExp> ::= export Identifier as html
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_JSON :
                //<ExportExp> ::= export Identifier as json
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_XML :
                //<ExportExp> ::= export Identifier as xml
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_AS_SQL_NOCHILD :
                //<ExportExp> ::= export as sql nochild
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_AS_HTML_NOCHILD :
                //<ExportExp> ::= export as html nochild
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_AS_JSON_NOCHILD :
                //<ExportExp> ::= export as json nochild
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_AS_XML_NOCHILD :
                //<ExportExp> ::= export as xml nochild
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_SQL_NOCHILD :
                //<ExportExp> ::= export Identifier as sql nochild
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_HTML_NOCHILD :
                //<ExportExp> ::= export Identifier as html nochild
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_JSON_NOCHILD :
                //<ExportExp> ::= export Identifier as json nochild
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_XML_NOCHILD :
                //<ExportExp> ::= export Identifier as xml nochild
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_BACKEXP_BACK :
                //<BackExp> ::= back
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_ROOTEXP_HOME :
                //<RootExp> ::= home
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_TABLESEXP_TABLES :
                //<TablesExp> ::= tables
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_TABLESEXP_TABLES_INTEGER :
                //<TablesExp> ::= tables Integer
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_COLUMNSEXP_COLUMNS :
                //<ColumnsExp> ::= columns
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_COLUMNSEXP_COLUMNS_INTEGER :
                //<ColumnsExp> ::= columns Integer
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_TOPNEXP_TOP_INTEGER :
                //<TopnExp> ::= top Integer
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_CHILDEXP_CHILD :
                //<ChildExp> ::= child
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_CHILDEXP_CHILD_INTEGER :
                //<ChildExp> ::= child Integer
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_CHILDEXP_CHILD_IDENTIFIER :
                //<ChildExp> ::= child Identifier
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_CATALOGEXP_CATALOG_IDENTIFIER :
                //<CatalogExp> ::= catalog Identifier
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_USEREXP_USER_IDENTIFIER :
                //<UserExp> ::= user Identifier
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_PASSWORDEXP_PASSWORD_IDENTIFIER :
                //<PasswordExp> ::= password Identifier
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_SERVEREXP_SERVER_IDENTIFIER :
                //<ServerExp> ::= server Identifier
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_ASEXP_AS_IDENTIFIER :
                //<AsExp> ::= as Identifier
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_HELPEXP_HELP :
                //<HelpExp> ::= help
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_QUERYEXP_QUERY_IDENTIFIER :
                //<QueryExp> ::= query Identifier
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_QUERYEXP_QUERY_IDENTIFIER_ROW_INTEGER :
                //<QueryExp> ::= query Identifier row Integer
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_TRANSFORMEXP_TRANSFORM :
                //<TransformExp> ::= transform
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_TRANSFORMEXP_TRANSFORM_IDENTIFIER :
                //<TransformExp> ::= transform Identifier
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_TRANSFORMEXP_TRANSFORM_IDENTIFIER_TEMPLATE_IDENTIFIER :
                //<TransformExp> ::= transform Identifier template Identifier
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_RUNQUERYEXP_RUN_IDENTIFIER :
                //<RunQueryExp> ::= run Identifier
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_RUNQUERYEXP_RUN_IDENTIFIER_WITH :
                //<RunQueryExp> ::= run Identifier with <ParamExpression>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_RUNQUERYEXP_RUN_IDENTIFIER_WITH_COMMA :
                //<RunQueryExp> ::= run Identifier with <ParamExpression> ',' <ParamExpression2>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_RUNQUERYEXP_RUN_IDENTIFIER_WITH_COMMA_COMMA :
                //<RunQueryExp> ::= run Identifier with <ParamExpression> ',' <ParamExpression2> ',' <ParamExpression3>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_RUNQUERYEXP_RUN_IDENTIFIER_WITH_COMMA_COMMA_COMMA :
                //<RunQueryExp> ::= run Identifier with <ParamExpression> ',' <ParamExpression2> ',' <ParamExpression3> ',' <ParamExpression4>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_RUNQUERYEXP_RUN_IDENTIFIER_WITH_COMMA_COMMA_COMMA_COMMA :
                //<RunQueryExp> ::= run Identifier with <ParamExpression> ',' <ParamExpression2> ',' <ParamExpression3> ',' <ParamExpression4> ',' <ParamExpression5>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_PARAMEXPRESSION_EQ :
                //<ParamExpression> ::= <ParamExpression> '=' <ParamValue>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_PARAMEXPRESSION :
                //<ParamExpression> ::= <ParamValue>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_PARAMEXPRESSION2_EQ :
                //<ParamExpression2> ::= <ParamExpression2> '=' <ParamValue>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_PARAMEXPRESSION2 :
                //<ParamExpression2> ::= <ParamValue>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_PARAMEXPRESSION3_EQ :
                //<ParamExpression3> ::= <ParamExpression3> '=' <ParamValue>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_PARAMEXPRESSION3 :
                //<ParamExpression3> ::= <ParamValue>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_PARAMEXPRESSION4_EQ :
                //<ParamExpression4> ::= <ParamExpression4> '=' <ParamValue>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_PARAMEXPRESSION4 :
                //<ParamExpression4> ::= <ParamValue>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_PARAMEXPRESSION5_EQ :
                //<ParamExpression5> ::= <ParamExpression5> '=' <ParamValue>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_PARAMEXPRESSION5 :
                //<ParamExpression5> ::= <ParamValue>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_PARAMVALUE_MINUS :
                //<ParamValue> ::= '-' <PValue>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_PARAMVALUE :
                //<ParamValue> ::= <PValue>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_PVALUE_IDENTIFIER :
                //<PValue> ::= Identifier
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_PVALUE_NULL :
                //<PValue> ::= null
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_PVALUE_INTEGER :
                //<PValue> ::= Integer
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_PVALUE_DECIMAL :
                //<PValue> ::= Decimal
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_PVALUE_STRINGLITERAL :
                //<PValue> ::= StringLiteral
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_GT :
                //<Expression> ::= <Expression> '>' <Negate Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_LT :
                //<Expression> ::= <Expression> '<' <Negate Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_LTEQ :
                //<Expression> ::= <Expression> '<=' <Negate Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_GTEQ :
                //<Expression> ::= <Expression> '>=' <Negate Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_EQ :
                //<Expression> ::= <Expression> '=' <Negate Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_LTGT :
                //<Expression> ::= <Expression> '<>' <Negate Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_IS :
                //<Expression> ::= <Expression> is <Negate Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_NOT_BETWEEN_AND :
                //<Expression> ::= <Expression> not between <Negate Exp> and <Negate Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_BETWEEN_AND :
                //<Expression> ::= <Expression> between <Negate Exp> and <Negate Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_LIKE_STRINGLITERAL :
                //<Expression> ::= <Expression> like StringLiteral
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION :
                //<Expression> ::= <Negate Exp>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_NEGATEEXP_MINUS :
                //<Negate Exp> ::= '-' <Value>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_NEGATEEXP_NOT :
                //<Negate Exp> ::= not <Value>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_NEGATEEXP :
                //<Negate Exp> ::= <Value>
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_VALUE_IDENTIFIER :
                //<Value> ::= Identifier
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_VALUE_LPAREN_RPAREN :
                //<Value> ::= '(' <Expression> ')'
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_VALUE_NULL :
                //<Value> ::= null
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_VALUE_INTEGER :
                //<Value> ::= Integer
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_VALUE_DECIMAL :
                //<Value> ::= Decimal
                //todo: Create a new object using the stored user objects.
                return null;

                case (int)RuleConstants.RULE_VALUE_STRINGLITERAL :
                //<Value> ::= StringLiteral
                //todo: Create a new object using the stored user objects.
                return null;

            }
            throw new RuleException("Unknown rule");
        }

        private void AcceptEvent(LALRParser parser, AcceptEventArgs args)
        {
            //todo: Use your fully reduced args.Token.UserObject
        }

        private void TokenErrorEvent(LALRParser parser, TokenErrorEventArgs args)
        {
            string message = "Token error with input: '"+args.Token.ToString()+"'";
            //todo: Report message to UI?
        }

        private void ParseErrorEvent(LALRParser parser, ParseErrorEventArgs args)
        {
            string message = "Parse error caused by token: '"+args.UnexpectedToken.ToString()+"'";
            //todo: Report message to UI?
        }


    }
}