using System;
using CRM.Domain.Core;

namespace CRM.Domain.Users.Events
{
    public class UserEmailChangedEvent : DomainEvent
    {
        public Guid UserId { get; }
        public string NewEmail { get; }

        internal UserEmailChangedEvent(Guid id, DateTime occuredAt, Guid userId, string newEmail)
        {
            Id = id;
            OccuredAt = occuredAt;
            UserId = userId;
            NewEmail = newEmail;
        }

        public static UserEmailChangedEvent Create(Guid userId, string newEmail) => new UserEmailChangedEvent(Guid.NewGuid(), DateTime.UtcNow, userId, newEmail);
    }
}