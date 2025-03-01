namespace BlinkHttp
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            App app = new App();
            await app.Run(args);
        }
    }
}
