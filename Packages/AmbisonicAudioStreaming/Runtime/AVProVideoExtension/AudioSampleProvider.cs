using System;
using UnityEngine;
using RenderHeads.Media.AVProVideo;

namespace AmbisonicAudioStreaming.AVProVideoExtension
{
    public enum ChannelType
    {
        Mono = 1,
        Stereo = 2,
        Ambisonics = 4,
    }

    public class AudioSampleProvider : MonoBehaviour, IAudioSampleProvider
    {
		[SerializeField] MediaPlayer _mediaPlayer = null;
        [SerializeField] ChannelType _outputChannelType = ChannelType.Ambisonics;
        [SerializeField] uint _bufferSampleCount = 4096; 

        public event Action OnPrepared;
        public event AudioFrameSampledEventHandler OnAudioSampled;

        private IMediaControl _mediaControl = null;
        private ushort _outputChannelCount;
        private float[] _outputPcmBuffer = null;
        private float[] _pcmBuffer = null;

        void Awake()
        {
            _mediaControl = _mediaPlayer.Control;
            _outputChannelCount = (ushort)_outputChannelType;

            _outputPcmBuffer = new float[_bufferSampleCount * _outputChannelCount];
            _pcmBuffer = new float[_bufferSampleCount * _outputChannelCount];

            OnPrepared?.Invoke();
        }

        void Start()
        {

        }

        void OnDestroy()
        {
            
        }

        void Update()
        {
            if (_mediaControl is null)
            {
                _mediaControl = _mediaPlayer.Control;
            }

            if (_mediaControl != null && _mediaControl.IsPlaying())
            {
                UpdateAudio();
            }
        }

        public void UpdateAudio()
        {
            var channelCount = (ushort)_mediaControl.GetAudioChannelCount();
            // Debug.Log($"UpdateAudio.ChannelCount: {channelCount}");

            if (channelCount <= 0)
            {
                return;
            }

            var requiredBufferSize = _bufferSampleCount * channelCount;
            if (_pcmBuffer is null || _pcmBuffer.Length != requiredBufferSize)
            {
                _pcmBuffer = new float[requiredBufferSize];
            }

            int samples = 0;

#if (UNITY_EDITOR_WIN || UNITY_EDITOR_OSX) || (!UNITY_EDITOR && (UNITY_STANDALONE_WIN || UNITY_WSA_10_0 || UNITY_STANDALONE_OSX || UNITY_IOS || UNITY_TVOS || UNITY_ANDROID))
            samples = _mediaControl.GrabAudio(_pcmBuffer, _pcmBuffer.Length, channelCount);
#endif
            if (samples != 0)
            {
                uint readPos = 0;
                uint writePos = 0;

                var lesserChannels = Math.Min(channelCount, _outputChannelCount);

                // Debug.Log($"channelCount: {channelCount}");
                // Debug.Log($"OutputChannelCount: {_outputChannelCount}");
                // Debug.Log($"LesserChannels: {lesserChannels}");

                for (int i = 0; i < _bufferSampleCount; ++i)
                {
                    for (int j = 0; j < lesserChannels; ++j)
                    {
                        _outputPcmBuffer[writePos + j] = _pcmBuffer[readPos + j];
                    }

                    readPos += channelCount;
                    writePos += _outputChannelCount;
                }

                OnAudioSampled?.Invoke(_bufferSampleCount, _outputChannelCount, _outputPcmBuffer);
            }
        }
    }
}