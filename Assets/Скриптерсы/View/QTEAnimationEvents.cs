using UnityEngine;
using Zenject;

namespace Скриптерсы.View
{
    public class QTEAnimationEvents: MonoBehaviour
    {
        [Inject] private QuickTimeEventView quickTimeEventView;
        [SerializeField] private ParticleSystem _particleSystemRight;
        [SerializeField] private ParticleSystem _particleSystemLeft;

        public void EyeBlink(Eyes eyes)
        {
            quickTimeEventView.BlinkEye(eyes);

            if (eyes == Eyes.Left)
            {
                _particleSystemLeft.Play();
            }
            else
            {
                _particleSystemRight.Play();
            }
        }
    }
}