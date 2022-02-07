using System;
using CRM.Domain.Users;

namespace CRM.Web.Infrastructure;

public interface IDomainLogger
{
    void UserTypeHasChanged(Guid userId, UserType oldType, UserType newType);
}