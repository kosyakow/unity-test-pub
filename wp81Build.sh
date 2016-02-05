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
build=$(expr $(expr $d1 / 60 - 23875980) / 1000)
rev=$(expr $(expr $d1 / 60 - 23875980) % 1000)
echo "Build:" $build
echo "Rev:" $rev

PROJECT_PATH="C:/work/unity"
#PROJECT_PATH="C:/jenkins/workspace/Mobile_Unity5_Windows_Test_App"

echo "Import plugin..."
"c:/Program Files/Unity/Editor/Unity" -quit -batchmode -nographics -projectPath $PROJECT_PATH/unity-test -importPackage $PROJECT_PATH/VungleSDK.unitypackage

echo "Build unity project..."
"c:/Program Files/Unity/Editor/Unity" -quit -batchmode -nographics -projectPath $PROJECT_PATH/unity-test -executeMethod BuildGenerator.BuildWP81 -CustomArgs:ver=$version.$build.$rev

echo "Build unity project again..."
"c:/Program Files/Unity/Editor/Unity" -quit -batchmode -nographics -projectPath $PROJECT_PATH/unity-test -executeMethod BuildGenerator.BuildWP81 -CustomArgs:ver=$version.$build.$rev

cp win81/UnityCommon.props unity-test/Builds/WSA/UnityCommon.props

NUGET=~/bin/nuget.exe
MS_BUILD_EXE="C:/Program Files (x86)/MSBuild/14.0/Bin/MSBuild.exe"

echo "Restoring project dependencies..."
"$NUGET" restore unity-test/Builds/WSA/VungleUnity5Test_wp81.sln

sed -i -- 's/Publisher=\"CN=Vungle\"/Publisher=\"OID.0.9.2342.19200300.100.1.1=11249643, CN=Vungle, OU=Vungle\"/g' unity-test/Builds/WSA/VungleUnity5Test_wp81/Package.appxmanifest

echo "Building UWP test"
"$MS_BUILD_EXE" "/p:Configuration=Release;Platform=ARM" unity-test/Builds/WSA/VungleUnity5Test_wp81/VungleUnity5Test_wp81.csproj
echo "Sign"
"c:/Users/pushok/Desktop/sign.sh"

echo "Create test zip"
PACKAGES="unity-test/Builds/WSA/VungleUnity5Test_wp81/AppPackages/VungleUnity5Test_wp81_*_Test"
for dir in $PACKAGES; do
	pushd "$dir"
	ZIPFILE=${PWD##*/}
	ZIPFILE=${ZIPFILE/_Test/}.zip
	echo "Creating $ZIPFILE archive..."
	zip -r -9 "$PROJECT_PATH/$ZIPFILE" *
	popd
done
