using System;
using UnityEngine;

namespace AmbisonicAudioStreaming.AVProVideoExtension.Samples
{
    public class CameraRotationUIView : MonoBehaviour
    {
        [SerializeField] ButtonEventTrigger _upButton;
        [SerializeField] ButtonEventTrigger _downButton;
        [SerializeField] ButtonEventTrigger _leftButton;
        [SerializeField] ButtonEventTrigger _rightButton;

        public Action<Vector2> OnRotate;

        private Vector2 _rotation = new Vector2(0f, 0f);

        private void Awake()
        {
            _upButton.OnPressed += OnUpPressedEventHandler;
            _upButton.OnReleased += OnUpReleasedEventHandler;
            _downButton.OnPressed += OnDownPressedEventHandler;
            _downButton.OnReleased += OnDownReleasedEventHandler;
            _leftButton.OnPressed += OnLeftPressedEventHandler;
            _leftButton.OnReleased += OnLeftReleasedEventHandler;
            _rightButton.OnPressed += OnRightPressedEventHandler;
            _rightButton.OnReleased += OnRightReleasedEventHandler;
        }

        private void Update()
        {
            if (_rotation != Vector2.zero)
            {
                OnRotate?.Invoke(_rotation);
            }
        }

        private void OnDestroy()
        {
            _upButton.OnPressed -= OnUpPressedEventHandler;
            _upButton.OnReleased -= OnUpReleasedEventHandler;
            _downButton.OnPressed -= OnDownPressedEventHandler;
            _downButton.OnReleased -= OnDownReleasedEventHandler;
            _leftButton.OnPressed -= OnLeftPressedEventHandler;
            _leftButton.OnReleased -= OnLeftReleasedEventHandler;
            _rightButton.OnPressed -= OnRightPressedEventHandler;
            _rightButton.OnReleased -= OnRightReleasedEventHandler;
        }

        private void OnUpPressedEventHandler()
        {
            _rotation.x = 1f;
        }

        private void OnUpReleasedEventHandler()
        {
            _rotation.x = 0f;
        }

        private void OnDownPressedEventHandler()
        {
            _rotation.x = -1f;
        }

        private void OnDownReleasedEventHandler()
        {
            _rotation.x = 0f;
        }

        private void OnLeftPressedEventHandler()
        {
            _rotation.y = -1f;
        }

        private void OnLeftReleasedEventHandler()
        {
            _rotation.y = 0f;
        }

        private void OnRightPressedEventHandler()
        {
            _rotation.y = 1f;
        }

        private void OnRightReleasedEventHandler()
        {
            _rotation.y = 0f;
        }
    }
}