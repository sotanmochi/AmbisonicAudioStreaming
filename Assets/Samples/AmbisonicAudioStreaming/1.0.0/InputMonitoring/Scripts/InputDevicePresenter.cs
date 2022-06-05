using System;

namespace AmbisonicAudioStreaming.Samples
{
    public class InputDevicePresenter : IDisposable
    {
        private InputDeviceUIView _uiView;
        private InputMonitoringContext _context;

        public InputDevicePresenter
        (
            InputDeviceUIView uiView,
            InputMonitoringContext context
        )
        {
            _uiView = uiView;
            _context = context;
        }

        public void Initialize()
        {
            var inputDevices = _context.GetInputDevices();
            _uiView.UpdateSelectionDropdown(inputDevices);
            _context.OnCurrentDeviceChanged += OnCurrentDeviceChanged;
        }

        public void Dispose()
        {            
            _context.OnCurrentDeviceChanged -= OnCurrentDeviceChanged;
        }

        private void OnCurrentDeviceChanged((int SampleRate, int Channels) deviceProperties)
        {
            _uiView.UpdateDeviceProperies(deviceProperties.SampleRate, deviceProperties.Channels);
        }
    }
}