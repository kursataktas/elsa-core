dotnet pack -o packages --version-suffix "rc1" -p:PackageVersion=2.9.3-rc1
dotnet nuget push packages/*.nupkg --api-key {key} -s https://api.nuget.org/v3/index.json --skip-duplicate