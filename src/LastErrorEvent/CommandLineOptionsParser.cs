using System.Globalization;

namespace LastErrorEvent;

public static class CommandLineOptionsParser
{
    private static readonly TimeSpan DefaultMaxAge = TimeSpan.FromHours(1);

    public static ParseResult Parse(string[] args, bool isInteractive)
    {
        TimeSpan maxAge = DefaultMaxAge;
        bool openEventViewer = false;
        bool promptForEventViewer = isInteractive;
        bool showHelp = false;

        for (int index = 0; index < args.Length; index++)
        {
            string argument = args[index];

            switch (argument)
            {
                case "-h":
                case "--help":
                    showHelp = true;
                    break;

                case "--open-event-viewer":
                    openEventViewer = true;
                    promptForEventViewer = false;
                    break;

                case "--no-prompt":
                    promptForEventViewer = false;
                    break;

                case "--max-age-hours":
                    if (index + 1 >= args.Length)
                    {
                        return ParseResult.Fail("Missing value for --max-age-hours.");
                    }

                    if (!double.TryParse(args[index + 1], NumberStyles.Float, CultureInfo.InvariantCulture, out double hours) || hours <= 0)
                    {
                        return ParseResult.Fail("--max-age-hours must be a positive number.");
                    }

                    maxAge = TimeSpan.FromHours(hours);
                    index++;
                    break;

                default:
                    return ParseResult.Fail($"Unknown argument '{argument}'.");
            }
        }

        return ParseResult.Success(new CommandLineOptions(maxAge, openEventViewer, promptForEventViewer, showHelp));
    }
}

public sealed record ParseResult(bool IsSuccess, CommandLineOptions? Options, string? ErrorMessage)
{
    public static ParseResult Success(CommandLineOptions options) => new(true, options, null);

    public static ParseResult Fail(string errorMessage) => new(false, null, errorMessage);
}
