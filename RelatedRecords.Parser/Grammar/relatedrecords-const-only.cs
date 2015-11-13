enum SymbolIndex : int
{
   @EOF           =  0, // (EOF)
   @ERROR         =  1, // (Error)
   @WHITESPACE    =  2, // Whitespace
   @MINUS         =  3, // '-'
   @LPAREN        =  4, // '('
   @RPAREN        =  5, // ')'
   @LT            =  6, // '<'
   @LTEQ          =  7, // '<='
   @LTGT          =  8, // '<>'
   @EQ            =  9, // '='
   @GT            = 10, // '>'
   @GTEQ          = 11, // '>='
   @AND           = 12, // and
   @AS            = 13, // as
   @BACK          = 14, // back
   @BETWEEN       = 15, // between
   @CATALOG       = 16, // catalog
   @CLONE         = 17, // clone
   @COLUMNS       = 18, // columns
   @DECIMAL       = 19, // Decimal
   @DEFAULT       = 20, // default
   @EXPORT        = 21, // export
   @HTML          = 22, // html
   @IDENTIFIER    = 23, // Identifier
   @IMPORT        = 24, // import
   @INTEGER       = 25, // Integer
   @IS            = 26, // is
   @JSON          = 27, // json
   @LIKE          = 28, // like
   @LOAD          = 29, // load
   @NEWLINE       = 30, // NewLine
   @NOT           = 31, // not
   @NULL          = 32, // null
   @ON            = 33, // on
   @PASSWORD      = 34, // password
   @RELATE        = 35, // relate
   @REMOVE        = 36, // remove
   @ROOT          = 37, // root
   @SERVER        = 38, // server
   @SQL           = 39, // sql
   @STRINGLITERAL = 40, // StringLiteral
   @TABLE         = 41, // table
   @TABLES        = 42, // tables
   @TO            = 43, // to
   @TOP           = 44, // top
   @UNRELATE      = 45, // unrelate
   @USER          = 46, // user
   @WHERE         = 47, // where
   @XML           = 48, // xml
   @ASEXP         = 49, // <AsExp>
   @BACKEXP       = 50, // <BackExp>
   @CATALOGEXP    = 51, // <CatalogExp>
   @CLONEEXP      = 52, // <CloneExp>
   @COLUMNSEXP    = 53, // <ColumnsExp>
   @COMMANDEXP    = 54, // <CommandExp>
   @EXPORTEXP     = 55, // <ExportExp>
   @EXPRESSION    = 56, // <Expression>
   @IMPORTEXP     = 57, // <ImportExp>
   @LOADEXP       = 58, // <LoadExp>
   @NEGATEEXP     = 59, // <Negate Exp>
   @NL            = 60, // <nl>
   @NLOPT         = 61, // <nl Opt>
   @PASSWORDEXP   = 62, // <PasswordExp>
   @RELATEEXP     = 63, // <RelateExp>
   @REMOVEEXP     = 64, // <RemoveExp>
   @ROOTEXP       = 65, // <RootExp>
   @SERVEREXP     = 66, // <ServerExp>
   @START         = 67, // <Start>
   @TABLEEXP      = 68, // <TableExp>
   @TABLESEXP     = 69, // <TablesExp>
   @TOPNEXP       = 70, // <TopnExp>
   @UNRELATEEXP   = 71, // <UnrelateExp>
   @USEREXP       = 72, // <UserExp>
   @VALUE         = 73  // <Value>
};

enum ProdIndex : int
{
   @NL_NEWLINE                                                            =  0, // <nl> ::= NewLine <nl>
   @NL_NEWLINE2                                                           =  1, // <nl> ::= NewLine
   @NLOPT_NEWLINE                                                         =  2, // <nl Opt> ::= NewLine <nl Opt>
   @NLOPT                                                                 =  3, // <nl Opt> ::= 
   @START                                                                 =  4, // <Start> ::= <nl Opt> <CommandExp> <nl Opt>
   @COMMANDEXP                                                            =  5, // <CommandExp> ::= <ImportExp>
   @COMMANDEXP2                                                           =  6, // <CommandExp> ::= <CloneExp>
   @COMMANDEXP3                                                           =  7, // <CommandExp> ::= <RemoveExp>
   @COMMANDEXP4                                                           =  8, // <CommandExp> ::= <LoadExp>
   @COMMANDEXP5                                                           =  9, // <CommandExp> ::= <TableExp>
   @COMMANDEXP6                                                           = 10, // <CommandExp> ::= <RelateExp>
   @COMMANDEXP7                                                           = 11, // <CommandExp> ::= <UnrelateExp>
   @COMMANDEXP8                                                           = 12, // <CommandExp> ::= <ExportExp>
   @COMMANDEXP9                                                           = 13, // <CommandExp> ::= <BackExp>
   @COMMANDEXP10                                                          = 14, // <CommandExp> ::= <RootExp>
   @COMMANDEXP11                                                          = 15, // <CommandExp> ::= <TablesExp>
   @COMMANDEXP12                                                          = 16, // <CommandExp> ::= <ColumnsExp>
   @COMMANDEXP13                                                          = 17, // <CommandExp> ::= <TopnExp>
   @IMPORTEXP_IMPORT                                                      = 18, // <ImportExp> ::= import <CatalogExp>
   @IMPORTEXP_IMPORT2                                                     = 19, // <ImportExp> ::= import <CatalogExp> <UserExp> <PasswordExp>
   @IMPORTEXP_IMPORT3                                                     = 20, // <ImportExp> ::= import <CatalogExp> <ServerExp> <UserExp> <PasswordExp>
   @CLONEEXP_CLONE                                                        = 21, // <CloneExp> ::= clone
   @CLONEEXP_CLONE2                                                       = 22, // <CloneExp> ::= clone <CatalogExp>
   @CLONEEXP_CLONE3                                                       = 23, // <CloneExp> ::= clone <AsExp>
   @CLONEEXP_CLONE4                                                       = 24, // <CloneExp> ::= clone <CatalogExp> <AsExp>
   @REMOVEEXP_REMOVE                                                      = 25, // <RemoveExp> ::= remove
   @REMOVEEXP_REMOVE2                                                     = 26, // <RemoveExp> ::= remove <CatalogExp>
   @LOADEXP_LOAD                                                          = 27, // <LoadExp> ::= load
   @LOADEXP_LOAD2                                                         = 28, // <LoadExp> ::= load <CatalogExp>
   @TABLEEXP_TABLE                                                        = 29, // <TableExp> ::= table
   @TABLEEXP_TABLE_IDENTIFIER                                             = 30, // <TableExp> ::= table Identifier
   @TABLEEXP_TABLE_IDENTIFIER_DEFAULT                                     = 31, // <TableExp> ::= table Identifier default
   @TABLEEXP_TABLE_IDENTIFIER_DEFAULT_WHERE                               = 32, // <TableExp> ::= table Identifier default where <Expression>
   @TABLEEXP_TABLE_IDENTIFIER_WHERE                                       = 33, // <TableExp> ::= table Identifier where <Expression>
   @RELATEEXP_RELATE_TO_IDENTIFIER_ON_IDENTIFIER_EQ_IDENTIFIER            = 34, // <RelateExp> ::= relate to Identifier on Identifier '=' Identifier
   @RELATEEXP_RELATE_IDENTIFIER_TO_IDENTIFIER_ON_IDENTIFIER_EQ_IDENTIFIER = 35, // <RelateExp> ::= relate Identifier to Identifier on Identifier '=' Identifier
   @UNRELATEEXP_UNRELATE_TO_IDENTIFIER                                    = 36, // <UnrelateExp> ::= unrelate to Identifier
   @UNRELATEEXP_UNRELATE_IDENTIFIER_TO_IDENTIFIER                         = 37, // <UnrelateExp> ::= unrelate Identifier to Identifier
   @EXPORTEXP_EXPORT_AS_SQL                                               = 38, // <ExportExp> ::= export as sql
   @EXPORTEXP_EXPORT_AS_HTML                                              = 39, // <ExportExp> ::= export as html
   @EXPORTEXP_EXPORT_AS_JSON                                              = 40, // <ExportExp> ::= export as json
   @EXPORTEXP_EXPORT_AS_XML                                               = 41, // <ExportExp> ::= export as xml
   @EXPORTEXP_EXPORT_IDENTIFIER_AS_SQL                                    = 42, // <ExportExp> ::= export Identifier as sql
   @EXPORTEXP_EXPORT_IDENTIFIER_AS_HTML                                   = 43, // <ExportExp> ::= export Identifier as html
   @EXPORTEXP_EXPORT_IDENTIFIER_AS_JSON                                   = 44, // <ExportExp> ::= export Identifier as json
   @EXPORTEXP_EXPORT_IDENTIFIER_AS_XML                                    = 45, // <ExportExp> ::= export Identifier as xml
   @BACKEXP_BACK                                                          = 46, // <BackExp> ::= back
   @ROOTEXP_ROOT                                                          = 47, // <RootExp> ::= root
   @TABLESEXP_TABLES                                                      = 48, // <TablesExp> ::= tables
   @TABLESEXP_TABLES_INTEGER                                              = 49, // <TablesExp> ::= tables Integer
   @COLUMNSEXP_COLUMNS                                                    = 50, // <ColumnsExp> ::= columns
   @COLUMNSEXP_COLUMNS_INTEGER                                            = 51, // <ColumnsExp> ::= columns Integer
   @TOPNEXP_TOP_INTEGER                                                   = 52, // <TopnExp> ::= top Integer
   @CATALOGEXP_CATALOG_IDENTIFIER                                         = 53, // <CatalogExp> ::= catalog Identifier
   @USEREXP_USER_IDENTIFIER                                               = 54, // <UserExp> ::= user Identifier
   @PASSWORDEXP_PASSWORD_IDENTIFIER                                       = 55, // <PasswordExp> ::= password Identifier
   @SERVEREXP_SERVER_IDENTIFIER                                           = 56, // <ServerExp> ::= server Identifier
   @ASEXP_AS_IDENTIFIER                                                   = 57, // <AsExp> ::= as Identifier
   @EXPRESSION_GT                                                         = 58, // <Expression> ::= <Expression> '>' <Negate Exp>
   @EXPRESSION_LT                                                         = 59, // <Expression> ::= <Expression> '<' <Negate Exp>
   @EXPRESSION_LTEQ                                                       = 60, // <Expression> ::= <Expression> '<=' <Negate Exp>
   @EXPRESSION_GTEQ                                                       = 61, // <Expression> ::= <Expression> '>=' <Negate Exp>
   @EXPRESSION_EQ                                                         = 62, // <Expression> ::= <Expression> '=' <Negate Exp>
   @EXPRESSION_LTGT                                                       = 63, // <Expression> ::= <Expression> '<>' <Negate Exp>
   @EXPRESSION_IS                                                         = 64, // <Expression> ::= <Expression> is <Negate Exp>
   @EXPRESSION_BETWEEN_AND                                                = 65, // <Expression> ::= <Expression> between <Negate Exp> and <Negate Exp>
   @EXPRESSION_LIKE_STRINGLITERAL                                         = 66, // <Expression> ::= <Expression> like StringLiteral
   @EXPRESSION                                                            = 67, // <Expression> ::= <Negate Exp>
   @NEGATEEXP_MINUS                                                       = 68, // <Negate Exp> ::= '-' <Value>
   @NEGATEEXP_NOT                                                         = 69, // <Negate Exp> ::= not <Value>
   @NEGATEEXP                                                             = 70, // <Negate Exp> ::= <Value>
   @VALUE_IDENTIFIER                                                      = 71, // <Value> ::= Identifier
   @VALUE_LPAREN_RPAREN                                                   = 72, // <Value> ::= '(' <Expression> ')'
   @VALUE_NULL                                                            = 73, // <Value> ::= null
   @VALUE_INTEGER                                                         = 74, // <Value> ::= Integer
   @VALUE_DECIMAL                                                         = 75, // <Value> ::= Decimal
   @VALUE_STRINGLITERAL                                                   = 76  // <Value> ::= StringLiteral
};
