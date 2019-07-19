using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.EventGrid;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.EventGrid.Models;
using Newtonsoft.Json;

namespace ServerlessWorkshop
{
    public static class StorageEventProcessor
    {
        [FunctionName("StorageEventProcessor")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "icecream",
                collectionName: "products",
                CreateIfNotExists = true,
                ConnectionStringSetting = "CosmosDbConnectionString")] IAsyncCollector<dynamic> products,
            ILogger log)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            EventGridSubscriber eventGridSubscriber = new EventGridSubscriber();
            EventGridEvent[] eventGridEvents = eventGridSubscriber.DeserializeEventGridEvents(requestBody);

            foreach (EventGridEvent eventGridEvent in eventGridEvents)
            {
                if (eventGridEvent.Data is SubscriptionValidationEventData)
                {
                    var eventData = (SubscriptionValidationEventData)eventGridEvent.Data;
                    
                    var responseData = new SubscriptionValidationResponse()
                    {
                        ValidationResponse = eventData.ValidationCode
                    };

                    return new OkObjectResult(responseData);
                }

                if (eventGridEvent.EventType == "Microsoft.Storage.BlobCreated" &&
                    eventGridEvent.Subject.ToLower().EndsWith(".json"))
                {
                    await products.AddAsync(eventGridEvent.Data);
                }
            }

            return new OkResult();
        }
    }
}