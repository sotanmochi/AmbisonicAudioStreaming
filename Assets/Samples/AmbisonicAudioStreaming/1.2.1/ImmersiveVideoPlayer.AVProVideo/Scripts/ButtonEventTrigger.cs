using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AmbisonicAudioStreaming.AVProVideoExtension.Samples
{
    public class ButtonEventTrigger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action OnPressed;
        public event Action OnReleased;

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            OnPressed?.Invoke();
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            OnReleased?.Invoke();
        }
    }
}