param location string = resourceGroup().location
var uniqueId = uniqueString(resourceGroup().id)

module keyVault 'modules/secrets/keyvault.bicep' = {
  name: 'keyVaultDeployment'
  params: {
    vaultName: 'kv-${uniqueId}'
    location: location
  }
}
module apiService 'modules/compute/appservice.bicep' = {
  name: 'apiDeployment'
  params: {
    appName: 'api-${uniqueId}'
    appServicePlanName: 'plan-api-${uniqueId}'
    location: location
    keyVaultName: keyVault.outputs.name
    appSettings: [
      {
        name: 'DatabaseName'
        value: 'urls'
      }
      {
        name: 'ContainerName'
        value: 'items'
      }
    ]  
  }

  // dependsOn: [
  //   keyVault
  // ]
}

module cosmosDb 'modules/storage/cosmos-db.bicep' = {
  name: 'cosmosDbDeployment'
  params: {
    name: 'cosmos-db-${uniqueId}'
    kind: 'GlobalDocumentDB'
    location: location
    databaseName: 'urls'
    locationName: 'Spain Central'
    keyVaultName: keyVault.outputs.name
  }

  // dependsOn: [
  //   keyVault
  // ]
}
module keyVaultRomleAssignment 'modules/secrets/key-vault-role-assignment.bicep' = {
  name: 'keyVaultRoleAssignmentDeployment'
  params: {
    keyVaultParam: keyVault.outputs.name
    principalIds: [
      apiService.outputs.principalId
    ]
  }
// dependsOn:[
//   keyVault
//   apiService
// ]

}
