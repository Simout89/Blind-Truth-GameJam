using UnityEngine;
using Zenject;

namespace Скриптерсы.View
{
    public class QTEAnimationEvents: MonoBehaviour
    {
        [Inject] private QuickTimeEventView quickTimeEventView;

        public void EyeBlink(Eyes eyes)
        {
            quickTimeEventView.BlinkEye(eyes);
        }
    }
}