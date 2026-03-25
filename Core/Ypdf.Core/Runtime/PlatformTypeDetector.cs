using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Ypdf.Core.Extensions;

namespace Ypdf.Core.Runtime;

public static class PlatformTypeDetector
{
    private const string _linuxIdentityDirectory = "/System/Library/CoreServices";
    private const string _macOSIdentityDirectory = "/proc";
    private const int _maxProcessWaitingTimeMs = 1000;

    public static bool IsWindows => SafeDetectPlatformType() == PlatformType.Windows;
    public static bool IsLinux => SafeDetectPlatformType() == PlatformType.Linux;
    public static bool IsMacOS => SafeDetectPlatformType() == PlatformType.MacOS;

    private static IReadOnlyCollection<string> LinuxPlatformNames => ["linux"];
    private static IReadOnlyCollection<string> MacOSPlatformNames => ["mac", "darwin"];

#if NET5_0_OR_GREATER
    public static PlatformType DetectPlatformType()
    {
        if (OperatingSystem.IsWindows())
            return PlatformType.Windows;

        if (OperatingSystem.IsLinux())
            return PlatformType.Linux;

        if (OperatingSystem.IsMacOS())
            return PlatformType.MacOS;

        return PlatformType.Other;
    }
#elif NET471_OR_GREATER
    public static PlatformType DetectPlatformType()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return PlatformType.Windows;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return PlatformType.Linux;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return PlatformType.MacOS;

        return PlatformType.Other;
    }
#else
    public static PlatformType DetectPlatformType()
    {
        PlatformID platformID = Environment.OSVersion.Platform;

        if (platformID == PlatformID.Win32S ||
            platformID == PlatformID.Win32Windows ||
            platformID == PlatformID.Win32NT ||
            platformID == PlatformID.WinCE)
        {
            return PlatformType.Windows;
        }

        if (platformID == PlatformID.MacOSX)
            return PlatformType.MacOS;

        return DetectUnixPlatformTypeUsingFingerprints();
    }
#endif

    public static bool TryDetectPlatformType(out PlatformType platformType)
    {
        try
        {
            platformType = DetectPlatformType();
            return true;
        }
        catch
        {
            platformType = PlatformType.Other;
            return false;
        }
    }

    public static PlatformType SafeDetectPlatformType(PlatformType platformTypeIfErrorOccured = PlatformType.Other)
    {
        return TryDetectPlatformType(out PlatformType platformType)
            ? platformType
            : platformTypeIfErrorOccured;
    }

    public static PlatformType DetectUnixPlatformTypeUsingFingerprints()
    {
        if (Directory.Exists(_linuxIdentityDirectory))
            return PlatformType.Linux;

        if (Directory.Exists(_macOSIdentityDirectory))
            return PlatformType.MacOS;

        return DetectUnixPlatformTypeUsingUname();
    }

    private static PlatformType DetectUnixPlatformTypeUsingUname()
    {
        var processStartInfo = new ProcessStartInfo("uname", "-s")
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true
        };

        string? platformName;

        try
        {
            using var process = new Process()
            {
                StartInfo = processStartInfo
            };

            process.Start();
            platformName = process.StandardOutput.ReadLine();

            process.WaitForExit(_maxProcessWaitingTimeMs);
        }
        catch
        {
            return PlatformType.Other;
        }

        if (string.IsNullOrEmpty(platformName))
            return PlatformType.Other;

        if (LinuxPlatformNames.Any(t => platformName.Contains(t, StringComparison.OrdinalIgnoreCase)))
            return PlatformType.Linux;

        if (MacOSPlatformNames.Any(t => platformName.Contains(t, StringComparison.OrdinalIgnoreCase)))
            return PlatformType.MacOS;

        return PlatformType.Other;
    }
}
