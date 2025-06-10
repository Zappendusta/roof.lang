using System.Diagnostics;

namespace Roof.Compiler;

public class Tokenizer(string source)
{
    private List<Token> Tokens { get; } = [];
    private string CurrentToken { get; set; } = "";
    private int I { get; set; }
    private int LineNum { get; set; }
    private int CharNum { get; set; }

    private char Consume()
    {
        var c = source[I];

        if (c == '\n')
        {
            LineNum += 1;
            CharNum = 0;
        }
        else
        {
            CharNum += 1;
        }


        I += 1;
        return c;
    }

    private bool CanConsume()
    {
        return source.Length > I;
    }

    private char? Peek(int ahead = 0)
    {
        if (source.Length <= I + ahead)
        {
            return null;
        }

        return source[I + ahead];
    }

    public List<Token> Tokenize()
    {
        while (CanConsume())
        {
            Process();
        }

        return Tokens;
    }

    private void Process()
    {
        CurrentToken = "";
        var c = Consume();
        CurrentToken += c;

        if (c is ' ' or '\n')
        {
            return;
        }

        if (c.IsAlpha())
        {
            var n = Peek();
            while (n != null && n.Value.IsAlphaNum())
            {
                CurrentToken += Consume();
                n = Peek();
            }

            var t = ReservedKeywords.ToToken(CurrentToken);
            if (t != null)
            {
                Tokens.Add(t.Value);
                return;
            }
        }
        else if (c == ';')
        {
            Tokens.Add(new Token(TokenType.Semi));
            return;
        }
        else if (c.IsNumber())
        {
            var n = Peek();
            while (n != null && n.Value.IsNumber())
            {
                CurrentToken += Consume();
                n = Peek();
            }

            Tokens.Add(new Token(TokenType.IntLit, CurrentToken));
            return;
        }

        throw new UnreachableException($"Unexpected token character at {LineNum + 1}:{CharNum + 1}");
    }
}

public static class CharExtensions
{
    public static bool IsNumber(this char c)
    {
        return c is >= '0' and <= '9';
    }

    public static bool IsAlpha(this char c)
    {
        return c is >= 'a' and <= 'z' or >= 'A' and <= 'Z';
    }

    public static bool IsAlphaNum(this char c)
    {
        return c.IsAlpha() || c.IsNumber();
    }
}