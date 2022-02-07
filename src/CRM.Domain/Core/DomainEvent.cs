using System;

namespace CRM.Domain.Core
{
    public abstract class DomainEvent
    {
        public DateTime OccuredAt { get; protected set; }
        public Guid Id { get; protected set; }
    }
}