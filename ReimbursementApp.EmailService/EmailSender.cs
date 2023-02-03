using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Communication.Email;
using Azure.Communication.Email.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ReimbursementApp.EmailService;

public static class EmailSender
{
    [FunctionName("EmailSender")]
    public static async Task RunAsync([ServiceBusTrigger("ManagerQueue", Connection = "ServiceBusConnectionString")] string myQueueItem, ILogger log)
    {
        Console.WriteLine(myQueueItem);
        string connectionString = Environment.GetEnvironmentVariable("CommunicationServiceConnectionString");
        EmailClient emailClient = new EmailClient(connectionString);
        
        //Replace with your domain and modify the content, recipient details as required

        EmailContent emailContent = new EmailContent("Welcome to Azure Communication Service Email APIs DotNet Integration World.");
        emailContent.PlainText = "This email message is sent from Azure Communication Service Email using .NET SDK.";
        List<EmailAddress> emailAddresses = new List<EmailAddress> { new EmailAddress("ajayofficial@outlook.in") { DisplayName = "Ajay Thiyo" }};
        EmailRecipients emailRecipients = new EmailRecipients(emailAddresses);
        EmailMessage emailMessage = new EmailMessage("DoNotReply@fd922f48-f26f-4cd1-ab41-d13ff8b94bb6.azurecomm.net", emailContent, emailRecipients);
        SendEmailResult emailResult = emailClient.Send(emailMessage,CancellationToken.None);
        
        
        Console.WriteLine($"MessageId = {emailResult.MessageId}");
        Response<SendStatusResult> messageStatus = null;
        messageStatus = emailClient.GetSendStatus(emailResult.MessageId);
        Console.WriteLine($"MessageStatus = {messageStatus.Value.Status}");
        TimeSpan duration = TimeSpan.FromMinutes(3);
        long start = DateTime.Now.Ticks;
        do
        {
            messageStatus = emailClient.GetSendStatus(emailResult.MessageId);
            if (messageStatus.Value.Status != SendStatus.Queued)
            {
                Console.WriteLine($"MessageStatus = {messageStatus.Value.Status}");
                break;
            }
            Thread.Sleep(10000);
            Console.WriteLine($"...");

        } while (DateTime.Now.Ticks - start < duration.Ticks);
        log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
    }
}