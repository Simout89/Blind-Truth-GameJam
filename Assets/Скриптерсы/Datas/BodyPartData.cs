using UnityEngine;

namespace Скриптерсы.Datas
{
    [CreateAssetMenu(menuName = "Datas/BodyPartData")]
    public class BodyPartData: ScriptableObject
    {
        [field: SerializeField] public float DamageMultiplayer { get; private set; } = 3f;

    }
}