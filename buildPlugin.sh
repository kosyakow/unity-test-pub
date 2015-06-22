rm ./VungleSDK.unitypackage
git clone git@github.com:Vungle/publisher-unity-plugin.git
echo "Build android stub"
cd ./publisher-unity-plugin/android-plugin
./build.sh
echo "Build plugin"
cd ./..
./create_unity_plugin.sh
cd ./..
mv ./publisher-unity-plugin/VungleSDK.unitypackage ./VungleSDK.unitypackage
mv ./publisher-unity-plugin/version.ver ./version.ver
echo "Clean"
rm -r -f ./publisher-unity-plugin