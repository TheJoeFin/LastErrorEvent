using System.Diagnostics;
using System.Runtime.Versioning;

namespace LastErrorEvent;

public static class EventViewerLauncher
{
    [SupportedOSPlatform("windows")]
    public static void Launch()
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "eventvwr.msc",
            UseShellExecute = true
        });
    }
}
