namespace BlinkHttp.Http
{
    public class RequestFile
    {
        public string FileName { get; }
        public byte[] Data { get; }

        public RequestFile(string fileName, byte[] data)
        {
            FileName = fileName;
            Data = data;
        }
    }
}
