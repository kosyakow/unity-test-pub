#! /bin/bash

set -e

. ~/build/runcmd.sh
currentPath=$(pwd)
echo "Path:" $currentPath
if [ -f ./version.ver ]; then
    version=$(cat ./version.ver)
else
    version="1.0"
fi
echo "Version:" $version
d1=$(date +%s)
#25.05.2015 16:00 MSK
build=$(expr $d1 / 60 - 23875980)
echo "Build:" $build

PROJECT_PATH="C:/work/unity/unity-test"

#/Applications/Unity/Unity.app/Contents/MacOS/Unity -quit -projectPath $currentPath/unity-test/ -importPackage $currentPath/VungleSDK.unitypackage
"c:/Program Files/Unity/Editor/Unity" -quit -projectPath $PROJECT_PATH -executeMethod BuildGenerator.BuildUWP -CustomArgs:ver=$version.$build

cp uwp/VungleUnity5Test.csproj.user unity-test/Builds/WSA/VungleUnity5Test/VungleUnity5Test.csproj.user
cp uwp/UnityCommon.props unity-test/Builds/WSA/UnityCommon.props

NUGET=~/bin/nuget.exe
MS_BUILD_EXE="C:/Program Files (x86)/MSBuild/14.0/Bin/MSBuild.exe"

echo "Restoring project dependencies..."
"$NUGET" restore unity-test/Builds/WSA/VungleUnity5Test.sln

echo "Building UWP test"
"$MS_BUILD_EXE" "/p:Configuration=Release;Platform=x86;AppxBundle=Always;AppxBundlePlatforms=x86|x64|ARM" unity-test/Builds/WSA/VungleUnity5Test/VungleUnity5Test.csproj

