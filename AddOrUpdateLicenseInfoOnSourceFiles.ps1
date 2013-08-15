Param(
    [bool]$remove = $FALSE,

    [string]$rootPath = ".",

    [string[]]$fileNames = @("*.cs","*.tt",,"*.ttinclude")
)

$pattern = "([:b]*\r?\n)*#region License(.*\r?\n)*?#endregion([:b]*\r?\n)*"

$sl = (Get-Item -Path "License.txt").OpenText()
$licenseText = $sl.ReadToEnd();
$sl.Close();

Get-ChildItem $rootPath -include $fileNames -recurse |
    ForEach-Object {
        $sr = $_.OpenText();
        $origText = $sr.ReadToEnd();
        $text = $origText -replace $pattern;
        $encoding = $sr.CurrentEncoding;
        $sr.Close();

        if ($remove)
        {
            if ($text -ne $origText)
            {
                $sw = New-Object System.IO.StreamWriter($_, $FALSE, $encoding)
                $sw.Write($text);
                $sw.Close();
            }
        }
        else
        {
            $sw = New-Object System.IO.StreamWriter($_, $FALSE, $encoding)
            $sw.WriteLine("#region License");
            $sw.WriteLine("/*");
            $sw.WriteLine("    This file is part of Sarcasm.");
            $sw.WriteLine();
            $sw.Write($licenseText);
            $sw.WriteLine("*/");
            $sw.WriteLine("#endregion");
            $sw.WriteLine();
            $sw.Write($text);
            $sw.Close();
        }
    }
