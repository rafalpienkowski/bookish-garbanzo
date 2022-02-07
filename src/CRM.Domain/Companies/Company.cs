using System;
using CRM.Domain.Companies.Events;
using CRM.Domain.Core;
using CRM.Domain.Helpers;

namespace CRM.Domain.Companies
{
    public class Company : Aggregate
    {
        public string DomainName { get; private set; }
        public int NumberOfEmployees { get; private set; }
        
        public bool Active { get; private set; }

        internal Company(Guid id, string domainName, int numberOfEmployees, bool active)
        {
            Id = id;
            DomainName = domainName;
            NumberOfEmployees = numberOfEmployees;
            Active = active;
        }

        public void ChangeNumberOfEmployees(int delta)
        {
            Precondition.Requires(NumberOfEmployees + delta >= 0);
            NumberOfEmployees += delta;
        }

        public bool IsEmailCorporate(string email)
        {
            var emailDomain = email.Split('@')[1];

            return emailDomain == DomainName;
        }

        public Result CanBeClosed() => NumberOfEmployees == 0 ? Result.Successful() : Result.Failure("Company with employees can't be closed");

        public void Close()
        {
            Precondition.Requires(CanBeClosed().IsSuccess);
            Active = false;
            PendingEvents.Add(CompanyClosedEvent.Create(Id));
        }

        public static Company CreateNew(Guid companyId, string domainName)
        {
            var company = new Company(companyId, domainName, 0, true);
            company.PendingEvents.Add(NewCompanyCreated.Created(companyId, domainName));
            
            return company;
        }
    }
}