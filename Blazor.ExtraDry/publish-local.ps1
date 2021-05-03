      
function HeadIsBranch($branch) {
    if($Env:BUILD_SOURCEBRANCHNAME -eq $null) {
        $headHash = git rev-parse HEAD
        $branchHash = git rev-parse $branch
        $headHash -eq $branchHash
    }
    else {
        $Env:BUILD_SOURCEBRANCHNAME -eq $branch
    }
}

function Coallesce($value1, $value2) {
    if($value1 -eq $null) { $value2 } else { $value1 }
}

$describe = git describe --tags
Write-Host "Git Version is $describe"
$bits = $describe.TrimStart("v").Split(".-")
$majorVersion = Coallesce $bits[0] 1
$minorVersion = Coallesce $bits[1] 0
$patchVersion = Coallesce $bits[2] 0
$prereleaseVersion = "head" # no semantic meaning
if(HeadIsBranch("develop")) {
    $prereleaseVersion = "alpha"
}
if(HeadIsBranch("release")) {
    $prereleaseVersion = "beta"
}
if(HeadIsBranch("main") -OR HeadIsBranch("master")) {
    $prereleaseVersion = ""
}
$semanticVersion = "$majorVersion.$minorVersion.$patchVersion-$prereleaseVersion".TrimEnd("-")

Write-Host "Semenatic version is $semanticVersion"

dotnet pack .\Blazor.ExtraDry.Core\Blazor.ExtraDry.Core.csproj -p:PackageVersion=$semanticVersion
dotnet pack .\Blazor.ExtraDry\Blazor.ExtraDry.csproj -p:PackageVersion=$semanticVersion

nuget add .\Blazor.ExtraDry.Core\bin\Debug\Blazor.ExtraDry.Core.$semanticVersion.nupkg -source $env:USERPROFILE\Repos\Nuget\
nuget add .\Blazor.ExtraDry\bin\Debug\Blazor.ExtraDry.$semanticVersion.nupkg -source $env:USERPROFILE\Repos\Nuget\
