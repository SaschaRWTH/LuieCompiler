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
 | constDeclaration
 ;

registerDeclaration
 : REGISTERKEYWORD ('[' size=expression ']')? IDENTIFIER ';'
 ;

constDeclaration
 : CONSTANTKEYWORD identifier=IDENTIFIER ':' type=TYPE '=' exp=expression';'
 ;

gateDeclaration
 : GATEKEYWORD identifier=IDENTIFIER '(' param=gateParameter ')' DO block END
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

register
 : IDENTIFIER
 | IDENTIFIER '[' index=expression ']'
 ;

range 
 : start=INTEGER '..' end=INTEGER
 | RANGE '(' length=expression ')'
 | RANGE '(' startIndex=expression ',' endIndex=expression ')'
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
 : func=FUNCTION '(' param=functionParameter ')'
 ;

functionParameter
 : IDENTIFIER (',' IDENTIFIER)*
 | expression (',' expression)*
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
 | POWER
 | MIN
 | MAX
 ;

fragment SIZEOF : 'sizeof';
fragment POWER : 'power';
fragment MIN : 'min';
fragment MAX : 'max';

SKIPSTAT        : 'skip';

TYPE : 'int'
     | 'uint'
     | 'double'
     ;

// Keywords
GATEKEYWORD : 'gate';
REGISTERKEYWORD : 'qubit';
CONSTANTKEYWORD : 'const';

RANGE : 'range';

IF     : 'qif';
ELSE   : 'else';
DO     : 'do';
END    : 'end';
FOR    : 'for';
IN    : 'in';


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