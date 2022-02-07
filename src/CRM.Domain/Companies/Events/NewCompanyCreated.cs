using System;
using CRM.Domain.Core;

namespace CRM.Domain.Companies.Events
{
    public class NewCompanyCreated : DomainEvent
    {
        public Guid CompanyId { get; }
        public string DomainName { get; }

        internal NewCompanyCreated(Guid companyId, string domainName)
        {
            CompanyId = companyId;
            DomainName = domainName;
        }

        public static NewCompanyCreated Created(Guid companyId, string domainName) =>
            new NewCompanyCreated(companyId, domainName);
    }
}