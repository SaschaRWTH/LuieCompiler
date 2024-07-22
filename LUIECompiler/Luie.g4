grammar Luie;

parse
 : block EOF
 ;

block
 : (declaration | statement)*
 ;

declaration
 : registerDeclaration
 | gateDeclaration
 ;

registerDeclaration
 : 'qubit' ('[' size=expression ']')? IDENTIFIER ';'
 ;

gateDeclaration
 : 'gate' IDENTIFIER '(' parameter ')' DO block END
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
 | RANGE '(' length=expression ')'
 ;

expression
 : left=expression op='+' right=term
 | left=expression op='-' right=term
 | term
 ;

term
 : left=term op='*' right=factor
 | left=term op='/' right=factor
 | factor
 ;

factor
 :'(' exp=expression ')'
 | func=function
 | identifier=IDENTIFIER
 | value=INTEGER
 | op='-' factor
 ;

function
 : func=FUNCTION '(' param=IDENTIFIER ')'
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

FUNCTION
 : SIZEOF
 ;

fragment SIZEOF : 'sizeof';

RANGE : 'range';

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