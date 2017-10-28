Param(
  [string]$Target="",
  [switch]$Release
)

$solution = (Get-Item "*.sln").FullName
$compiler = "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\amd64\MSBuild.exe"

if ((Test-Path $compiler) -eq $false)
{
  Write-Error "Compiler not found at: $compiler"
  break
}

$params = @(
  $solution

  "/nologo"
  "/verbosity:quiet"
  "/maxcpucount"
)

# if was given path instead of target name try to convert it
if (($Target.Trim() -ne "") -and (Test-Path $Target)) { $Target = (Get-Item $Target).Name }
$Target = $Target.Trim().Replace(".", "_")
if ($Target -ne "")
{
  $params += "/t:$Target"
}


if ($Release.IsPresent)
{
  $params += "/p:Configuration=Release"
}
# DEBUG build
else
{
  $params += '/p:Configuration=Debug'

  # disable build events
  $params += "/p:PreBuildEvent="
  $params += "/p:PostBuildEvent="
}


"$compiler $params"
& $compiler $params

