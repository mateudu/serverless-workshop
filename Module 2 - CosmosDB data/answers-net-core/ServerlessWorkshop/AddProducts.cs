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
    public static class AddProducts
    {
        [FunctionName("AddProducts")]
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
            dynamic product = JsonConvert.DeserializeObject(requestBody);
            await products.AddAsync(product);

            return new OkResult();
        }
    }
}
