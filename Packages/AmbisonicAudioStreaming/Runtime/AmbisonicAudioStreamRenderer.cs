using System;
using Unity.Collections;
using UnityEngine;

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
        private int _head;

        private RingBuffer<float> _ringBuffer = new RingBuffer<float>(Int16.MaxValue * 8);
        private float[] _readBuffer = new float[4096];

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

            Debug.Log($"[AmbisonicAudioStreamRenderer] RingBuffer.Capacity: {_ringBuffer.Capacity}");

            _initialized = true;
        }

        private void Update()
        {
            if (_ringBuffer.Count > _readBuffer.Length)
            { 
                _ringBuffer.Dequeue(_readBuffer);

                _audioClip.SetData(_readBuffer, _head);
                
                _head += _readBuffer.Length / _channels;
                _head %= _audioClipSamples;
 
                if (!_audioSource.isPlaying && _head > _delaySamples)
                {
                    _audioSource.Play();
                }
            }
        }

        public void PushAudioFrame(float[] data)
        {
            if (!_initialized) { return; }
            _ringBuffer.Enqueue(data);
        }

        public void PushAudioFrame(NativeArray<float> data)
        {
            throw new NotImplementedException();
            // if (!_initialized) { return; }
            // _ringBuffer.Enqueue(data);
        }

        public void Clear()
        {
            _audioSource.Stop();
            _head = 0;
        }
    }
}