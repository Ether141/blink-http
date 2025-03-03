namespace BlinkHttp.Http
{
    internal interface IHttpResult
    {
        public byte[] Data { get; }
        public string ContentType { get; }
    }
}
