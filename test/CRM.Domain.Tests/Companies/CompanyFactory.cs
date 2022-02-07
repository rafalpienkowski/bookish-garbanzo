using System;
using CRM.Domain.Companies;

namespace CRM.Domain.Tests.Companies
{
    internal static class CompanyFactory
    {
        internal static Company CreateCompany(string domain, int numberOfEmployees)
            => new Company(Guid.NewGuid(), domain, numberOfEmployees, true);
        
    }
}