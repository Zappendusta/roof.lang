using System.Diagnostics;
using System.Text;
using Roof.Compiler;

if (args.Length != 2)
{
    Console.WriteLine("Usage: Roof.Compiler <path-to-roof-file> <output-file>");
    return;
}

var inFile = args[0];
var outFile = args[1];

var fileExists = File.Exists(inFile);

if (!fileExists)
{
    Console.WriteLine($"File '{inFile}' does not exist.");
    return;
}

var inFileContent = File.ReadAllText(inFile);

var tokenizer = new Tokenizer(inFileContent);
var tokens = tokenizer.Tokenize();
TokensToFile(tokens, outFile);

return;

void TokensToFile(List<Token> tokens, string outFile)
{
    var sb = new StringBuilder();

    sb.AppendLine("    .global _start");
    sb.AppendLine("_start:");

    for (var i = 0; i < tokens.Count; i++)
    {
        var token = tokens[i];

        if (token.TokenType == TokenType.Exit)
        {
            if (tokens.Count > i + 1 && tokens[i + 1].TokenType == TokenType.IntLit)
            {
                if (tokens.Count > i + 2 && tokens[i + 2].TokenType == TokenType.Semi)
                {
                    sb.Append("    mov x0, #");
                    sb.Append(tokens[i + 1].Value);
                    sb.AppendLine();
                    sb.AppendLine("    mov x8, #93");
                    sb.AppendLine("    svc #0");
                }
            }
        }
    }

    File.WriteAllText($"{outFile}.s", sb.ToString());
    Syscall($"as {outFile}.s -o {outFile}.o");
    // File.Delete($"{outFile}.s");
    Syscall($"ld {outFile}.o -o {outFile}");
    // File.Delete($"{outFile}.o");
}

void Syscall(string call)
{
    var split = call.Split(' ', 2);
    var hasArgs = split.Length > 1;

    var process = Process.Start(new ProcessStartInfo
    {
        FileName = split[0],
        Arguments = hasArgs ? split[1] : null,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true
    });

    process?.WaitForExit();
}