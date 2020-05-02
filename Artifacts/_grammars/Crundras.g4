grammar Crundras;

program : (statement)*;

statement: inputStatement | outputStatement | compoundStatement | selectionStatement | iterationStatement | declarationStatement | assignmentStatement;
selectionStatement: 'if' '(' expression ')' statement;
iterationStatement: 'for' assignmentExpression 'to' expression 'by' expression 'while' '(' expression ')' statement 'rof' ';';
compoundStatement: '{' (statement)* '}';
declarationStatement: TypeSpecifier Identifier ';';
assignmentStatement: assignmentExpression ';';

assignmentExpression: Identifier '=' expression;

expression: sign? ( '(' expression ')' | literal | Identifier ) (operator sign? ( '(' expression ')' | literal | Identifier) )*;

inputStatement: '$' Identifier ';';
outputStatement: '@' expression ';';

TypeSpecifier: 'int' | 'float';

Identifier: Letter (Letter | Digit)*;

literal: IntegerLiteral | FloatingLiteral;
FloatingLiteral: IntegerLiteral '.' IntegerLiteral?;
IntegerLiteral: Digit+;

Letter: [a-zA-Z];
Digit: [0-9];

sign: ('+' | '-');
operator: arithmeticOperator | relationalOperator;
arithmeticOperator: ('+' | '-' | '*' | '**' | '/' | '%');
relationalOperator: ('<' | '>' | '<=' | '>='| '==' | '!=');
WS: [ \t\r\n]+ -> skip;