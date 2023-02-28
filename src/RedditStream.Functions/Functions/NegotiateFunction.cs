using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace RedditStream.Functions.Functions;

public static class NegotiateFunction
{
    //Needed for clients to call in order to establish connection to signalr hub
    [FunctionName("Negotiate")]
    public static SignalRConnectionInfo Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest request,
        [SignalRConnectionInfo(HubName = "%SignalR_HubName%", ConnectionStringSetting = "SignalR_ConnectionString")] SignalRConnectionInfo connectionInfo)
    {
        return connectionInfo;
    }
}
