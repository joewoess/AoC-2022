// ANTLR Grammar to parse a nested list of integers, e.g. [[1,2],[3,4]]
lexer grammar IntList;

// Whitespace handeling
WHITESPACE: [ \t\f\r\n]+ -> skip; // toss out whitespace

// Fragments to match the tokens

fragment DELIMITER: ','; // match comma
fragment LISTSTART: '['; // match left bracket
fragment LISTEND: ']'; // match right bracket

// Tokens to match the grammar

NUMBER: '0' ..'9'+; // match integers
ENTRY: NUMBER | LIST; // match entries
MULTIPLEENTRIES:
	ENTRY (DELIMITER ENTRY)*; // match multiple entries
LIST: LISTSTART MULTIPLEENTRIES? LISTEND; // match list

// Rules to parse the grammar
