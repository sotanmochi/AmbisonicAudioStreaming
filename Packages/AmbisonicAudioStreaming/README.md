# Ambisonic Audio Streaming

## Tested Environment
- Unity 2020.3.27f1
- Audio Interface: [Zoom H3-VR](https://zoomcorp.com/ja/jp/handheld-recorders/handheld-recorders/h3-vr-360-audio-recorder/) 4ch Ambisonics mode (AmbiX)

## Dependencies
### Samples/InputMonitoring
- [libsoundio 1.0.4](https://github.com/keijiro/jp.keijiro.libsoundio/tree/1.0.4)  
  Licensed under the MIT License. Copyright (c) 2015 Andrew Kelley

- [System.Memory 4.5.3](https://www.nuget.org/packages/System.Memory/4.5.3)  
  Licensed under the MIT License. Copyright (c) .NET Foundation and Contributors

### Samples/Streaming


## How to install
`Packages/manifest.json`

```
{
  "dependencies": {
    "jp.sotanmochi.ambisonicaudiostreaming" : "",
    ...
  }
}
```

## How to install for Samples
`Packages/manifest.json`

### .NET Standard 2.0 (Unity 2021.1 or earlier)
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
    "jp.sotanmochi.ambisonicaudiostreaming" : "",
    "org.nuget.system.memory": "4.5.3",
    "jp.keijiro.libsoundio": "1.0.4",
    ...
  }
}
```
