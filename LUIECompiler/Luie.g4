grammar Luie;

parse
 : block EOF
 ;

block
 : (declaration | statement)*
 ;

declaration
 : 'qubit' ('[' SIZE ']')? IDENTIFIER ';'
 ;
 
statement
 : gateapplication
 | qifStatement 
 | SKIPSTAT ';'
 ;

gateapplication
 : GATE IDENTIFIER ';'
 ;

qifStatement
 : IF IDENTIFIER ifStat elseStat? END
 ;
 
ifStat
 : DO block
 ;

elseStat
 : ELSE DO block
 ;

GATE
 : XGATE
 | ZGATE
 | HGATE
 ;

fragment XGATE : 'x';
fragment ZGATE : 'z';
fragment HGATE : 'h';

IF       : 'qif';
ELSE     : 'else';
DO       : 'do';
END       : 'end';

SKIPSTAT     : 'skip';

SIZE 
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