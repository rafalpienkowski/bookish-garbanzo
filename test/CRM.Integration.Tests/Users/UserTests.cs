using System.Threading.Tasks;
using CRM.Domain.Users;
using CRM.Integration.Tests.Framework;
using CRM.Web.Features.Users;
using CRM.Web.Infrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CRM.Integration.Tests.Users;

public class UserTests : IntegrationTest
{
    [Fact]
    public async Task Changing_email_from_corporate_to_non_corporate()
    {
        var user = await CreateUser("user@mycompany.com", UserType.Employee);
        var company = await CreateCompany("mycompany.com", 1);
        var busSpy = new BusSpy();
        var messageBus = new FakeMessageBus(busSpy);
        var domainLoggerMock = new Mock<IDomainLogger>();
        var sut = new UsersController(Database, messageBus, domainLoggerMock.Object);

        var result = await sut.ChangeEmail(company.Id, user.Id, "new@gmail.com");

        result.Should().BeOfType<OkResult>();

        var userFromDb = await Database.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        userFromDb
            .ShouldExist()
            .WithEmail("new@gmail.com")
            .WithType(UserType.Customer);

        var companyFromDb = await Database.Companies.FirstOrDefaultAsync(c => c.Id == company.Id);
        companyFromDb
            .ShouldExist()
            .WithNumberOfEmployees(0);
        
        busSpy.ShouldSendNumberOfMessages(1).WithEmailChangedMessage(user.Id, "new@gmail.com");
        domainLoggerMock.Verify(_ => _.UserTypeHasChanged(user.Id, UserType.Employee, UserType.Customer), Times.Once);
    }
}