using CRM.Domain.Companies;
using FluentAssertions;

namespace CRM.Integration.Tests.Framework;

public static class CompanyExtensions
{
    public static Company ShouldExist(this Company company)
    {
        company.Should().NotBeNull();

        return company;
    }

    public static Company WithNumberOfEmployees(this Company company, int numberOfEmployees)
    {
        company.NumberOfEmployees.Should().Be(numberOfEmployees);

        return company;
    }
}