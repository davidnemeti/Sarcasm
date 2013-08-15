Param(
    [string]$assemblyInfoPath = "Sarcasm\Properties\AssemblyInfo.cs"
)

$sr = (Get-Item -Path $assemblyInfoPath).OpenText()
$origText = $sr.ReadToEnd();
$encoding = $sr.CurrentEncoding;
$sr.Close();

$matchPattern = "\[assembly\: AssemblyFileVersion\(`"([0-9]+)\.([0-9]+)\.([0-9]+)\.([0-9]+)`"\)\]";

$origText -match $matchPattern;

$major = [int] $Matches[1];
$minor = [int] $Matches[2];
$build = [int] $Matches[3];
$revision = [int] $Matches[4];

$build++;

$replacePattern = [string]::Format("[assembly: AssemblyFileVersion(`"{0}.{1}.{2}.{3}`")]", $major, $minor, $build, $revision);

$text = $origText -replace $matchPattern, $replacePattern

$sw = New-Object System.IO.StreamWriter((Get-Item -Path $assemblyInfoPath), $FALSE, $encoding)
$sw.Write($text);
$sw.Close();
