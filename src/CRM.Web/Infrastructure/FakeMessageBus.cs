using System;

namespace CRM.Web.Infrastructure;

public class FakeMessageBus : IMessageBus
{
    private readonly IBus _bus;

    public FakeMessageBus(IBus bus)
    {
        _bus = bus;
    }

    public void SendEmailChangedMessage(Guid userId, string newEmail)
    {
        _bus.Send($"Sending message about user with id {userId} about his/her new email: '{newEmail}'");
    }
}