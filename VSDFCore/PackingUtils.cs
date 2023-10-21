using System.IO.Compression;

namespace VSDFCore;

public static class PackingUtils
{
    public static bool UnpackFile(string source, string dest, out string tempDir)
    {
        if (!File.Exists(source) || !Directory.Exists(dest))
        {
            tempDir = string.Empty;
            return false;
        }

        tempDir = $"{dest}\\{Guid.NewGuid()}";

        ZipFile.ExtractToDirectory(source, tempDir);

        return true;
    }

    public static bool PackFile(string tempDir, string dest, string? outputFileName = null, bool deleteTempDir = true)
    {
        if (!Directory.Exists(tempDir) || !Directory.Exists(dest)) return false;

        ZipFile.CreateFromDirectory(tempDir,
            $"{dest}{outputFileName ?? Guid.NewGuid().ToString()}.vsdf",
            CompressionLevel.SmallestSize, false);

        if (!deleteTempDir) return true;

        Directory.Delete(tempDir, true);

        return true;
    }
}