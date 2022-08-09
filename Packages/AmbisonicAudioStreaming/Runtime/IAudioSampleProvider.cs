using System;

namespace AmbisonicAudioStreaming
{
    public delegate void AudioFrameSampledEventHandler(uint sampleFrameCount, ushort channelCount, float[] buffer);

    public interface IAudioSampleProvider
    {
        event Action OnPrepared;
        event AudioFrameSampledEventHandler OnAudioSampled;
    }
}