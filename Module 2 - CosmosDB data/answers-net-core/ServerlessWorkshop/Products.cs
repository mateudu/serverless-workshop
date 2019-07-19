using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ServerlessWorkshop
{
    public static class Products
    {
        [FunctionName("Products")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "icecream",
                collectionName: "products",
                ConnectionStringSetting = "CosmosDBConnectionString",
                Id = "{Query.id}")] dynamic product,
            ILogger log)
        {
            if (product != null)
            {
                return new OkObjectResult(product);
            }
            else
            {
                return new BadRequestObjectResult("Please pass in an id query parameter");
            }
        }
    }
}
