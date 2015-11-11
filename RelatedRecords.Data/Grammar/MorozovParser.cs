
using System;
using System.Reflection;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Collections;

using GoldParser;

namespace RelatedRecords.Parser
{
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
        
        public static GoldParser.Parser CreateParser(TextReader reader)
        {
           if (_init)
           {
                return new GoldParser.Parser(reader, m_grammar);
           }
           throw new Exception("You must first Initialize the Factory before creating a parser!");
        }
    }
        
    public abstract class ASTNode
    {
        public abstract bool IsTerminal
        {
            get;
        }
    }
    
    /// <summary>
    /// Derive this class for Terminal AST Nodes
    /// </summary>
    public class TerminalNode : ASTNode
    {
        private Symbol m_symbol;
        private string m_text;
        private int m_lineNumber;
        private int m_linePosition;

        public TerminalNode(GoldParser.Parser theParser)
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
    public class NonTerminalNode : ASTNode
    {
        private int m_reductionNumber;
        private Rule m_rule;
        private ArrayList m_array = new ArrayList();

        public NonTerminalNode(GoldParser.Parser theParser)
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
            get { return m_array[index] as ASTNode; }
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

    public class MorozovParser
    {
        ParserContext m_context;
        ASTNode m_AST;
        string m_errorString;
        GoldParser.Parser m_parser;
        
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
            m_context = new ParserContext(m_parser);
            
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

    public class ParserContext
    {
        // instance fields
        private GoldParser.Parser m_parser;
        
        private TextReader m_inputReader;
        
        // constructor
        public ParserContext(GoldParser.Parser prser)
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
