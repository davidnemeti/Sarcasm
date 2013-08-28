param([string]$project)

function CreateZipFile( [string] $zipfilename, [string] $sourcedir )
{
    [Reflection.Assembly]::LoadWithPartialName( "System.IO.Compression.FileSystem" )
    $compressionLevel = [System.IO.Compression.CompressionLevel]::Optimal
    [System.IO.Compression.ZipFile]::CreateFromDirectory( $sourcedir, $zipfilename, $compressionLevel, $false )
}

if ($project -eq "")
{
    $projects = [Management.Automation.Host.ChoiceDescription[]] (
        (new-Object Management.Automation.Host.ChoiceDescription "&Irony", "Irony"),
        (new-Object Management.Automation.Host.ChoiceDescription "&Sarcasm", "Sarcasm")
        );

    $projectIndex = $host.ui.PromptForChoice("Deploy", "Select project to deploy:", $projects, -1)

    $project = $projects[$projectIndex].HelpMessage
}

$msbuild = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"

if ($project -eq "Irony")
{
    & $msbuild ..\Irony\Irony\010.Irony.2010.csproj /p:Configuration=Release
    & $msbuild ..\Irony\Irony\Irony.PCL.csproj /p:Configuration=Release

    Copy-Item -Path ..\Irony\Irony\bin\Release\Irony.dll -Destination Libraries -Force
    Copy-Item -Path ..\Irony\Irony\bin\Release\Irony.PCL.dll -Destination Libraries -Force
}
elseif ($project -eq "Sarcasm")
{
    & $msbuild Sarcasm.sln /p:Configuration=Release

    New-Item -Path Downloads\Temp -ItemType directory -Force

#    Copy-Item -Path Sarcasm\bin\Release\Irony.dll -Destination Downloads\Temp -Force
    Copy-Item -Path Sarcasm\bin\Release\Irony.PCL.dll -Destination Downloads\Temp -Force
#    Copy-Item -Path Sarcasm\bin\Release\Sarcasm.dll -Destination Downloads\Temp -Force
    Copy-Item -Path Sarcasm\bin\Release\Sarcasm.PCL.dll -Destination Downloads\Temp -Force
    Copy-Item -Path DecorationConnectors\DecorationConnector.WPF\bin\Release\DecorationConnector.WPF.dll -Destination Downloads\Temp -Force
    Copy-Item -Path DecorationConnectors\DecorationConnector.Forms\bin\Release\DecorationConnector.Forms.dll -Destination Downloads\Temp -Force
    Copy-Item -Path DecorationConnectors\DecorationConnector.Silverlight\bin\Release\DecorationConnector.Silverlight.dll -Destination Downloads\Temp -Force
    Copy-Item -Path DecorationConnectors\DecorationConnector.Android\bin\Release\DecorationConnector.Android.dll -Destination Downloads\Temp -Force

    $zipFileName = "Downloads\Sarcasm" + "_" + (Get-Date).ToShortDateString().TrimEnd('.').Replace('.', '-') + ".zip"
    Remove-Item -Path $zipFileName -Force
    CreateZipFile -zipfilename $zipFileName -sourcedir Downloads\Temp

    Remove-Item -Path Downloads\Temp -Force -Recurse
}
