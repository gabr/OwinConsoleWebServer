#.\OwinConsole\bin\Debug\OwinConsole.exe

$params = @(
  ("/path:" + (pwd).path + "\OwinConsole")
)

& "C:\Program Files (x86)\IIS Express\iisexpress.exe" $params
