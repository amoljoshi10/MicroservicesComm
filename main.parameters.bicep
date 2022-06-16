{
  "$schema": "https://schema.management.azure.com/schemas/2019-04-01/deploymentParameters.json#",
  "contentVersion": "1.0.0.0",
  "parameters": {
    "appServicePlanName": {
      "value": "AppServicePlanForAPIS"
    },
    "appServiceName": {
      "value": "OnlineShoppingApp22"
    },
    "skuName": {
      "value": "S1"
    },
    "slotName": {
      "value": "Staging"
    },
    "location": {
      "value": "westus"
    },
    "skuCapacity": {
      "value": "0"
    },
    "runtimeStack": {
      "value": "DOTNETCORE|3.1"
    }
  }
}