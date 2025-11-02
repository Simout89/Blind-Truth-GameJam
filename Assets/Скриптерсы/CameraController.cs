using System;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

namespace Скриптерсы
{
    public class CameraController: MonoBehaviour
    {
        [SerializeField] private CinemachineCamera _camera;

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
        
        public void Enable()
        {
            
        }

        public void Disable()
        {
            
        }
    }
}