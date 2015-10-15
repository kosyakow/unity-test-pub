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
[ -f ./unity-test/VungleUnity5Test.ipa ] && rm ./unity-test/VungleUnity5Test.ipa

/Applications/Unity/Unity.app/Contents/MacOS/Unity -quit -projectPath $currentPath/unity-test/ -importPackage $currentPath/VungleSDK.unitypackage
/Applications/Unity/Unity.app/Contents/MacOS/Unity -projectPath $currentPath/unity-test/ -executeMethod BuildGenerator.BuildiPhone -CustomArgs:ver=$version.$build &
sleep 180
./sikuli/runScript -r ./sikuli/test.sikuli
sed -i '' 's/iPhone Developer/iPhone Distribution/g' ./unity-test/VungleUnity5Test/Unity-iPhone.xcodeproj/project.pbxproj
xcodebuild -project ./unity-test/VungleUnity5Test/Unity-iPhone.xcodeproj -scheme Unity-iPhone archive -archivePath ./unity-test/VungleUnity5Test.xcarchive
xcodebuild -exportArchive -exportFormat ipa -archivePath "./unity-test/VungleUnity5Test.xcarchive/" -exportPath "./unity-test/VungleUnity5Test.ipa" -exportProvisioningProfile "iOSTeam Provisioning Profile: *"
mv ./unity-test/VungleUnity5Test.ipa .
rm -r -f ./unity-test/VungleUnity5Test
rm -r -f ./unity-test/VungleUnity5Test.xcarchive
puck -api_token=d6cb4cec883a44a5a39a0ed21a845ff3 -app_id=e781d6951a80c01cb09f767ca8a28011 -submit=auto -download=true -notify=false -open=nothing VungleUnity5Test.ipa
sleep 10