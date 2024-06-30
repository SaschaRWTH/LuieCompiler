grammar Luie;

parse
 : block EOF
 ;

block
 : (declaration | statement)*
 ;

declaration
 : 'qubit' ('[' size=INTEGER ']')? IDENTIFIER ';'
 ;
 
statement
 : gateapplication
 | qifStatement 
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

parameter
 : register (',' register)*
 ;

register
 : IDENTIFIER
 | IDENTIFIER '[' index=INTEGER ']'
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