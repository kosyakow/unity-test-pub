set -ve
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

[ -f ./unity-test/VungleUnityTest.apk ] && rm ./unity-test/VungleUnityTest.apk
/Applications/Unity/Unity.app/Contents/MacOS/Unity -quit -projectPath $currentPath/unity-test/ -importPackage $currentPath/VungleSDK.unitypackage 
/Applications/Unity/Unity.app/Contents/MacOS/Unity -projectPath $currentPath/unity-test/ -executeMethod BuildGenerator.BuildAndroid -CustomArgs:ver=$version.$build &
./sikuli/runScript -r ./sikuli/test.sikuli
mv ./unity-test/VungleUnityTest.apk .
puck -api_token=d6cb4cec883a44a5a39a0ed21a845ff3 -app_id=e649705ac96a08062cddc4465708e7ec -submit=auto -download=true -notify=false -open=nothing VungleUnityTest.apk
sleep 10
