using System;
using System.Linq;
using AudioUtilityToolkit;

namespace AmbisonicAudioStreaming.Samples
{
    public class InputMonitoringContext : IDisposable
    {
        public event Action<(int SampleRate, int Channels)> OnCurrentDeviceChanged;

        private AmbisonicAudioStreamRenderer _audioStreamRenderer;
        private InputStream _inputStream;

        public InputMonitoringContext(AmbisonicAudioStreamRenderer audioStreamRenderer)
        {
            _audioStreamRenderer = audioStreamRenderer;
        }

        public void Dispose()
        {
            if (_inputStream != null)
            {
                _inputStream.OnProcessFrame -= OnProcessFrame;
                _inputStream.Dispose();
            }
        }

        public string[] GetInputDevices()
        {
            return AudioDeviceDriver.InputDeviceList.Select(device => device.DeviceName).ToArray();
        }

        public void SetInputDevice(string deviceName)
        {
            _audioStreamRenderer.Clear();

            if (_inputStream != null)
            {
                _inputStream.OnProcessFrame -= OnProcessFrame;
            }

            _inputStream = AudioDeviceDriver.GetInputDevice(deviceName);

            if (_inputStream != null && _inputStream.ChannelCount == _audioStreamRenderer.Channels)
            {
                _inputStream.OnProcessFrame += OnProcessFrame;
                OnCurrentDeviceChanged?.Invoke((_inputStream.SampleRate, _inputStream.ChannelCount));
            }
        }

        private void OnProcessFrame(float[] data)
        {
            _audioStreamRenderer.PushAudioFrame(data);
        }
    }
}