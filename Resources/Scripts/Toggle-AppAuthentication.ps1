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
    az webapp auth update -g $ResourceGroup -n $FunctionAppName --enabled $Toggle
}
catch {
   throw $_
}