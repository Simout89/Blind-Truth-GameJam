using System;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;
using Скриптерсы.Services;

namespace Скриптерсы
{
    public class CameraController: MonoBehaviour
    {
        [SerializeField] private CinemachineCamera _camera;
        public Transform CameraTransform => _camera.transform;

        [SerializeField] private CinemachinePanTilt _panTilt;
        [SerializeField] private CinemachineInputAxisController _inputAxisController;
        
        [Inject] private SettingService _settingService;
        public CinemachinePanTilt CinemachinePanTilt => _panTilt;
        
        
        private void OnEnable()
        {
            _settingService.OnSettingsChanged += HandleSettingsChanged;
        }
        
        private void OnDisable()
        {
            _settingService.OnSettingsChanged -= HandleSettingsChanged;
        }

        private void HandleSettingsChanged()
        {
            _inputAxisController.Controllers[0].Input.Gain = _settingService.MouseSensitivity * 1;
            _inputAxisController.Controllers[1].Input.Gain = _settingService.MouseSensitivity * -1;
        }

        public void FovFade(float additionFov, float fadeInDuration, float fadeOutDuration)
        {
            DOTween.Kill(_camera);

            float currentFov = _camera.Lens.FieldOfView;
            float targetFov = currentFov + additionFov;

            DOTween.To(() => _camera.Lens.FieldOfView, 
                    x => {
                        var lens = _camera.Lens;
                        lens.FieldOfView = x;
                        _camera.Lens = lens;
                    }, 
                    targetFov, 
                    fadeInDuration)
                .OnComplete(() =>
                {
                    DOTween.To(() => _camera.Lens.FieldOfView,
                        x => {
                            var lens = _camera.Lens;
                            lens.FieldOfView = x;
                            _camera.Lens = lens;
                        },
                        currentFov,
                        fadeOutDuration);
                });
        }
        
        public void Shake(float tiltIntensity, float panIntensity, float duration)
        {
            float targetTilt = _panTilt.TiltAxis.Value + tiltIntensity;
            float targetPan = _panTilt.PanAxis.Value + panIntensity;
            
            DOTween.To(() => _panTilt.TiltAxis.Value,
                x => _panTilt.TiltAxis.Value = x,
                targetTilt,
                duration);
            
            DOTween.To(() => _panTilt.PanAxis.Value,
                x => _panTilt.PanAxis.Value = x,
                targetPan,
                duration);
        }

        public void SetPanTilt(float tillit)
        {
            _panTilt.PanAxis.Value = tillit;
        }
        
        public void Enable()
        {
            _panTilt.enabled = true;
            _inputAxisController.enabled = true;
        }

        public void Disable()
        {
            _panTilt.enabled = false;
            _inputAxisController.enabled = false;
        }

    }
}