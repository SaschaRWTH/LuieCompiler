grammar Luie;

parse
 : block EOF
 ;

block
 : (declaration | statement)*
 ;

declaration
 : 'qubit' ('[' size=expression ']')? IDENTIFIER ';'
 ;
 
statement
 : gateapplication
 | qifStatement 
 | forstatement 
 | SKIPSTAT ';'
 ;
 
gateapplication
 : GATE register (',' register)* ';'
 ;

qifStatement
 : IF register ifStat elseStat? END
 ;
 
ifStat
 : DO block
 ;

elseStat
 : ELSE DO block
 ;

forstatement
 : FOR IDENTIFIER IN range DO block END
 ;

parameter
 : register (',' register)*
 ;

register
 : IDENTIFIER
 | IDENTIFIER '[' index=expression ']'
 ;

range 
 : start=INTEGER '..' end=INTEGER
 ;

expression
 : left=expression '+' right=term
 | left=expression '-' right=term
 | term
 ;

term
 : left=term '*' right=factor
 | left=term '/' right=factor
 | factor
 ;

factor
 :'(' expression ')'
 | identifier=IDENTIFIER
 | value=INTEGER
 | '-' factor
 ;

GATE
 : XGATE
 | CXGATE
 | CCXGATE
 | ZGATE
 | YGATE
 | HGATE
 ;

fragment XGATE  : 'x';
fragment CXGATE  : 'cx';
fragment CCXGATE  : 'ccx';
fragment ZGATE  : 'z';
fragment YGATE  : 'y';
fragment HGATE  : 'h';

IF     : 'qif';
ELSE   : 'else';
DO     : 'do';
END    : 'end';
FOR    : 'for';
IN    : 'in';

SKIPSTAT        : 'skip';

INTEGER 
 : [1-9] [0-9]* | '0'
 ;

IDENTIFIER 
 : [a-zA-Z_] [a-zA-Z_0-9]*
 ;

COMMENT
 : ( '//' ~[\r\n]* | '/*' .*? '*/' ) -> skip
 ;

SPACE
 : [ \t\r\n\u000C] -> skip
 ;