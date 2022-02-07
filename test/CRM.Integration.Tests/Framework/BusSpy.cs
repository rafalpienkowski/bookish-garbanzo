using System;
using System.Collections.Generic;
using CRM.Web.Infrastructure;
using FluentAssertions;

namespace CRM.Integration.Tests.Framework;

public class BusSpy : IBus
{
    private readonly List<string> _messages = new();

    public void Send(string message) => _messages.Add(message);

    public BusSpy ShouldSendNumberOfMessages(int number)
    {
        _messages.Count.Should().Be(number);
            
        return this;
    }

    public BusSpy WithEmailChangedMessage(Guid userId, string newEmail)
    {
        var message = $"Sending message about user with id {userId} about his/her new email: '{newEmail}'";
        _messages.Should().Contain(message);

        return this;
    }
}