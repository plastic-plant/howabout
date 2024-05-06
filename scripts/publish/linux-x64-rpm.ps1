 # Get the name of the current script without extension to use as a build config name.
$currentScript = [System.IO.Path]::GetFileNameWithoutExtension($MyInvocation.MyCommand.Path)

# Build a release by its preset build configuration.
../../src/make/make.ps1 --buildconfig $currentScript
exit $LASTEXITCODE;