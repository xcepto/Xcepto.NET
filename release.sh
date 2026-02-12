#!/bin/bash
PACKAGE=$1/$2/$2.csproj
VERSION=$3

dotnet build $PACKAGE -c Release -p:Version=$VERSION
dotnet pack $PACKAGE -c Release -p:Version=$VERSION -o ./nupkg
dotnet nuget push ./nupkg/$2.$VERSION.nupkg --api-key $NUGET_API_KEY --source https://api.nuget.org/v3/index.json
