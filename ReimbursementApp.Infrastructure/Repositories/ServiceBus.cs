using System.Text;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using ReimbursementApp.Domain.Models;
using ReimbursementApp.Infrastructure.Interfaces;

namespace ReimbursementApp.Infrastructure.Repositories;

public class ServiceBus : IServiceBus
{
    private readonly IConfiguration _configuration;

    public ServiceBus(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    
    public async Task SendMessageAsync(dynamic payload, string queueName)
    {
        string message = JsonSerializer.Serialize(payload);
        // using Azure.Messaging.ServiceBus;
        // Because ServiceBusClient implements IAsyncDisposable, we'll create it 
        // with "await using" so that it is automatically disposed for us.
        await using var client = new ServiceBusClient(_configuration["AzureServiceBusConnectionString"]);

        // The sender is responsible for publishing messages to the queue.
        ServiceBusSender sender = client.CreateSender(queueName);
        ServiceBusMessage queueMessage = new ServiceBusMessage(message);

        await sender.SendMessageAsync(queueMessage);
    }
    
    public async Task<List<ReimbursementRequest>> ReceiveMessagesAsync(string queueName)
    {
        await using var client = new ServiceBusClient(_configuration["AzureServiceBusConnectionString"]);

        // The receiver is responsible for reading messages from the queue.
      
        ServiceBusReceiver receiver = client.CreateReceiver(queueName);
        var messages  = await receiver.PeekMessagesAsync(20, null, default);
        var result = messages.Select(message => message.Body.ToObjectFromJson<ReimbursementRequest>()).ToList();
        
        // ServiceBusReceivedMessage receivedMessage =await  receiver.ReceiveMessageAsync();
        return result;
    }

    public async Task RemoveMessageFromQueue(string queueName, int id)
    {
        await using var client = new ServiceBusClient(_configuration["AzureServiceBusConnectionString"]);
        var receiver = client.CreateReceiver(queueName);
        
        var messages  = await receiver.ReceiveMessagesAsync(20, null, default);
        foreach (var message in messages)
        {
            if (message.Body.ToObjectFromJson<ReimbursementRequest>().Id == id)
            {
                await receiver.CompleteMessageAsync(message);
                break;
            }
        }
    }
    
}