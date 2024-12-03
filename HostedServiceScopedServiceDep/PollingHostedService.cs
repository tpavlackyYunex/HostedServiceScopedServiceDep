namespace HostedServiceScopedServiceDep;

public class PollingHostedService : IHostedService
{
    private readonly ILogger<PollingHostedService> _logger;
    private readonly INumberService _numberService;

    public PollingHostedService(ILogger<PollingHostedService> logger, INumberService numberService)
    {
        _logger = logger;
        _numberService = numberService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(Poll, cancellationToken);

        return Task.CompletedTask;
    }

    private void Poll()
    {
        while (true)
        {
            _logger.LogInformation("Next number: {Number} (instanceID: {InstanceID})", _numberService.GetNextNumber(), _numberService.Id);
            Task.Delay(1000).Wait();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}


public interface INumberService : IDisposable
{
    Guid Id { get; }
    int GetNextNumber();
}

public class NumberService : INumberService
{
    private int _value;

    public Guid Id { get; } = Guid.NewGuid();

    public int GetNextNumber()
    {
        return _value++;
    }

    public void Dispose()
    {
        Console.WriteLine($"Disposed instance: {Id}");
    }
}