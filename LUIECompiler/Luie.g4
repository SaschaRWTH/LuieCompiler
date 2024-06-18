grammar Luie;

parse
 : block EOF
 ;

block
 : (declaration | statement)*
 ;

declaration
 : 'qubit' IDENTIFIER ';'
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
 : ifStat elseStat? END
 ;
 
ifStat
 : IF IDENTIFIER DO block
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

IDENTIFIER 
 : [a-zA-Z_] [a-zA-Z_0-9]*
 ;

COMMENT
 : ( '//' ~[\r\n]* | '/*' .*? '*/' ) -> skip
 ;

SPACE
 : [ \t\r\n\u000C] -> skip
 ;