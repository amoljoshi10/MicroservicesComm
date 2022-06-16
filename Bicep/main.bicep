param appServicePlanName string
param appServiceName string
param skuName string 
param slotName string 
param location string
param skuCapacity string 
param runtimeStack string 

resource appServicePlan 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: appServicePlanName
  location: location
  properties: {
    reserved: true
  }
  sku: {
    name: 'S1''
    capacity: '0'
  }
  kind: 'linux'
}
resource appService 'Microsoft.Web/sites@2021-03-01' = {
  name: appServiceName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      linuxFxVersion: runtimeStack
    }
  }
}

resource appServiceSlot 'Microsoft.Web/sites/slots@2020-12-01' =  {
  name: '${appService.name}/${slotName}'
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      linuxFxVersion: runtimeStack
    }
  }
}
