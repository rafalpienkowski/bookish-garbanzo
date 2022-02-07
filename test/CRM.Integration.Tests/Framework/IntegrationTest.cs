using System;
using System.Linq;
using System.Threading.Tasks;
using CRM.Domain.Companies;
using CRM.Domain.Users;
using CRM.Web.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CRM.Integration.Tests.Framework;

public abstract class IntegrationTest : IAsyncLifetime
{
    private const string ConnectionString = "Host=docker.local;Port=5555;Database=app;Username=webapp;Password=somePassword123#@!;SearchPath=crm";
    protected readonly CrmContext Database;

    protected IntegrationTest()
    {
        var optionsBuilder = new DbContextOptionsBuilder<CrmContext>();
        optionsBuilder.UseNpgsql(ConnectionString);

        Database = new CrmContext(optionsBuilder.Options);
    }

    protected async Task<User> CreateUser(string userEmail, UserType userType)
    {
        var user = User.CreateNew(Guid.NewGuid(), userEmail, userType);
        await Database.Users.AddAsync(user);

        await Database.SaveChangesAsync();

        return user;
    }

    protected async Task<Company> CreateCompany(string domainName, int numberOfEmployees)
    {
        var company = Company.CreateNew(Guid.NewGuid(), domainName);
        company.ChangeNumberOfEmployees(numberOfEmployees);
        await Database.Companies.AddAsync(company);

        await Database.SaveChangesAsync();

        return company;
    }

    public Task InitializeAsync() => Task.CompletedTask;
    
    public async Task DisposeAsync()
    {
        Database.Companies.RemoveRange(Database.Companies.ToList());
        Database.Users.RemoveRange(Database.Users.ToList());
        
        await Database.SaveChangesAsync();
        await Database.DisposeAsync();
    }
}