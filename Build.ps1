# Taken from psake https://github.com/psake/psake

<#
.SYNOPSIS
  This is a helper function that runs a scriptblock and checks the PS variable $lastexitcode
  to see if an error occcured. If an error is detected then an exception is thrown.
  This function allows you to run command-line programs without having to
  explicitly check the $lastexitcode variable.
.EXAMPLE
  exec { svn info $repository_trunk } "Error executing SVN. Please verify SVN command-line client is installed"
#>
function Exec {
  [CmdletBinding()]
  param(
    [Parameter(Position = 0, Mandatory = 1)][scriptblock]$cmd,
    [Parameter(Position = 1, Mandatory = 0)][string]$errorMessage = ($msgs.error_bad_command -f $cmd)
  )
  & $cmd
  if ($lastexitcode -ne 0) {
    throw ("Exec: " + $errorMessage)
  }
}

function CleanArtifacts($artifactsFolder) {
  if (Test-Path $artifactsFolder) { 
    Remove-Item $artifactsFolder -Force -Recurse 
  }  
}

function GetVersionSuffix {
  if ($env:APPVEYOR) {
    return "{0:0000}" -f [convert]::ToInt32("0" + $env:APPVEYOR_BUILD_NUMBER, 10)
  }

  $commitHash = $(git rev-parse --short HEAD)
  return "local-$commitHash"
}

function PrintBuildInformation($versionSuffix) {
  $buildKind = ""
  if ($env:APPVEYOR) {
    if ($env:APPVEYOR_PULL_REQUEST_NUMBER -ne "") {
      $buildKind = "It is AppVeyor CI build for $env:APPVEYOR_PULL_REQUEST_NUMBER PR into $env:APPVEYOR_REPO_BRANCH branch."
    }
    elseif ($env:APPVEYOR_REPO_TAG -eq $true) {
      $buildKind = "It is AppVeyor CI build for tag $env:APPVEYOR_REPO_TAG_NAME in $env:APPVEYOR_REPO_BRANCH branch."
    }
    else {
      $buildKind = "It is AppVeyor CI build for $env:APPVEYOR_REPO_BRANCH branch."
    }
  }
  else {
    $buildKind = "It is a local build."
  }

  Write-Host "BUILD: $buildKind"
  Write-Output "BUILD: Package version suffix is $versionSuffix"
}

function Build($solutionFile, $versionSuffix) {
  exec { & dotnet build $solutionFile --configuration Release --version-suffix $versionSuffix }
}

function Test($solutionFile) {
  exec { & dotnet test $solutionFile --configuration Release --no-build --no-restore }
}

function MakePackage($packageProject, $artifactsFolder, $versionSuffix) {
  exec { & dotnet pack $packageProject --configuration Release --output $artifactsFolder --include-symbols --no-build --version-suffix $versionSuffix }
}


$solutionFile = ".\sources\Eshva.DockerCompose.sln"
$packageProject = ".\sources\Eshva.DockerCompose\Eshva.DockerCompose.csproj"
$artifactsFolder = ".\artifacts"
$versionSuffix = GetVersionSuffix

PrintBuildInformation $versionSuffix
CleanArtifacts $artifactsFolder
Build $solutionFile $versionSuffix
Test $solutionFile
MakePackage $packageProject $artifactsFolder $versionSuffix
