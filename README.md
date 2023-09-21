# bicep-button

## Limitations
- This requires a service to be running (currently an Azure Function app).
- Doesn't work with modules
- Is fixed to a particular Bicep version

## Creating a button
1. Obtain a link to your Bicep file (e.g. `https://raw.githubusercontent.com/Azure/azure-quickstart-templates/master/100-blank-template/main.bicep`)
1. Prepend it with the following: `https://bicepbutton.azurewebsites.net/api/Deploy/uri` (e.g. `https://bicepbutton.azurewebsites.net/api/Deploy/uri/https://raw.githubusercontent.com/Azure/azure-quickstart-templates/master/100-blank-template/main.bicep`)
1. URI encode the string using a utility such as https://meyerweb.com/eric/tools/dencoder/.
1. Use the following Markdown to generate the button, replacing `<encoded_uri_here>` with the encoded value from the previous step:
    ```md
    [![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/<uri_here>)
    ```
    For example, you should end up with something like:
    ```md
    [![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fbicepbutton.azurewebsites.net%2Fapi%2FDeploy%3Furi%3Dhttps%3A%2F%2Fraw.githubusercontent.com%2FAzure%2Fazure-quickstart-templates%2Fmaster%2F100-blank-template%2Fmain.bicep)
    ```

## Test it out
Here are some working samples of this functionality:

### 100-blank-template/main.bicep
[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fbicepbutton.azurewebsites.net%2Fapi%2FDeploy%3Furi%3Dhttps%3A%2F%2Fraw.githubusercontent.com%2FAzure%2Fazure-quickstart-templates%2Fmaster%2F100-blank-template%2Fmain.bicep)

### demos/private-aks-cluster/main.bicep
[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fbicepbutton.azurewebsites.net%2Fapi%2FDeploy%3Furi%3Dhttps%3A%2F%2Fraw.githubusercontent.com%2FAzure%2Fazure-quickstart-templates%2Fmaster%2Fdemos%2Fprivate-aks-cluster%2Fmain.bicep)
