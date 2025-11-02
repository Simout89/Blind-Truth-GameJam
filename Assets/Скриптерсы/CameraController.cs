using System;
using DG.Tweening;
using UnityEngine;

namespace Скриптерсы
{
    public class CameraController: MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        
        public void FovFade(float additionFov, float fadeInDuration, float fadeOutDuration)
        {
            _camera.DOComplete();

            float fov = _camera.fieldOfView;
        
            _camera.DOFieldOfView(fov + additionFov, fadeInDuration) 
                .OnComplete(() =>
                    _camera.DOFieldOfView(fov, fadeOutDuration));
        }
        
        public void Enable()
        {
            
        }

        public void Disable()
        {
            
        }
    }
}