using UnityEngine;

namespace Скриптерсы.Datas
{
    [CreateAssetMenu(menuName = "Datas/WeaponFeedBackData")]
    public class WeaponFeedBackData: ScriptableObject
    {
        [field: Header("Fov")]
        [field: SerializeField] public float additionFov { get; private set; } = 3f;
        [field: SerializeField] public float fadeInDuration { get; private set; } = 0.04f;
        [field: SerializeField] public float fadeOutDuration { get; private set; } = 0.1f;
        [field: Header("MouseShade")]
        [field: SerializeField] public float tiltIntensity { get; private set; } = -3f;
        [field: SerializeField] public Vector2 leftRightTilt { get; private set; } = new Vector2(-3, 3f);
        [field: SerializeField] public float duration { get; private set; } = 0.1f;

    }
}