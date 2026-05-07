using System.Text;

namespace LastErrorEvent;

public static class ApplicationErrorFormatter
{
    public static string FormatClipboardText(ApplicationErrorEvent applicationErrorEvent)
    {
        StringBuilder builder = new();
        builder.AppendLine("Last Error Event");
        builder.AppendLine($"CapturedAtLocal: {DateTimeOffset.Now:O}");
        builder.AppendLine($"TimeCreatedLocal: {applicationErrorEvent.TimeCreated:O}");
        builder.AppendLine($"LogName: {applicationErrorEvent.LogName}");
        builder.AppendLine($"Provider: {applicationErrorEvent.ProviderName}");
        builder.AppendLine($"EventId: {applicationErrorEvent.EventId}");

        AppendIfPresent(builder, "RecordId", applicationErrorEvent.RecordId);
        AppendIfPresent(builder, "Level", applicationErrorEvent.Level);
        AppendIfPresent(builder, "MachineName", applicationErrorEvent.MachineName);
        AppendIfPresent(builder, "ProcessId", applicationErrorEvent.ProcessId);
        AppendIfPresent(builder, "ThreadId", applicationErrorEvent.ThreadId);

        if (applicationErrorEvent.DataFields.Count > 0)
        {
            builder.AppendLine();
            builder.AppendLine("EventData:");

            foreach (EventField field in applicationErrorEvent.DataFields)
            {
                builder.AppendLine($"{field.Name}: {field.Value}");
            }
        }

        builder.AppendLine();
        builder.AppendLine("Message:");
        builder.AppendLine(applicationErrorEvent.Message);

        return builder.ToString().TrimEnd();
    }

    private static void AppendIfPresent<T>(StringBuilder builder, string label, T? value)
    {
        if (value is null)
        {
            return;
        }

        builder.AppendLine($"{label}: {value}");
    }
}
