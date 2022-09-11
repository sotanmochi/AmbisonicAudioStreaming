using System;
using System.Diagnostics;
using System.Threading;
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

        private static readonly double TimestampsToTicks = TimeSpan.TicksPerSecond / (double)Stopwatch.Frequency;
        private static readonly int TargetFrameTimeMilliseconds = (int)(1000.0f / 60); // 60[fps]

        private IMediaControl _mediaControl = null;
        private ushort _outputChannelCount;
        private float[] _outputPcmBuffer = null;
        private float[] _pcmBuffer = null;

        private Thread _audioThread;
        private CancellationTokenSource _cancellationTokenSource;

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
            _cancellationTokenSource = new CancellationTokenSource();
            _audioThread = new Thread(AudioLoop);
            _audioThread.Start();
        }

        void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
        }

        private void AudioLoop()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                var begin = Stopwatch.GetTimestamp();

                UpdateAudio();

                var end = Stopwatch.GetTimestamp();
                var elapsedTicks = (end - begin) * TimestampsToTicks;
                var elapsedMilliseconds = (long)elapsedTicks / TimeSpan.TicksPerMillisecond;

                // UnityEngine.Debug.Log($"AudioThread: {elapsedMilliseconds}[ms]");

                var waitForNextFrameMilliseconds = (int)(TargetFrameTimeMilliseconds - elapsedMilliseconds);
                if (waitForNextFrameMilliseconds > 0)
                {
                    Thread.Sleep(waitForNextFrameMilliseconds);
                }
            }
        }

        private void UpdateAudio()
        {
            if (_mediaControl is null)
            {
                _mediaControl = _mediaPlayer.Control;
            }

            if (_mediaControl is null || !_mediaControl.IsPlaying())
            {
                return;
            }

            var channelCount = (ushort)_mediaControl.GetAudioChannelCount();
            // UnityEngine.Debug.Log($"UpdateAudio.ChannelCount: {channelCount}");

            if (channelCount <= 0)
            {
                UnityEngine.Debug.LogError($"[AmbisonicAudioStreaming] AudioChannelCount: {channelCount}");
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

                // Special case
                // Streaming ambisonic audio data (AmbiX format) using HLS with 5.1-channel codec mode.
                if (channelCount == 6 && _outputChannelCount == 4)
                {
                    for (int i = 0; i < _bufferSampleCount; ++i)
                    {
                        _outputPcmBuffer[writePos + 0] = _pcmBuffer[readPos + 0]; // W <- L
                        _outputPcmBuffer[writePos + 1] = _pcmBuffer[readPos + 1]; // Y <- R
                        _outputPcmBuffer[writePos + 2] = _pcmBuffer[readPos + 4]; // Z <- Ls
                        _outputPcmBuffer[writePos + 3] = _pcmBuffer[readPos + 5]; // X <- Rs

                        readPos += channelCount;
                        writePos += _outputChannelCount;
                    }
                }
                // General case
                else
                {
                    var lesserChannels = Math.Min(channelCount, _outputChannelCount);

                    for (int i = 0; i < _bufferSampleCount; ++i)
                    {
                        for (int j = 0; j < lesserChannels; ++j)
                        {
                            _outputPcmBuffer[writePos + j] = _pcmBuffer[readPos + j];
                        }

                        readPos += channelCount;
                        writePos += _outputChannelCount;
                    }
                }

                OnAudioSampled?.Invoke(_bufferSampleCount, _outputChannelCount, _outputPcmBuffer);
            }
        }
    }
}