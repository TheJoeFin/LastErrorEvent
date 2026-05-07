using System.Diagnostics.Eventing.Reader;
using System.Runtime.Versioning;

namespace LastErrorEvent;

public static class ApplicationErrorLogReader
{
    private const string ApplicationLogName = "Application";
    private const string ProviderName = "Application Error";
    private const int ApplicationErrorEventId = 1000;

    [SupportedOSPlatform("windows")]
    public static EventLookupResult GetLatestApplicationError(TimeSpan maxAge, DateTimeOffset now)
    {
        string queryText = $"*[System[(EventID={ApplicationErrorEventId}) and Provider[@Name='{ProviderName}']]]";
        EventLogQuery query = new(ApplicationLogName, PathType.LogName, queryText)
        {
            ReverseDirection = true,
            TolerateQueryErrors = false
        };

        using EventLogReader reader = new(query);

        while (reader.ReadEvent() is EventRecord record)
        {
            using (record)
            {
                if (record.TimeCreated is null)
                {
                    continue;
                }

                DateTimeOffset eventTime = new(record.TimeCreated.Value);
                if (eventTime < now - maxAge)
                {
                    return EventLookupResult.OlderThanMaxAge(eventTime);
                }

                return EventLookupResult.Found(Map(record));
            }
        }

        return EventLookupResult.NotFound();
    }

    [SupportedOSPlatform("windows")]
    private static ApplicationErrorEvent Map(EventRecord record)
    {
        string message = record.FormatDescription() ?? "No event description was available.";
        string xml = record.ToXml();
        IReadOnlyList<EventField> dataFields = ApplicationErrorXmlParser.ParseDataFields(xml);

        return new ApplicationErrorEvent(
            record.LogName ?? ApplicationLogName,
            record.ProviderName ?? ProviderName,
            record.Id,
            record.RecordId,
            record.LevelDisplayName,
            record.MachineName,
            new DateTimeOffset(record.TimeCreated!.Value),
            record.ProcessId,
            record.ThreadId,
            dataFields,
            message,
            xml);
    }
}
