using System.Diagnostics.Eventing.Reader;
using System.ComponentModel;
using System.Runtime.Versioning;

namespace LastErrorEvent;

internal static class Program
{
    [SupportedOSPlatform("windows")]
    private static int Main(string[] args)
    {
        if (!OperatingSystem.IsWindows())
        {
            Console.Error.WriteLine("lee only runs on Windows.");
            return 1;
        }

        bool isInteractive = !Console.IsInputRedirected && !Console.IsOutputRedirected;
        ParseResult parseResult = CommandLineOptionsParser.Parse(args, isInteractive);
        if (!parseResult.IsSuccess)
        {
            Console.Error.WriteLine(parseResult.ErrorMessage);
            Console.Error.WriteLine();
            Console.Error.WriteLine("Run 'lee --help' for usage.");
            return 1;
        }

        CommandLineOptions options = parseResult.Options!;
        if (options.ShowHelp)
        {
            Console.WriteLine(HelpText.Content);
            return 0;
        }

        EventLookupResult lookupResult;

        try
        {
            lookupResult = ApplicationErrorLogReader.GetLatestApplicationError(options.MaxAge, DateTimeOffset.Now);
        }
        catch (UnauthorizedAccessException exception)
        {
            Console.Error.WriteLine($"Unable to read the Windows Application log: {exception.Message}");
            return 1;
        }
        catch (EventLogException exception)
        {
            Console.Error.WriteLine($"Unable to query the Windows Application log: {exception.Message}");
            return 1;
        }

        return lookupResult.Status switch
        {
            EventLookupStatus.Found => HandleFoundEvent(lookupResult.Event!),
            EventLookupStatus.OlderThanMaxAge => HandleMissingEvent(
                $"No Application Error event was found in the last {options.MaxAge.TotalHours:G} hour(s). " +
                $"The newest matching event was at {lookupResult.MostRecentEventTime:O}.",
                options),
            _ => HandleMissingEvent("No Application Error events were found in the Windows Application log.", options)
        };
    }

    [SupportedOSPlatform("windows")]
    private static int HandleFoundEvent(ApplicationErrorEvent applicationErrorEvent)
    {
        string clipboardText = ApplicationErrorFormatter.FormatClipboardText(applicationErrorEvent);

        try
        {
            ClipboardService.CopyText(clipboardText);
        }
        catch (InvalidOperationException exception)
        {
            Console.Error.WriteLine($"The event was found, but copying to the clipboard failed: {exception.Message}");
            Console.Error.WriteLine();
            Console.Error.WriteLine(clipboardText);
            return 1;
        }
        catch (Win32Exception exception)
        {
            Console.Error.WriteLine($"The event was found, but copying to the clipboard failed: {exception.Message}");
            Console.Error.WriteLine();
            Console.Error.WriteLine(clipboardText);
            return 1;
        }

        Console.WriteLine($"Copied Application Error event {applicationErrorEvent.EventId} from {applicationErrorEvent.TimeCreated:O} to the clipboard.");
        return 0;
    }

    [SupportedOSPlatform("windows")]
    private static int HandleMissingEvent(string message, CommandLineOptions options)
    {
        Console.WriteLine(message);

        if (options.OpenEventViewer)
        {
            return LaunchEventViewer();
        }

        if (options.PromptForEventViewer && ShouldLaunchEventViewer())
        {
            return LaunchEventViewer();
        }

        return 0;
    }

    [SupportedOSPlatform("windows")]
    private static bool ShouldLaunchEventViewer()
    {
        Console.Write("Open Event Viewer now? [y/N]: ");
        string? response = Console.ReadLine();

        return string.Equals(response, "y", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(response, "yes", StringComparison.OrdinalIgnoreCase);
    }

    [SupportedOSPlatform("windows")]
    private static int LaunchEventViewer()
    {
        try
        {
            EventViewerLauncher.Launch();
        }
        catch (Win32Exception exception)
        {
            Console.Error.WriteLine($"Unable to open Event Viewer: {exception.Message}");
            return 1;
        }

        Console.WriteLine("Opened Event Viewer.");
        return 0;
    }
}
