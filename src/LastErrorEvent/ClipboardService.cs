using System.Diagnostics;
using System.Runtime.Versioning;

namespace LastErrorEvent;

public static class ClipboardService
{
    [SupportedOSPlatform("windows")]
    public static void CopyText(string value)
    {
        ProcessStartInfo startInfo = new()
        {
            FileName = "clip.exe",
            RedirectStandardInput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using Process process = Process.Start(startInfo)
            ?? throw new InvalidOperationException("clip.exe could not be started.");

        process.StandardInput.Write(value);
        process.StandardInput.Close();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"clip.exe exited with code {process.ExitCode}.");
        }
    }
}
