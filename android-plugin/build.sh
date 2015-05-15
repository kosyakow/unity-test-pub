UNITY_HOME=/Applications/Unity/Unity.app
ANDROID_HOME=~/Library/Android/sdk
NDK_HOME=~/bin/android-ndk-r10d

pushd src
javac -classpath ${UNITY_HOME}/Contents/PlaybackEngines/AndroidPlayer/release/bin/classes.jar:${ANDROID_HOME}/platforms/android-22/android.jar:../libs/vunglePub.jar com/vungle/VunglePlugin.java
jar cvf vunglePlugin.jar com/vungle/*.class
popd
cp libs/vunglePub.jar ../UnityProject/Assets/Plugins/Android/Vungle_lib/libs/
mv src/vunglePlugin.jar ../UnityProject/Assets/Plugins/Android/Vungle_lib/libs/
