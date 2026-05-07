namespace LastErrorEvent;

public sealed record EventField(string Name, string? Value);

public sealed record ApplicationErrorEvent(
    string LogName,
    string ProviderName,
    int EventId,
    long? RecordId,
    string? Level,
    string? MachineName,
    DateTimeOffset TimeCreated,
    int? ProcessId,
    int? ThreadId,
    IReadOnlyList<EventField> DataFields,
    string Message,
    string Xml);
