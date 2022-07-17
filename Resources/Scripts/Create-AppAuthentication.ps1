<#
.SYNOPSIS
Update an APIM API with an openapi definition

.DESCRIPTION
Update an APIM API with a openapi definition

.PARAMETER ResourceGroup
The name of the resource group that contains the APIM instnace

.PARAMETER FunctionAppName
The name of the APIM instance

.PARAMETER Toggle
The name of the API to update

.EXAMPLE
Import-ApimOpenApiDefinitionFromFile -ApimResourceGroup dfc-foo-bar-rg -InstanceName dfc-foo-bar-apim -ApiName bar -OpenApiSpecificationFile some-file.yaml -Verbose

#>
[CmdletBinding()]
Param(
    [Parameter(Mandatory = $true)]
    [String]$FunctionAppName
)

try {

    Write-Verbose "Getting FQDN"
    $WebAppFDQN = "https://$($FunctionAppName).azurewebsites.net"

    $AADsuffix = "/.auth/login/aad/callback" # AD Online is hardcoded to redirect to this path!!
    $urls = "https://$($WebAppFDQN)$($AADsuffix)";

    $AADappName = $FunctionAppName

    Write-Verbose "Checking if appId exists"
    $appCheck = az ad app list --display-name $AADappName | ConvertFrom-Json
    $appExists = $appCheck.Length -gt 0
    if (!$appExists) {

        Write-Verbose "appId doesnt exist, so creatinbg a new one"
        az ad app create --display-name $AADappName --homepage="https://$($WebAppFDQN)" --reply-urls $urls --oauth2-allow-implicit-flow true
    }

    Write-Verbose "Get appp id"

    $AADappId = $(az ad app list --display-name $AADappName --query [].appId -o tsv)
    Write-Host "##vso[task.setvariable variable=FunctionAppId]$($AADappId)"
    Write-Output "##vso[task.setvariable variable=FunctionAppId]$($AADappId)"

}
catch {
    throw $_
}