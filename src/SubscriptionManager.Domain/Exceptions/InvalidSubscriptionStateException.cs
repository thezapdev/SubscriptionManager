using System;
namespace SubscriptionManager.Domain.Exceptions;

public sealed class InvalidSubscriptionStateException : DomainException
{
	public InvalidSubscriptionStateException(string message) : base(message){} 
}
