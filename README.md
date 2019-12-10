# Chemistry Lab VR

Chemistry Lab VR project using the Unity game engine and the Windows Mixed Reality Toolkit.

## How to Run

Prerequisites:
* Unity version `2019.2.12f1`
* Windows 10 SDK version `18362` or later

### Development

1. In `File > Build Settings` switch to the `Universal Windows Platform`.
2. In `Edit > Project Settings > Player` check `Virtual Reality Supported` & add the `Windows Mixed Reality` SDK.
3. In `Edit > Project Settings > Audio` select `MS HRTF Spatializer` as the spatializer plugin.
4. Use the play button in the Unity editor.

## Creating an Experiment

Please use a copy of `Assets/Scenes/Base.unity` when creating a new experiment. This scene already has the mixed reality toolkit setup and includes the pointer interaction, player movement & grabbing. The scene also has a basic ground plane and four walls, these can be removed if desired.

Please link refrences to websites or tutorials that you deem important in the refrences section below.

## References

[Windows Mixed Reality Tutorial](https://medium.com/@cassiealynn/10-steps-to-get-you-started-with-mixed-reality-development-using-unity-148abea10e57)

[Mixed Reality Basics](https://docs.microsoft.com/en-us/windows/mixed-reality/holograms-100)

[Windows 10 SDK](https://developer.microsoft.com/en-US/windows/downloads/windows-10-sdk)

[Mixed Reality Toolkit](https://github.com/Microsoft/MixedRealityToolkit-Unity/releases)

[Autodesk Character Generator](https://charactergenerator.autodesk.com/account/myavatars.aspx)

[SALSA Lip-Sync](https://crazyminnowstudio.com/posts/video-tutorial-easy-lip-sync-for-autodesk-character-generator-models/)

[Dialogflow V2 Unity](https://itp-xstory.github.io/uniFlow/)

[Mixed Reality Simulator](https://docs.microsoft.com/en-us/windows/mixed-reality/advanced-hololens-emulator-and-mixed-reality-simulator-input)

[Mixed Reality Toolkit Input Event Types](https://microsoft.github.io/MixedRealityToolkit-Unity/Documentation/Input/InputEvents.html)

[Mixed Reality Toolkit Documentation](https://microsoft.github.io/MixedRealityToolkit-Unity/README.html)

[Google Server To Server Auth Documentation](https://developers.google.com/identity/protocols/OAuth2ServiceAccount)

[Dialogflow Documentation](https://cloud.google.com/dialogflow/docs/)

[Dialogflow REST API Reference](https://cloud.google.com/dialogflow/docs/reference/rest/v2-overview)
