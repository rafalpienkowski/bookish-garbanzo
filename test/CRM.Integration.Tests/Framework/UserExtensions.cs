using CRM.Domain.Users;
using FluentAssertions;

namespace CRM.Integration.Tests.Framework;

public static class UserExtensions
{
    public static User ShouldExist(this User user)
    {
        user.Should().NotBeNull();
        
        return user;
    }

    public static User WithEmail(this User user, string email)
    {
        user.Email.Should().Be(email);

        return user;
    }

    public static User WithType(this User user, UserType type)
    {
        user.Type.Should().Be(type);

        return user;
    }
}