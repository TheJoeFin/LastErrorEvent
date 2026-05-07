namespace LastErrorEvent.Tests;

public sealed class CommandLineOptionsParserTests
{
    [Fact]
    public void Parse_UsesInteractiveDefaults()
    {
        ParseResult result = CommandLineOptionsParser.Parse([], isInteractive: true);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Options);
        Assert.Equal(TimeSpan.FromHours(1), result.Options!.MaxAge);
        Assert.True(result.Options.PromptForEventViewer);
        Assert.False(result.Options.OpenEventViewer);
    }

    [Fact]
    public void Parse_AcceptsMaxAgeAndEventViewerFlags()
    {
        ParseResult result = CommandLineOptionsParser.Parse(["--max-age-hours", "2.5", "--open-event-viewer"], isInteractive: true);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Options);
        Assert.Equal(TimeSpan.FromHours(2.5), result.Options!.MaxAge);
        Assert.True(result.Options.OpenEventViewer);
        Assert.False(result.Options.PromptForEventViewer);
    }

    [Fact]
    public void Parse_RejectsInvalidMaxAge()
    {
        ParseResult result = CommandLineOptionsParser.Parse(["--max-age-hours", "0"], isInteractive: false);

        Assert.False(result.IsSuccess);
        Assert.Equal("--max-age-hours must be a positive number.", result.ErrorMessage);
    }
}
