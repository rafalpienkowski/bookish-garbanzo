using System;
using CRM.Domain.Companies.Events;
using FluentAssertions;
using Xunit;

namespace CRM.Domain.Tests.Companies
{
    public class CompanyTests
    {
        [InlineData("my-company.com", "chief@my-company.com", true)]
        [InlineData("my-company.com", "random@gmail.com", false)]
        [Theory]
        public void Differentiates_a_corporate_email_from_non_corporate(string domain, string email,
            bool expectedResult)
        {
            var sut = CompanyFactory.CreateCompany(domain, 0);

            var result = sut.IsEmailCorporate(email);

            result.Should().Be(expectedResult);
        }

        [InlineData(3,4)]
        [InlineData(123,124)]
        [Theory]
        public void Company_number_of_employees_has_to_be_at_least_zero(int delta, int expectedNumberOfEmployees)
        {
            var sut = CompanyFactory.CreateCompany("my-company.com", 1);
            
            sut.ChangeNumberOfEmployees(delta);

            sut.NumberOfEmployees.Should().Be(expectedNumberOfEmployees);
        }

        [InlineData(-5)]
        [InlineData(-111)]
        [Theory]
        public void Company_not_allow_to_decrease_number_of_employees_below_zero(int delta)
        {
            var sut = CompanyFactory.CreateCompany("my-company.com", 3);

            var act = () => sut.ChangeNumberOfEmployees(delta);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Company_can_be_closed_when_nobody_works_there()
        {
            var sut = CompanyFactory.CreateCompany("my-company.com", 0);
            
            sut.Close();

            sut.PendingEvents.Should().ContainEquivalentOf(CompanyClosedEvent.Create(sut.Id));
        }
    }
}