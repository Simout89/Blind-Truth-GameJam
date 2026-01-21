using UnityEngine;

namespace Скриптерсы.Datas
{
    [CreateAssetMenu(menuName = "Datas/FirstAidKitData")]
    public class FirstAidKitData: ScriptableObject
    {
        [field: SerializeField] public float HealAmount { get; private set; } = 3;

    }
}