# Dometrain urm shortener build as you go

## Notes 2024 12 31

La creation des resource group automatiquement dans Azure, via az inline script, ne marche pas. Je suis obligé de créer les resource group à la main d'abord.
Cela marche quand je fais la même commande à partir de pws dans vscode
Est-ce qu'il faut passer par un autre version ???
Est-ce qu'il faudrait créer les resource en utilisant un module bicep, dans le main.bicep? cela ferait un peu plus de sens d'ailleurs

## Where is the Azure portal

> Azure portal  
> [https://portal.azure.com/#home](https://portal.azure.com/#home)

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
az login --tenant 2547a5f3-13ef-4878-88f0-fcd66e0ab8f7
```

### Create resource group

```pws
az group create --name urlshortener-dev --location westeurope
```

### Create the resource in the resource group

#### What if

```bash
az deployment group what-if --resource-group urlshortener-dev --template-file ./infrastructure/main.bicep
```

#### Creation

```bash
 az deployment group create --resource-group urlshortener-dev --template-file main.bicep 
```

### Creation of user for Github actions

```bash
az ad sp create-for-rbac --name "GitHub-Actions-SP" --role contributor --scopes /subscriptions/2920ee69-c334-43a4-a0bd-e0e966a54d8f
```

### Apply to Custom contributor role

```bash
az ad sp create-for-rbac --name "GitHub-Actions-SP" --role "infra_deploy" --scopes /subscriptions/2920ee69-c334-43a4-a0bd-e0e966a54d8f --sdk-auth
```

#### Configure a federated identity credential on an app

[Federated identity github / Azure deployment](https://github.com/AzureAD/microsoft-identity-web/wiki/Federated-Identity-Credential-(FIC)-with-a-Managed-Service-Identity-(MSI))

[Microsoft Azure link](https://learn.microsoft.com/en-us/entra/workload-id/workload-identity-federation-create-trust-user-assigned-managed-identity?pivots=identity-wif-mi-methods-azp)

#### Retrieve the AZURE_API_PUBLISH_PROFILE

az webapp deployment list-publishing-profiles --name api-rixxremzmcmyw --resource-group urlshortener-dev --xml

=> full result to be copies in the github secret
