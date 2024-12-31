param location string = resourceGroup().location
param appServicePlanName string
param appName string
param keyVaultName string

resource appServicePlan 'Microsoft.Web/serverfarms@2023-12-01' = {
  kind: 'linux'
  location: location
  name: appServicePlanName
  properties: {
    reserved: true
  }
  sku: {
    name: 'B1'
  }
}

resource webApp 'Microsoft.Web/sites@2023-12-01' = {
  name: appName
  location: location
  identity:{
    type:'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|8.0'
      appSettings: [
        {
          name: 'KeyVaultName'
          value: keyVaultName
        }
      ]
    }
  }
}

resource webAppConfig 'Microsoft.Web/sites/config@2023-12-01' = {
  name: 'web'
  parent: webApp
  properties: {
    scmType: 'GitHub'
  }
}

// resource authSettings 'Microsoft.Web/sites/config@2023-12-01' existing = {
//   name: 'authsettingsV2'
//   parent: webApp
// }

output appServiceId string = webApp.id
// output principalId string = authSettings.properties.identityProviders.azureActiveDirectory.registration.clientId
// output principalId string = authSettings.id
output principalId string = webApp.identity.principalId
