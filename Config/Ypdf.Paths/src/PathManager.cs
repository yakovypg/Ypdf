namespace Ypdf.Paths;

public static class PathManager
{
    public static bool TryPrepareDirectories()
    {
        try
        {
            Directories.Prepare();
            return true;
        }
#pragma warning disable CA1031 // Do not catch general exception types
        catch
        {
            return false;
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }

    public static bool TryPrepareFiles()
    {
        try
        {
            Files.Prepare();
            return true;
        }
#pragma warning disable CA1031 // Do not catch general exception types
        catch
        {
            return false;
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }
}
