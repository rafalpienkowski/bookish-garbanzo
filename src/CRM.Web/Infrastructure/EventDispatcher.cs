using System.Collections.Generic;
using CRM.Domain.Core;
using CRM.Domain.Users.Events;

namespace CRM.Web.Infrastructure;

public class EventDispatcher
{
    private readonly IMessageBus _messageBus;
    private readonly IDomainLogger _domainLogger;

    public EventDispatcher(IMessageBus messageBus, IDomainLogger domainLogger)
    {
        _messageBus = messageBus;
        _domainLogger = domainLogger;
    }

    public void Dispatch(List<DomainEvent> events)
    {
        foreach (var domainEvent in events)
        {
            Dispatch(domainEvent);
        }
    }

    private void Dispatch(DomainEvent domainEvent)
    {
        switch (domainEvent)
        {
            case UserEmailChangedEvent userEmailChanged:
                _messageBus.SendEmailChangedMessage(userEmailChanged.UserId, userEmailChanged.NewEmail);
                break;
            case UserTypeChangedEvent userTypeChangedEvent:
                _domainLogger.UserTypeHasChanged(userTypeChangedEvent.UserId, userTypeChangedEvent.OldType, userTypeChangedEvent.NewType);
                break;
        }
    }
}