using UnityEngine;

namespace AmbisonicAudioStreaming.AVProVideoExtension.Samples
{
    public class CameraRotator
    {
        private Transform _camera;
        private float _scaleFactor = 0.3f;

        public CameraRotator(Transform camera, float scaleFactor)
        {
            _camera = camera;
            _scaleFactor = scaleFactor;
        }

        public void Rotate(float vertical, float horizontal)
        {
            if (_camera is null) return;

            var diff = new Vector3(-vertical * _scaleFactor, horizontal * _scaleFactor, 0f);

            _camera.localEulerAngles += diff;
        }
    }
}