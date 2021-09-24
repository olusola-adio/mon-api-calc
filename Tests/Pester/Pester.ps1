param (
    [Parameter(Mandatory = $true)]
    [string]
    $TestsPath,

    [Parameter(Mandatory = $false)]
    [switch]
    $Publish,

    [Parameter(Mandatory=$false)]
    [string]
    $ResultsPath,

    [Parameter(Mandatory=$true)]
    [string]
    $TestResultsFile,

    [Parameter(Mandatory=$true)]
    [string]
    $CodeCoverageResultsFile
)

$pesterModule = Get-Module -Name Pester -ListAvailable | Where-Object {$_.Version -like '5.*'}
if (!$pesterModule) { 
    try {
        Install-Module -Name Pester -Scope CurrentUser -Force -SkipPublisherCheck -MinimumVersion "5.0"
        $pesterModule = Get-Module -Name Pester -ListAvailable | Where-Object {$_.Version -like '5.*'}
    }
    catch {
        Write-Error "Failed to install the Pester module."
    }
}

Write-Host "Pester version: $($pesterModule.Version.Major).$($pesterModule.Version.Minor).$($pesterModule.Version.Build)"
$pesterModule | Import-Module

if ($Publish) {
    if (!(Test-Path -Path $ResultsPath)) {
        New-Item -Path $ResultsPath -ItemType Directory -Force | Out-Null
    }
}

# Write-Host "Fetching tests:"
$Tests = (Get-ChildItem -Path $($TestsPath) -Recurse | Where-Object {$_.Name -like "*.Tests.ps1"}).FullName

$Powershellfiles = (Get-ChildItem -Recurse | Where-Object {$_.Name -like "*.psm1" -or $_.Name -like "*.ps1" -and $_.FullName -notlike "*\tests\*"}).FullName
Write-Host "Powershell files count $($Powershellfiles.Count)"

$Params = [ordered]@{
    Path = $Tests;
}

$Container = New-PesterContainer @Params

$Configuration = [PesterConfiguration]@{
    Run          = @{
        Container = $Container
    }
    Output       = @{
        Verbosity = 'Diagnostic'
    }
    Filter = @{
        Tag = 'Quality'
    }
    TestResult   = @{
        Enabled      = $true
        OutputFormat = "NUnitXml"
        OutputPath   = "$($ResultsPath)\$($TestResultsFile)"
    }
    CodeCoverage = @{
        Enabled      = $false
        Path         = $Powershellfiles
        OutputFormat = "JaCoCo"
        OutputPath   = "$($ResultsPath)\$($CodeCoverageResultsFile)"
    }
    Should = @{
        ErrorAction = 'Continue'
    }
}

if ($Publish) {
    Invoke-Pester -Configuration $Configuration
}
else {
    Invoke-Pester -Container $Container -Output Detailed
}
