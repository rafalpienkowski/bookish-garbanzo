using System;
using CRM.Domain.Core;

namespace CRM.Domain.Companies.Events
{
    public class CompanyClosedEvent : DomainEvent
    {
        public Guid CompanyId { get; }

        internal CompanyClosedEvent(Guid companyId)
        {
            CompanyId = companyId;
        }

        public static CompanyClosedEvent Create(Guid companyId) => new CompanyClosedEvent(companyId);
    }
}