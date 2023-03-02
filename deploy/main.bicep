param baseName string

#disable-next-line no-loc-expr-outside-params
var location = resourceGroup().location

resource storageBlobDataOwnerRoleDefinition 'Microsoft.Authorization/roleDefinitions@2018-01-01-preview' existing = {
  scope: subscription()
  name: 'b7e6dc6d-f1e8-4753-8033-0f276bb0955b'
}

resource webJobsStg 'Microsoft.Storage/storageAccounts@2019-04-01' = {
  name: '${baseName}${toLower(uniqueString(resourceGroup().id))}'
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
}

resource appInsights 'Microsoft.Insights/components@2020-02-02-preview' = {
  name: baseName
  kind: 'web'
  location: location
  properties: {
    Application_Type: 'web'
  }
}

resource backingFarm 'Microsoft.Web/serverfarms@2018-02-01' = {
  name: baseName
  location: location
  properties: {
    maximumElasticWorkerCount: 1
  }
  sku: {
    tier: 'Dynamic'
    name: 'Y1'
  }
}

resource backingSite 'Microsoft.Web/sites@2018-11-01' = {
  name: baseName
  location: location
  kind: 'functionapp'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    siteConfig: {
      appSettings: [
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet-isolated'
        }
        {
          name: 'AzureWebJobsStorage__accountName'
          value: webJobsStg.name
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appInsights.properties.InstrumentationKey
        }
      ]
      netFrameworkVersion: 'v7.0'
      cors: {
        allowedOrigins: ['*']
      }
    }
    serverFarmId: backingFarm.id
    httpsOnly: true
  }
}

resource roleAssignmentWebJobs 'Microsoft.Authorization/roleAssignments@2020-10-01-preview' = {
  name: guid(webJobsStg.id, backingSite.id, storageBlobDataOwnerRoleDefinition.id)
  properties: {
    principalId: backingSite.identity.principalId
    roleDefinitionId: storageBlobDataOwnerRoleDefinition.id
    principalType: 'ServicePrincipal'
  }
  scope: webJobsStg
}
