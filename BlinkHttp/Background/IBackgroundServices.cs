namespace BlinkHttp.Background;

/// <summary>
/// Defines a contract for managing background services.
/// </summary>
public interface IBackgroundServices
{
    /// <summary>
    /// Checks if a specific background service is currently running.
    /// </summary>
    /// <typeparam name="T">The type of the background service to check.</typeparam>
    /// <returns><c>true</c> if the service is running; otherwise, <c>false</c>.</returns>
    bool IsRunning<T>() where T : IBackgroundService;

    /// <summary>
    /// Starts a specific background service.
    /// </summary>
    /// <typeparam name="T">The type of the background service to start.</typeparam>
    void StartService<T>() where T : IBackgroundService;

    /// <summary>
    /// Stops a specific background service asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the background service to stop.</typeparam>
    /// <returns>A <see cref="Task"/> representing the asynchronous stop operation.</returns>
    Task StopServiceAsync<T>() where T : IBackgroundService;
}
