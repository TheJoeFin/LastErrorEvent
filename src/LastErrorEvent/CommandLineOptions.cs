namespace LastErrorEvent;

public sealed record CommandLineOptions(
    TimeSpan MaxAge,
    bool OpenEventViewer,
    bool PromptForEventViewer,
    bool ShowHelp);
