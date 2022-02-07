using System;
using CRM.Domain.Core;
using CRM.Domain.Tests.Companies;
using CRM.Domain.Users;
using CRM.Domain.Users.Events;
using FluentAssertions;
using Xunit;

namespace CRM.Domain.Tests.Users
{
    public class UserTests
    {
        [Fact]
        public void Changing_email_from_non_corporate_to_corporate()
        {
            var company = CompanyFactory.CreateCompany("my-company.com", 1);
            var sut = CreateUser("user@gmail.com", UserType.Customer);
            
            sut.ChangeEmail("new@my-company.com", company);

            company.NumberOfEmployees.Should().Be(2);
            sut.Email.Should().Be("new@my-company.com");
            sut.Type.Should().Be(UserType.Employee);
            EventsContainEmailChangedEvent(sut, UserEmailChangedEvent.Create(sut.Id, "new@my-company.com"));
            EventsContainUserTypeChangedEvent(sut, UserTypeChangedEvent.Create(sut.Id, UserType.Customer, UserType.Employee));
        }

        [Fact]
        public void Changing_email_from_corporate_to_non_corporate()
        {
            var company = CompanyFactory.CreateCompany("my-company.com", 1);
            var sut = CreateUser("user@my-company.com", UserType.Employee);
            
            sut.ChangeEmail("new@gmail.com", company);

            company.NumberOfEmployees.Should().Be(0);
            sut.Email.Should().Be("new@gmail.com");
            sut.Type.Should().Be(UserType.Customer);
            EventsContainEmailChangedEvent(sut, UserEmailChangedEvent.Create(sut.Id, "new@gmail.com"));
            EventsContainUserTypeChangedEvent(sut, UserTypeChangedEvent.Create(sut.Id, UserType.Employee, UserType.Customer));
        }

        [Fact]
        public void Changing_email_without_changing_user_type()
        {
            var company = CompanyFactory.CreateCompany("my-company.com", 1);
            var sut = CreateUser( "user@my-company.com", UserType.Employee);
            
            sut.ChangeEmail("new@my-company.com", company);

            company.NumberOfEmployees.Should().Be(1);
            sut.Email.Should().Be("new@my-company.com");
            sut.Type.Should().Be(UserType.Employee);
            EventsContainEmailChangedEvent(sut, UserEmailChangedEvent.Create(sut.Id, "new@my-company.com"));
        }

        [Fact]
        public void Changing_email_to_the_same_one()
        {
            var company = CompanyFactory.CreateCompany("my-company.com", 1);
            var sut = CreateUser( "user@my-company.com", UserType.Customer);
            
            sut.ChangeEmail("user@my-company.com", company);

            company.NumberOfEmployees.Should().Be(1);
            sut.Email.Should().Be("user@my-company.com");
            sut.Type.Should().Be(UserType.Customer);
            sut.PendingEvents.Should().BeEmpty();
        }

        [Fact]
        public void Changing_confirmed_email_is_not_possible()
        {
            var company = CompanyFactory.CreateCompany("my-company.com", 1);
            var sut = CreateUser( "user@my-company.com", UserType.Customer, isEmailConfirmed: true);
            
            var action = () => sut.ChangeEmail("user@my-company.com", company);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        private static User CreateUser(string email, UserType userType, bool isEmailConfirmed = false)
            => new User(Guid.NewGuid(), email, userType, isEmailConfirmed);

        private static void EventsContainEmailChangedEvent(User sut, UserEmailChangedEvent emailChangedEvent)
        {
            sut.PendingEvents.Should().ContainEquivalentOf(emailChangedEvent, options =>
            {
                options.Excluding(e => e.OccuredAt);
                options.Excluding(e => e.Id);

                return options;
            });
        }

        private static void EventsContainUserTypeChangedEvent(User sut, UserTypeChangedEvent userTypeChangedEvent)
        {
            sut.PendingEvents.Should().ContainEquivalentOf(userTypeChangedEvent, options =>
            {
                options.Excluding(e => e.OccuredAt);
                options.Excluding(e => e.Id);

                return options;
            });
            
        }
    }
}