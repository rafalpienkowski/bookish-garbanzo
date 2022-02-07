using System;

namespace CRM.Web.Infrastructure;

public interface IMessageBus
{
    void SendEmailChangedMessage(Guid userId, string newEmail);
}