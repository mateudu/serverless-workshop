# Module 1 - Serverless API

As part of BFYOC ice cream services they need to keep each franchise and branch connected with the catalogue of flavors that they offer.  As such you need a callable API each store and supporting systems can call for details on flavors offered.  

This module will walk you through building and testing your first Azure Function locally, and publishing to the cloud.  The Azure Function will be the serverless API to expose the data on the different flavors that you offer.

## Pre-requisites

* A modern laptop running Windows 10, Mac OSX Mac OS X 10.12 or higher
* Your preferred IDE (integrated development environments) - Visual Studio Code or Visual Studio
    > NOTE: While you can complete this entire workshop in any language and editor you prefer, to make applicable to as many operating systems as possible most of the samples and examples will assume VS Code + JavaScript.
      * If using Visual Studio Code in Windows, OSX, or Linux make sure you have the latest Visual Studio Code version for your OS. You can follow <a href="https://code.visualstudio.com/tutorials/functions-extension/getting-started" target="_blank">the **first page** of pre-requisites as described here</a> to get the Azure Functions extension configured
      * If using Visual Studio for in Windows, make sure you have the <a href="https://www.visualstudio.com/vs/" target="_blank">latest Visual Studio 2017</a> with the `Azure development workload` selected.
* [Azure Functions Core Tools]()
* [.NET Core 2.1 SDK](https://www.microsoft.com/net/download)
* (Optional if writing in JavaScript) [NodeJS 8 (LTS) or 10 (Current)](https://nodejs.org/en/download/)

## Challenge

Run and test an Azure Function locally where you can do a `GET` on a specific endpoint and pass in a product ID.  The product ID will return information on the product flavor.  For example if you did the following HTTP Request:

```
GET http://{myFunctionEndpoint}/api/product?id=1
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

### Documentation

* <a href="https://docs.microsoft.com/en-us/azure/azure-functions/functions-overview" target="_blank">An introduction to Azure Functions</a>
* <a href="https://docs.microsoft.com/en-us/azure/azure-functions/functions-test-a-function" target="_blank">Strategies for testing your code in Azure Functions</a>
* <a href="https://code.visualstudio.com/tutorials/functions-extension/getting-started" target="_blank">Creating Azure Functions from Visual Studio Code</a> - this tutorial is for JavaScript but it is very similar for C#
* <a href="https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-local" target="_blank">Code and test Azure Functions locally</a>
* <a href="https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local" target="_blank">Code and test Azure Functions locally</a>