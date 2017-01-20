    grammar CS2;

/*
 * Parser Rules
 */

program
	: (function_declaration | declaration SEMICOLON)*
	;

declaration 
	: type ID (COMMA ID)*
	;

function_declaration
	: mod? type ID OPEN_PAREN parameter_list CLOSE_PAREN block 
	;

mod
	: PUBLIC
	| PRIVATE
	;

statement
	: for_loop
	| while_loop
	| (function_call SEMICOLON)
	| (declaration SEMICOLON)
	| assignment
	| block
	| if_statement
	| return_statement
	;

return_statement
	: RETURN evaluatable? SEMICOLON
	;

block
	: OPEN_BRACE statement* CLOSE_BRACE
	;

if_statement
	: IF OPEN_PAREN evaluatable CLOSE_PAREN block (ELSE block)?
	;

for_loop
	: FOR OPEN_PAREN assignment evaluatable SEMICOLON evaluatable CLOSE_PAREN block
	;

while_loop
	: WHILE OPEN_PAREN evaluatable CLOSE_PAREN block
	;

parameter_list
	: parameter?
	;

parameter
	: type ID
	;

assignment
	: (declaration | ID) ASSIGNMENT evaluatable SEMICOLON
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
	: multiplyingExpression ((PLUS | MINUS) multiplyingExpression)*  
	;

multiplyingExpression   
	: atom ((STAR | DIV) atom)*
	;

atom
	: REAL
	| ID
	| OPEN_PAREN expression CLOSE_PAREN
	| function_call
	;

constant 
	: REAL 
	| char_constant
	| string_constant
	;

char_constant
	: SINGLE_QUOTE LETTER SINGLE_QUOTE
	;

string_constant
	: DOUBLE_QOUTE (REAL | LETTER)+ DOUBLE_QOUTE
	;

function_call 
	: ID OPEN_PAREN (argument? | argument (COMMA argument)* ) CLOSE_PAREN
	;

argument
	: function_call
	| operation
	| ID
	;

pre_unary_operator
	: MINUS
	| post_unary_operator
	;

post_unary_operator
	: OP_INC
	| OP_DEC
	;

type 
	: TYPE_VOID
	| TYPE_INT
	| TYPE_DOUBLE
	| TYPE_STRING
	| arrayType
	;

arrayType
	: (TYPE_INT	| TYPE_DOUBLE | TYPE_STRING) OPEN_BRACKET CLOSE_BRACKET
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

SINGLE_QUOTE:             '\'';
DOUBLE_QOUTE:             '"';
COMMA:                    ',';
OPEN_BRACKET:             '[';
CLOSE_BRACKET:            ']';
OPEN_PAREN:               '(';
CLOSE_PAREN:		  ')';
OPEN_BRACE:               '{';
CLOSE_BRACE:              '}';
SEMICOLON:                ';';
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
ASSIGNMENT:               '=';
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