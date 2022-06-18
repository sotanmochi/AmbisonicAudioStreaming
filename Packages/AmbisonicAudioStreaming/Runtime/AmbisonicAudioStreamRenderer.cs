using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace AmbisonicAudioStreaming
{
    [RequireComponent(typeof(AudioSource))]
    public class AmbisonicAudioStreamRenderer : MonoBehaviour
    {
        public int Channels => _channels;
        public int SamplingRate => _samplingRate; // [Hz]
        public int AudioClipSamples => _audioClipSamples;

        private readonly float _delayTimeSeconds = 0.2f;

        private AudioSource _audioSource;
        private AudioClip _audioClip;

        private int _channels;
        private int _samplingRate;
        private int _audioClipSamples;
        private int _delaySamples;

        private float[] _buffer = Array.Empty<float>();
        private int _bufferLengthBytes;
        private int _head;

        private bool _initialized;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();

            if (!_audioSource.clip.ambisonic)
            {
                Debug.LogError($"[AmbisonicAudioStreamRenderer] Cannot initialize. The audio clip is not ambisonic.");
                _initialized = false;
                return;
            }

            _audioClip = _audioSource.clip;

            _channels = _audioClip.channels;
            _samplingRate = _audioClip.frequency;
            _audioClipSamples = _audioClip.samples;

            _delaySamples = (int)(_samplingRate * _delayTimeSeconds);

            _initialized = true;
        }

        public async void PushAudioFrame(float[] data)
        {
            if (!_initialized) { return; }
            
            if (_buffer.Length != data.Length)
            {
                _buffer = new float[data.Length];
                _bufferLengthBytes = _buffer.Length * 4;
            }
            
            await UniTask.SwitchToMainThread();
            
            Buffer.BlockCopy(data, 0, _buffer, 0, _bufferLengthBytes);
            _audioClip.SetData(_buffer, _head);
            
            _head += data.Length / _channels;
            _head %= _audioClipSamples;
            
            if (!_audioSource.isPlaying && _head > _delaySamples)
            {
                _audioSource.Play();
            }
        }

        public void Clear()
        {
            _audioSource.Stop();
            _head = 0;
        }
    }
}