#!/bin/bash
set -e

# This script creates the necessary registry infrastructure and configures GitHub OpenID Connect to allow
# GitHub actions to push to the registry in its CD pipeline.
usage="Usage: ./initial_setup.sh <tenantId> <subscriptionId>"
tenantId=${1:?"Missing tenantId. ${usage}"}
subId=${2:?"Missing subscriptionId. ${usage}"}

repoOwner="anthony-c-martin"
repoName="bicep-button"
rgName="bicepbutton"
rgLocation="East US"

az account set -n "$subId"
az group create \
  --location "$rgLocation" \
  --name "$rgName"

appCreate=$(az ad app create --display-name $rgName)
appId=$(echo $appCreate | jq -r '.appId')
appOid=$(echo $appCreate | jq -r '.id')

spCreate=$(az ad sp create --id $appId)
spId=$(echo $spCreate | jq -r '.id')
az role assignment create --role owner --subscription $subId --assignee-object-id $spId --assignee-principal-type ServicePrincipal --scope /subscriptions/$subId/resourceGroups/$rgName

repoSubject="repo:$repoOwner/$repoName:ref:refs/heads/main"
az rest --method POST --uri "https://graph.microsoft.com/beta/applications/$appOid/federatedIdentityCredentials" --body '{"name":"'$repoName'","issuer":"https://token.actions.githubusercontent.com","subject":"'$repoSubject'","description":"GitHub OIDC Connection","audiences":["api://AzureADTokenExchange"]}'

echo "Now configure the following GitHub Actions secrets:"
echo "  CLIENT_ID: $appId"
echo "  SUBSCRIPTION_ID: $subId"
echo "  TENANT_ID: $tenantId"