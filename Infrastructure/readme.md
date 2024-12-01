# Dometrain urm shortener build as you go

## Infrastructure as Code

### Download Azure CLI

> need to have install Azure CLI on the machine 1st ;-)
[Download Azure CLI](https://learn.microsoft.com/en-us/cli/azure/)

We need to have a powershell with admin right

```pws
Install-Module -Name Az -Force
```

### log into Azure

```pws
az login
```

In my case I have to:

```pws
az login --tenant 2547a5f3-13ef-4878-88f0-fcd66e0ab8f71
```

### Create resource group

```pws
az group create --name urlshortener-dev --location westeurope
```

> did it via the UI

### Create the resource in the resource group

#### What if

```powershell
 az deployment group what-if --resource-group urlshortener-dev --template-file main.bicep 
```

#### Creation

> ne marche pas 🤷‍♂️

```psh
 az deployment group create --resource-group urlshortener-dev --template-file main.bicep 
```

### Creation of user for Github actions

```bash
az ad sp create-for-rbac --name "GitHub-Actions-SP" --role contributor --scopes /subscriptions/2920ee69-c334-43a4-a0bd-e0e966a54d8f
```

#### Configure a federated identity credential on an app


[Federated identity github / Azure deployment](https://github.com/AzureAD/microsoft-identity-web/wiki/Federated-Identity-Credential-(FIC)-with-a-Managed-Service-Identity-(MSI))

[Microsoft Azure link](https://learn.microsoft.com/en-us/entra/workload-id/workload-identity-federation-create-trust-user-assigned-managed-identity?pivots=identity-wif-mi-methods-azp)

#### Retrieve the AZURE_API_PUBLISH_PROFILE

az webapp deployment list-publishing-profiles --name api-rixxremzmcmyw --resource-group urlshortener-dev --xml

=> full result to be copies in the github secret
