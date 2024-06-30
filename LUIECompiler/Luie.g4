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

// TODO: Change IDENTIFIER TO parameter
gateapplication
 : GATE register ';'
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
 | ZGATE
 | HGATE
 ;



fragment XGATE  : 'x';
fragment ZGATE  : 'z';
fragment HGATE  : 'h';

IF     : 'qif';
ELSE   : 'else';
DO     : 'do';
END    : 'end';

SKIPSTAT        : 'skip';

INTEGER 
 : [1-9] [0-9]*
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