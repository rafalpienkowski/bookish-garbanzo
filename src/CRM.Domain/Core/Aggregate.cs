using System;
using System.Collections.Generic;

namespace CRM.Domain.Core
{
    public abstract class Aggregate
    {
        public Guid Id { get; protected set; }

        public List<DomainEvent> PendingEvents { get; } = new List<DomainEvent>();
    }
}