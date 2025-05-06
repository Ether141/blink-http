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
                await Task.Delay(2000, ct);
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
        if (!IsRunning)
        {
            return;
        }

        cts.Cancel();
        IsRunning = false;
        await Task.CompletedTask;
    }
}
