using System;

namespace AmbisonicAudioStreaming.Samples
{
    public class InputDeviceController : IDisposable
    {
        private InputDeviceUIView _uiView;
        private InputMonitoringContext _context;

        public InputDeviceController
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
            _uiView.OnDeviceSelected += OnDeviceSelectedEventHandler;
        }

        public void Dispose()
        {
            _uiView.OnDeviceSelected -= OnDeviceSelectedEventHandler;
        }

        private void OnDeviceSelectedEventHandler(string deviceName)
        {
            _context.SetInputDevice(deviceName);
        }
    }
}