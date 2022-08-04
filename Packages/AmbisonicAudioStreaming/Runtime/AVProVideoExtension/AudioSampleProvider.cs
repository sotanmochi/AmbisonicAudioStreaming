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

    public class AudioSampleProvider : MonoBehaviour
    {
		[SerializeField] MediaPlayer _mediaPlayer = null;
        [SerializeField] ChannelType _outputChannelType = ChannelType.Ambisonics;
        [SerializeField] uint _sampleFrameCount = 4096; 

        public event Action<uint, ushort, float[]> OnAudioSampled;

        private IMediaControl _mediaControl = null;
        private uint _outputChannelCount;
        private float[] _pcmBuffer = null;

        void Awake()
        {
            _mediaControl = _mediaPlayer.Control;
            _outputChannelCount = (uint)_outputChannelType;
            _pcmBuffer = new float[_sampleFrameCount * _outputChannelCount];
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

            if (channelCount <= 0)
            {
                return;
            }

            // Debug.Assert(_pcmBuffer.Length % _outputChannelCount == 0);
            // Debug.Assert(channelCount == _outputChannelCount);

            var requiredBufferSize = _sampleFrameCount * channelCount;
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
                OnAudioSampled?.Invoke(_sampleFrameCount, channelCount, _pcmBuffer);
            }
        }
    }
}