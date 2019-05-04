using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Plagiarism.Lexer
{
    public class LexicalAnalyzer
    {
        private TextReader reader;
        private char peek = ' ';
        private bool EOF = false;

        public static HashSet<string> keywords = new HashSet<string>();

        private List<Lexeme> result = new List<Lexeme>();

        public LexicalAnalyzer()
        {
            if (keywords.Count == 0)
            {
                initKeys();
            }
        }

        public LexicalAnalyzer(TextReader reader)
        {
            this.reader = reader;
            if (keywords.Count == 0)
            {
                initKeys();
            }
        }

        private static void initKeys()
        {
            char[] keys = "\"asm\" | \"auto\" | \"break\" | \"case\" | \"char\" | \"class\" | \"const\" | \"continue\" | \"default\" | \"delete\" | \"do\" | \"double\" | \"else\" | \"enum\" | \"extern\" | \"float\" | \"for\" | \"friend\" | \"goto\" | \"if\" | \"inline\" | \"int\" | \"long\" | \"new\" | \" _operator\" | \"private\" | \"protected\" | \"public\" | \"register\" | \"return\" | \"short\" | \"signed\" | \"sizeof\" | \"static\" | \"struct\" | \"switch\" | \"template\" | \"this\" | \"throw\" | \"typedef\" | \"union\" | \"unsigned\" | \"virtual\" | \"void\" | \"volatile\" | \"while\" | \"namespace\" | \"using\"".ToCharArray();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < keys.Length; ++i)
            {
                if (char.IsLetter(keys[i]))
                {
                    sb.Append(keys[i]);
                }
                else
                {
                    if (sb.Length > 0)
                    {
                        keywords.Add(sb.ToString());
                        sb = new StringBuilder();
                    }
                }
            }
        }

        private bool isSymbol()
        {
            return char.IsLetter(peek) || peek == '_';
        }

        private bool isNumber()
        {
            return char.IsDigit(peek) || peek == '.';
        }

        private void identity()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(peek);
            while (getNext())
            {
                if (peek == '\n' || peek == '\t' || peek == '\r' || peek == ' ' || peek == '{'
                        || peek == '}' || peek == ';' || !(char.IsLetter(peek) || char.IsDigit(peek) || peek == '_'))
                {
                    break;
                }
                else
                {
                    sb.Append(peek);
                }
            }
            String token = sb.ToString();
            if (keywords.Contains(token))
            {
                result.Add(new Lexeme(token, LexerType.KEYWORD));
            }
            else
            {
                result.Add(new Lexeme(token, LexerType.IDENTIFIER));
            }
            test();
        }

        private void real(StringBuilder starter)
        {
            StringBuilder sb = starter;
            while (getNext())
            {
                if (char.IsWhiteSpace(peek) || peek == ';' || peek == '{' || peek == '}')
                {
                    break;
                }
                else
                {
                    sb.Append(peek);
                }
            }
            try
            {
                Double.Parse(sb.ToString());
                result.Add(new Lexeme(sb.ToString(), LexerType.REAL));
            }
            catch (Exception exc)
            {
                result.Add(new Lexeme(sb.ToString(), LexerType.ERROR));
            }

            test();
        }

        private void number()
        {
            if (peek == '.')
            {
                getNext();
                if (char.IsDigit(peek))
                {
                    real((new StringBuilder()).Append("0." + peek));
                }
                else if (isSymbol())
                {
                    result.Add(new Lexeme(".", LexerType.PUNCTIUATION));
                    identity();
                }
                else
                {
                    if (peek == '*')
                    {
                        result.Add(new Lexeme(".*", LexerType.OPERATOR));
                    }
                    else if (peek == '.')
                    {
                        getNext();
                        if (peek == '.')
                        {
                            result.Add(new Lexeme("...", LexerType.OPERATOR));
                        }
                        else
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append(".." + peek);
                            while (peek != '\n')
                            {
                                getNext();
                                if (!char.IsWhiteSpace(peek))
                                    sb.Append(peek);
                            }
                            result.Add(new Lexeme(sb.ToString(), LexerType.ERROR));
                        }
                    }
                    else
                    {
                        result.Add(new Lexeme("." + peek, LexerType.ERROR));
                    }
                }
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(peek);

                while (getNext())
                {
                    if (char.IsWhiteSpace(peek) || !char.IsDigit(peek))
                    {
                        break;
                    }
                    else
                    {
                        sb.Append(peek);
                    }
                }
                try
                {
                    int.Parse(sb.ToString());
                    result.Add(new Lexeme(sb.ToString(), LexerType.INT));
                }
                catch (Exception ex)
                {
                    try
                    {
                        double.Parse(sb.ToString());
                        result.Add(new Lexeme(sb.ToString(), LexerType.REAL));
                    }
                    catch (Exception exc)
                    {
                        result.Add(new Lexeme(sb.ToString(), LexerType.ERROR));
                    }
                }

                test();
            }
        }

        private void cstring()
        {
            StringBuilder sb = new StringBuilder();
            getNext();

            while (peek != '\"')
            {
                if (peek != '\n' && peek != '\t')
                {
                    sb.Append(peek);
                    if (!getNext())
                        break;
                }
                else
                {
                    break;
                }
            }

            if (peek == '\"')
            {
                result.Add(new Lexeme(sb.ToString(), LexerType.CSTRING));
            }
            else
            {
                result.Add(new Lexeme("\"" + sb.ToString(), LexerType.ERROR));
            }
        }

        private void cchar()
        {
            getNext();
            char t = peek;
            if (t == '\'')
            {
                result.Add(new Lexeme("\'\'", LexerType.ERROR));
            }
            else
            {
                getNext();
                if (peek == '\'')
                {
                    result.Add(new Lexeme("" + t, LexerType.CHAR));
                }
                else
                {
                    if (t == '\\' && (peek == 'n' || peek == 'r' || peek == '0' || peek == 'a' || peek == 'b' || peek == 't' || peek == 'v' || peek == 'f'))
                    {
                        char u = peek;
                        getNext();
                        if (peek == '\'')
                        {
                            result.Add(new Lexeme("" + t + u, LexerType.CHAR));
                        }
                        else
                        {
                            result.Add(new Lexeme("\'" + t + u + peek, LexerType.ERROR));
                        }
                    }
                    else
                    {
                        result.Add(new Lexeme("\'" + t + peek, LexerType.ERROR));
                    }
                }
            }
        }

        private void _operator()
        {
            if (peek == '+')
            {
                getNext();
                if (peek == '+' || peek == '=')
                {
                    result.Add(new Lexeme("+" + peek, LexerType.OPERATOR));
                }
                else
                {
                    result.Add(new Lexeme("+", LexerType.OPERATOR));
                    test();
                }
            }
            else if (peek == '-')
            {
                getNext();
                if (peek == '>')
                {
                    getNext();
                    if (peek == '*')
                    {
                        result.Add(new Lexeme("->*", LexerType.OPERATOR));
                    }
                    else
                    {
                        result.Add(new Lexeme("->", LexerType.OPERATOR));
                        test();
                    }
                }
                else if (peek == '-' || peek == '=')
                {
                    result.Add(new Lexeme("-" + peek, LexerType.OPERATOR));
                }
                else
                {
                    result.Add(new Lexeme("-", LexerType.OPERATOR));
                    test();
                }
            }
            else if (peek == '/')
            {
                getNext();
                if (peek == '=')
                {
                    result.Add(new Lexeme("/" + peek, LexerType.OPERATOR));
                }
                else
                {
                    result.Add(new Lexeme("/", LexerType.OPERATOR));
                    test();
                }
            }
            else if (peek == '*')
            {
                getNext();
                if (peek == '=')
                {
                    result.Add(new Lexeme("*" + peek, LexerType.OPERATOR));
                }
                else
                {
                    result.Add(new Lexeme("*", LexerType.OPERATOR));
                    test();
                }
            }
            else if (peek == '%')
            {
                getNext();
                if (peek == '=')
                {
                    result.Add(new Lexeme("%" + peek, LexerType.OPERATOR));
                }
                else
                {
                    result.Add(new Lexeme("%", LexerType.OPERATOR));
                    test();
                }
            }
            else if (peek == '!')
            {
                getNext();
                if (peek == '=')
                {
                    result.Add(new Lexeme("!" + peek, LexerType.OPERATOR));
                }
                else
                {
                    result.Add(new Lexeme("!", LexerType.OPERATOR));
                    test();
                }
            }
            else if (peek == '^')
            {
                getNext();
                if (peek == '=')
                {
                    result.Add(new Lexeme("^" + peek, LexerType.OPERATOR));
                }
                else
                {
                    result.Add(new Lexeme("^", LexerType.OPERATOR));
                    test();
                }
            }
            else if (peek == '&')
            {
                getNext();
                if (peek == '=' || peek == '&')
                {
                    result.Add(new Lexeme("&" + peek, LexerType.OPERATOR));
                }
                else
                {
                    result.Add(new Lexeme("&", LexerType.OPERATOR));
                    test();
                }
            }
            else if (peek == '|')
            {
                getNext();
                if (peek == '=' || peek == '|')
                {
                    result.Add(new Lexeme("|" + peek, LexerType.OPERATOR));
                }
                else
                {
                    result.Add(new Lexeme("|", LexerType.OPERATOR));
                    test();
                }
            }
            else if (peek == '=')
            {
                getNext();
                if (peek == '=')
                {
                    result.Add(new Lexeme("=" + peek, LexerType.OPERATOR));
                }
                else
                {
                    result.Add(new Lexeme("=", LexerType.OPERATOR));
                    test();
                }
            }
            else if (peek == '>')
            {
                getNext();
                if (peek == '>')
                {
                    getNext();
                    if (peek == '=')
                    {
                        result.Add(new Lexeme(">>=", LexerType.OPERATOR));
                    }
                    else
                    {
                        result.Add(new Lexeme(">>", LexerType.OPERATOR));
                        test();
                    }
                }
                else if (peek == '=')
                {
                    result.Add(new Lexeme(">" + peek, LexerType.OPERATOR));
                }
                else
                {
                    result.Add(new Lexeme(">", LexerType.OPERATOR));
                    test();
                }
            }
            else if (peek == '<')
            {
                getNext();
                if (peek == '<')
                {
                    getNext();
                    if (peek == '=')
                    {
                        result.Add(new Lexeme("<<=", LexerType.OPERATOR));
                    }
                    else
                    {
                        result.Add(new Lexeme("<<", LexerType.OPERATOR));
                        test();
                    }
                }
                else if (peek == '=')
                {
                    result.Add(new Lexeme("<" + peek, LexerType.OPERATOR));
                }
                else
                {
                    result.Add(new Lexeme("<", LexerType.OPERATOR));
                    test();
                }
            }
            else if (peek == ':')
            {
                getNext();
                if (peek == ':')
                {
                    result.Add(new Lexeme("::", LexerType.OPERATOR));
                }
                else
                {
                    result.Add(new Lexeme(":", LexerType.OPERATOR));
                    test();
                }
            }
            else
            {
                punctuation();
            }
        }

        private void punctuation()
        {
            if (peek == '(' || peek == '{' || peek == '[' || peek == ']' || peek == '}' || peek == ')'
                    || peek == ';' || peek == '?' || peek == '\"' || peek == ',' || peek == '.')
            {
                result.Add(new Lexeme("" + peek, LexerType.PUNCTIUATION));
            }
            else
            {
                result.Add(new Lexeme("" + peek, LexerType.ERROR));
            }
        }

        private void comment()
        {
            StringBuilder sb = new StringBuilder();
            getNext();
            if (peek == '*')
            {
                bool isTermBegin = false;
                while (getNext())
                {
                    if (peek == '*')
                    {
                        if (!isTermBegin)
                        {
                            isTermBegin = true;
                        }
                        else
                        {
                            sb.Append('*');
                        }
                    }
                    else if (isTermBegin && peek == '/')
                    {
                        result.Add(new Lexeme(sb.ToString(), LexerType.COMMENT));
                        return;
                    }
                    else if (peek == '\n' || peek == '\t' || peek == '\r')
                    {
                        sb.Append(' ');
                    }
                    else
                    {
                        if (isTermBegin)
                        {
                            isTermBegin = false;
                            sb.Append('*');
                        }
                        sb.Append(peek);
                    }
                }
                result.Add(new Lexeme(sb.ToString(), LexerType.ERROR));
            }
            else if (peek == '/')
            {
                while (getNext())
                {
                    if (peek == '\n')
                    {
                        result.Add(new Lexeme(sb.ToString(), LexerType.COMMENT));
                        return;
                    }
                    else if (peek == '\n' || peek == '\t' || peek == '\r')
                    {
                        sb.Append(' ');
                    }
                    else
                    {
                        sb.Append(peek);
                    }
                }
                result.Add(new Lexeme(sb.ToString(), LexerType.COMMENT));
            }
            else if (peek == '=')
            {
                result.Add(new Lexeme("/" + peek, LexerType.OPERATOR));
            }
            else
            {
                result.Add(new Lexeme("/", LexerType.OPERATOR));
                test();
            }
        }

        private void preprocessor()
        {
            getNext();
            if (peek == '#')
            {
                result.Add(new Lexeme("##", LexerType.OPERATOR));
            }
            else if (!char.IsLetter(peek))
            {
                result.Add(new Lexeme("" + peek, LexerType.ERROR));
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(peek);
                while (getNext() && !char.IsWhiteSpace(peek))
                {
                    sb.Append(peek);
                }

                if (char.IsWhiteSpace(peek))
                {
                    String s = sb.ToString();
                    if (s.Equals("define") || s.Equals("include") || s.Equals("define") || s.Equals("pragma") || s.Equals("error")
                            || s.Equals("warning") || s.Equals("undef") || s.Equals("line") || s.Equals("if") || s.Equals("else")
                            || s.Equals("elif") || s.Equals("endif") || s.Equals("ifdef") || s.Equals("ifndef"))
                    {
                        result.Add(new Lexeme("#" + s, LexerType.PREPROCESSOR));
                    }
                    else
                    {
                        result.Add(new Lexeme("#" + s, LexerType.ERROR));
                    }
                }
                else
                {
                    result.Add(new Lexeme("#" + sb.ToString(), LexerType.ERROR));
                }
            }
        }

        private bool getNext()
        {
            try
            {
                int t;
                if ((t = reader.Read()) > 0) {
                    peek = (char)t;
                    return true;
                } else {
                    EOF = true;
                    return false;
                }
            }
            catch (IOException iox)
            {
                return false;
            }
        }

        public List<Lexeme> process(TextReader reader)
        {
            result = new List<Lexeme>();
            this.reader = reader;

            while (getNext())
            {
                test();
            }

            return result;
        }

        public List<Lexeme> process(string code)
        {
            result = new List<Lexeme>();
            this.reader = new StringReader(code);

            while (getNext())
            {
                test();
            }

            return result;
        }


        private void test()
        {
            if (EOF)
                return;

            if (char.IsWhiteSpace(peek))
                return;

            if (isSymbol())
            {
                identity();
            }
            else if (isNumber())
            {
                number();
            }
            else if (peek == '\"')
            {
                cstring();
            }
            else if (peek == '\'')
            {
                cchar();
            }
            else if (peek == '/')
            {
                comment();
            }
            else if (peek == '#')
            {
                preprocessor();
            }
            else
            {
                _operator();
            }
        }
    }
}
