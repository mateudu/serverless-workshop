# Module 3 - Making BFYOC event driven

BFYOC ice cream is always updating their ice cream offerings and you need to keep your franchise's ice cream availability constantly up to date. Each time BFYOC adds a new flavor, you'll need to update your CosmodDB from module two with the new offering. BFYOC now has a staff writer and a photographer, so each ice cream will be made available to you with the product data, description, and photo as separate files.

BFYOC is also making its marketing efforts event driven. You will also be creating a system that will publish your own custom event every time an order is placed which you will use later on.

This module will walk you though subscribing to Azure events as well as publishing custom events in order to build event based reactive programs using Azure Event Grid.

## Pre-requisites

* You must have your app backed by CosmosDB from module two. If you are stuck there or want to jump ahead, you can always use the answers folder in the previous modules.

## Challenge

There are two parts to this module. The can be done in any order - part one will make you familiar with Azure native events and more advanced functions scenarios; part two will make you familiar with Event Grid Topics and custom events. Module four will build on what we create in part two.

### Part One

So far, you should have implemented a simple API to add products to CosmosDB using just a POST, however now that we have a staff writer and photographer, we don't want to add a product until the data, photo, and description are all available.

Create a storage account of kind `storagev2 (general purpose v2)` or `blob` where your staff writer, photographer, and inventory manager will upload the files to blob storage. Once *all three* files are uploaded for a given ice cream, add a new document to CosmosDB with the aggregated information from all three files:

```json
{
    "id": "081517EG",
    "flavor": "Wonder Blast",
    "price-per-scoop": 0.60,
    "photo-url": "https://example.blob.core.windows.net/examplecontainer/081517EG-photo.png",
    "description": " Topping biscuit cookie chocolate bar lemon drops oat cake gummies jelly. Chocolate cake donut chocolate cupcake. Wafer gingerbread croissant liquorice tootsie roll. Cake lemon drops jujubes jujubes chocolate jelly beans marzipan fruitcake oat cake. Sweet roll tiramisu topping. Cheesecake tootsie roll icing fruitcake sesame snaps bonbon jelly-o biscuit."
}
```

To correlate the files, each will be prefixed with the product ID.

* Product data: JSON containing the product ID, flavor, and price as in the previous modules `081517EG-data.json`
* Product description: Text file containing a few sentences describing the flavor `081517EG-description.txt`
* Product photo: Photo of the product `081517EG-photo.png`

Two sets of sample files have been provided for you in the supporting-files folder of this module.

### Tips

1. Using Event Grid will allow you to subscribe to `Microsoft.Storage.BlobCreated` events and have then pushed anywhere in real time including (*foreshadowing*) a Durable function. [Here is a quickstart](https://docs.microsoft.com/en-us/azure/event-grid/blob-event-quickstart-portal).
1. You can deploy a pre-built web app by clicking the button below to send your events to and see them flowing in real time. Super handy for testing.

    <a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure-Samples%2Fazure-event-grid-viewer%2Fmaster%2Fazuredeploy.json" target="_blank"><img src="http://azuredeploy.net/deploybutton.png"/></a>

    * Connect Event Subscriptions to the website by setting the Subscription's Endpoint to `https://<your-site-name>.azurewebsites.net/api/updates`.
    * View your website by navigating to `https://<your-site-name>.azurewebsites.net`.

    ![View new site](./media/grid-viewer.png)
1. Use [Durable Functions](https://docs.microsoft.com/en-us/azure/azure-functions/durable-functions-overview) to create stateful functions in a serverless environment. This will help you solve the problem of waiting for all three files to be available before creating your new document in CosmosDB.

### Guided instructions

<details><summary>Click to open</summary><p>

TODO
 
</p></details>

### Part Two

Our system is now consuming events via Event Grid, but we also want to be able to publish them to trigger other processes every time an order is placed. Create an Azure Function that [publishes an event to a custom topic](https://docs.microsoft.com/en-us/azure/event-grid/post-to-custom-topic) with data about the order every time it is triggered.

The event posted to the custom topic must be of the form:

```json
[
  {
    "id": string,
    "eventType": "BFYOC.IceCream.Order",
    "subject": string,
    "eventTime": string-in-date-time-format,
    "data":{
        "orderId": string,
        "itemOrdered": string,
        "email": string
    },
    "dataVersion": string
  }
]
```

The data payload is the order data, and the outer envelope is the event metadata.

You may input your order and trigger the Function however you choose, however, we recommend a POST to an HTTP triggered Function as in the first two modules for simplicity:

```
POST http://{myFunctionEndpoint}/api/iceCreamOrder
```

```json
{
  "orderId": "1",
  "itemOrdered": "52325",
  "email": "hello@contoso.com"
}
``` 

### Tips

1. You will need to start by [creating a custom topic](https://docs.microsoft.com/en-us/azure/event-grid/scripts/event-grid-cli-create-custom-topic) to push your custom events to. This can be done via the portal, CLI, or PowerShell.
1. You will need to follow the same process as in modules one and two in order to create an HTTP triggered function.
1. Once you have your function posting to your custom topic, you can test it out by [subscribing to your topic](https://docs.microsoft.com/en-us/azure/event-grid/scripts/event-grid-cli-subscribe-custom-topic) via the Portal, CLI, or Powershell and sending the events to the same web app that you made in part one of this module.

### Guided instructions

<details><summary>Click to open</summary><p>

1. Open the Azure Portal and create an Event Grid Topic.
  ![Create Custom Topic](./media/create-topic.png)
  * Note your Topic endpoint and key, you will need these later.

  We are creating the Topic as a place to send an event every time an order is placed for ice cream. This will allow us subscribe to events regarding ice cream orders and decouple any future downstream processes. Our marketing, operations, and management teams could all subscribe to this topic and listen to events relevant to them without modifying this module.

1. Open your project in VS Code from the previous two modules.

  We need a new function that will handle incoming orders and create an event every time an order is made. Let's go ahead and create that.

1. In the Visual Studio Code extension for Azure Functions, click the lightning bolt icon to add a new function to this app.
1. Select the current folder and add to the existing app. This function will also be HTTP triggered.
1. Name it `iceCreamOrder` and give it `anonymous` access permissions.
1. Replace the code in the new `index.js` for `iceCreamOrder` with the following:

  ```javascript
  //TODO
  module.exports = async function (context, req) {
        context.log('Add product processed a request.');
    
        context.bindings.product = req.body;
    };
  ```

  Make sure you update the `<topic-endpoint>` and `<aeg-sas-key>` with that of your topic from the first step.

  What we are doing here is taking the body of the HTTP request and making it the data payload of an Event Grid event. Then all we have to do is add our SAS key as a header value and make an HTTP POST to the topic endpoint with our event as the message body.

1. Replace the contents of the `function.json` file in the `addProducts` folder with the following:

    ```json
    {
      "disabled": false,
      "bindings": [
        {
          "authLevel": "anonymous",
          "type": "httpTrigger",
          "direction": "in",
          "name": "req",
          "methods": [
            "post"
          ]
        },
        {
          "type": "http",
          "direction": "out",
          "name": "res"
        }
      ]
    }
    ```

  You may be notice once again that this set of bindings is very similar to the others we have created so far. In fact the main thing we care about here that is different from the default binding is we are telling our function it should expect an HTTP POST to trigger it rather than a GET.




  Now lets test everything to see it running and makes sure it works.

1. If you have not already created an Event Grid Viewer web app, deploy one now by clicking the button below.

    <a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure-Samples%2Fazure-event-grid-viewer%2Fmaster%2Fazuredeploy.json" target="_blank"><img src="http://azuredeploy.net/deploybutton.png"/></a>

1. Navigate to your website at `https://<your-site-name>.azurewebsites.net`.

  ![View new site](./media/grid-viewer.png)

1. Now, to see your orders flowing in real time, open the Azure Portal and navigate to your ice cream order Topic. Create an new event subscription on the topic and set the endpoint to `https://<your-site-name>.azurewebsites.net/api/updates`.

  * You may see a "You won't see any events until you start placing orders.

# BEGIN FUNCTION GUIDE


    This is very similar to the input binding from before, with a few changes.  We still will call the binding `product` (as referenced in the code).  However the `direction` in this case is `out` meaning it will *write* to CosmosDB instead of reading.  We also are setting `createIfNotExists` to `true`, which means if this collection and database doesn't already exist in the CosmosDB account, it will create one for us.

    The last part is we need to associate the CosmosDB we created in the first few steps with these functions.  You may have noticed we've referenced `CosmosDbConnectionString` a few times.  Those connection string settings and environment variables are stored in `local.settings.json`

1. Open the `local.settings.json` file at the root of the project.  It should look like this:

    ```json
    {
      "IsEncrypted": false,
      "Values": {
        "AzureWebJobsStorage": "",
        "FUNCTIONS_WORKER_RUNTIME": "node"
      }
    }
    ```

    To get our functions running we need to do two things.  First we need to add a connection string for `AzureWebJobsStorage`.  This is a storage account the function will use for state and to integrate with some triggers and bindings like CosmosDB.  The seccond is we need to add a new settings `CosmosDbConnectionString`.  This is the setting that will give our previous functions access to the cosmosDB account we created.

1. Open the Azure Portal to the resource group with your published function app from step 1.  You should see a Storage Account in that resource group (green square icon).  Open it, select **Access Keys** in the left-hand nav, and copy the **Connection string** for **key1**.  Paste this value in the quotes for `AzureWebJobsStorage` in the `local.settings.json` file.
1. Add a new `Values` for `CosmosDbConnectionString` in the `local.settings.json` file and paste in the connection string from the CosmosDB account created in the earlier steps.
1. Your `local.settings.json` should now look something like this:
    
    ```json
    {
        "IsEncrypted": false,
        "Values": {
          "AzureWebJobsStorage": "DefaultEndpointsProtocol=https;AccountName=amazing-lab;AccountKey=ShhhThisIsASecret;EndpointSuffix=core.windows.net",
          "CosmosDbConnectionString": "AccountEndpoint=https://awesome-function-lab.documents.azure.com:443/;AccountKey=Thisisasecret;",
          "FUNCTIONS_WORKER_RUNTIME": "node"
        }
      }
      
    ```

1. Click the **Debug** menu and **Start Debugging**.
    > The first time you debug a function that has a binding or trigger other than HTTP / timer, the local runtime will install the extension.  The latest version of VS Code should do this automatically for you.  However if not you may need to run the command `func extensions install` at the root of the project.

    You should see two URLs generated like the following:

    > Http Functions:
    > addProducts: http://localhost:7071/api/addProducts
    > products: http://localhost:7071/api/products

1. Open Postman to create a document.  
    1. Create a `POST` request to `http://localhost:7071/api/addProducts`
    1. Select **Body**, choose **raw** and toggle the type to **JSON (application/json)**
    1. Add the following product:

    ```json
    {
         "id": "2",
         "flavor": "Coco Mountain",
         "price-per-scoop": 2.99
    }
    ```  

    ![](media/postman.png)  

1. Send the request, you should get a 200 response back.  If you go now to the CosmosDB Visual Studio extension or the CosmosDB account in the Azure Portal and opening the Data explorer, you should see this added into the CosmosDB account (icecream database, products collection).
1. Make another request but add in a second flavor
    ```json
    {
         "id": "1",
         "flavor": "Rainbow Road",
         "price-per-scoop": 3.99
    }
    ```

    Our CosmosDB backend should have both files.

1. Using Postman (or a web browser), query both of these documents
    1. Make a `GET` request to `http://localhost:7071/api/products?id=2` and `http://localhost:7071/api/products?id=1`
    1. You should see the docs returned from CosmosDB (as well as some additional properties CosmosDB has added)
    
    ```json
    {
        "id": "2",
        "_rid": "Z7l8ALN6PzQBAAAAAAAAAA==",
        "_self": "dbs/Z7l8AA==/colls/Z7l8ALN6PzQ=/docs/Z7l8ALN6PzQBAAAAAAAAAA==/",
        "_ts": 1536957687,
        "_etag": "\"6200bebd-0000-0000-0000-5b9c1cf70000\"",
        "flavor": "Coco Mountain",
        "price-per-scoop": 2.99
    }
    ```

    Now that the app is working and backed by CosmosDB, we need to publish this update.

1. Open the Azure Functions extension in VS Code and click the up-arrow icon to publish
1. Choose the current folder, and select the function app created in step 1
    1. You should see a notification that the app is updating
1. There is one last step.  We need to update the application settings in your published app to have the changes we made to local settings.  In the VS Code Azure Functions extension, find your function app, open it, right-click the "Application Settings" and choose **Upload local settings..**.  This will push your local settings up to your published app.

    ![](media/uploadfromlocal.png)

1. Open your function in the Azure Portal, get the URLs, and verify the functions work in your published apps

# END FUNCTION GUIDE

</p></details>

## Documentation

* [An overview of Azure Event Grid](https://docs.microsoft.com/en-us/azure/event-grid/overview)
* [Blob Storage quickstart](https://docs.microsoft.com/en-us/azure/event-grid/blob-event-quickstart-portal)
* [Custom Events quickstart](https://docs.microsoft.com/en-us/azure/event-grid/custom-event-quickstart-portal)
* [How to receive events from Event Grid](https://docs.microsoft.com/en-us/azure/event-grid/receive-events)
* [Available tutorials, quickstarts, and docs on Event Sources](https://docs.microsoft.com/en-us/azure/event-grid/event-sources)
* [Available tutorials, quickstarts, and docs on Event Handlers](https://docs.microsoft.com/en-us/azure/event-grid/event-handlers)