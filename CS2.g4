grammar CS2;

/*
 * Parser Rules
 */

program
	: (function_declaration | declaration ';')* EOF
	;

declaration 
	: type ID (',' ID)*
	;

function_declaration
	: mod? type ID '(' parameter_list ')' block 
	;

mod
	: PUBLIC
	| PRIVATE
	;

statement
	: for_loop
	| while_loop
	| (function_call ';')
	| (declaration ';')
	| assignment
	| block
	| if_statement
	| return_statement
	;

return_statement
	: RETURN evaluatable? ';'
	;

block
	: '{' statement* '}'
	;

if_statement
	: IF '(' evaluatable ')' block (ELSE block)?
	;

for_loop
	: 'for' '(' assignment evaluatable ';' evaluatable ')' block
	;

while_loop
	: 'while' '(' evaluatable ')' block
	;

parameter_list
	: parameter?
	;

parameter
	: type ID
	;

assignment
	: (declaration
	| ID) '=' evaluatable ';'
	;

evaluatable
	: function_call
	| constant
	| ID
	| operation
	;

operation
	: unary_operation
	| relational_operation
	| expression
	;

unary_operation
	: (pre_unary_operator expression)
	| (expression post_unary_operator)
	;

relational_operation   
	: expression relop expression  
	;

relop
	: LT
	| GT
	| OP_LE
	| OP_GE
	| OP_EQ
	;

expression   
	: multiplyingExpression ((PLUS
	| MINUS) multiplyingExpression)*  
	;

multiplyingExpression   
	: atom ((STAR
	| DIV) atom)*
	;

atom
	: REAL
	| ID
	| '(' expression ')'
	| function_call
	;

constant 
	: (REAL
	| char_constant
	| string_constant)
	;

char_constant
	: '\''LETTER'\''
	;

string_constant
	: '"'(REAL
	| LETTER)+'"'
	;

function_call 
	: ID '(' (argument?
	| argument (',' argument)* ) ')'
	;

argument
	: function_call
	| operation
	| ID
	;

pre_unary_operator
	: '-'
	| post_unary_operator
	;

post_unary_operator
	: '++'
	| '--'
	;

type 
	: TYPE_VOID
	| TYPE_INT
	| TYPE_DOUBLE
	| arrayType
	;

arrayType
	: (TYPE_INT
	| TYPE_DOUBLE
	| TYPE_STRING)'[]'
	;


/*
 * Lexer Rules
 */

TYPE_BOOL:          'bool';
TYPE_INT:           'int';
TYPE_DOUBLE:        'double';
TYPE_STRING:        'string';
TYPE_VOID:          'void';

FALSE:              'false';
TRUE:               'true';

IF:                 'if';
ELSE:               'else';
FOR:                'for';
WHILE:              'while';
			     
CONST:              'const';
RETURN:             'return';
			     
PRIVATE:            'private';
PROTECTED:          'protected';
PUBLIC:             'public';


//SEMICOLON:                ';';
PLUS:                     '+';
MINUS:                    '-';
STAR:                     '*';
DIV:                      '/';
PERCENT:                  '%';
AMP:                      '&';
BITWISE_OR:               '|';
CARET:                    '^';
BANG:                     '!';
TILDE:                    '~';
//ASSIGNMENT:               '=';
LT:                       '<';
GT:                       '>';
INTERR:                   '?';
DOUBLE_COLON:             '::';
OP_COALESCING:            '??';
OP_INC:                   '++';
OP_DEC:                   '--';
OP_AND:                   '&&';
OP_OR:                    '||';
OP_PTR:                   '->';
OP_EQ:                    '==';
OP_NE:                    '!=';
OP_LE:                    '<=';
OP_GE:                    '>=';


/*
OP_ADD_ASSIGNMENT:        '+=';
OP_SUB_ASSIGNMENT:        '-=';
OP_MULT_ASSIGNMENT:       '*=';
OP_DIV_ASSIGNMENT:        '/=';
OP_MOD_ASSIGNMENT:        '%=';
OP_AND_ASSIGNMENT:        '&=';
OP_OR_ASSIGNMENT:         '|=';
OP_XOR_ASSIGNMENT:        '^=';
OP_LEFT_SHIFT:            '<<';
OP_LEFT_SHIFT_ASSIGNMENT: '<<=';
*/


REAL
	: INTEGER 
	| DIGIT*('.'DIGIT+)
	;

INTEGER
	: DIGIT+
	;

ID
	: [_a-zA-Z][_a-zA-Z0-9]*
	;

LETTER
	: [a-zA-Z]
	;

DIGIT
	: [0-9]
	;

WS 
	: [ \t\r\n]+ -> channel(HIDDEN)  // skip spaces, tabs, newlines
	;