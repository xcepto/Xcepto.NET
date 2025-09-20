#!/bin/bash
PACKAGE=./$1/$1.csproj
VERSION=$2

dotnet build $PACKAGE -c Release -p:Version=$VERSION
dotnet pack $PACKAGE -c Release -p:Version=$VERSION -o ./nupkg
dotnet nuget push ./nupkg/$1.$VERSION.nupkg --api-key $NUGET_API_KEY --source https://api.nuget.org/v3/index.json
