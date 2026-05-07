namespace LastErrorEvent;

public enum EventLookupStatus
{
    Found,
    NotFound,
    OlderThanMaxAge
}

public sealed record EventLookupResult(
    EventLookupStatus Status,
    ApplicationErrorEvent? Event,
    DateTimeOffset? MostRecentEventTime)
{
    public static EventLookupResult Found(ApplicationErrorEvent applicationErrorEvent) =>
        new(EventLookupStatus.Found, applicationErrorEvent, applicationErrorEvent.TimeCreated);

    public static EventLookupResult NotFound() =>
        new(EventLookupStatus.NotFound, null, null);

    public static EventLookupResult OlderThanMaxAge(DateTimeOffset mostRecentEventTime) =>
        new(EventLookupStatus.OlderThanMaxAge, null, mostRecentEventTime);
}
