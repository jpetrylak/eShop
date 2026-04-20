$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Path $PSScriptRoot -Parent
$buildArtifactDirectories = Get-ChildItem -Path $repoRoot -Directory -Recurse -Force |
    Where-Object {
        $_.Name -in @("bin", "obj") -and
        -not ($_.Attributes -band [System.IO.FileAttributes]::ReparsePoint)
    }

foreach ($directory in $buildArtifactDirectories) {
    Write-Host "Removing $($directory.FullName)"
    Remove-Item -Path $directory.FullName -Recurse -Force
}
