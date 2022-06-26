using UnityEngine;

namespace AmbisonicAudioStreaming.Samples
{
    public class CameraRotationController : MonoBehaviour
    {
        [SerializeField] Transform _camera;
        [SerializeField] float _scaleFactor = 0.3f;
        [SerializeField] CameraRotationUIView _uiView;

        private CameraRotator _cameraRotator;

        private void Awake()
        {
            _cameraRotator = new CameraRotator(_camera, _scaleFactor);
            _uiView.OnRotate += OnRotateEventHandler;
        }

        private void OnDestroy()
        {
            _uiView.OnRotate -= OnRotateEventHandler;
        }

        private void Update()
        {
            var vertical = Input.GetAxis("Vertical");
            var horizontal = Input.GetAxis("Horizontal");

            if (vertical != 0f || horizontal != 0f)
            {
                _cameraRotator.Rotate(vertical, horizontal);
            }
        }

        private void OnRotateEventHandler(Vector2 rotation)
        {
            _cameraRotator.Rotate(rotation.x, rotation.y);
        }
    }
}