using System.Xml.Linq;

namespace LastErrorEvent;

public static class ApplicationErrorXmlParser
{
    public static IReadOnlyList<EventField> ParseDataFields(string xml)
    {
        XDocument document = XDocument.Parse(xml);
        XNamespace eventsNamespace = "http://schemas.microsoft.com/win/2004/08/events/event";
        List<EventField> fields = new();
        int unnamedIndex = 1;

        IEnumerable<XElement> dataElements = document
            .Descendants(eventsNamespace + "EventData")
            .Elements(eventsNamespace + "Data");

        foreach (XElement element in dataElements)
        {
            string name = (string?)element.Attribute("Name") is { Length: > 0 } attributeName
                ? attributeName
                : $"Field{unnamedIndex++}";

            fields.Add(new EventField(name, NormalizeValue(element.Value)));
        }

        return fields;
    }

    private static string NormalizeValue(string value) => value.Trim();
}
