namespace LastErrorEvent;

public static class HelpText
{
    public static string Content =>
        string.Join(
            Environment.NewLine,
            [
                "Last Error Event",
                string.Empty,
                "Usage:",
                "  lee [--max-age-hours <hours>] [--open-event-viewer] [--no-prompt]",
                string.Empty,
                "Options:",
                "  --max-age-hours <hours>  Only use an Application Error event newer than this value. Default: 1",
                "  --open-event-viewer      Open Event Viewer automatically when no recent error is found",
                "  --no-prompt              Do not offer to open Event Viewer when no recent error is found",
                "  -h, --help               Show this help text"
            ]);
}
