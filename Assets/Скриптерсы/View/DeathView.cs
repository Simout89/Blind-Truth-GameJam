using DG.Tweening;
using UnityEngine;

namespace Скриптерсы.View
{
    public class DeathView: MonoBehaviour
    {
        [SerializeField] private CanvasGroup image;
        
        public void Die()
        {
            image.DOFade(1, 1);
        }

        public void Respawn()
        {
            image.alpha = 1;
            
            image.DOFade(0, 1);
        }
    }
}