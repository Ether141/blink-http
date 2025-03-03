namespace BlinkHttp.Http
{
    internal class FileResult : IHttpResult
    {
        public byte[] Data { get; }
        public string ContentType { get; }

        public FileResult(byte[] data, string contentType)
        {
            Data = data;
            ContentType = contentType;
        }
    }
}
