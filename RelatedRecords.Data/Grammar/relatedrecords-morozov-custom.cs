
using System;
using System.Reflection;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.Generic;

using GoldParser;

namespace Morozov.Parsing
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
        SYMBOL_EOF           =  0, // (EOF)
        SYMBOL_ERROR         =  1, // (Error)
        SYMBOL_WHITESPACE    =  2, // Whitespace
        SYMBOL_MINUS         =  3, // '-'
        SYMBOL_LPAREN        =  4, // '('
        SYMBOL_RPAREN        =  5, // ')'
        SYMBOL_LT            =  6, // '<'
        SYMBOL_LTEQ          =  7, // '<='
        SYMBOL_LTGT          =  8, // '<>'
        SYMBOL_EQ            =  9, // '='
        SYMBOL_GT            = 10, // '>'
        SYMBOL_GTEQ          = 11, // '>='
        SYMBOL_AND           = 12, // and
        SYMBOL_AS            = 13, // as
        SYMBOL_BACK          = 14, // back
        SYMBOL_BETWEEN       = 15, // between
        SYMBOL_CATALOG       = 16, // catalog
        SYMBOL_CLONE         = 17, // clone
        SYMBOL_COLUMNS       = 18, // columns
        SYMBOL_DECIMAL       = 19, // Decimal
        SYMBOL_DEFAULT       = 20, // default
        SYMBOL_EXPORT        = 21, // export
        SYMBOL_HTML          = 22, // html
        SYMBOL_IDENTIFIER    = 23, // Identifier
        SYMBOL_IMPORT        = 24, // import
        SYMBOL_INTEGER       = 25, // Integer
        SYMBOL_IS            = 26, // is
        SYMBOL_JSON          = 27, // json
        SYMBOL_LIKE          = 28, // like
        SYMBOL_LOAD          = 29, // load
        SYMBOL_NEWLINE       = 30, // NewLine
        SYMBOL_NOT           = 31, // not
        SYMBOL_NULL          = 32, // null
        SYMBOL_ON            = 33, // on
        SYMBOL_PASSWORD      = 34, // password
        SYMBOL_RELATE        = 35, // relate
        SYMBOL_REMOVE        = 36, // remove
        SYMBOL_ROOT          = 37, // root
        SYMBOL_SERVER        = 38, // server
        SYMBOL_SQL           = 39, // sql
        SYMBOL_STRINGLITERAL = 40, // StringLiteral
        SYMBOL_TABLE         = 41, // table
        SYMBOL_TABLES        = 42, // tables
        SYMBOL_TO            = 43, // to
        SYMBOL_TOP           = 44, // top
        SYMBOL_UNRELATE      = 45, // unrelate
        SYMBOL_USER          = 46, // user
        SYMBOL_WHERE         = 47, // where
        SYMBOL_XML           = 48, // xml
        SYMBOL_ASEXP         = 49, // <AsExp>
        SYMBOL_BACKEXP       = 50, // <BackExp>
        SYMBOL_CATALOGEXP    = 51, // <CatalogExp>
        SYMBOL_CLONEEXP      = 52, // <CloneExp>
        SYMBOL_COLUMNSEXP    = 53, // <ColumnsExp>
        SYMBOL_COMMANDEXP    = 54, // <CommandExp>
        SYMBOL_EXPORTEXP     = 55, // <ExportExp>
        SYMBOL_EXPRESSION    = 56, // <Expression>
        SYMBOL_IMPORTEXP     = 57, // <ImportExp>
        SYMBOL_LOADEXP       = 58, // <LoadExp>
        SYMBOL_NEGATEEXP     = 59, // <Negate Exp>
        SYMBOL_NL            = 60, // <nl>
        SYMBOL_NLOPT         = 61, // <nl Opt>
        SYMBOL_PASSWORDEXP   = 62, // <PasswordExp>
        SYMBOL_RELATEEXP     = 63, // <RelateExp>
        SYMBOL_REMOVEEXP     = 64, // <RemoveExp>
        SYMBOL_ROOTEXP       = 65, // <RootExp>
        SYMBOL_SERVEREXP     = 66, // <ServerExp>
        SYMBOL_START         = 67, // <Start>
        SYMBOL_TABLEEXP      = 68, // <TableExp>
        SYMBOL_TABLESEXP     = 69, // <TablesExp>
        SYMBOL_TOPNEXP       = 70, // <TopnExp>
        SYMBOL_UNRELATEEXP   = 71, // <UnrelateExp>
        SYMBOL_USEREXP       = 72, // <UserExp>
        SYMBOL_VALUE         = 73  // <Value>
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
        RULE_COMMANDEXP4                                                           =  8, // <CommandExp> ::= <LoadExp>
        RULE_COMMANDEXP5                                                           =  9, // <CommandExp> ::= <TableExp>
        RULE_COMMANDEXP6                                                           = 10, // <CommandExp> ::= <RelateExp>
        RULE_COMMANDEXP7                                                           = 11, // <CommandExp> ::= <UnrelateExp>
        RULE_COMMANDEXP8                                                           = 12, // <CommandExp> ::= <ExportExp>
        RULE_COMMANDEXP9                                                           = 13, // <CommandExp> ::= <BackExp>
        RULE_COMMANDEXP10                                                          = 14, // <CommandExp> ::= <RootExp>
        RULE_COMMANDEXP11                                                          = 15, // <CommandExp> ::= <TablesExp>
        RULE_COMMANDEXP12                                                          = 16, // <CommandExp> ::= <ColumnsExp>
        RULE_COMMANDEXP13                                                          = 17, // <CommandExp> ::= <TopnExp>
        RULE_IMPORTEXP_IMPORT                                                      = 18, // <ImportExp> ::= import <CatalogExp>
        RULE_IMPORTEXP_IMPORT2                                                     = 19, // <ImportExp> ::= import <CatalogExp> <UserExp> <PasswordExp>
        RULE_IMPORTEXP_IMPORT3                                                     = 20, // <ImportExp> ::= import <CatalogExp> <ServerExp> <UserExp> <PasswordExp>
        RULE_CLONEEXP_CLONE                                                        = 21, // <CloneExp> ::= clone
        RULE_CLONEEXP_CLONE2                                                       = 22, // <CloneExp> ::= clone <CatalogExp>
        RULE_CLONEEXP_CLONE3                                                       = 23, // <CloneExp> ::= clone <AsExp>
        RULE_CLONEEXP_CLONE4                                                       = 24, // <CloneExp> ::= clone <CatalogExp> <AsExp>
        RULE_REMOVEEXP_REMOVE                                                      = 25, // <RemoveExp> ::= remove
        RULE_REMOVEEXP_REMOVE2                                                     = 26, // <RemoveExp> ::= remove <CatalogExp>
        RULE_LOADEXP_LOAD                                                          = 27, // <LoadExp> ::= load
        RULE_LOADEXP_LOAD2                                                         = 28, // <LoadExp> ::= load <CatalogExp>
        RULE_TABLEEXP_TABLE                                                        = 29, // <TableExp> ::= table
        RULE_TABLEEXP_TABLE_IDENTIFIER                                             = 30, // <TableExp> ::= table Identifier
        RULE_TABLEEXP_TABLE_IDENTIFIER_DEFAULT                                     = 31, // <TableExp> ::= table Identifier default
        RULE_TABLEEXP_TABLE_IDENTIFIER_DEFAULT_WHERE                               = 32, // <TableExp> ::= table Identifier default where <Expression>
        RULE_TABLEEXP_TABLE_IDENTIFIER_WHERE                                       = 33, // <TableExp> ::= table Identifier where <Expression>
        RULE_RELATEEXP_RELATE_TO_IDENTIFIER_ON_IDENTIFIER_EQ_IDENTIFIER            = 34, // <RelateExp> ::= relate to Identifier on Identifier '=' Identifier
        RULE_RELATEEXP_RELATE_IDENTIFIER_TO_IDENTIFIER_ON_IDENTIFIER_EQ_IDENTIFIER = 35, // <RelateExp> ::= relate Identifier to Identifier on Identifier '=' Identifier
        RULE_UNRELATEEXP_UNRELATE_TO_IDENTIFIER                                    = 36, // <UnrelateExp> ::= unrelate to Identifier
        RULE_UNRELATEEXP_UNRELATE_IDENTIFIER_TO_IDENTIFIER                         = 37, // <UnrelateExp> ::= unrelate Identifier to Identifier
        RULE_EXPORTEXP_EXPORT_AS_SQL                                               = 38, // <ExportExp> ::= export as sql
        RULE_EXPORTEXP_EXPORT_AS_HTML                                              = 39, // <ExportExp> ::= export as html
        RULE_EXPORTEXP_EXPORT_AS_JSON                                              = 40, // <ExportExp> ::= export as json
        RULE_EXPORTEXP_EXPORT_AS_XML                                               = 41, // <ExportExp> ::= export as xml
        RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_SQL                                    = 42, // <ExportExp> ::= export Identifier as sql
        RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_HTML                                   = 43, // <ExportExp> ::= export Identifier as html
        RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_JSON                                   = 44, // <ExportExp> ::= export Identifier as json
        RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_XML                                    = 45, // <ExportExp> ::= export Identifier as xml
        RULE_BACKEXP_BACK                                                          = 46, // <BackExp> ::= back
        RULE_ROOTEXP_ROOT                                                          = 47, // <RootExp> ::= root
        RULE_TABLESEXP_TABLES                                                      = 48, // <TablesExp> ::= tables
        RULE_TABLESEXP_TABLES_INTEGER                                              = 49, // <TablesExp> ::= tables Integer
        RULE_COLUMNSEXP_COLUMNS                                                    = 50, // <ColumnsExp> ::= columns
        RULE_COLUMNSEXP_COLUMNS_INTEGER                                            = 51, // <ColumnsExp> ::= columns Integer
        RULE_TOPNEXP_TOP_INTEGER                                                   = 52, // <TopnExp> ::= top Integer
        RULE_CATALOGEXP_CATALOG_IDENTIFIER                                         = 53, // <CatalogExp> ::= catalog Identifier
        RULE_USEREXP_USER_IDENTIFIER                                               = 54, // <UserExp> ::= user Identifier
        RULE_PASSWORDEXP_PASSWORD_IDENTIFIER                                       = 55, // <PasswordExp> ::= password Identifier
        RULE_SERVEREXP_SERVER_IDENTIFIER                                           = 56, // <ServerExp> ::= server Identifier
        RULE_ASEXP_AS_IDENTIFIER                                                   = 57, // <AsExp> ::= as Identifier
        RULE_EXPRESSION_GT                                                         = 58, // <Expression> ::= <Expression> '>' <Negate Exp>
        RULE_EXPRESSION_LT                                                         = 59, // <Expression> ::= <Expression> '<' <Negate Exp>
        RULE_EXPRESSION_LTEQ                                                       = 60, // <Expression> ::= <Expression> '<=' <Negate Exp>
        RULE_EXPRESSION_GTEQ                                                       = 61, // <Expression> ::= <Expression> '>=' <Negate Exp>
        RULE_EXPRESSION_EQ                                                         = 62, // <Expression> ::= <Expression> '=' <Negate Exp>
        RULE_EXPRESSION_LTGT                                                       = 63, // <Expression> ::= <Expression> '<>' <Negate Exp>
        RULE_EXPRESSION_IS                                                         = 64, // <Expression> ::= <Expression> is <Negate Exp>
        RULE_EXPRESSION_BETWEEN_AND                                                = 65, // <Expression> ::= <Expression> between <Negate Exp> and <Negate Exp>
        RULE_EXPRESSION_LIKE_STRINGLITERAL                                         = 66, // <Expression> ::= <Expression> like StringLiteral
        RULE_EXPRESSION                                                            = 67, // <Expression> ::= <Negate Exp>
        RULE_NEGATEEXP_MINUS                                                       = 68, // <Negate Exp> ::= '-' <Value>
        RULE_NEGATEEXP_NOT                                                         = 69, // <Negate Exp> ::= not <Value>
        RULE_NEGATEEXP                                                             = 70, // <Negate Exp> ::= <Value>
        RULE_VALUE_IDENTIFIER                                                      = 71, // <Value> ::= Identifier
        RULE_VALUE_LPAREN_RPAREN                                                   = 72, // <Value> ::= '(' <Expression> ')'
        RULE_VALUE_NULL                                                            = 73, // <Value> ::= null
        RULE_VALUE_INTEGER                                                         = 74, // <Value> ::= Integer
        RULE_VALUE_DECIMAL                                                         = 75, // <Value> ::= Decimal
        RULE_VALUE_STRINGLITERAL                                                   = 76  // <Value> ::= StringLiteral
    };

        // this class will construct a parser without having to process
        //  the CGT tables with each creation.  It must be initialized
        //  before you can call CreateParser()
    public sealed class ParserFactory
    {
        static Grammar m_grammar;
        static bool _init;
        
        private ParserFactory()
        {
        }
        
        private static BinaryReader GetResourceReader(string resourceName)
        {  
            Assembly assembly = Assembly.GetExecutingAssembly();   
            Stream stream = assembly.GetManifestResourceStream(resourceName);
            return new BinaryReader(stream);
        }
        
        public static void InitializeFactoryFromFile(string FullCGTFilePath)
        {
            if (!_init)
            {
               BinaryReader reader = new BinaryReader(new FileStream(FullCGTFilePath,FileMode.Open));
               m_grammar = new Grammar( reader );
               _init = true;
            }
        }
        
        public static void InitializeFactoryFromResource(string resourceName)
        {
            if (!_init)
            {
                BinaryReader reader = GetResourceReader(resourceName);
                m_grammar = new Grammar( reader );
                _init = true;
            }
        }
        
        public static Parser CreateParser(TextReader reader)
        {
           if (_init)
           {
                return new Parser(reader, m_grammar);
           }
           throw new Exception("You must first Initialize the Factory before creating a parser!");
        }
    }
        
    public abstract partial class ASTNode
    {
        public abstract bool IsTerminal
        {
            get;
        }
    }
    
    /// <summary>
    /// Derive this class for Terminal AST Nodes
    /// </summary>
    public partial class TerminalNode : ASTNode
    {
        private Symbol m_symbol;
        private string m_text;
        private int m_lineNumber;
        private int m_linePosition;

        public TerminalNode(Parser theParser)
        {
            m_symbol = theParser.TokenSymbol;
            m_text = theParser.TokenSymbol.ToString();
            m_lineNumber = theParser.LineNumber;
            m_linePosition = theParser.LinePosition;
        }

        public override bool IsTerminal
        {
            get
            {
                return true;
            }
        }
        
        public Symbol Symbol
        {
            get { return m_symbol; }
        }

        public string Text
        {
            get { return m_text; }
        }

        public override string ToString()
        {
            return m_text;
        }

        public int LineNumber 
        {
            get { return m_lineNumber; }
        }

        public int LinePosition
        {
            get { return m_linePosition; }
        }
    }
    
    /// <summary>
    /// Derive this class for NonTerminal AST Nodes
    /// </summary>
    public partial class NonTerminalNode : ASTNode
    {
        private int m_reductionNumber;
        private Rule m_rule;
        private List<ASTNode> m_array = new List<ASTNode>();

        public NonTerminalNode(Parser theParser)
        {
            m_rule = theParser.ReductionRule;
        }
        
        public override bool IsTerminal
        {
            get
            {
                return false;
            }
        }

        public int ReductionNumber 
        {
            get { return m_reductionNumber; }
            set { m_reductionNumber = value; }
        }

        public int Count 
        {
            get { return m_array.Count; }
        }

        public ASTNode this[int index]
        {
            get { return m_array[index]; }
        }

        public void AppendChildNode(ASTNode node)
        {
            if (node == null)
            {
                return ; 
            }
            m_array.Add(node);
        }

        public Rule Rule
        {
            get { return m_rule; }
        }

    }

    public partial class MyParser
    {
        MyParserContext m_context;
        ASTNode m_AST;
        string m_errorString;
        Parser m_parser;
        
        public int LineNumber
        {
            get
            {
                return m_parser.LineNumber;
            }
        }

        public int LinePosition
        {
            get
            {
                return m_parser.LinePosition;
            }
        }

        public string ErrorString
        {
            get
            {
                return m_errorString;
            }
        }

        public string ErrorLine
        {
            get
            {
                return m_parser.LineText;
            }
        }

        public ASTNode SyntaxTree
        {
            get
            {
                return m_AST;
            }
        }

        public bool Parse(string source)
        {
            return Parse(new StringReader(source));
        }

        public bool Parse(StringReader sourceReader)
        {
            m_parser = ParserFactory.CreateParser(sourceReader);
            m_parser.TrimReductions = true;
            m_context = new MyParserContext(m_parser);
            
            while (true)
            {
                switch (m_parser.Parse())
                {
                    case ParseMessage.LexicalError:
                        m_errorString = string.Format("Lexical Error. Line {0}. Token {1} was not expected.", m_parser.LineNumber, m_parser.TokenText);
                        return false;

                    case ParseMessage.SyntaxError:
                        StringBuilder text = new StringBuilder();
                        foreach (Symbol tokenSymbol in m_parser.GetExpectedTokens())
                        {
                            text.Append(' ');
                            text.Append(tokenSymbol.ToString());
                        }
                        m_errorString = string.Format("Syntax Error. Line {0}. Expecting: {1}.", m_parser.LineNumber, text.ToString());
                        
                        return false;
                    case ParseMessage.Reduction:
                        m_parser.TokenSyntaxNode = m_context.CreateASTNode();
                        break;

                    case ParseMessage.Accept:
                        m_AST = m_parser.TokenSyntaxNode as ASTNode;
                        m_errorString = null;
                        return true;

                    case ParseMessage.InternalError:
                        m_errorString = "Internal Error. Something is horribly wrong.";
                        return false;

                    case ParseMessage.NotLoadedError:
                        m_errorString = "Grammar Table is not loaded.";
                        return false;

                    case ParseMessage.CommentError:
                        m_errorString = "Comment Error. Unexpected end of input.";
                        
                        return false;

                    case ParseMessage.CommentBlockRead:
                    case ParseMessage.CommentLineRead:
                        // don't do anything 
                        break;
                }
            }
         }

    }

    public partial class MyParserContext
    {

        // instance fields
        private Parser m_parser;
        
        private TextReader m_inputReader;
        

        
        // constructor
        public MyParserContext(Parser prser)
        {
            m_parser = prser;   
        }
       

        private string GetTokenText()
        {
            // delete any of these that are non-terminals.

            switch (m_parser.TokenSymbol.Index)
            {

                case (int)SymbolConstants.SYMBOL_EOF :
                //(EOF)
                //Token Kind: 3
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_ERROR :
                //(Error)
                //Token Kind: 7
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_WHITESPACE :
                //Whitespace
                //Token Kind: 2
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_MINUS :
                //'-'
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_LPAREN :
                //'('
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_RPAREN :
                //')'
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_LT :
                //'<'
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_LTEQ :
                //'<='
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_LTGT :
                //'<>'
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_EQ :
                //'='
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_GT :
                //'>'
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_GTEQ :
                //'>='
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_AND :
                //and
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_AS :
                //as
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_BACK :
                //back
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_BETWEEN :
                //between
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_CATALOG :
                //catalog
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_CLONE :
                //clone
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_COLUMNS :
                //columns
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_DECIMAL :
                //Decimal
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_DEFAULT :
                //default
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_EXPORT :
                //export
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_HTML :
                //html
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_IDENTIFIER :
                //Identifier
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_IMPORT :
                //import
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_INTEGER :
                //Integer
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_IS :
                //is
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_JSON :
                //json
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_LIKE :
                //like
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_LOAD :
                //load
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_NEWLINE :
                //NewLine
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_NOT :
                //not
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_NULL :
                //null
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_ON :
                //on
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_PASSWORD :
                //password
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_RELATE :
                //relate
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_REMOVE :
                //remove
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_ROOT :
                //root
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_SERVER :
                //server
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_SQL :
                //sql
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_STRINGLITERAL :
                //StringLiteral
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_TABLE :
                //table
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_TABLES :
                //tables
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_TO :
                //to
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_TOP :
                //top
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_UNRELATE :
                //unrelate
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_USER :
                //user
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_WHERE :
                //where
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_XML :
                //xml
                //Token Kind: 1
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_ASEXP :
                //<AsExp>
                //Token Kind: 0
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_BACKEXP :
                //<BackExp>
                //Token Kind: 0
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_CATALOGEXP :
                //<CatalogExp>
                //Token Kind: 0
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_CLONEEXP :
                //<CloneExp>
                //Token Kind: 0
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_COLUMNSEXP :
                //<ColumnsExp>
                //Token Kind: 0
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_COMMANDEXP :
                //<CommandExp>
                //Token Kind: 0
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_EXPORTEXP :
                //<ExportExp>
                //Token Kind: 0
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_EXPRESSION :
                //<Expression>
                //Token Kind: 0
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_IMPORTEXP :
                //<ImportExp>
                //Token Kind: 0
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_LOADEXP :
                //<LoadExp>
                //Token Kind: 0
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_NEGATEEXP :
                //<Negate Exp>
                //Token Kind: 0
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_NL :
                //<nl>
                //Token Kind: 0
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_NLOPT :
                //<nl Opt>
                //Token Kind: 0
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_PASSWORDEXP :
                //<PasswordExp>
                //Token Kind: 0
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_RELATEEXP :
                //<RelateExp>
                //Token Kind: 0
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_REMOVEEXP :
                //<RemoveExp>
                //Token Kind: 0
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_ROOTEXP :
                //<RootExp>
                //Token Kind: 0
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_SERVEREXP :
                //<ServerExp>
                //Token Kind: 0
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_START :
                //<Start>
                //Token Kind: 0
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_TABLEEXP :
                //<TableExp>
                //Token Kind: 0
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_TABLESEXP :
                //<TablesExp>
                //Token Kind: 0
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_TOPNEXP :
                //<TopnExp>
                //Token Kind: 0
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_UNRELATEEXP :
                //<UnrelateExp>
                //Token Kind: 0
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_USEREXP :
                //<UserExp>
                //Token Kind: 0
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                case (int)SymbolConstants.SYMBOL_VALUE :
                //<Value>
                //Token Kind: 0
                //todo: uncomment the next line if it's a terminal token ( if Token Kind = 1 )
                // return m_parser.TokenString;
                return null;

                default:
                    throw new SymbolException("You don't want the text of a non-terminal symbol");

            }
            
        }

        public ASTNode CreateASTNode()
        {
            switch (m_parser.ReductionRule.Index)
            {
                case (int)RuleConstants.RULE_NL_NEWLINE :
                //<nl> ::= NewLine <nl>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_NL_NEWLINE2 :
                //<nl> ::= NewLine
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_NLOPT_NEWLINE :
                //<nl Opt> ::= NewLine <nl Opt>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_NLOPT :
                //<nl Opt> ::= 
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_START :
                //<Start> ::= <nl Opt> <CommandExp> <nl Opt>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP :
                //<CommandExp> ::= <ImportExp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP2 :
                //<CommandExp> ::= <CloneExp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP3 :
                //<CommandExp> ::= <RemoveExp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP4 :
                //<CommandExp> ::= <LoadExp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP5 :
                //<CommandExp> ::= <TableExp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP6 :
                //<CommandExp> ::= <RelateExp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP7 :
                //<CommandExp> ::= <UnrelateExp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP8 :
                //<CommandExp> ::= <ExportExp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP9 :
                //<CommandExp> ::= <BackExp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP10 :
                //<CommandExp> ::= <RootExp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP11 :
                //<CommandExp> ::= <TablesExp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP12 :
                //<CommandExp> ::= <ColumnsExp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_COMMANDEXP13 :
                //<CommandExp> ::= <TopnExp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_IMPORTEXP_IMPORT :
                //<ImportExp> ::= import <CatalogExp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_IMPORTEXP_IMPORT2 :
                //<ImportExp> ::= import <CatalogExp> <UserExp> <PasswordExp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_IMPORTEXP_IMPORT3 :
                //<ImportExp> ::= import <CatalogExp> <ServerExp> <UserExp> <PasswordExp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_CLONEEXP_CLONE :
                //<CloneExp> ::= clone
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_CLONEEXP_CLONE2 :
                //<CloneExp> ::= clone <CatalogExp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_CLONEEXP_CLONE3 :
                //<CloneExp> ::= clone <AsExp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_CLONEEXP_CLONE4 :
                //<CloneExp> ::= clone <CatalogExp> <AsExp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_REMOVEEXP_REMOVE :
                //<RemoveExp> ::= remove
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_REMOVEEXP_REMOVE2 :
                //<RemoveExp> ::= remove <CatalogExp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_LOADEXP_LOAD :
                //<LoadExp> ::= load
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_LOADEXP_LOAD2 :
                //<LoadExp> ::= load <CatalogExp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_TABLEEXP_TABLE :
                //<TableExp> ::= table
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_TABLEEXP_TABLE_IDENTIFIER :
                //<TableExp> ::= table Identifier
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_TABLEEXP_TABLE_IDENTIFIER_DEFAULT :
                //<TableExp> ::= table Identifier default
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_TABLEEXP_TABLE_IDENTIFIER_DEFAULT_WHERE :
                //<TableExp> ::= table Identifier default where <Expression>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_TABLEEXP_TABLE_IDENTIFIER_WHERE :
                //<TableExp> ::= table Identifier where <Expression>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_RELATEEXP_RELATE_TO_IDENTIFIER_ON_IDENTIFIER_EQ_IDENTIFIER :
                //<RelateExp> ::= relate to Identifier on Identifier '=' Identifier
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_RELATEEXP_RELATE_IDENTIFIER_TO_IDENTIFIER_ON_IDENTIFIER_EQ_IDENTIFIER :
                //<RelateExp> ::= relate Identifier to Identifier on Identifier '=' Identifier
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_UNRELATEEXP_UNRELATE_TO_IDENTIFIER :
                //<UnrelateExp> ::= unrelate to Identifier
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_UNRELATEEXP_UNRELATE_IDENTIFIER_TO_IDENTIFIER :
                //<UnrelateExp> ::= unrelate Identifier to Identifier
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_AS_SQL :
                //<ExportExp> ::= export as sql
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_AS_HTML :
                //<ExportExp> ::= export as html
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_AS_JSON :
                //<ExportExp> ::= export as json
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_AS_XML :
                //<ExportExp> ::= export as xml
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_SQL :
                //<ExportExp> ::= export Identifier as sql
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_HTML :
                //<ExportExp> ::= export Identifier as html
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_JSON :
                //<ExportExp> ::= export Identifier as json
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_EXPORTEXP_EXPORT_IDENTIFIER_AS_XML :
                //<ExportExp> ::= export Identifier as xml
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_BACKEXP_BACK :
                //<BackExp> ::= back
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_ROOTEXP_ROOT :
                //<RootExp> ::= root
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_TABLESEXP_TABLES :
                //<TablesExp> ::= tables
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_TABLESEXP_TABLES_INTEGER :
                //<TablesExp> ::= tables Integer
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_COLUMNSEXP_COLUMNS :
                //<ColumnsExp> ::= columns
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_COLUMNSEXP_COLUMNS_INTEGER :
                //<ColumnsExp> ::= columns Integer
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_TOPNEXP_TOP_INTEGER :
                //<TopnExp> ::= top Integer
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_CATALOGEXP_CATALOG_IDENTIFIER :
                //<CatalogExp> ::= catalog Identifier
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_USEREXP_USER_IDENTIFIER :
                //<UserExp> ::= user Identifier
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_PASSWORDEXP_PASSWORD_IDENTIFIER :
                //<PasswordExp> ::= password Identifier
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_SERVEREXP_SERVER_IDENTIFIER :
                //<ServerExp> ::= server Identifier
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_ASEXP_AS_IDENTIFIER :
                //<AsExp> ::= as Identifier
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_GT :
                //<Expression> ::= <Expression> '>' <Negate Exp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_LT :
                //<Expression> ::= <Expression> '<' <Negate Exp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_LTEQ :
                //<Expression> ::= <Expression> '<=' <Negate Exp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_GTEQ :
                //<Expression> ::= <Expression> '>=' <Negate Exp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_EQ :
                //<Expression> ::= <Expression> '=' <Negate Exp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_LTGT :
                //<Expression> ::= <Expression> '<>' <Negate Exp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_IS :
                //<Expression> ::= <Expression> is <Negate Exp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_BETWEEN_AND :
                //<Expression> ::= <Expression> between <Negate Exp> and <Negate Exp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_LIKE_STRINGLITERAL :
                //<Expression> ::= <Expression> like StringLiteral
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION :
                //<Expression> ::= <Negate Exp>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_NEGATEEXP_MINUS :
                //<Negate Exp> ::= '-' <Value>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_NEGATEEXP_NOT :
                //<Negate Exp> ::= not <Value>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_NEGATEEXP :
                //<Negate Exp> ::= <Value>
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_VALUE_IDENTIFIER :
                //<Value> ::= Identifier
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_VALUE_LPAREN_RPAREN :
                //<Value> ::= '(' <Expression> ')'
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_VALUE_NULL :
                //<Value> ::= null
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_VALUE_INTEGER :
                //<Value> ::= Integer
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_VALUE_DECIMAL :
                //<Value> ::= Decimal
                //todo: Perhaps create an object in the AST.
                return null;

                case (int)RuleConstants.RULE_VALUE_STRINGLITERAL :
                //<Value> ::= StringLiteral
                //todo: Perhaps create an object in the AST.
                return null;

                default:
                    throw new RuleException("Unknown rule: Does your CGT Match your Code Revision?");
            }
            
        }

    }
    
}
