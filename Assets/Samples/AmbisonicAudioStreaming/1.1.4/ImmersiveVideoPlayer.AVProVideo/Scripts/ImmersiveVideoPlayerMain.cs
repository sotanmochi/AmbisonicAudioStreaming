using UnityEngine;

namespace AmbisonicAudioStreaming.AVProVideoExtension.Samples
{
    /// <summary>
    /// Entry point
    /// </summary>
    public class ImmersiveVideoPlayerMain : MonoBehaviour
    {
        [SerializeReference] private AudioSampleProvider _audioSampleProvider;
        [SerializeField] private AmbisonicAudioStreamRenderer _audioStreamRenderer;

        private bool _prepared;

        private void Awake()
        {
            _audioSampleProvider.OnAudioSampled += OnAudioSampledEventHandler;
            Debug.Log($"Awake@ImmersiveVideoPlayerMain");
        }

        private void OnDestroy()
        {
            _audioSampleProvider.OnAudioSampled -= OnAudioSampledEventHandler;
        }

        private async void Start()
        {
            // await UniTask.WaitUntil(() => _prepared);
        }

        private void OnAudioSampledEventHandler(uint sampleCount, ushort channelCount, float[] buffer)
        {
            Debug.Log($"OnAudioSampled@ImmersiveVideoPlayerMain");
            _audioStreamRenderer.PushAudioFrame(buffer);
        }
    }
}