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
        catch
        {
            return false;
        }
    }

    public static bool TryPrepareFiles()
    {
        try
        {
            Files.Prepare();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
