using System;

namespace Скриптерсы.Services
{
    public class SettingService
    {
        public float MouseSensitivity = 1f;
        public event Action OnSettingsChanged;

        public void ChangeMouseSensitivity(float value)
        {
            MouseSensitivity = value;
            OnSettingsChanged?.Invoke();
        }
    }
}