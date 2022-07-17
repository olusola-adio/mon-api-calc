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
    [Parameter(Mandatory=$true)]
    [String]$ResourceGroup,
    [Parameter(Mandatory=$true)]
    [String]$FunctionAppName,
    [Parameter(Mandatory=$true)]
    [bool]$Toggle
)

try {

    Write-Verbose "setting AuthV2 on"
    az extension add --name authV2

    Write-Verbose "Toggle enabled to $($Toggle)"
    az webapp auth update -g $ResourceGroup -n $FunctionAppName --enabled $Toggle

    $AADappId = $(az ad app list --display-name $FunctionAppName --query [].appId -o tsv)
    Write-Host "##vso[task.setvariable variable=FunctionAppId]$($AADappId)"
    Write-Output "##vso[task.setvariable variable=FunctionAppId]$($AADappId)"
}
catch {
   throw $_
}