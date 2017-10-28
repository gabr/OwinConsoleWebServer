# NuGet configuration: %AppData%\NuGet\NuGet.config
#                      $env:AppData\NuGet\NuGet.config

nuget update -self              -NonInteractive
nuget restore                   -NonInteractive
nuget update .\OwinConsole.sln  -NonInteractive

