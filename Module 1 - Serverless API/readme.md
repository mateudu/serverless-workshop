# Module 1 - Serverless API

As part of BFYOC ice cream services they need to keep each franchise and branch connected with the catalogue of flavors that they offer.  As such you need a callable API each store and supporting systems can call for details on flavors offered.  

This module will walk you through building and testing your first Azure Function locally, and publishing to the cloud.  The Azure Function will be the serverless API to expose the data on the different flavors that you offer.

## Pre-requisites

* A modern laptop running Windows 10, Mac OSX Mac OS X 10.12 or higher
* Your preferred IDE (integrated development environments) - Visual Studio Code or Visual Studio
    > NOTE: While you can complete this entire workshop in any language and editor you prefer, to make applicable to as many operating systems as possible most of the samples and examples will assume VS Code + JavaScript.  
      * If using Visual Studio Code in Windows, OSX, or Linux make sure you have the latest Visual Studio Code version for your OS. You can follow <a href="https://code.visualstudio.com/tutorials/functions-extension/getting-started" target="_blank">the **first page** of pre-requisites as described here</a> to get the Azure Functions extension configured  
      * If using Visual Studio for in Windows, make sure you have the <a href="https://www.visualstudio.com/vs/" target="_blank">latest Visual Studio 2017</a> with the `Azure development workload` selected.  
* [.NET Core 2.1 SDK](https://www.microsoft.com/net/download)
* [NodeJS 8 (LTS) or 10 (Current)](https://nodejs.org/en/download/)
* [Azure Functions Core Tools v2](https://github.com/Azure/azure-functions-core-tools#installing)  

## Challenge

Run and test an Azure Function locally where you can do a `GET` on a specific endpoint and pass in a product ID.  The product ID will return information on the product flavor.  For example if you did the following HTTP Request:

```
GET http://{myFunctionEndpoint}/api/products?id=1
```

It would return:

```json
{
  "id": "1",
  "flavor": "Rainbow Road",
  "price-per-scoop": 3.99
}
```

For this challenge, just have the function return static data.  No need to connect to a database behind the scenes.

You or your team must be able to show this function running locally and published to Azure.

**You should be writing this function in the latest v2 runtime. If non-Windows this is only option, if Windows be sure to select in Visual Studio or VS Code (may be listed as `beta`).**

### Tips

1. First things first, make sure you get the tools installed listed above.  
1. Create a new Azure Function project and select the HTTP template.
    * The HTTP function template will return "Hello, {name}", where name is a property passed in to a query parameter or as the body.
    * Make changes to the HTTP template so instead of returning "Hello, {name}" it returns the above `json` payload when a query parameter `id` is set to `1`.
1. Check out the [documentation](#documentation) for more guidance

### Guided instructions

<details><summary>Click to open</summary><p>

1. Open Visual Studio
1. Click on the extensions category on the left-hand nav and verify or install the **Azure Logic Apps Tools for Visual Studio** extension is installed (this may require restarting Visual Studio)
1. Create a new solution
1. Create new Azure Function project (without any function, set `Storage Account` option to `None`)
1. Create new Azure Function. Right click on created project and select **Add** -> **New Azure Function...**. Select **HTTP Trigger** for the trigger.  Give it any name you like (I'll name it `Products`)
1. Select **Anonymous** for the authentication type.  **Function** would also work but requires a key is passed in a header or query parameter to execute the function once published.
1. You should now see a default Azure Functions template like the following:

    ```cs
    public static class Products
    {
        [FunctionName("Products")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
    ```

    >IMPORTANT: If you don't see this template you may be targeting the ~1 runtime (wouldn't have the `async` modifier on the method) or using an out of date version of function core tools / extension

1. Make the following changes so that your function returns the suggested string:

    ```cs
    public static class Products
    {
        [FunctionName("Products")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var id = req.Query["id"];

            if (id == "1")
            {
                return new OkObjectResult(new
                {
                    id = 1,
                    flavor = "Rainbow Road",
                    pricePerScoop = 3.99
                });
            }
            else
            {
                return new BadRequestObjectResult("Please pass in an id query parameter");
            }
        }
    }
    ```

    You may also notice in the file browser there is a `function.json` file. Go ahead and open this and look. It describes the trigger you are using, and any bindings.  It should be set for HTTPTrigger.

1. Click **Debug** at the top and **Start Debugging**

    You should notice the Azure Functions runtime spins up in the terminal window.  If all the code is valid you should be prompted with a URL to call to execute the function.  Something like `http://localhost:7071/api/products`

1. While the runtime is still running, click on the link or copy it to a browser to execute the function.  Make sure you append a query parameter for ID as specified.  So the call should be like `http://localhost:7071/api/products?id=1`.  You should see a response like the following returned:

    ```json
    {
        "id": "1",
        "flavor": "Rainbow Road",
        "price-per-scoop": 3.99
    }
    ```

1. The final step is publishing this app to Azure.  Kill the terminal (click the trash icon) or close the console to stop the runtime.
1. Create new Function App in Azure. Make sure to choose the correct configuration values.
1. Right click on the project in Visual Studio and publish to Function App in Azure.
    
Congratulations! You've now published an Azure Function as an API in the cloud.

</p></details>

## Documentation

* <a href="https://docs.microsoft.com/en-us/azure/azure-functions/functions-overview" target="_blank">An introduction to Azure Functions</a>
* <a href="https://docs.microsoft.com/en-us/azure/azure-functions/functions-test-a-function" target="_blank">Strategies for testing your code in Azure Functions</a>
* <a href="https://code.visualstudio.com/tutorials/functions-extension/getting-started" target="_blank">Creating Azure Functions from Visual Studio Code</a> - this tutorial is for JavaScript but it is very similar for C#
* <a href="https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-local" target="_blank">Code and test Azure Functions locally</a>
* <a href="https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local" target="_blank">Code and test Azure Functions locally</a>