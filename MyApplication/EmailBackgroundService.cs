using BlinkHttp.Background;

namespace MyApplication;

internal class EmailBackgroundService : IBackgroundService
{
    public bool IsRunning { get; private set; } = false;

    private readonly CancellationTokenSource cts = new CancellationTokenSource();
    private readonly CancellationToken ct;

    public EmailBackgroundService()
    {
        ct = cts.Token;
    }

    public async Task StartAsync()
    {
        IsRunning = true;
        await Loop();
    }

    private async Task Loop()
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(1000, ct);
            }
            catch (TaskCanceledException)
            {
                break;
            }

            Console.WriteLine("sending email");
        }
    }

    public async Task StopAsync()
    {
        Console.WriteLine("stop async");
        cts.Cancel();
        IsRunning = false;
        await Task.CompletedTask;
    }
}
