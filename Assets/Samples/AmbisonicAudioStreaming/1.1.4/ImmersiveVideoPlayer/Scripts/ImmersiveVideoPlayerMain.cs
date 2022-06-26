using Unity.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace AmbisonicAudioStreaming.Samples
{
    /// <summary>
    /// Entry point
    /// </summary>
    public class ImmersiveVideoPlayerMain : MonoBehaviour
    {
        [SerializeField] private VideoAudioSampleProvider _videoAudioSampleProvider;
        [SerializeField] private AmbisonicAudioStreamRenderer _audioStreamRenderer;

        private bool _prepared;

        private void Awake()
        {
            _videoAudioSampleProvider.OnPrepared += OnPreparedEventHandler;
            _videoAudioSampleProvider.OnAudioSampled += OnAudioSampledEventHandler;
        }

        private void OnDestroy()
        {
            _videoAudioSampleProvider.OnPrepared -= OnPreparedEventHandler;
            _videoAudioSampleProvider.OnAudioSampled -= OnAudioSampledEventHandler;
        }

        private async void Start()
        {
            await UniTask.WaitUntil(() => _prepared);
            _videoAudioSampleProvider.VideoPlayer.Play();
        }

        private void OnPreparedEventHandler()
        {
            _prepared = true;
            var channelCount = _videoAudioSampleProvider.ChannelCount;
            Debug.Log($"[VideoPlayerContext] VideoAudioSampleProvider is prepared.");
            Debug.Log($"[VideoPlayerContext] VideoAudioSampleProvider.ChannelCount: {channelCount}");
        }

        private void OnAudioSampledEventHandler(uint sampleCount, ushort channelCount, NativeArray<float> buffer)
        {
            _audioStreamRenderer.PushAudioFrame(buffer.ToArray());
        }
    }
}