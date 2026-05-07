namespace LastErrorEvent.Tests;

public sealed class ApplicationErrorXmlParserTests
{
    [Fact]
    public void ParseDataFields_UsesProvidedNamesAndNumbersUnnamedFields()
    {
        const string xml =
            """
            <Event xmlns="http://schemas.microsoft.com/win/2004/08/events/event">
              <EventData>
                <Data Name="FaultingApplicationName">demo.exe</Data>
                <Data>unnamed-value</Data>
              </EventData>
            </Event>
            """;

        IReadOnlyList<EventField> fields = ApplicationErrorXmlParser.ParseDataFields(xml);

        Assert.Collection(
            fields,
            field =>
            {
                Assert.Equal("FaultingApplicationName", field.Name);
                Assert.Equal("demo.exe", field.Value);
            },
            field =>
            {
                Assert.Equal("Field1", field.Name);
                Assert.Equal("unnamed-value", field.Value);
            });
    }
}
