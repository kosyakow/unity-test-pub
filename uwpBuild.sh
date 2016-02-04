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

#PROJECT_PATH="C:/work/unity"
PROJECT_PATH="C:/jenkins/workspace/Mobile_Unity5_Windows_Test_App"

echo "Import plugin..."
"c:/Program Files/Unity/Editor/Unity" -quit -batchmode -nographics -projectPath $PROJECT_PATH/unity-test -importPackage $PROJECT_PATH/VungleSDK.unitypackage

echo "Build unity project..."
"c:/Program Files/Unity/Editor/Unity" -quit -batchmode -nographics -projectPath $PROJECT_PATH/unity-test -executeMethod BuildGenerator.BuildUWP -CustomArgs:ver=$version.$build

echo "Build unity project again..."
"c:/Program Files/Unity/Editor/Unity" -quit -batchmode -nographics -projectPath $PROJECT_PATH/unity-test -executeMethod BuildGenerator.BuildUWP -CustomArgs:ver=$version.$build

cp uwp/VungleUnity5Test.csproj.user unity-test/Builds/WSA/VungleUnity5Test_win10/VungleUnity5Test_win10.csproj.user
cp uwp/UnityCommon.props unity-test/Builds/WSA/UnityCommon.props

NUGET=~/bin/nuget.exe
MS_BUILD_EXE="C:/Program Files (x86)/MSBuild/14.0/Bin/MSBuild.exe"

echo "Restoring project dependencies..."
"$NUGET" restore unity-test/Builds/WSA/VungleUnity5Test_win10.sln

echo "Building UWP test"
"$MS_BUILD_EXE" "/p:Configuration=Release;Platform=x86;AppxBundle=Always;AppxBundlePlatforms=x86|x64|ARM" unity-test/Builds/WSA/VungleUnity5Test_win10/VungleUnity5Test_win10.csproj

echo "Create test zip"
PACKAGES="unity-test/Builds/WSA/VungleUnity5Test_win10/AppPackages/VungleUnity5Test_win10_*_Test"
for dir in $PACKAGES; do
	pushd "$dir"
	ZIPFILE=${PWD##*/}
	ZIPFILE=${ZIPFILE/_Test/}.zip
	echo "Creating $ZIPFILE archive..."
	zip -r -9 "$PROJECT_PATH/$ZIPFILE" *
	popd
done
