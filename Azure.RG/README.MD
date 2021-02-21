# ARM Templates and Services
## Resource types

### Azure Monitor
* [Azure Monitor Overview](https://docs.microsoft.com/en-us/azure/azure-monitor/overview)
* [Log Analytics](https://docs.microsoft.com/en-us/azure/azure-monitor/samples/resource-manager-workspace)
* Resource type Microsoft.OperationalInsights/workspaces - Create Azure Log Analytics
!(Azure Monitor)[https://docs.microsoft.com/en-us/azure/azure-monitor/media/overview/overview.png]


### Azure Application Insights
* [Application Insights overview](https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview)
* Resource type Microsoft.Insights/components - Create Azure App insights Analytics
!(Application Insights overview)[https://docs.microsoft.com/en-us/azure/azure-monitor/app/media/app-insights-overview/diagram.png]


### Azure Alerts and Actions group
* [Action Groups](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/action-groups)


### Samples
* [Deploy log anaytics and App Insights ARM Template](/templates/monitor/azuredeploy.json)



## Deploy Azure Application Infrastructure
```

# Connect to subscription
Connect-AzureRmAccount -Subscription 'Free Trial'

# Deploy monitoring service
./Deploy-AzureResourceGroup.ps1 -ResourceGroupLocation 'West Europe' -ResourceGroupName 'CryptoBot1' -TemplateFile 'templates/monitor/azuredeploy.json' -TemplateParametersFile 'templates/monitor/azuredeploy.parameters.json'

# Deploy AppService plan and App Services
./Deploy-AzureResourceGroup.ps1 -ResourceGroupLocation 'West Europe' -ResourceGroupName 'CryptoBot1' -TemplateFile 'templates/platform/azuredeploy.json' -TemplateParametersFile 'templates/platform/azuredeploy.parameters.json'


# Deploy AppService plan and App Services
./Deploy-AzureResourceGroup.ps1 -ResourceGroupLocation 'West Europe' -ResourceGroupName 'CryptoBot1' -TemplateFile 'templates/service-farm/azuredeploy.json' -TemplateParametersFile 'templates/service-farm/azuredeploy.parameters.json'

# Deploy AppService plan and App Services
./Deploy-AzureResourceGroup.ps1 -ResourceGroupLocation 'West Europe' -ResourceGroupName 'CryptoBot1' -TemplateFile 'templates/server-cosmosdb/azuredeploy.json' -TemplateParametersFile 'templates/server-cosmosdb/azuredeploy.parameters.json'


```