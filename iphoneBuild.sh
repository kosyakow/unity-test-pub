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
[ -f ./unity-test/VungleUnityTest.ipa ] && rm ./unity-test/VungleUnityTest.ipa

/Applications/Unity/Unity.app/Contents/MacOS/Unity -quit -batchmode -projectPath $currentPath/unity-test/ -importPackage $currentPath/VungleSDK.unitypackage
/Applications/Unity/Unity.app/Contents/MacOS/Unity -projectPath $currentPath/unity-test/ -executeMethod BuildGenerator.BuildiPhone -CustomArgs:ver=$version.$build &
./sikuli/runScript -r ./sikuli/test.sikuli
sed -i '' 's/iPhone Developer/iPhone Distribution/g' ./unity-test/VungleUnityTest/Unity-iPhone.xcodeproj/project.pbxproj
xcodebuild -project ./unity-test/VungleUnityTest/Unity-iPhone.xcodeproj -scheme Unity-iPhone archive -archivePath ./unity-test/VungleUnityTest.xcarchive
xcodebuild -exportArchive -exportFormat ipa -archivePath "./unity-test/VungleUnityTest.xcarchive/" -exportPath "./unity-test/VungleUnityTest.ipa" -exportProvisioningProfile "Vungle In House Distribution"
mv ./unity-test/VungleUnityTest.ipa .
rm -r -f ./unity-test/VungleUnityTest
rm -r -f ./unity-test/VungleUnityTest.xcarchive
puck -app_id=5d9a02e5dd6c44a33d17e424da6a076b -submit=auto -download=true -notify=false VungleUnityTest.ipa