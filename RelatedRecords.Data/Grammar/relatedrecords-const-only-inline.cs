enum SymbolConstants : int
{
   /// <c> (EOF) </c>
   SYM_EOF           = 0,     
   /// <c> (Error) </c>
   SYM_ERROR         = 1,     
   /// <c> Whitespace </c>
   SYM_WHITESPACE    = 2,     
   /// <c> '-' </c>
   SYM_MINUS         = 3,     
   /// <c> '(' </c>
   SYM_LPAREN        = 4,     
   /// <c> ')' </c>
   SYM_RPAREN        = 5,     
   /// <c> '&lt;' </c>
   SYM_LT            = 6,     
   /// <c> '&lt;=' </c>
   SYM_LTEQ          = 7,     
   /// <c> '&lt;&gt;' </c>
   SYM_LTGT          = 8,     
   /// <c> '=' </c>
   SYM_EQ            = 9,     
   /// <c> '&gt;' </c>
   SYM_GT            = 10,     
   /// <c> '&gt;=' </c>
   SYM_GTEQ          = 11,     
   /// <c> and </c>
   SYM_AND           = 12,     
   /// <c> as </c>
   SYM_AS            = 13,     
   /// <c> back </c>
   SYM_BACK          = 14,     
   /// <c> between </c>
   SYM_BETWEEN       = 15,     
   /// <c> catalog </c>
   SYM_CATALOG       = 16,     
   /// <c> clone </c>
   SYM_CLONE         = 17,     
   /// <c> columns </c>
   SYM_COLUMNS       = 18,     
   /// <c> Decimal </c>
   SYM_DECIMAL       = 19,     
   /// <c> default </c>
   SYM_DEFAULT       = 20,     
   /// <c> export </c>
   SYM_EXPORT        = 21,     
   /// <c> html </c>
   SYM_HTML          = 22,     
   /// <c> Identifier </c>
   SYM_IDENTIFIER    = 23,     
   /// <c> import </c>
   SYM_IMPORT        = 24,     
   /// <c> Integer </c>
   SYM_INTEGER       = 25,     
   /// <c> is </c>
   SYM_IS            = 26,     
   /// <c> json </c>
   SYM_JSON          = 27,     
   /// <c> like </c>
   SYM_LIKE          = 28,     
   /// <c> load </c>
   SYM_LOAD          = 29,     
   /// <c> NewLine </c>
   SYM_NEWLINE       = 30,     
   /// <c> not </c>
   SYM_NOT           = 31,     
   /// <c> null </c>
   SYM_NULL          = 32,     
   /// <c> on </c>
   SYM_ON            = 33,     
   /// <c> password </c>
   SYM_PASSWORD      = 34,     
   /// <c> relate </c>
   SYM_RELATE        = 35,     
   /// <c> remove </c>
   SYM_REMOVE        = 36,     
   /// <c> root </c>
   SYM_ROOT          = 37,     
   /// <c> server </c>
   SYM_SERVER        = 38,     
   /// <c> sql </c>
   SYM_SQL           = 39,     
   /// <c> StringLiteral </c>
   SYM_STRINGLITERAL = 40,     
   /// <c> table </c>
   SYM_TABLE         = 41,     
   /// <c> tables </c>
   SYM_TABLES        = 42,     
   /// <c> to </c>
   SYM_TO            = 43,     
   /// <c> top </c>
   SYM_TOP           = 44,     
   /// <c> unrelate </c>
   SYM_UNRELATE      = 45,     
   /// <c> user </c>
   SYM_USER          = 46,     
   /// <c> where </c>
   SYM_WHERE         = 47,     
   /// <c> xml </c>
   SYM_XML           = 48,     
   /// <c> &lt;AsExp&gt; </c>
   SYM_ASEXP         = 49,     
   /// <c> &lt;BackExp&gt; </c>
   SYM_BACKEXP       = 50,     
   /// <c> &lt;CatalogExp&gt; </c>
   SYM_CATALOGEXP    = 51,     
   /// <c> &lt;CloneExp&gt; </c>
   SYM_CLONEEXP      = 52,     
   /// <c> &lt;ColumnsExp&gt; </c>
   SYM_COLUMNSEXP    = 53,     
   /// <c> &lt;CommandExp&gt; </c>
   SYM_COMMANDEXP    = 54,     
   /// <c> &lt;ExportExp&gt; </c>
   SYM_EXPORTEXP     = 55,     
   /// <c> &lt;Expression&gt; </c>
   SYM_EXPRESSION    = 56,     
   /// <c> &lt;ImportExp&gt; </c>
   SYM_IMPORTEXP     = 57,     
   /// <c> &lt;LoadExp&gt; </c>
   SYM_LOADEXP       = 58,     
   /// <c> &lt;Negate Exp&gt; </c>
   SYM_NEGATEEXP     = 59,     
   /// <c> &lt;nl&gt; </c>
   SYM_NL            = 60,     
   /// <c> &lt;nl Opt&gt; </c>
   SYM_NLOPT         = 61,     
   /// <c> &lt;PasswordExp&gt; </c>
   SYM_PASSWORDEXP   = 62,     
   /// <c> &lt;RelateExp&gt; </c>
   SYM_RELATEEXP     = 63,     
   /// <c> &lt;RemoveExp&gt; </c>
   SYM_REMOVEEXP     = 64,     
   /// <c> &lt;RootExp&gt; </c>
   SYM_ROOTEXP       = 65,     
   /// <c> &lt;ServerExp&gt; </c>
   SYM_SERVEREXP     = 66,     
   /// <c> &lt;Start&gt; </c>
   SYM_START         = 67,     
   /// <c> &lt;TableExp&gt; </c>
   SYM_TABLEEXP      = 68,     
   /// <c> &lt;TablesExp&gt; </c>
   SYM_TABLESEXP     = 69,     
   /// <c> &lt;TopnExp&gt; </c>
   SYM_TOPNEXP       = 70,     
   /// <c> &lt;UnrelateExp&gt; </c>
   SYM_UNRELATEEXP   = 71,     
   /// <c> &lt;UserExp&gt; </c>
   SYM_USEREXP       = 72,     
   /// <c> &lt;Value&gt; </c>
   SYM_VALUE         = 73      
};

enum RuleConstants : int
{
   /// <c> &lt;nl&gt; ::= NewLine &lt;nl&gt; </c>
   PROD_NL_NEWLINE                                                            = 0,    
   /// <c> &lt;nl&gt; ::= NewLine </c>
   PROD_NL_NEWLINE2                                                           = 1,    
   /// <c> &lt;nl Opt&gt; ::= NewLine &lt;nl Opt&gt; </c>
   PROD_NLOPT_NEWLINE                                                         = 2,    
   /// <c> &lt;nl Opt&gt; ::=  </c>
   PROD_NLOPT                                                                 = 3,    
   /// <c> &lt;Start&gt; ::= &lt;nl Opt&gt; &lt;CommandExp&gt; &lt;nl Opt&gt; </c>
   PROD_START                                                                 = 4,    
   /// <c> &lt;CommandExp&gt; ::= &lt;ImportExp&gt; </c>
   PROD_COMMANDEXP                                                            = 5,    
   /// <c> &lt;CommandExp&gt; ::= &lt;CloneExp&gt; </c>
   PROD_COMMANDEXP2                                                           = 6,    
   /// <c> &lt;CommandExp&gt; ::= &lt;RemoveExp&gt; </c>
   PROD_COMMANDEXP3                                                           = 7,    
   /// <c> &lt;CommandExp&gt; ::= &lt;LoadExp&gt; </c>
   PROD_COMMANDEXP4                                                           = 8,    
   /// <c> &lt;CommandExp&gt; ::= &lt;TableExp&gt; </c>
   PROD_COMMANDEXP5                                                           = 9,    
   /// <c> &lt;CommandExp&gt; ::= &lt;RelateExp&gt; </c>
   PROD_COMMANDEXP6                                                           = 10,    
   /// <c> &lt;CommandExp&gt; ::= &lt;UnrelateExp&gt; </c>
   PROD_COMMANDEXP7                                                           = 11,    
   /// <c> &lt;CommandExp&gt; ::= &lt;ExportExp&gt; </c>
   PROD_COMMANDEXP8                                                           = 12,    
   /// <c> &lt;CommandExp&gt; ::= &lt;BackExp&gt; </c>
   PROD_COMMANDEXP9                                                           = 13,    
   /// <c> &lt;CommandExp&gt; ::= &lt;RootExp&gt; </c>
   PROD_COMMANDEXP10                                                          = 14,    
   /// <c> &lt;CommandExp&gt; ::= &lt;TablesExp&gt; </c>
   PROD_COMMANDEXP11                                                          = 15,    
   /// <c> &lt;CommandExp&gt; ::= &lt;ColumnsExp&gt; </c>
   PROD_COMMANDEXP12                                                          = 16,    
   /// <c> &lt;CommandExp&gt; ::= &lt;TopnExp&gt; </c>
   PROD_COMMANDEXP13                                                          = 17,    
   /// <c> &lt;ImportExp&gt; ::= import &lt;CatalogExp&gt; </c>
   PROD_IMPORTEXP_IMPORT                                                      = 18,    
   /// <c> &lt;ImportExp&gt; ::= import &lt;CatalogExp&gt; &lt;UserExp&gt; &lt;PasswordExp&gt; </c>
   PROD_IMPORTEXP_IMPORT2                                                     = 19,    
   /// <c> &lt;ImportExp&gt; ::= import &lt;CatalogExp&gt; &lt;ServerExp&gt; &lt;UserExp&gt; &lt;PasswordExp&gt; </c>
   PROD_IMPORTEXP_IMPORT3                                                     = 20,    
   /// <c> &lt;CloneExp&gt; ::= clone </c>
   PROD_CLONEEXP_CLONE                                                        = 21,    
   /// <c> &lt;CloneExp&gt; ::= clone &lt;CatalogExp&gt; </c>
   PROD_CLONEEXP_CLONE2                                                       = 22,    
   /// <c> &lt;CloneExp&gt; ::= clone &lt;AsExp&gt; </c>
   PROD_CLONEEXP_CLONE3                                                       = 23,    
   /// <c> &lt;CloneExp&gt; ::= clone &lt;CatalogExp&gt; &lt;AsExp&gt; </c>
   PROD_CLONEEXP_CLONE4                                                       = 24,    
   /// <c> &lt;RemoveExp&gt; ::= remove </c>
   PROD_REMOVEEXP_REMOVE                                                      = 25,    
   /// <c> &lt;RemoveExp&gt; ::= remove &lt;CatalogExp&gt; </c>
   PROD_REMOVEEXP_REMOVE2                                                     = 26,    
   /// <c> &lt;LoadExp&gt; ::= load </c>
   PROD_LOADEXP_LOAD                                                          = 27,    
   /// <c> &lt;LoadExp&gt; ::= load &lt;CatalogExp&gt; </c>
   PROD_LOADEXP_LOAD2                                                         = 28,    
   /// <c> &lt;TableExp&gt; ::= table </c>
   PROD_TABLEEXP_TABLE                                                        = 29,    
   /// <c> &lt;TableExp&gt; ::= table Identifier </c>
   PROD_TABLEEXP_TABLE_IDENTIFIER                                             = 30,    
   /// <c> &lt;TableExp&gt; ::= table Identifier default </c>
   PROD_TABLEEXP_TABLE_IDENTIFIER_DEFAULT                                     = 31,    
   /// <c> &lt;TableExp&gt; ::= table Identifier default where &lt;Expression&gt; </c>
   PROD_TABLEEXP_TABLE_IDENTIFIER_DEFAULT_WHERE                               = 32,    
   /// <c> &lt;TableExp&gt; ::= table Identifier where &lt;Expression&gt; </c>
   PROD_TABLEEXP_TABLE_IDENTIFIER_WHERE                                       = 33,    
   /// <c> &lt;RelateExp&gt; ::= relate to Identifier on Identifier '=' Identifier </c>
   PROD_RELATEEXP_RELATE_TO_IDENTIFIER_ON_IDENTIFIER_EQ_IDENTIFIER            = 34,    
   /// <c> &lt;RelateExp&gt; ::= relate Identifier to Identifier on Identifier '=' Identifier </c>
   PROD_RELATEEXP_RELATE_IDENTIFIER_TO_IDENTIFIER_ON_IDENTIFIER_EQ_IDENTIFIER = 35,    
   /// <c> &lt;UnrelateExp&gt; ::= unrelate to Identifier </c>
   PROD_UNRELATEEXP_UNRELATE_TO_IDENTIFIER                                    = 36,    
   /// <c> &lt;UnrelateExp&gt; ::= unrelate Identifier to Identifier </c>
   PROD_UNRELATEEXP_UNRELATE_IDENTIFIER_TO_IDENTIFIER                         = 37,    
   /// <c> &lt;ExportExp&gt; ::= export as sql </c>
   PROD_EXPORTEXP_EXPORT_AS_SQL                                               = 38,    
   /// <c> &lt;ExportExp&gt; ::= export as html </c>
   PROD_EXPORTEXP_EXPORT_AS_HTML                                              = 39,    
   /// <c> &lt;ExportExp&gt; ::= export as json </c>
   PROD_EXPORTEXP_EXPORT_AS_JSON                                              = 40,    
   /// <c> &lt;ExportExp&gt; ::= export as xml </c>
   PROD_EXPORTEXP_EXPORT_AS_XML                                               = 41,    
   /// <c> &lt;ExportExp&gt; ::= export Identifier as sql </c>
   PROD_EXPORTEXP_EXPORT_IDENTIFIER_AS_SQL                                    = 42,    
   /// <c> &lt;ExportExp&gt; ::= export Identifier as html </c>
   PROD_EXPORTEXP_EXPORT_IDENTIFIER_AS_HTML                                   = 43,    
   /// <c> &lt;ExportExp&gt; ::= export Identifier as json </c>
   PROD_EXPORTEXP_EXPORT_IDENTIFIER_AS_JSON                                   = 44,    
   /// <c> &lt;ExportExp&gt; ::= export Identifier as xml </c>
   PROD_EXPORTEXP_EXPORT_IDENTIFIER_AS_XML                                    = 45,    
   /// <c> &lt;BackExp&gt; ::= back </c>
   PROD_BACKEXP_BACK                                                          = 46,    
   /// <c> &lt;RootExp&gt; ::= root </c>
   PROD_ROOTEXP_ROOT                                                          = 47,    
   /// <c> &lt;TablesExp&gt; ::= tables </c>
   PROD_TABLESEXP_TABLES                                                      = 48,    
   /// <c> &lt;TablesExp&gt; ::= tables Integer </c>
   PROD_TABLESEXP_TABLES_INTEGER                                              = 49,    
   /// <c> &lt;ColumnsExp&gt; ::= columns </c>
   PROD_COLUMNSEXP_COLUMNS                                                    = 50,    
   /// <c> &lt;ColumnsExp&gt; ::= columns Integer </c>
   PROD_COLUMNSEXP_COLUMNS_INTEGER                                            = 51,    
   /// <c> &lt;TopnExp&gt; ::= top Integer </c>
   PROD_TOPNEXP_TOP_INTEGER                                                   = 52,    
   /// <c> &lt;CatalogExp&gt; ::= catalog Identifier </c>
   PROD_CATALOGEXP_CATALOG_IDENTIFIER                                         = 53,    
   /// <c> &lt;UserExp&gt; ::= user Identifier </c>
   PROD_USEREXP_USER_IDENTIFIER                                               = 54,    
   /// <c> &lt;PasswordExp&gt; ::= password Identifier </c>
   PROD_PASSWORDEXP_PASSWORD_IDENTIFIER                                       = 55,    
   /// <c> &lt;ServerExp&gt; ::= server Identifier </c>
   PROD_SERVEREXP_SERVER_IDENTIFIER                                           = 56,    
   /// <c> &lt;AsExp&gt; ::= as Identifier </c>
   PROD_ASEXP_AS_IDENTIFIER                                                   = 57,    
   /// <c> &lt;Expression&gt; ::= &lt;Expression&gt; '&gt;' &lt;Negate Exp&gt; </c>
   PROD_EXPRESSION_GT                                                         = 58,    
   /// <c> &lt;Expression&gt; ::= &lt;Expression&gt; '&lt;' &lt;Negate Exp&gt; </c>
   PROD_EXPRESSION_LT                                                         = 59,    
   /// <c> &lt;Expression&gt; ::= &lt;Expression&gt; '&lt;=' &lt;Negate Exp&gt; </c>
   PROD_EXPRESSION_LTEQ                                                       = 60,    
   /// <c> &lt;Expression&gt; ::= &lt;Expression&gt; '&gt;=' &lt;Negate Exp&gt; </c>
   PROD_EXPRESSION_GTEQ                                                       = 61,    
   /// <c> &lt;Expression&gt; ::= &lt;Expression&gt; '=' &lt;Negate Exp&gt; </c>
   PROD_EXPRESSION_EQ                                                         = 62,    
   /// <c> &lt;Expression&gt; ::= &lt;Expression&gt; '&lt;&gt;' &lt;Negate Exp&gt; </c>
   PROD_EXPRESSION_LTGT                                                       = 63,    
   /// <c> &lt;Expression&gt; ::= &lt;Expression&gt; is &lt;Negate Exp&gt; </c>
   PROD_EXPRESSION_IS                                                         = 64,    
   /// <c> &lt;Expression&gt; ::= &lt;Expression&gt; between &lt;Negate Exp&gt; and &lt;Negate Exp&gt; </c>
   PROD_EXPRESSION_BETWEEN_AND                                                = 65,    
   /// <c> &lt;Expression&gt; ::= &lt;Expression&gt; like StringLiteral </c>
   PROD_EXPRESSION_LIKE_STRINGLITERAL                                         = 66,    
   /// <c> &lt;Expression&gt; ::= &lt;Negate Exp&gt; </c>
   PROD_EXPRESSION                                                            = 67,    
   /// <c> &lt;Negate Exp&gt; ::= '-' &lt;Value&gt; </c>
   PROD_NEGATEEXP_MINUS                                                       = 68,    
   /// <c> &lt;Negate Exp&gt; ::= not &lt;Value&gt; </c>
   PROD_NEGATEEXP_NOT                                                         = 69,    
   /// <c> &lt;Negate Exp&gt; ::= &lt;Value&gt; </c>
   PROD_NEGATEEXP                                                             = 70,    
   /// <c> &lt;Value&gt; ::= Identifier </c>
   PROD_VALUE_IDENTIFIER                                                      = 71,    
   /// <c> &lt;Value&gt; ::= '(' &lt;Expression&gt; ')' </c>
   PROD_VALUE_LPAREN_RPAREN                                                   = 72,    
   /// <c> &lt;Value&gt; ::= null </c>
   PROD_VALUE_NULL                                                            = 73,    
   /// <c> &lt;Value&gt; ::= Integer </c>
   PROD_VALUE_INTEGER                                                         = 74,    
   /// <c> &lt;Value&gt; ::= Decimal </c>
   PROD_VALUE_DECIMAL                                                         = 75,    
   /// <c> &lt;Value&gt; ::= StringLiteral </c>
   PROD_VALUE_STRINGLITERAL                                                   = 76     
};
