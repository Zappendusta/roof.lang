namespace Roof.Compiler;

public class Tokenizer(string source)
{
    private List<Token> Tokens { get; } = [];
    private string CurrentToken { get; set; } = "";
    private int I { get; set; }
    private int Line { get; set; }

    private char Consume()
    {
        var c = source[I];
        I += 1;
        return c;
    }

    private bool CanConsume()
    {
        return source.Length > I;
    }

    private char? Peek(int ahead = 1)
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

    private void CreateToken()
    {
        var token = ReservedKeywords.ToToken(CurrentToken);
        if (token != null)
        {
            Tokens.Add(token.Value);
            CurrentToken = "";
            return;
        }

        token = new Token
        {
            Value = CurrentToken,
            TokenType = TokenType.IntLit,
        };

        Tokens.Add(token.Value);
        CurrentToken = "";
    }

    private void Process()
    {
        var c = Consume();

        switch (c)
        {
            case '\n':
                Line++;
                break;
            case ' ' when string.IsNullOrWhiteSpace(CurrentToken):
                return;
            case ' ':
                CreateToken();
                return;
            case ';':
                if (!string.IsNullOrWhiteSpace(CurrentToken))
                {
                    CreateToken();
                }

                CurrentToken += c;
                CreateToken();
                break;
        }

        CurrentToken += c;
    }
}