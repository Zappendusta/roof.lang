namespace Roof.Compiler;

public static class ReservedKeywords
{
    private const string Exit = "exit";
    private const string Semi = ";";

    public static string[] All =
    [
        Exit,
        Semi,
    ];

    public static Token? ToToken(string rawToken)
    {
        if (string.IsNullOrWhiteSpace(rawToken))
        {
            return null;
        }

        if (rawToken.Equals(Exit, StringComparison.OrdinalIgnoreCase))
        {
            return new Token
            {
                TokenType = TokenType.Exit
            };
        }

        if (rawToken.Equals(Semi, StringComparison.OrdinalIgnoreCase))
        {
            return new Token
            {
                TokenType = TokenType.Semi
            };
        }


        return null;
    }
}