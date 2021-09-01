grammar JustDsl;

LPAREN:             '(';
RPAREN:             ')';
FUNCTION_START:     '#';
ARG_SEPARATOR:      ',';

IDENTIFIER
    : [a-zA-Z] [a-zA-Z\-_] *
    ;

start : func EOF ;

func
    : FUNCTION_START IDENTIFIER LPAREN parameter_list? RPAREN
    ;

argument
   : func
   ;

parameter_list
   : config argument_list
   | no_config_parameter_list
   ;

argument_list
   : (ARG_SEPARATOR argument)*
   ;

no_config_parameter_list
   : argument (ARG_SEPARATOR argument)*
   ;

config
   : json
   ;

json
   : value
   ;

obj
   : '{' pair (',' pair)* '}'
   | '{' '}'
   ;

pair
   : STRING ':' value
   ;

arr
   : '[' value (',' value)* ']'
   | '[' ']'
   ;

value
   : STRING
   | NUMBER
   | obj
   | arr
   | 'true'
   | 'false'
   | 'null'
   ;


STRING
   : '"' (ESC | SAFECODEPOINT)* '"'
   ;


fragment ESC
   : '\\' (["\\/bfnrt] | UNICODE)
   ;
fragment UNICODE
   : 'u' HEX HEX HEX HEX
   ;
fragment HEX
   : [0-9a-fA-F]
   ;
fragment SAFECODEPOINT
   : ~ ["\\\u0000-\u001F]
   ;


NUMBER
   : '-'? INT ('.' [0-9] +)? EXP?
   ;


fragment INT
   : '0' | [1-9] [0-9]*
   ;

// no leading zeros

fragment EXP
   : [Ee] [+\-]? INT
   ;

// \- since - means "range" inside [...]

WS
   : [ \t\n\r] + -> skip
   ;