using BlinkHttp.Http;

namespace BlinkHttp.Files;

internal static class FilesManager
{
    internal static string WebFolderPath => HttpServer.WebFolderPath;

    internal static bool FileExists(Uri url) => File.Exists(GetLocalPathFile(url));

    internal static bool FileExists(string localPath) => File.Exists(localPath);

    internal static string GetLocalPathFile(Uri url) => url.AbsolutePath == "/" ? Path.Combine(WebFolderPath, "index.html") : Path.Combine(WebFolderPath, url.AbsolutePath[1..]);

    internal static string GetLocalPathFile(string localPath) => Path.Combine(WebFolderPath, localPath);

    internal static byte[] LoadFile(Uri url) => LoadFile(GetLocalPathFile(url));

    internal static byte[] LoadFile(string localPath)
    {
        if (!File.Exists(localPath))
        {
            throw new FileNotFoundException($"File \"{localPath}\" does not exist on the server.");
        }

        return File.ReadAllBytes(localPath);
    }
}
