using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using RedditStream.Domains;
using System.Text;
using System.Text.Json;

namespace RedditStream.Functions;

public class LiveThreadMessageFunction
{
    [FunctionName("LiveThreadMessage")]
    public void Run(
        [QueueTrigger("reddit-live-thread-message", Connection = "RedditStreamStorage")] string myQueueItem,
        //[SignalR(HubName = "%SignalR_HubName%", ConnectionStringSetting = "SignalR_ConnectionString")] IAsyncCollector<SignalRMessage> signalRMessageCollector,
        ILogger log)
    {
        log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

        //Send out a SignalR message that the frontend/clients can receive and update UI accordingly.
        var message = JsonSerializer.Deserialize<LiveThreadMessage>(myQueueItem);

        //signalRMessageCollector.AddAsync(new SignalRMessage
        //{
        //    Target = "MessageReceived",
        //    Arguments = new[] 
        //    { 
        //        new 
        //        { 
        //            message.Id, 
        //            message.Type, 
        //            message.Payload.Data.Author, 
        //            message.Payload.Data.Body 
        //        }
        //    }
        //});
    }
}
