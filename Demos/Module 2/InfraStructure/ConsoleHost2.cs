using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace InfraStructure;

public class ConsoleHost2 : IHostedService
{

    private readonly ICounter _counter;
    private readonly ILogger<ConsoleHost2> _logger;

    public ConsoleHost2(ICounter counter, ILogger<ConsoleHost2> logger)
    {
        _logger = logger;
        //Console.WriteLine("De tweede host");
        _logger.LogInformation("De tweede host");
        _counter = counter;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
            for (int i = 0; i < 5; i++)
            {
                _counter.Increment();
                _counter.Show();
            }
            return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
