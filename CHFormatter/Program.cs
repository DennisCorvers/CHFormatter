using CHFormatter;
using System.Reflection;
using System.Text.RegularExpressions;

class Program
{
    private static Regex m_cmdRgx = new Regex(@"(\s+)(?=\*)|(?<=\*)(\s+)");

    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Enter parent folder to trim songs:");
            var message = Console.ReadLine();

            if (IsQuit(message))
                break;

            if (!string.IsNullOrEmpty(message))
            {
                HandleMessage(message);
            }
        }
    }

    private static void HandleMessage(string message)
    {
        var progressCallback = new Action<string>((progress) =>
        {
            Console.WriteLine(progress);
        });

        var cmds = SplitCommandLine(message);

        if (cmds.Length == 0)
            return;

        if (!TryParseFormatOptions(cmds, out FormatOptions formatOptions))
            return;

        try
        {
            var formatter = new NameFormatter(cmds[0], formatOptions);
            formatter.Format(progressCallback);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private static string[] SplitCommandLine(string message)
    {
        var parts = m_cmdRgx
            .Split(message)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToArray();

        return parts;
    }

    private static bool TryParseFormatOptions(string[] options, out FormatOptions formatOptions)
    {
        formatOptions = FormatOptions.Default;

        if (options.Length == 1)
            return true;

        for (int i = 1; i < options.Length; i++)
        {
            var option = options[i];
            switch (option.ToLower())
            {
                case "*a":
                    formatOptions.RemoveArtist = false;
                    break;
                default:
                    Console.WriteLine($"Unknown command: {option}");
                    return false;
            }
        }

        return true;
    }

    private static bool IsQuit(string? message)
    {
        if (string.CompareOrdinal(message, "quit") == 0)
            return true;

        if (string.CompareOrdinal(message, "q") == 0)
            return true;

        return false;
    }
}