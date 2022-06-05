using UnityEngine;

namespace AmbisonicAudioStreaming.Samples
{
    /// <summary>
    /// Entry point
    /// </summary>
    public class InputMonitoringMain : MonoBehaviour
    {
        [SerializeField] private AmbisonicAudioStreamRenderer _audioStreamRenderer;
        [SerializeField] private InputDeviceUIView _inputDeviceUIView;

        private InputMonitoringContext _context;
        private InputDeviceController _controller;
        private InputDevicePresenter _presenter;

        private void Awake()
        {
            _context = new InputMonitoringContext(_audioStreamRenderer);
            _controller = new InputDeviceController(_inputDeviceUIView, _context);
            _presenter = new InputDevicePresenter(_inputDeviceUIView, _context);
        }

        private void Start()
        {
            _controller.Initialize();
            _presenter.Initialize();
        }

        private void OnDestroy()
        {
            _context.Dispose();
            _controller.Dispose();
            _presenter.Dispose();
        }
    }
}