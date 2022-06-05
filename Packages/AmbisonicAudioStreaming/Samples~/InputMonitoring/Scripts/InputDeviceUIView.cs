using System;
using UnityEngine;
using UnityEngine.UI;

namespace AmbisonicAudioStreaming.Samples
{
    public class InputDeviceUIView : MonoBehaviour
    {
        [SerializeField] private string _dropdownText;
        [SerializeField] private Dropdown _dropdown;
        [SerializeField] private Text _samplingRate;
        [SerializeField] private Text _channels;

        public event Action<string> OnDeviceSelected;

        private string[] _items = Array.Empty<string>();

        private void Awake()
        {
            _dropdown.onValueChanged.AddListener(OnValueChangedEventHandler);
        }

        private void OnDestroy()
        {
            _dropdown.onValueChanged.RemoveAllListeners();
        }

        public void UpdateDeviceProperies(int samplingRate, int channels)
        {
            _samplingRate.text = $"SamplingRate: {samplingRate} [Hz]";
            _channels.text = $"Channels: {channels}";
        }

        public void UpdateSelectionDropdown(string[] items)
        {
            _items = items;

            _dropdown.ClearOptions();
            _dropdown.RefreshShownValue();
            _dropdown.options.Add(new Dropdown.OptionData { text = _dropdownText });
            
            foreach(var item in items)
            {
                _dropdown.options.Add(new Dropdown.OptionData { text = item });
            }
            
            _dropdown.RefreshShownValue();
        }

        private void OnValueChangedEventHandler(int selectedIndex)
        {
            if (selectedIndex < 1) { return; }
            
            var deviceIndex = selectedIndex - 1;
            var deviceName = _items[deviceIndex];

            OnDeviceSelected?.Invoke(deviceName);
        }
    }
}