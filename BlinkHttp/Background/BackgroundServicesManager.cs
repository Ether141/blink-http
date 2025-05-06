namespace BlinkHttp.Background;

internal class BackgroundServicesManager : IBackgroundServices
{
    private readonly List<(IBackgroundService service, bool autoStart)> services;

    internal BackgroundServicesManager(List<(IBackgroundService service, bool autoStart)> services)
    {
        this.services = services;
    }

    public T GetService<T>() where T : IBackgroundService => (T)GetServiceByType<T>();

    public bool IsRunning<T>() where T : IBackgroundService
    {
        IBackgroundService service = GetServiceByType<T>();
        return service.IsRunning;
    }

    public void StartService<T>() where T : IBackgroundService
    {
        IBackgroundService service = GetServiceByType<T>();

        if (!service.IsRunning)
        {
            service.StartAsync();
        }
    }

    public async Task StopServiceAsync<T>() where T : IBackgroundService
    {
        IBackgroundService service = GetServiceByType<T>();

        if (service.IsRunning)
        {
            await service.StopAsync();
        }
    }

    internal void StartAllServices()
    {
        foreach (var service in services)
        {
            if (service.autoStart)
            {
                StartService(service.service);
            }
        }
    }

    internal async Task StopAllServicesAsync()
    {
        foreach (var service in services)
        {
            await service.service.StopAsync();
        }
    }

    private IBackgroundService GetServiceByType<T>() where T : IBackgroundService =>
        services.FirstOrDefault(s => s.service.GetType() == typeof(T)).service ?? throw new ArgumentException("Background task with given type is not registered.");

    private void StartService(IBackgroundService service)
    {
        _ = Task.Run(service.StartAsync);
    }
}
