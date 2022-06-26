// References: https://docs.unity3d.com/2020.3/Documentation/ScriptReference/Experimental.Audio.AudioSampleProvider.html

using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Experimental.Audio;
using UnityEngine.Experimental.Video;

namespace AmbisonicAudioStreaming
{
    [RequireComponent(typeof(VideoPlayer))]
    public class VideoAudioSampleProvider : MonoBehaviour
    {
        public delegate void AudioSampledEventHandler(uint sampleCount, ushort channelCount, NativeArray<float> buffer);
        
        public event Action OnPrepared;
        public event AudioSampledEventHandler OnAudioSampled;
        
        public VideoPlayer VideoPlayer => _videoPlayer;
        public uint ChannelCount => _audioSampleProvider.channelCount;
        
        private VideoPlayer _videoPlayer;
        private AudioSampleProvider _audioSampleProvider;
        
        private void Start()
        {
            _videoPlayer = GetComponent<VideoPlayer>();
            _videoPlayer.audioOutputMode = VideoAudioOutputMode.APIOnly;
            _videoPlayer.EnableAudioTrack(0, true);
            _videoPlayer.prepareCompleted += PreparedEventHandler;
            _videoPlayer.Prepare();
        }
        
        private void OnDestroy()
        {
            if (_videoPlayer != null)
            {
                _videoPlayer.prepareCompleted -= PreparedEventHandler;
            }
        }
        
        private void PreparedEventHandler(VideoPlayer videoPlayer)
        {
            _audioSampleProvider = videoPlayer.GetAudioSampleProvider(0);
            _audioSampleProvider.sampleFramesAvailable += SampleFramesEventHandler;
            _audioSampleProvider.enableSampleFramesAvailableEvents = true;
            _audioSampleProvider.freeSampleFrameCountLowThreshold = _audioSampleProvider.maxSampleFrameCount / 2;
            OnPrepared?.Invoke();
        }
        
        private void SampleFramesEventHandler(AudioSampleProvider provider, uint sampleFrameCount)
        {
            using (var buffer = new NativeArray<float>((int)sampleFrameCount * provider.channelCount, Allocator.Temp))
            {
                var sampleCount = provider.ConsumeSampleFrames(buffer);
                OnAudioSampled?.Invoke(sampleCount, provider.channelCount, buffer);
            }
        }
    }
}
