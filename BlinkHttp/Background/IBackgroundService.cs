namespace BlinkHttp.Background;

/// <summary>
/// Defines a background service with start and stop functionality.
/// </summary>
public interface IBackgroundService
{
    /// <summary>
    /// Gets a value indicating whether the service is currently running.
    /// </summary>
    bool IsRunning { get; }

    /// <summary>
    /// Starts the background service asynchronously. This method should return <seealso cref="Task"/> which represents background service operation. Then it will be used to run on background thread.
    /// </summary>
    Task StartAsync();

    /// <summary>
    /// Stops the background service asynchronously. <seealso cref="BlinkHttp.Application.WebApplication"/> will wait for this operation to be completed.
    /// </summary>
    Task StopAsync();
}
