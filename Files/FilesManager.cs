using BlinkHttp.Http;

namespace BlinkHttp.Files
{
    internal static class FilesManager
    {
        internal static string WebFolderPath => HttpServer.WebFolderPath;

        internal static bool FileExists(Uri url)
        {
            string pathFromUrl = url.AbsolutePath[1..];
            string localPath = Path.Combine(WebFolderPath, pathFromUrl);
            return File.Exists(localPath);
        }

        internal static string GetLocalPathFile(Uri url) => url.AbsolutePath == "/" ? Path.Combine(WebFolderPath, "index.html") : Path.Combine(WebFolderPath, url.AbsolutePath[1..]);
    }
}
