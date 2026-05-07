namespace LastErrorEvent.Tests;

public sealed class ApplicationErrorFormatterTests
{
    [Fact]
    public void FormatClipboardText_IncludesMetadataEventDataAndMessage()
    {
        ApplicationErrorEvent applicationErrorEvent = new(
            "Application",
            "Application Error",
            1000,
            42,
            "Error",
            "WORKSTATION",
            new DateTimeOffset(2026, 5, 6, 19, 0, 0, TimeSpan.Zero),
            1234,
            5678,
            new List<EventField>
            {
                new("FaultingApplicationName", "demo.exe"),
                new("ExceptionCode", "0xc0000005")
            },
            "Faulting application name: demo.exe",
            "<Event />");

        string formatted = ApplicationErrorFormatter.FormatClipboardText(applicationErrorEvent);

        Assert.Contains("Last Error Event", formatted);
        Assert.Contains("Provider: Application Error", formatted);
        Assert.Contains("EventId: 1000", formatted);
        Assert.Contains("FaultingApplicationName: demo.exe", formatted);
        Assert.Contains("ExceptionCode: 0xc0000005", formatted);
        Assert.Contains("Message:", formatted);
        Assert.Contains("Faulting application name: demo.exe", formatted);
    }
}
