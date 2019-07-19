using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ServerlessWorkshop
{
    public static class IceCreamOrder
    {
        private static readonly string EventGridKey = Environment.GetEnvironmentVariable("EventGridKey");
        private static readonly string EventGridEndpoint = Environment.GetEnvironmentVariable("EventGridEndpoint");

        [FunctionName("IceCreamOrder")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (!string.IsNullOrWhiteSpace(requestBody))
            {
                dynamic data = JsonConvert.DeserializeObject(requestBody);

                var events = new List<EventGridEvent>
                {
                    new EventGridEvent
                    {
                        Id = Guid.NewGuid().ToString(),
                        Subject = "BFYOC/stores/serverlessWorkshop/orders",
                        DataVersion = "2.0",
                        EventType = "BFYOC.IceCream.Order",
                        Data = data,
                        EventTime = DateTime.UtcNow
                    }
                };

                var eventGridClient = new EventGridClient(new TopicCredentials(EventGridKey));
                await eventGridClient.PublishEventsAsync(EventGridEndpoint, events);

                return new OkObjectResult(data);
            }
            else
            {
                return new BadRequestObjectResult("Please pass an ice cream order in the request body");
            }
        }
    }
}
