ECHO "Building TestExt Nuget package"

msbuild TestExt.sln /t:Clean;Rebuild /p:Configuration=Release
Nuget.exe pack testext.nuspec
move *.nupkg .\Build\Output\Release