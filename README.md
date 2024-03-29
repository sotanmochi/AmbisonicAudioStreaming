# Ambisonic Audio Streaming

## Tested Environment
- Unity 2020.3.27f1 and Unity 2021.3.6f1
- Microphone & Audio Interface: [Zoom H3-VR](https://zoomcorp.com/ja/jp/handheld-recorders/handheld-recorders/h3-vr-360-audio-recorder/) 4ch Ambisonics mode (AmbiX)

## Third party assets
このプロジェクトには、以下のアセットが含まれています。  
The following assets are included in this project.

- [Resonance Audio SDK for Unity v1.2.1](https://github.com/resonance-audio/resonance-audio-unity-sdk/releases/tag/v1.2.1)  
  Licensed under the Apache License, Version 2.0.  
  Copyright (c) 2018 Google Inc.

## Dependencies
- [Google Resonance Audio Spatializer 1.18.3](https://docs.unity.cn/Packages/com.unity.google.resonance.audio@1.18/manual/index.html)  
  Licensed under the Unity Companion License for Unity-dependent projects--see [Unity Companion License](https://unity.com/legal/licenses/unity-companion-license).  
  Copyright © 2018 Unity Technologies ApS

- [System.Memory 4.5.3](https://www.nuget.org/packages/System.Memory/4.5.3)  
  Licensed under the MIT License. Copyright (c) .NET Foundation and Contributors

### Samples/ImmersiveVideoPlayer.AVProVideo
- [AVPro Video](https://renderheads.com/products/avpro-video/)  
  Free trial version is available on [GitHub](https://github.com/RenderHeads/UnityPlugin-AVProVideo/releases).

### Samples/InputMonitoring
- [libsoundio 1.0.4](https://github.com/keijiro/jp.keijiro.libsoundio/tree/1.0.4)  
  Licensed under the MIT License. Copyright (c) 2015 Andrew Kelley

### Samples/LiveStreaming
- [Agora Extension for Unity](https://github.com/sotanmochi/AgoraExtension-Unity)  
  Licensed under the MIT License. Copyright (c) 2021 Soichiro Sugimoto

- [Agora Video SDK for Unity v3.7.0 (May 9, 2022)](https://assetstore.unity.com/packages/tools/video/agora-video-sdk-for-unity-134502)

## How to install package
`Packages/manifest.json`

<details>
<summary>.NET Standard 2.1 (Unity 2021.2 or later)</summary>

```
{
  "dependencies": {
    "jp.sotanmochi.ambisonicaudiostreaming": "https://github.com/sotanmochi/AmbisonicAudioStreaming.git?path=Packages/AmbisonicAudioStreaming#1.2.1",
    ...
  }
}
```

</details>

<details>
<summary>.NET Standard 2.0 (Unity 2021.1 or earlier)</summary>

```
{
  "dependencies": {
    "jp.sotanmochi.ambisonicaudiostreaming": "https://github.com/sotanmochi/AmbisonicAudioStreaming.git?path=Packages/AmbisonicAudioStreaming#1.2.1",
    "org.nuget.system.memory": "4.5.3",
    ...
  },
  "scopedRegistries": [
    {
      "name": "Unity NuGet",
      "url": "https://unitynuget-registry.azurewebsites.net",
      "scopes": [ "org.nuget" ]
    },
    ...
  ],
}
```

</details>

## How to install samples

<image src="./Packages/AmbisonicAudioStreaming/Documentation~/ImportSamples.png" width="75%">

### ImmersiveVideoPlayer and ImmersiveVideoPlayer.AVProVideo
`Packages/manifest.json`

<details>
<summary>.NET Standard 2.1 (Unity 2021.2 or later)</summary>

```
{
  "dependencies": {
    "jp.sotanmochi.ambisonicaudiostreaming": "https://github.com/sotanmochi/AmbisonicAudioStreaming.git?path=Packages/AmbisonicAudioStreaming#1.2.1",
    "com.cysharp.unitask": "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask#2.3.1",
    ...
  }
}
```

</details>

<details>
<summary>.NET Standard 2.0 (Unity 2021.1 or earlier)</summary>

```
{
  "dependencies": {
    "jp.sotanmochi.ambisonicaudiostreaming": "https://github.com/sotanmochi/AmbisonicAudioStreaming.git?path=Packages/AmbisonicAudioStreaming#1.2.1",
    "org.nuget.system.memory": "4.5.3",
    "com.cysharp.unitask": "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask#2.3.1",
    ...
  },
  "scopedRegistries": [
    {
      "name": "Unity NuGet",
      "url": "https://unitynuget-registry.azurewebsites.net",
      "scopes": [ "org.nuget" ]
    }
  ]
}
```

</details>

#### Third party assets
- [AVPro Video](https://renderheads.com/products/avpro-video/)  
  Free trial version is available on [GitHub](https://github.com/RenderHeads/UnityPlugin-AVProVideo/releases).

### InputMonitoring
`Packages/manifest.json`

<details>
<summary>.NET Standard 2.0 (Unity 2021.1 or earlier)</summary>

```
{
  "dependencies": {
    "jp.sotanmochi.ambisonicaudiostreaming": "https://github.com/sotanmochi/AmbisonicAudioStreaming.git?path=Packages/AmbisonicAudioStreaming#1.2.1",
    "org.nuget.system.memory": "4.5.3",
    "jp.keijiro.libsoundio": "1.0.4",
    "com.cysharp.unitask": "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask#2.3.1",
    ...
  },
  "scopedRegistries": [
    {
      "name": "Unity NuGet",
      "url": "https://unitynuget-registry.azurewebsites.net",
      "scopes": [ "org.nuget" ]
    },
    {
      "name": "Keijiro",
      "url": "https://registry.npmjs.com",
      "scopes": [ "jp.keijiro" ]
    }
  ]
}
```

</details>

### LiveStreaming
`Packages/manifest.json`

<details>
<summary>.NET Standard 2.0 (Unity 2021.1 or earlier)</summary>

```
{
  "scopedRegistries": [
    {
      "name": "Unity NuGet",
      "url": "https://unitynuget-registry.azurewebsites.net",
      "scopes": [ "org.nuget" ]
    },
    {
      "name": "Keijiro",
      "url": "https://registry.npmjs.com",
      "scopes": [ "jp.keijiro" ]
    }
  ],
  "dependencies": {
    "jp.sotanmochi.ambisonicaudiostreaming": "https://github.com/sotanmochi/AmbisonicAudioStreaming.git?path=Packages/AmbisonicAudioStreaming",
    "jp.sotanmochi.agora-extension": "https://github.com/sotanmochi/AgoraExtension-Unity.git?path=AgoraExtension-Unity/Packages/AgoraExtension",
    "com.cysharp.unitask": "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask#2.3.1",
    "org.nuget.system.memory": "4.5.3",
    "jp.keijiro.libsoundio": "1.0.4",
    ...
  }
}
```

</details>

## Project Settings
<image src="./Packages/AmbisonicAudioStreaming/Documentation~/ProjectSettings.png" width="75%">

### Define symbol for AVProVideo extension
<image src="./Packages/AmbisonicAudioStreaming/Samples~/ImmersiveVideoPlayer.AVProVideo/DefineSymbol01.png" width="75%">

<image src="./Packages/AmbisonicAudioStreaming/Samples~/ImmersiveVideoPlayer.AVProVideo/DefineSymbol02.png" width="75%">
