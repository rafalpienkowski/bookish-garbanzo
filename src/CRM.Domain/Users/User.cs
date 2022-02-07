using System;
using System.Runtime.CompilerServices;
using CRM.Domain.Companies;
using CRM.Domain.Core;
using CRM.Domain.Helpers;
using CRM.Domain.Users.Events;

[assembly: InternalsVisibleTo("CRM.Domain.Tests")]
namespace CRM.Domain.Users
{
    public class User : Aggregate
    {
        public string Email { get; private set; }
        public UserType Type { get; private set; }
        public bool IsEmailConfirmed { get; private set; }

        internal User(Guid id, string email, UserType type, bool isEmailConfirmed)
        {
            Id = id;
            Email = email;
            Type = type;
            IsEmailConfirmed = isEmailConfirmed;
        }

        public static User CreateNew(Guid id, string email, UserType type) => new User(id, email, type, false); 

        public Result CanChangeEmail() => IsEmailConfirmed ? Result.Failure("Can't change confirmed email") : Result.Successful();

        public void ChangeEmail(string newEmail, Company company)
        {
            Precondition.Requires(CanChangeEmail().IsSuccess);
            
            if (Email == newEmail)
            {
                return;
            }

            var newType = company.IsEmailCorporate(newEmail) ? UserType.Employee : UserType.Customer;

            if (Type != newType)
            {
                var delta = newType == UserType.Employee ? 1 : -1;
                company.ChangeNumberOfEmployees(delta);
                PendingEvents.Add(new UserTypeChangedEvent(Id, Type, newType));
            }

            Email = newEmail;
            Type = newType;
            
            PendingEvents.Add(UserEmailChangedEvent.Create(Id, newEmail));
        }
    }
}