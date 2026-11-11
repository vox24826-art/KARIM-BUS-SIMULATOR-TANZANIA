# Android APK Export Checklist

This checklist is for the final export step after the Unity project is opened in a machine with Unity 6 and Android build support installed.

## Required tools

- Unity 6
- Android Build Support module
- Android SDK
- Android NDK
- JDK

## Unity project settings

1. Open the project in Unity 6.
2. Set platform to Android in Build Settings.
3. Set package name: com.karimgames.karimbussimulatortanzania
4. Set product name: KARIM BUS SIMULATOR TANZANIA
5. Set company name: KARIM GAMES STUDIO LIMITED
6. Enable IL2CPP for Android.
7. Choose minimum Android SDK level suitable for the target devices.
8. Set target API level to a modern Android version supported by the environment.
9. Enable install location if needed.
10. Ensure the scene is configured to load the game start scene.

## Build settings

- Build type: Release
- Scripting backend: IL2CPP
- Texture compression: ETC2/ASTC if supported
- Optimize for Android performance
- Test on realistic mid-range Android hardware

## APK creation

1. Select File > Build Settings.
2. Choose Android and click Switch Platform.
3. Click Build.
4. Choose the output folder.
5. Wait for Unity to write the APK.
6. Transfer to Android device and install.

## Final testing

- launch app
- check main menu
- verify garage and career screens
- test route flow
- confirm save system loads
- confirm gameplay works on device

## Important note

This project is a Unity-ready prototype foundation. The real APK binary can only be produced when Unity with Android tooling is available on the host machine.
