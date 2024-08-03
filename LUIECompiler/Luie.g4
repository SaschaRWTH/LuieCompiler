grammar Luie;

parse
 : mainblock EOF
 ;

mainblock
 : gateDeclaration* (declaration | statement)*
 ;

block
 : (declaration | statement)*
 ;

declaration
 : registerDeclaration
 ;

registerDeclaration
 : 'qubit' ('[' size=expression ']')? IDENTIFIER ';'
 ;

gateDeclaration
 : 'gate' identifier=IDENTIFIER '(' param=gateParameter? ')' DO block END
 ;

gateParameter 
 : IDENTIFIER (',' IDENTIFIER)*
 ;

statement
 : gateapplication
 | qifStatement 
 | forstatement 
 | SKIPSTAT ';'
 ;
 
gateapplication
 : gate register (',' register)* ';'
 ;

gate 
 : type=CONSTANTGATE
 | parameterizedGate=PARAMETERIZEDGATE '(' param=expression ')'
 | identifier=IDENTIFIER
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

CONSTANTGATE
 : XGATE
 | CXGATE
 | CCXGATE
 | ZGATE
 | YGATE
 | HGATE
 ;

PARAMETERIZEDGATE 
 : PHASEGATE
 ;

fragment XGATE  : 'x';
fragment CXGATE  : 'cx';
fragment CCXGATE  : 'ccx';
fragment ZGATE  : 'z';
fragment YGATE  : 'y';
fragment HGATE  : 'h';
fragment PHASEGATE  : 'p';

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