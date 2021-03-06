﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Plagiarism.Lexer
{
    public enum LexerType
    {
        ERROR,
        INT,
        REAL,
        CHAR,
        CSTRING,
        OPERATOR,
        PREPROCESSOR,
        KEYWORD,
        PUNCTIUATION,
        IDENTIFIER,
        COMMENT
    }

    public class Lexeme
    {
        public string value;
        public LexerType type;

        public Lexeme(string value, LexerType type)
        {
            this.value = value;
            this.type = type;
        }

        public static string MergeLexemes(IEnumerable<Lexeme> lexemes)
        {
            var sb = new StringBuilder();
            foreach(var lex in lexemes)
            {
                sb.Append(lex.value);
            }
            return sb.ToString();
        }
    }
}
