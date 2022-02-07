using System;
using CRM.Domain.Users;
using Microsoft.Extensions.Logging;

namespace CRM.Web.Infrastructure;

public class DomainLogger : IDomainLogger
{
    private readonly ILogger _logger;

    public DomainLogger(ILogger logger)
    {
        _logger = logger;
    }


    public void UserTypeHasChanged(Guid userId, UserType oldType, UserType newType)
    {
        _logger.LogInformation($"User {userId} changed type from {oldType} to {newType}");
    }
}