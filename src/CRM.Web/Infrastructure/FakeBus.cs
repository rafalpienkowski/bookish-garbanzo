using Microsoft.Extensions.Logging;

namespace CRM.Web.Infrastructure;

public class FakeBus : IBus
{
    private readonly ILogger<FakeBus> _logger;

    public FakeBus(ILogger<FakeBus> logger)
    {
        _logger = logger;
    }

    public void Send(string message)
    {
        _logger.LogDebug(message);
    }
}