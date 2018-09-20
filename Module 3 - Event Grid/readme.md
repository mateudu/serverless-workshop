# Module 3 - Making BFYOC event driven

BFYOC ice cream is always updating their ice cream offerings and you need to keep your franchise's ice cream availability constantly up to date. Each time BFYOC adds a new flavor, you'll need to update your CosmodDB from module two with the new offering. BFYOC now has a staff writer and a photographer, so each ice cream will be made available to you with the product data, description, and photo as separate files.

BFYOC is also making its marketing efforts event driven. You will also be creating a system that will publish your own custom event every time an order is placed which you will use later on.

This module will walk you though subscribing to Azure events as well as publishing custom events in order to build event based reactive programs using Azure Event Grid.

## Pre-requisites

* You must have your app backed by CosmosDB from module two. If you are stuck there or want to jump ahead, you can always use the answers folder in the previous modules.

## Challenge

There are two parts to this module.

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
    "eventType": string,
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

 TODO

</p></details>

## Documentation

* [An overview of Azure Event Grid](https://docs.microsoft.com/en-us/azure/event-grid/overview)
* [Blob Storage quickstart](https://docs.microsoft.com/en-us/azure/event-grid/blob-event-quickstart-portal)
* [Custom Events quickstart](https://docs.microsoft.com/en-us/azure/event-grid/custom-event-quickstart-portal)
* [How to receive events from Event Grid](https://docs.microsoft.com/en-us/azure/event-grid/receive-events)
* [Available tutorials, quickstarts, and docs on Event Sources](https://docs.microsoft.com/en-us/azure/event-grid/event-sources)
* [Available tutorials, quickstarts, and docs on Event Handlers](https://docs.microsoft.com/en-us/azure/event-grid/event-handlers)