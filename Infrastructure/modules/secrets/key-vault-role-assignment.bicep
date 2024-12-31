param keyVaultParam string
param principalIds array
param principalType string = 'ServicePrincipal'
// to retrieve the roleDefinitionID go there
// https://learn.microsoft.com/en-us/azure/role-based-access-control/built-in-roles
//param roleDefinitionId string = '4633458b-17de-408a-b874-044Sc86b69e6'
param roleDefinitionReaderId string = 'acdd72a7-3385-48ef-bd42-f606fba81ae7'

resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' existing = {
  name: keyVaultParam
}

resource keyVaultResourceAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = [ 
  for principalId in principalIds: {
    name: guid(keyVault.id, principalId, roleDefinitionReaderId)
    scope: keyVault
    properties: {
      roleDefinitionId: subscriptionResourceId( 'Microsoft.Authorization/roleDefintions', roleDefinitionReaderId)
      principalId: principalId
      principalType: principalType
    }
  }
]
