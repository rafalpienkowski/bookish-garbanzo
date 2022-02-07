using System;
using CRM.Domain.Core;

namespace CRM.Domain.Users.Events
{
    public class UserTypeChangedEvent : DomainEvent
    {
        public Guid UserId { get; }
        public UserType OldType { get; }
        public UserType NewType { get; }

        internal UserTypeChangedEvent(Guid userId, UserType oldType, UserType newType)
        {
            UserId = userId;
            OldType = oldType;
            NewType = newType;
        }

        public static UserTypeChangedEvent Create(Guid userId, UserType oldType, UserType newType) =>
            new UserTypeChangedEvent(userId, oldType, newType);
    }
}