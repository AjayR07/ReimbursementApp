using Azure.Messaging.ServiceBus;
using ReimbursementApp.Domain.Models;

namespace ReimbursementApp.Infrastructure.Interfaces;

public interface IServiceBus
{
    Task SendMessageAsync(dynamic message, string queueName);

    Task<List<ReimbursementRequest>> ReceiveMessagesAsync(string queueName);

    Task RemoveMessageFromQueue(string queueName, int id);
}